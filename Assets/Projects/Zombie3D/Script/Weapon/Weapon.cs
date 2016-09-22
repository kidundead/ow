using UnityEngine;
using System.Collections;

namespace Zombie3D
{
    public enum WeaponType
    {
        NoGun = 0,
        AssaultRifle = 1,
        ShotGun = 2,
        RocketLauncher = 3,
        MachineGun = 4,
        LaserGun = 5,
        Sniper = 6,
        Saw = 7,
        GrenadeRifle = 8,
        NurseSaliva = 9
    }

    public enum WeaponExistState
    {
        Locked = 0,
        Unlocked,
        Owned
    }

    public abstract class Weapon
    {

        //U3D Component
        protected GameObject hitParticles;
        protected GameObject projectile;
        protected Camera cameraComponent;
        protected Transform cameraTransform;
        protected Transform rightGun;
        protected BaseCameraScript gameCamera;
        protected GameObject gunfire;
        protected GameObject gun;
        protected Transform weaponBoneTrans;
        protected ResourceConfigScript rConf;
        protected AudioSource shootAudio;

        protected GameConfig gConfig;
        protected GameScene gameScene;

        protected Player player;

        protected Vector3 aimTarget;

        protected bool isCDing = false;
        protected float hitForce;

        protected float range;

        protected float lastShootTime = 0;

        protected int maxCapacity;
        protected int maxGunLoad;
        protected int capacity;
        protected int bulletCount;
        protected float maxDeflection = 0.0f;
        protected Vector2 deflection;


        protected float damage;
        protected float attackFrenquency;
        protected float accuracy;


        protected float speedDrag;
        protected Vector3 lastHitPosition;
        protected int price;

        public int DamageLevel { get; set; }
        public int FrequencyLevel { get; set; }
        public int AccuracyLevel { get; set; }

        public WeaponConfig WConf { get; set; }
        public bool IsSelectedForBattle { get; set; }
        public abstract void Fire(float deltaTime);
        public abstract void StopFire();
        public WeaponExistState Exist { get; set; }

        public abstract WeaponType GetWeaponType();
        public abstract void changeReticle();

        public string Info
        {
            get
            {
                return Name;
            }
        }

        public string Name { get; set; }

        public int Price
        {
            get
            {
                return price;
            }
        }

        public float Accuracy
        {
            get
            {
                return accuracy;
            }
            set
            {
                accuracy = value;
            }
        }

        public virtual void DoLogic()
        {

        }

        public Weapon()
        {

        }

        public virtual void LoadConfig()
        {
            gConfig = GameApp.GetInstance().GetGameConfig();
        }

        public float GetSpeedDrag()
        {
            return speedDrag;
        }

        public int MaxGunLoad
        {
            get
            {
                return maxGunLoad;
            }
        }

        public static string GetWeaponNameEnd(WeaponType wType)
        {
            string weaponNameEnd = "";
            if (wType == WeaponType.RocketLauncher)
            {
                weaponNameEnd = "_RPG";
            }
            else if (wType == WeaponType.ShotGun)
            {
                weaponNameEnd = "_Shotgun";
            }
            else if (wType == WeaponType.Sniper)
            {
                weaponNameEnd = "_RPG";
            }
            else if (wType == WeaponType.LaserGun)
            {
                weaponNameEnd = "";


            }
            else if (wType == WeaponType.MachineGun)
            {
                weaponNameEnd = "";
            }
            else if (wType == WeaponType.Saw)
            {
                weaponNameEnd = "_Saw";
            }
            else
            {
                weaponNameEnd = "";
            }
            return weaponNameEnd;
        }


        public float Damage
        {
            get
            {
                return damage;
            }
            set
            {
                damage = value;
            }


        }

        public void Upgrade(float power, float frequency, float accur)
        {
            int i;
            if (power != 0)
            {
                damage += power;
                i = (int)(damage * 100);
                damage = (float)(i * 1.0) / 100;
                DamageLevel++;
            }

            if (frequency != 0)
            {
                attackFrenquency -= frequency;
                i = (int)(attackFrenquency * 100);
                attackFrenquency = (float)(i * 1.0) / 100;
                FrequencyLevel++;
            }

            if (accur != 0)
            {
                accuracy += accur;
                i = (int)(accuracy * 100);
                accuracy = (float)(i * 1.0) / 100;
                AccuracyLevel++;
            }

            if (DamageLevel + FrequencyLevel + AccuracyLevel == 10)
            {
                GameApp.GetInstance().GetGameState().Achievement.UpgradeTenTimes();
            }
           

        }

        public float GetNextLevelDamage()
        {
            float nextDamage = damage + damage * WConf.damageConf.upFactor;
            nextDamage = Math.SignificantFigures(nextDamage, 4);
            
            return nextDamage;
        }

        public bool IsMaxLevelDamage()
        {
            return (DamageLevel >= WConf.damageConf.maxLevel);
        }

        public bool IsMaxLevelCD()
        {
            return (FrequencyLevel >= WConf.attackRateConf.maxLevel);
        }

        public bool IsMaxLevelAccuracy()
        {
            return (AccuracyLevel >= WConf.accuracyConf.maxLevel);
        }

        public int GetDamageUpgradePrice()
        {
            int level = DamageLevel;
            float basePrice = WConf.damageConf.basePrice;
            float upPriceRate = WConf.damageConf.upPriceFactor;
            float nextPrice = basePrice;

            for (int i = 0; i < level; i++)
            {
                nextPrice = nextPrice * (1 + upPriceRate);
            }

            return (int)nextPrice / 100 * 100;
        }

        public int GetFrequencyUpgradePrice()
        {
            int level = FrequencyLevel;
            float basePrice = WConf.attackRateConf.basePrice;
            float upPriceRate = WConf.attackRateConf.upPriceFactor;
            float nextPrice = basePrice;

            for (int i = 0; i < level; i++)
            {
                nextPrice = nextPrice * (1 + upPriceRate);
            }
            
            return (int)nextPrice / 100 * 100;
        }
        public int GetAccuracyUpgradePrice()
        {
            int level = AccuracyLevel;
            float basePrice = WConf.accuracyConf.basePrice;
            float upPriceRate = WConf.accuracyConf.upPriceFactor;
            float nextPrice = basePrice;

            for (int i = 0; i < level; i++)
            {
                nextPrice = nextPrice * (1 + upPriceRate);
            }
            
            return (int)nextPrice / 100 * 100;
        }

        public float GetNextLevelFrequency()
        {
            float nextAttackFrenquency = attackFrenquency - attackFrenquency * WConf.attackRateConf.upFactor;
            nextAttackFrenquency = Math.SignificantFigures(nextAttackFrenquency, 4);
            return nextAttackFrenquency;
        }

        public float GetNextLevelAccuracy()
        {
            float nextAccuracy = accuracy + accuracy * WConf.accuracyConf.upFactor;
            nextAccuracy = Math.SignificantFigures(nextAccuracy, 4);
            return nextAccuracy;
        }


        public float GetLastShootTime()
        {
            return lastShootTime;
        }

        public float AttackFrequency
        {
            get
            {
                return attackFrenquency;
            }
            set
            {
                attackFrenquency = value;
            }
        }


        public virtual void Init()
        {
            gameScene = GameApp.GetInstance().GetGameScene();
            rConf = GameApp.GetInstance().GetResourceConfig();

            gameCamera = gameScene.GetCamera();
            cameraComponent = gameCamera.GetComponent<Camera>();
            cameraTransform = gameCamera.CameraTransform;
            player = gameScene.GetPlayer();       //! 获取场景中的人物

            aimTarget = new Vector3();

            hitParticles = rConf.hitparticles;
            projectile = rConf.projectile;
            hitForce = 0f;
            weaponBoneTrans = player.GetTransform().Find(BoneName.WEAPON_PATH);   //! 获取人物中挂载武器的节点

            //gun = weaponBoneTrans.Find("Rifle").gameObject;
            CreateGun();
            gun.transform.parent = weaponBoneTrans;


            BindGunAndFire();

            shootAudio = gun.GetComponent<AudioSource>();
            if (shootAudio == null)
            {

            }
            else
            {

            }
            GunOff();



        }


        public abstract void CreateGun();


        public virtual void FireUpdate(float deltaTime)
        {

        }

        public virtual void AutoAim(float deltaTime)
        {

        }

        public void BindGunAndFire()
        {
            /*
            gun.transform.position = weaponBoneTrans.position;
            gun.transform.rotation = weaponBoneTrans.rotation;
            gun.transform.parent = GameApp.GetInstance().GetPlayer().GetTransform();
            //gun.transform.Rotate(90,0,0);
            Matrix4x4 m = new Matrix4x4();
            m.SetTRS(weaponBoneTrans.localPosition, weaponBoneTrans.localRotation, weaponBoneTrans.localScale);



            GameObject gunbone = gun.transform.Find("Bone01").gameObject;
            Transform[] bones = new Transform[1];
            Matrix4x4[] bonePose = new Matrix4x4[1];
            bones[0] = weaponBoneTrans;

            bonePose[0] = m.inverse;
            SkinnedMeshRenderer smr = gunbone.renderer as SkinnedMeshRenderer;

            BoneWeight[] weights = new BoneWeight[smr.sharedMesh.vertexCount];
            for (int i = 0; i < smr.sharedMesh.vertexCount; i++)
            {
                weights[i].boneIndex0 = 0;
                weights[i].weight0 = 1;
            }

            foreach (Transform t in bones)
            {
                Matrix4x4 tm = new Matrix4x4();
                tm.SetTRS(t.localPosition, t.localRotation, t.localScale);
                //Debug.Log(tm.inverse.ToString());
            }


            smr.bones = bones;
            smr.sharedMesh.boneWeights = weights;
            smr.sharedMesh.bindposes = bonePose;
            */

            rightGun = gun.transform;

            //RectangleMesh rMesh = new RectangleMesh();
            //Material mat = Resources.Load("gunfire") as Material;
            //Vector3 pos = gun.transform.TransformPoint(0, 0, 0.3f);
            //gunfire = rMesh.Initialize(mat, "gunfire", 1, 1, pos);
            //gunfire.transform.parent = gun.transform;
        }


        public virtual void GetBullet()
        {
            capacity += maxGunLoad;
            capacity = Mathf.Clamp(capacity, 0, maxCapacity);

        }

        public void AddBullets(int num)
        {

            if (GameApp.GetInstance().GetGameState().Avatar == AvatarType.Marine)
            {
                num = (int)(num * Constant.MARINE_POWER_UP);
            }
            BulletCount += num;
            BulletCount = Mathf.Clamp(BulletCount, 0, 9999);
        }

        public virtual void MaxBullet()
        {
            BulletCount = maxGunLoad;
        }


        public virtual void GunOn()
        {

            GameObject model = gun.transform.Find("Bone01").gameObject;
            if (model.GetComponent<Renderer>() == null)
            {
                model = model.transform.Find("Bone02").gameObject;
            }
            model.GetComponent<Renderer>().enabled = true;


        }


        public virtual bool HaveBullets()
        {
            if (BulletCount == 0)
            {
                StopFire();
                return false;
            }
            else
            {
                return true;
            }
        }


        public virtual bool CouldMakeNextShoot()
        {
            if (Time.time - lastShootTime > attackFrenquency)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public virtual void GunOff()
        {
            /*
            gun.active = false;
            for (int i = 0; i < gun.transform.GetChildCount(); i++)
            {
                GameObject obj = gun.transform.GetChild(i).gameObject;
                obj.active = false;
            }
            */

            GameObject model = gun.transform.Find("Bone01").gameObject;
            if (model.GetComponent<Renderer>() == null)
            {
                model = model.transform.Find("Bone02").gameObject;
            }
            model.GetComponent<Renderer>().enabled = false;

            StopFire();
        }

        public virtual int BulletCount
        {
            get
            {
                return 0;
            }
            set
            {

            }
        }

        public int Capacity
        {
            get
            {
                return capacity;
            }
        }

        public Vector2 Deflection
        {
            get
            {
                return deflection;
            }
        }
    }
}