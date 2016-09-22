using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Zombie3D
{

    //Player state
    public enum EPlayerState
    {
        Idle,
        Shooting,
        GotHit,
        Dead
    }


    public enum AvatarType
    {
        Human = 0,
        Plumber,
        Nerd,
        Doctor,
        Cowboy,
        Swat,
        Marine,
        EnegyArmor,
        AvatarCount
    }


    public class Player
    {

        public static PlayerState IDLE_STATE = new PlayerIdleState();
        public static PlayerState RUN_STATE = new PlayerRunState();
        public static PlayerState SHOOT_STATE = new PlayerShootState();
        public static PlayerState RUNSHOOT_STATE = new PlayerRunAndShootState();
        public static PlayerState GOTHIT_STATE = new PlayerGotHitState();
        public static PlayerState DEAD_STATE = new PlayerDeadState();


        //U3D Components

        protected GameObject playerObject;
        protected BaseCameraScript gameCamera;
        protected Transform playerTransform;
        protected CharacterController charController;
        protected Animation animation;
        protected Collider collider;
        protected GameObject powerObj = null;
        protected Transform respawnTrans;
        protected PlayerConfig playerConfig;
        protected AudioPlayer audioPlayer;


        protected AvatarType avatarType;
        protected PlayerState playerState;
        protected EPlayerState epState;
        protected InputController inputController;
        protected Vector3 getHitFlySpeed;
        protected BombSpot bombSpot = null;
        protected Vector3 lastHitPosition;


        //weapons
        protected Weapon weapon;
        protected List<Weapon> weaponList;
        public WayPointScript NearestWayPoint { get; set; }

        //properties of player
        protected float maxHp;
        protected float hp;
        protected float guiHp = 0;
        protected float walkSpeed;
        protected float powerBuff;
        protected float powerBuffStartTime;
        protected float lastUpdateNearestWayPointTime;
        protected int currentWeaponIndex = 0;

        protected float gothitEndTime = 0;

        protected string weaponNameEnd;


        protected bool isRunning = false;

        public Vector3 HitPoint { get; set; }

        public float WalkSpeed
        {
            get
            {
                if (avatarType != AvatarType.Plumber)
                {

                    float speed = GameApp.GetInstance().GetGameConfig().playerConf.walkSpeed - weapon.GetSpeedDrag();

                    if (avatarType == AvatarType.Cowboy)
                    {
                        return speed * Constant.COWBOY_SPEED_UP;
                    }
                    else if (avatarType == AvatarType.EnegyArmor)
                    {
                        speed = GameApp.GetInstance().GetGameConfig().playerConf.walkSpeed;
                        return speed * Constant.ENEGY_ARMOR_SPEED_UP;
                    }
                    else
                    {
                        return speed;
                    }

                }
                else
                {
                    return GameApp.GetInstance().GetGameConfig().playerConf.walkSpeed;
                }
            }
        }


        public InputController InputController
        {
            get
            {
                return inputController;
            }
        }

        public bool IsRunning
        {
            get
            {
                return isRunning;
            }
        }

        public void RandomSawAnimation()
        {
            if (Math.RandomRate(50))
            {
                weaponNameEnd = "_Saw";
            }
            else
            {
                weaponNameEnd = "_Saw2";
            }


        }

        public void ResetSawAnimation()
        {
            if(weapon.GetWeaponType() == WeaponType.Saw)
            weaponNameEnd = "_Saw";
        }

        public string WeaponNameEnd
        {
            get
            {
                return weaponNameEnd;

            }
            set
            {
                weaponNameEnd = value;
            }
        }
        public Vector3 GetHitFlySpeed
        {
            get
            {
                return getHitFlySpeed;
            }
        }

        public Vector3 LastHitPosition
        {
            get
            {
                return lastHitPosition;
            }
            set
            {
                lastHitPosition = value;
            }
        }

        public void CreateScreenBlood(float damage)
        {
            if (gameCamera != null)
            {
                gameCamera.CreateScreenBlood(1.0f);
            }
        }

        public void Move(Vector3 motion)
        {
            if (charController != null)
            {
                charController.Move(motion);
                if (playerState == RUN_STATE || playerState == RUNSHOOT_STATE)
                {
                    audioPlayer.PlayAudio(AudioName.WALK);
                }
            }
        }

        public BombSpot BombSpot
        {
            get
            {
                return bombSpot;
            }
            set
            {
                bombSpot = value;
            }
        }

        public GameObject PlayerObject
        {
            get
            {
                return playerObject;
            }
        }

        public float PowerBuff
        {
            get
            {
                return powerBuff;
            }
        }

        public float GetGuiHp()
        {
            return guiHp;
        }

        public float GetHp()
        {
            return hp;
        }

        public float GetMaxHp()
        {
            return maxHp;
        }

        public Transform GetTransform()
        {
            return playerTransform;
        }

        public Collider GetCollider()
        {
            return collider;
        }

        public Transform GetRespawnTransform()
        {
            return respawnTrans;
        }

        public void Init()
        {
			//! 随机出生点
            GameObject[] pss = GameObject.FindGameObjectsWithTag("Respawn");
            int rnd = Random.Range(0, pss.Length);

            GameObject ps = pss[rnd];
            respawnTrans = ps.transform;
            avatarType = GameApp.GetInstance().GetGameState().Avatar;
            //! playerObject = AvatarFactory.GetInstance().CreateAvatar(avatarType);   //! 创建角色
            
			playerObject = AvatarFactory.GetInstance ().CreateAvatar (Zombie3D.AvatarType.EnegyArmor); //! 创建钢铁侠
			playerObject.transform.position = ps.transform.position;
            playerObject.transform.rotation = ps.transform.rotation;

            playerObject.name = "Player";                    
            playerTransform = playerObject.transform;
            playerConfig = GameApp.GetInstance().GetGameConfig().playerConf;
            int armorLevel = GameApp.GetInstance().GetGameState().ArmorLevel;
            hp = playerConfig.hp * (1 + armorLevel * 0.5f);
            maxHp = hp;

			//! 特种兵的血量 × 2
            if (avatarType == AvatarType.Swat)
            {
                hp = hp * Constant.SWAT_HP;
                maxHp = hp;
            }
            else if (avatarType == AvatarType.EnegyArmor)
            {
                hp = hp * Constant.ENEGY_ARMOR_HP_BOOST;
                maxHp = hp;
            }


            gameCamera = GameApp.GetInstance().GetGameScene().GetCamera();
            charController = playerObject.GetComponent<CharacterController>();  //! 控制器
            animation = playerObject.GetComponent<UnityEngine.Animation>();     //! 动画器
            collider = playerObject.GetComponent<Collider>();                   //! 碰撞检测

			//! 音效
            audioPlayer = new AudioPlayer();
            Transform folderTrans = playerTransform.Find("Audio");
            audioPlayer.AddAudio(folderTrans, AudioName.GETITEM);
            audioPlayer.AddAudio(folderTrans, AudioName.DEAD);
            audioPlayer.AddAudio(folderTrans, AudioName.SWITCH);
            audioPlayer.AddAudio(folderTrans, AudioName.WALK);


			//! 为什么又初始化一边武器列表
            GameApp.GetInstance().GetGameState().InitWeapons();


            weaponList = GameApp.GetInstance().GetGameState().GetBattleWeapons();


            playerState = Player.IDLE_STATE;



            foreach (Weapon w in weaponList)
            {
                w.Init();
            }


			Weapon powerWeapon = null;
			float maxDamage = 0;
            foreach (Weapon w in weaponList)
            {
				if (maxDamage < w.Damage)
				{
					maxDamage = w.Damage;
					powerWeapon = w;
				}

                //! if (w.IsSelectedForBattle)
                //! {
                //!     ChangeWeapon(w);
                //!     break;
                //! }
            }

			//! 切换武器
			ChangeWeapon(powerWeapon);


            walkSpeed = GameApp.GetInstance().GetGameConfig().playerConf.walkSpeed - weapon.GetSpeedDrag();  //! 人物速度 - 武器武器负重

            ChangeToNormalState();


            if (gameCamera.GetCameraType() == CameraType.TPSCamera)
            {
                inputController = new TPSInputController();
                inputController.Init();
            }
            else if (gameCamera.GetCameraType() == CameraType.TopWatchingCamera)
            {
                inputController = new TopWatchingInputController();
                inputController.Init();
            }


            UpdateNearestWayPoint();

        }


        public AvatarType GetAvatarType()
        {
            return avatarType;
        }

        public void UpdateNearestWayPoint()
        {
            GameObject[] points = GameObject.FindGameObjectsWithTag(TagName.WAYPOINT);
            float minDisSqrPlayer = 99999.0f;
            foreach (GameObject wObj in points)
            {
                WayPointScript w = wObj.GetComponent<WayPointScript>();
                float disPlayer = (w.transform.position - playerTransform.position).magnitude;

                if (disPlayer < minDisSqrPlayer)
                {
                    Ray ray = new Ray(playerTransform.position + new Vector3(0, 0.5f, 0), w.transform.position - playerTransform.position);

                    RaycastHit hit;
                    if (!Physics.Raycast(ray, out hit, disPlayer, 1 << PhysicsLayer.WALL | 1 << PhysicsLayer.FLOOR))
                    {
                        NearestWayPoint = w;
                        minDisSqrPlayer = disPlayer;
                    }
                }
            }
        }

        public void Run()
        {
            isRunning = true;
        }

        public void StopRun()
        {
            isRunning = false;
        }


        public void SetState(PlayerState state)
        {
            //Debug.Log(state);
            playerState = state;
        }


        public bool IsPlayingAnimation(string name)
        {
            return animation.IsPlaying(name);
        }

        public bool AnimationEnds(string name)
        {
            //Debug.Log(animation[name].time + "," + animation[name].clip.length);
            if (animation[name].time >= animation[name].clip.length * Constant.ANIMATION_ENDS_PERCENT || animation[name].wrapMode == WrapMode.Loop)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsAnimationPlayedPercentage(string aniName, float percentage)
        {
            
            if (animation[aniName].time >= animation[aniName].clip.length * percentage)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public void Animate(string animationName, WrapMode wrapMode)
        {
            if (animation[animationName] != null)
            {
                animation[animationName].wrapMode = wrapMode;
                if (!IsPlayingAnimation(AnimationName.PLAYER_GOTHIT) || (animationName.StartsWith(AnimationName.PLAYER_DEATH)))
                {
                    if ((wrapMode == WrapMode.Loop) || (!animation.IsPlaying(animationName) && animationName != AnimationName.PLAYER_GOTHIT))
                    {
                        animation.CrossFade(animationName);
                    }
                    else
                    {
                        animation.Stop();
                        animation.Play(animationName);
                    }
                }
                else
                {
                }
            }

        }

        public void CheckBombSpot()
        {
            bombSpot = null;
            List<BombSpot> bsl = GameApp.GetInstance().GetGameScene().GetBombSpots();
            foreach (BombSpot bs in bsl)
            {
                bs.DoLogic();
                if (bs.CheckInSpot())
                {
                    bombSpot = bs;
                }
                else if (bs.isInstalling())
                {
                    bombSpot = bs;
                }
            }

        }

        public void ZoomIn(float deltaTime)
        {
            //zoom camera fov preparing shooting
            if (weapon.GetWeaponType() == WeaponType.AssaultRifle || weapon.GetWeaponType() == WeaponType.MachineGun)
            {
                gameCamera.ZoomIn(deltaTime);
            }

        }

        public void AutoAim(float deltaTime)
        {
            weapon.AutoAim(deltaTime);
        }



        public void Fire(float deltaTime)
        {


            if (GameApp.GetInstance().GetGameScene().PlayingState == PlayingState.GamePlaying)
            {
                weapon.Fire(deltaTime);
            }

        }

        public void ZoomOut(float deltaTime)
        {
            //camera.fov =  Mathf.Lerp(camera.fov, CAMERA_NORMAL_FOV, deltaTime * Constant.CAMERA_ZOOM_SPEED);

            gameCamera.ZoomOut(deltaTime);
        }
        public void StopFire()
        {


            weapon.StopFire();
        }


        public void DoLogic(float deltaTime)
        {
            playerState.NextState(this, deltaTime);


            if (guiHp != hp)
            {
                float diff = Mathf.Abs(guiHp - hp);
                guiHp = Mathf.MoveTowards(guiHp, hp, diff * 5 * deltaTime);

            }

            if (powerBuff != 1.0f && Time.time - powerBuffStartTime > Constant.POWERBUFFTIME)
            {
                ChangeToNormalState();
            }


            foreach (Weapon w in weaponList)
            {
                if (w.IsSelectedForBattle)
                {
                    w.DoLogic();
                }
            }

            if (avatarType == AvatarType.Doctor && playerState != DEAD_STATE)
            {
                hp += maxHp * Constant.DOCTOR_HP_RECOVERY / 100.0f * deltaTime;
                if (hp > maxHp)
                {
                    hp = maxHp;
                }

            }



            if (Time.time - lastUpdateNearestWayPointTime > 1.0f)
            {
                UpdateNearestWayPoint();
                lastUpdateNearestWayPointTime = Time.time;
            }

        }

        /*
     Quest quest = GameApp.GetInstance().GetGameScene().GetQuest();

     if (quest.GetQuestType() == QuestType.Bomb)
     {
         CheckBombSpot();
     }
     */
        /*
        public void DoLogic(float deltaTime)
        {

            lastHitPosition = Vector3.zero;

            InputInfo inputInfo = new InputInfo();
            inputController.ProcessInput(deltaTime, inputInfo);
            
            if (inputInfo.IsMoving())
            {
                Run();
            }
            else
            {
                StopRun();
            }

            if (!inputInfo.fire && inputInfo.IsMoving() && epState != EPlayerState.GotHit && epState != EPlayerState.Dead)
            {
                if (!IsPlayingAnimation(AnimationName.PLAYER_RUNFIRE + weaponNameEnd)
                    && !IsPlayingAnimation(AnimationName.PLAYER_SHOT + weaponNameEnd))
                {
                    SetState(EPlayerState.Idle);
                }
            }
            Animate();
            if (inputInfo.fire)
            {
                Fire();
            }
            if (inputInfo.stopFire)
            {
                StopFire();
            }
            //Debug.Log(inputInfo.moveDirection);
            float speed = walkSpeed;
            if (epState == EPlayerState.Shooting)
            {
                speed = walkSpeed * 0.6f;
            }

            Move(inputInfo.moveDirection * (deltaTime * speed));


            

            if (guiHp != hp)
            {
                float diff = Mathf.Abs(guiHp - hp);
                guiHp = Mathf.MoveTowards(guiHp, hp, diff * 5 * deltaTime);

            }

            if (powerBuff != 1.0f && Time.time - powerBuffStartTime > Constant.POWERBUFFTIME)
            {
                ChangeToNormalState();
            }

            foreach (Weapon w in weaponList)
            {
                if (w.IsSelectedForBattle)
                {
                    w.DoLogic();
                }
            }


        }
         * */

        /*
        public void GetHit(float damage, Vector3 flySpeed)
        {
            //deduct hp
            hp -= damage;
            hp = Mathf.Clamp(hp, 0, maxHp);

            //check if player is dead
            if (hp == 0)
            {
                epState = EPlayerState.Dead;
                weapon.StopFire();
                //animation.CrossFade(AnimationName.PLAYER_DEATH);
            }
            getHitFlySpeed = flySpeed;
        }
        */

        public float HP
        {
            get
            {
                return hp;
            }
        }
        public void OnHit(float damage)
        {
            //deduct hp
            hp -= damage;
            hp = (int)hp;
            hp = Mathf.Clamp(hp, 0, maxHp);

            playerState.OnHit(this, damage);
        }

        public bool CouldGetAnotherHit()
        {
            if (Time.time - gothitEndTime > 0.5f)
            {
                gothitEndTime = Time.time;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void OnDead()
        {
            audioPlayer.PlayAudio(AudioName.DEAD);
            weapon.StopFire();
            int rnd = Random.Range(1, 4);
            Animate(AnimationName.PLAYER_DEATH+rnd, WrapMode.ClampForever);   //! 播放死亡动画

			//! 显示红色 血的图片
            Transform sbdTrans = gameCamera.gameObject.transform.Find("Screen_Blood_Dead");
            if (sbdTrans != null)
            {
                //! sbdTrans.gameObject.active = true;
				sbdTrans.gameObject.SetActive (true);
            }

			//! 删除碰撞检测
            GameObject.Destroy(playerObject.GetComponent<Collider>());

            GameScene gameScene = GameApp.GetInstance().GetGameScene();
            gameScene.PlayingState = PlayingState.GameLose;

            GameApp.GetInstance().GetGameState().Achievement.LoseGame();

            GameUIScript ui2 = GameObject.Find("SceneGUI").GetComponent<GameUIScript>();
            ui2.GetPanel(GameUIName.GAME_OVER).Show();
            //BattleEndUI battleEndUI = ui2.GetComponent<BattleEndUI>();
            //battleEndUI.enabled = true;

            gameCamera.GetComponent<AudioSource>().Stop();
            gameCamera.loseAudio.Play();

        }

        public void GetHealed(int point)
        {
            hp += point;
            hp = Mathf.Clamp(hp, 0, maxHp);
        }

        public void GetFullyHealed()
        {
            hp = maxHp;
        }

        public void ChangeWeapon(Weapon w)
        {
            if (w.IsSelectedForBattle)
            {
                if (weapon != null)
                {
                    weapon.GunOff();
                    Animate(AnimationName.PLAYER_IDLE + weaponNameEnd, WrapMode.Loop);
                }
                weapon = w;
                weapon.changeReticle();   //! 切换十字对准线
                weapon.GunOn();

                audioPlayer.PlayAudio(AudioName.SWITCH);

                if (weapon.GetWeaponType() == WeaponType.RocketLauncher)
                {
                    weaponNameEnd = "_RPG";
                }
                else if (weapon.GetWeaponType() == WeaponType.ShotGun)
                {
                    weaponNameEnd = "_Shotgun";
                }
                else if (weapon.GetWeaponType() == WeaponType.Sniper)
                {
                    weaponNameEnd = "_RPG";
                }
                else if (weapon.GetWeaponType() == WeaponType.LaserGun)
                {
                    weaponNameEnd = "";
                }
                else if (weapon.GetWeaponType() == WeaponType.MachineGun)
                {
                    weaponNameEnd = "";
                }
                else if (weapon.GetWeaponType() == WeaponType.Saw)
                {
                    weaponNameEnd = "_Saw";
                }
                else
                {
                    weaponNameEnd = "";
                }

                gameCamera.isAngelVFixed = false;
                /*
                if (weapon.GetWeaponType() == WeaponType.LaserGun)
                {
                    gameCamera.fixedAngelV = -2f;
                    gameCamera.isAngelVFixed = true;
                }
                */
                GameUIScript gameUI = GameObject.Find("SceneGUI").GetComponent<GameUIScript>();
                gameUI.SetWeaponLogo(weapon.GetWeaponType());
            }
        }

        public void NextWeapon()
        {
            //switch to next weapon


            currentWeaponIndex++;
            if (currentWeaponIndex >= weaponList.Count)
            {
                currentWeaponIndex = 0;
            }

            for (; ; )
            {

                bool couldUse = true;
                if (weaponList[currentWeaponIndex] == null)
                {
                    couldUse = false;
                }
                else if (!weaponList[currentWeaponIndex].IsSelectedForBattle)
                {
                    couldUse = false;
                }


                if (couldUse)
                {
                    break;
                }
                else
                {
                    currentWeaponIndex++;
                    if (currentWeaponIndex >= weaponList.Count)
                    {
                        currentWeaponIndex = 0;
                    }
                }

            }


            ChangeWeapon(weaponList[currentWeaponIndex]);

        }

        public void OnPickUp(ItemType itemID)
        {
            //pick up items

            audioPlayer.PlayAudio(AudioName.GETITEM);
            if (itemID == ItemType.Hp)
            {
                GetHealed((int)maxHp);
            }
            else if (itemID == ItemType.Gold)
            {
                int cash = (int)(Constant.WOODBOX_LOOT * GameApp.GetInstance().GetGameScene().GetDifficultyCashDropFactor);

                GameApp.GetInstance().GetGameState().AddCash(cash);
            }

            else if (itemID == ItemType.Power)
            {
                ChangeToPowerBuffState();

            }
            else if (itemID == ItemType.AssaultGun)
            {
                //List<Weapon> weaponList = GameApp.GetInstance().GetGameState().GetWeapons();
                foreach (Weapon w in weaponList)
                {
                    if (w.GetWeaponType() == WeaponType.AssaultRifle)
                    {
                        w.AddBullets(w.WConf.bullet / 4);
                        break;
                    }
                }
            }
            else if (itemID == ItemType.ShotGun)
            {
                foreach (Weapon w in weaponList)
                {
                    if (w.GetWeaponType() == WeaponType.ShotGun)
                    {
                        w.AddBullets(w.WConf.bullet / 4);
                        break;
                    }
                }
            }
            else if (itemID == ItemType.RocketLauncer)
            {
                foreach (Weapon w in weaponList)
                {
                    if (w.GetWeaponType() == WeaponType.RocketLauncher)
                    {
                        w.AddBullets(w.WConf.bullet / 4);
                        break;
                    }
                }
            }
            else if (itemID == ItemType.LaserGun)
            {
                foreach (Weapon w in weaponList)
                {
                    if (w.GetWeaponType() == WeaponType.LaserGun)
                    {
                        w.AddBullets(w.WConf.bullet / 4);
                        break;
                    }
                }
            }
            else if (itemID == ItemType.Saw)
            {
                foreach (Weapon w in weaponList)
                {
                    if (w.GetWeaponType() == WeaponType.Saw)
                    {
                        w.AddBullets(w.WConf.bullet / 4);
                        break;
                    }
                }
            }
            else if (itemID == ItemType.Sniper)
            {
                foreach (Weapon w in weaponList)
                {
                    if (w.GetWeaponType() == WeaponType.Sniper)
                    {
                        w.AddBullets(w.WConf.bullet / 4);
                        break;
                    }
                }
            }
            else if (itemID == ItemType.MachineGun)
            {
                foreach (Weapon w in weaponList)
                {
                    if (w.GetWeaponType() == WeaponType.MachineGun)
                    {
                        w.AddBullets(w.WConf.bullet / 4);
                        break;
                    }
                }
            }

        }

        public Weapon GetWeapon()
        {
            return weapon;
        }

        public void SetTransparent(bool bTrue)
        {
            if (bTrue)
            {
                playerObject.transform.Find("Avatar_Suit").GetComponent<Renderer>().material.shader = Shader.Find("iPhone/AlphaBlend_Color");
                playerObject.transform.Find("Avatar_Suit").GetComponent<Renderer>().material.SetColor("_TintColor", new Color(1f,1f,1f,0.1f));
            }
            else
            {
                playerObject.transform.Find("Avatar_Suit").GetComponent<Renderer>().material.shader = Shader.Find("iPhone/SolidTexture");
            }
        }

        public void ChangeToPowerBuffState()
        {
            powerBuff = Constant.POWER_BUFF;
            powerBuffStartTime = Time.time;
            GameObject powerLogoPrefab = GameApp.GetInstance().GetResourceConfig().powerLogo;

            if (powerObj == null)
            {
                powerObj = Object.Instantiate(powerLogoPrefab, playerTransform.TransformPoint(Vector3.up * 2.0f), Quaternion.identity) as GameObject;
                powerObj.transform.parent = playerTransform;
            }
            //playerObject.transform.Find("buff").gameObject.active = true;

            if (avatarType != AvatarType.EnegyArmor)
            {
                Color color = playerObject.transform.Find("Avatar_Suit").GetComponent<AlphaAnimationScript>().startColor;
                playerObject.transform.Find("Avatar_Suit").GetComponent<Renderer>().material.shader = Shader.Find("iPhone/SolidTextureBright");
                playerObject.transform.Find("Avatar_Suit").GetComponent<Renderer>().material.SetColor("_TintColor", color);
                playerObject.transform.Find("Avatar_Suit").GetComponent<AlphaAnimationScript>().enableBrightAnimation = true;

                Transform cap = playerObject.transform.Find("Avatar_Cap");
                if (cap != null)
                {
                    cap.GetComponent<Renderer>().material.shader = Shader.Find("iPhone/SolidTextureBright");
                    cap.GetComponent<Renderer>().material.SetColor("_TintColor", color);
                    cap.GetComponent<AlphaAnimationScript>().enableBrightAnimation = true;
                }
            }
        }

        public void ChangeToNormalState()
        {
            powerBuff = 1.0f;
            //playerObject.transform.Find("buff").gameObject.active = false;

            if (avatarType != AvatarType.EnegyArmor)
            {
                Color color = new Color(0.8f, 0.8f, 0.8f);


                playerObject.transform.Find("Avatar_Suit").GetComponent<Renderer>().material.shader = Shader.Find("iPhone/SolidTexture");
                playerObject.transform.Find("Avatar_Suit").GetComponent<Renderer>().material.SetColor("_TintColor", color);
                playerObject.transform.Find("Avatar_Suit").GetComponent<AlphaAnimationScript>().enableBrightAnimation = false;

                Transform cap = playerObject.transform.Find("Avatar_Cap");
                if (cap != null)
                {
                    cap.GetComponent<Renderer>().material.shader = Shader.Find("iPhone/SolidTexture");
                    cap.GetComponent<Renderer>().material.SetColor("_TintColor", color);
                    cap.GetComponent<AlphaAnimationScript>().enableBrightAnimation = false;

                }
            }
            GameObject.Destroy(powerObj);
            powerObj = null;

        }

    }
}