using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Zombie3D
{
    /*      Fat Zombie Class
     * 
     * 
     */
    public class Tank : Enemy
    {

        public static EnemyState RUSHINGSTART_STATE = new RushingStartState();
        public static EnemyState RUSHING_STATE = new RushingState();
        public static EnemyState RUSHINGATTACK_STATE = new RushingAttackState();


        protected Collider handCollider;
        protected Vector3 targetPosition;

        protected Vector3[] p = new Vector3[4];

        protected bool startAttacking;

        protected float rushingInterval;
        protected float rushingSpeed;
        protected float rushingDamage;
        protected float rushingAttackDamage;

        protected float lastRushingTime;

        protected string rndRushingAnimationName;

        protected Vector3 rushingTarget;
        protected bool rushingCollided;
        protected bool rushingAttacked;

        protected Collider leftHandCollider;



        // Use this for initialization
        public override void Init(GameObject gObject)
        {

            base.Init(gObject);

            if (Application.loadedLevelName == SceneName.SCENE_ARENA)
            {
                pathFinding = new FastPathFinding();
            }
            handCollider = enemyTransform.Find(BoneName.ENEMY_HAND).GetComponent<Collider>();
            leftHandCollider = enemyTransform.Find(BoneName.ENEMY_LEFTHAND).GetComponent<Collider>();


            collider = enemyTransform.GetComponent<Collider>();// enemyTransform.Find(BoneName.ENEMY_BODY).collider;
            controller = enemyTransform.GetComponent<Collider>() as CharacterController;
            lastTarget = Vector3.zero;
            attackRange = 3f;

            onhitRate = 10f;
            lastRushingTime = -99;
            startAttacking = false;
            rushingCollided = false;
            rushingAttacked = false;

            MonsterConfig mConf = gConfig.GetMonsterConfig("Tank");

            hp = mConf.hp * gameScene.GetDifficultyHpFactor;
            attackDamage = mConf.damage * gameScene.GetDifficultyDamageFactor;
            attackFrequency = mConf.attackRate;
            runSpeed = mConf.walkSpeed;
            rushingInterval = mConf.rushInterval;
            rushingSpeed = mConf.rushSpeed;
            rushingDamage = mConf.rushDamage * gameScene.GetDifficultyDamageFactor;
            rushingAttackDamage = mConf.rushAttackDamage * gameScene.GetDifficultyDamageFactor;
            lootCash = mConf.lootCash;

            if (IsElite)
            {
                hp *= 1.2f;
                runSpeed += 1f;
                attackDamage *= 1.2f;
                rushingDamage *= 1.2f;
                rushingAttackDamage *= 1.2f;

            }


        }

        //enemy on hit
        public override void OnHit(DamageProperty dp, WeaponType weaponType, bool criticalAttack)
        {
            if (state != GRAVEBORN_STATE)
            {
                beWokeUp = true;

                if (player.GetAvatarType() == AvatarType.Marine)
                {
                    dp.damage *= Constant.MARINE_POWER_UP;
                }
                else if (player.GetAvatarType() == AvatarType.EnegyArmor)
                {
                    dp.damage *= Constant.ENEGY_ARMOR__DAMAGE_BOOST;
                }


                Object.Instantiate(rConfig.hitBlood, enemyTransform.position + Vector3.up * 1.5f, Quaternion.identity);
                hp -= dp.damage;

                if (weaponType == WeaponType.RocketLauncher || weaponType == WeaponType.Sniper || weaponType == WeaponType.LaserGun)
                {
                    gotHitTime = Time.time;
                    state.OnHit(this);
                }
                else if (weaponType == WeaponType.AssaultRifle || weaponType == WeaponType.Saw)
                {
                    if (Math.RandomRate(10) && Time.time - gotHitTime > 2.0f)
                    {
                        gotHitTime = Time.time;
                        state.OnHit(this);
                    }
                }
                else if (weaponType == WeaponType.ShotGun || weaponType == WeaponType.MachineGun)
                {
                    if (Math.RandomRate(30))
                    {
                        gotHitTime = Time.time;
                        PlayBloodExplodeEffect(enemyTransform.TransformPoint(new Vector3(0, 1f, 1f)));
                        state.OnHit(this);
                    }
                }
            }
        }


        public override void OnAttack()
        {

            base.OnAttack();

            Animate(AnimationName.ENEMY_ATTACK, WrapMode.ClampForever);
            startAttacking = true;
            lastAttackTime = Time.time;
        }

        public override void CheckHit()
        {

            if (startAttacking && animation[AnimationName.ENEMY_ATTACK].time > animation[AnimationName.ENEMY_ATTACK].clip.length * 0.8f)
            {
                Collider pcollider = player.GetCollider();
                if (pcollider != null)
                {
                    if (handCollider.bounds.Intersects(pcollider.bounds))
                    {
                        player.OnHit(attackDamage);

                    }
                }
                //lastAttackTime = Time.time;
                startAttacking = false;
            }
        }


        public override void OnDead()
        {
            gameScene.IncreaseKills();
            audio.PlayAudio(AudioName.DEAD);
            deadTime = Time.time;
            PlayBloodEffect();
            animation[AnimationName.ENEMY_DEATH1].wrapMode = WrapMode.ClampForever;
            animation.CrossFade(AnimationName.ENEMY_DEATH1);
            enemyObject.layer = PhysicsLayer.DEADBODY;
            enemyObject.SendMessage("OnLoot");

            GameApp.GetInstance().GetGameState().Achievement.KillEnemy();
            GameApp.GetInstance().GetGameState().Achievement.CheckAchievemnet_BraveHeart();
        }


        public bool Rush(float deltaTime)
        {

            Collider pcollider = player.GetCollider();
            enemyTransform.LookAt(rushingTarget);
            Vector3 moveDir = enemyTransform.TransformDirection(Vector3.forward) * rushingSpeed + Physics.gravity * 0.5f;


            controller.Move(moveDir * deltaTime);
            if (!rushingCollided)
            {
                if (pcollider != null)
                {
                    Vector3 relativePos = enemyTransform.InverseTransformPoint(player.GetTransform().position);
                    //Debug.Log(relativePos);
                    if (relativePos.sqrMagnitude < 5.0f * 5.0f && relativePos.z > 1.0f)
                    {
                        player.OnHit(rushingDamage);
                        //player.GetHit(attackDamage * 2.0f,getHitFlySpeed);
                        lastAttackTime = Time.time;
                        rushingCollided = true;
                    }
                    /*
                    if (collider.bounds.Intersects(pcollider.bounds))
                    {
                        //Vector3 getHitFlySpeed = (rushingTarget - enemyTransform.position).normalized *10.0f + Vector3.up*10;
                        player.OnHit(rushingDamage);
                        //player.GetHit(attackDamage * 2.0f,getHitFlySpeed);
                        lastAttackTime = Time.time;
                        rushingCollided = true;
                    }*/
                }
            }

            foreach (Enemy enemy in gameScene.GetEnemies().Values)
            {
                if (enemy.GetState() == Enemy.DEAD_STATE || enemy.EnemyType == EnemyType.E_TANK)
                {
                    continue;
                }

                if (collider.bounds.Intersects(enemy.GetCollider().bounds))
                {
                    DamageProperty dp = new DamageProperty();
                    dp.damage = rushingDamage;
                    enemy.OnHit(dp, WeaponType.NoGun, true);
                }
            }

            GameObject[] woodboxes = gameScene.GetWoodBoxes();
            foreach (GameObject woodbox in woodboxes)
            {
                if (woodbox != null)
                {
                    if (collider.bounds.Intersects(woodbox.GetComponent<Collider>().bounds))
                    {

                        WoodBoxScript ws = woodbox.GetComponent<WoodBoxScript>();
                        ws.OnHit(attackDamage);

                    }
                }
            }

            if ((enemyTransform.position - rushingTarget).sqrMagnitude < 1.0f || (enemyTransform.position - player.GetTransform().position).sqrMagnitude < 4.0f || Time.time - lastRushingTime > 3.0f)
            {
                animation[AnimationName.ENEMY_RUSHINGEND].wrapMode = WrapMode.ClampForever;
                animation.CrossFade(AnimationName.ENEMY_RUSHINGEND);
                return true;
            }

            return false;

        }

        public bool RushAttack(float deltaTime)
        {


            Collider pcollider = player.GetCollider();
            if (!rushingAttacked && animation[AnimationName.ENEMY_RUSHINGEND].time >= animation[AnimationName.ENEMY_RUSHINGEND].clip.length * 0.3f)
            {
                if (pcollider != null)
                {
                    if (collider.bounds.Intersects(pcollider.bounds) || leftHandCollider.bounds.Intersects(pcollider.bounds))
                    {
                        player.OnHit(rushingAttackDamage);
                        lastAttackTime = Time.time;
                    }
                }
                rushingAttacked = true;

            }
            if (rushingAttacked && IsAnimationPlayedPercentage(AnimationName.ENEMY_RUSHINGEND, 1))
            {
                rushingAttacked = false;
                return true;
            }
            else
            {
                return false;
            }


        }


        public override EnemyState EnterSpecialState(float deltaTime)
        {
            //Could rush
            if (Time.time - lastRushingTime > rushingInterval && enemyTransform.position.y < Constant.FLOORHEIGHT + 0.20f)
            {

                rushingTarget = new Vector3(target.position.x, enemyTransform.position.y, target.position.z);
                Vector3 dir = (rushingTarget - enemyTransform.position).normalized;
                rushingTarget += dir * 5.0f;
                lastRushingTime = Time.time;
                float dis = (rushingTarget - enemyTransform.position).magnitude;
                Ray ray;
                RaycastHit hit;
                ray = new Ray(enemyTransform.position + new Vector3(0, 0.5f, 0), dir);


                if (!Physics.Raycast(ray, out hit, dis, 1 << PhysicsLayer.WALL | 1 << PhysicsLayer.TRANSPARENT_WALL | 1 << PhysicsLayer.TANK_WALL))
                {
                    enemyTransform.LookAt(rushingTarget);
                    rushingCollided = false;
                    animation[AnimationName.ENEMY_RUSHINGSTART].wrapMode = WrapMode.ClampForever;
                    animation[AnimationName.ENEMY_RUSHINGSTART].speed = 2.0f;
                    animation.CrossFade(AnimationName.ENEMY_RUSHINGSTART);
                    return RUSHINGSTART_STATE;

                }

            }
            return null;
        }
        /*
        public void MoveByGravity(float deltaTime)
        {
            Vector3 moveDir = Physics.gravity * 0.5f;


            controller.Move(moveDir * deltaTime);
        }
        */
        public override void DoMove(float deltaTime)
        {
            Vector3 moveDirection = enemyTransform.TransformDirection(Vector3.forward);
            moveDirection += Physics.gravity * 0.5f;
            controller.Move(moveDirection * runSpeed * deltaTime);
            audio.PlayAudio(AudioName.WALK);
        }


        public override void FindPath()
        {

            if (Application.loadedLevelName == SceneName.SCENE_ARENA)
            {

                Vector3 tpoint = target.position;
                //tpoint.y = enemyTransform.position.y;
                //enemyTransform.LookAt(tpoint);
                //enemyTransform.Translate(Vector3.forward * runSpeed * deltaTime);
                
                if (Time.time - lastPathFindingTime > 0.5f)
                {

                    lastPathFindingTime = Time.time;

                    tpoint.y = enemyTransform.position.y;

                    //1.Make player as lastTarget at first time
                    if (lastTarget == Vector3.zero)
                    {
                        lastTarget = target.position;
                    }


                    //2.Check if the player is behind the wall.
                    //  If the player is not reachable, follow the path.

                    //RaycastHit hit;

                    ray = new Ray(enemyTransform.position + new Vector3(0, 0.5f, 0), tpoint - (enemyTransform.position + new Vector3(0, 0.0f, 0)));

                    if (Physics.Raycast(ray, out rayhit, 100, 1 << PhysicsLayer.WALL | 1 << PhysicsLayer.TRANSPARENT_WALL | 1 << PhysicsLayer.TANK_WALL | 1 << PhysicsLayer.PLAYER))
                    {
                        if (rayhit.collider.gameObject.name == "Player")
                        {
                            lastTarget = tpoint;
                            nextPoint = -1;
                        }
                        else
                        {

                            float minDis = 999999;


                            //No next path point set yet(could see player before)
                            if (nextPoint == -1)
                            {
                                //Find a nearest point that could be reached
                                for (int i = 0; i < path.Length; i++)
                                {

                                    float dis = (path[i] - enemyTransform.position).magnitude;
                                    if (dis < minDis)
                                    {
                                        Ray sray = new Ray(enemyTransform.position + new Vector3(0, 0.8f, 0), path[i] - enemyTransform.position);
                                        RaycastHit hit;
                                        if (!Physics.Raycast(sray, out hit, dis, 1 << PhysicsLayer.WALL | 1 << PhysicsLayer.TRANSPARENT_WALL | 1 << PhysicsLayer.TANK_WALL))
                                        {
                                            lastTarget = path[i];
                                            minDis = dis;
                                            nextPoint = i;

                                        }
                                    }


                                }
                                //Debug.Log("Can't see Player, Find New Point");
                            }
                            else
                            {
                                //already on the path, reached on point, then compare previous with next point
                                if ((enemyTransform.position - lastTarget).sqrMagnitude < 2 * 2)
                                {
                                    int previous = nextPoint - 1;
                                    int next = nextPoint + 1;

                                    if (next == path.Length)
                                    {
                                        next = 0;
                                    }

                                    if (previous == -1)
                                    {
                                        previous = path.Length - 1;
                                    }


                                    //distance comparision
                                    if (((path[next] - player.GetTransform().position).magnitude + (path[next] - enemyTransform.position).magnitude) < ((path[previous] - player.GetTransform().position).magnitude + (path[previous] - enemyTransform.position).magnitude))
                                    {
                                        nextPoint = next;
                                        //Debug.Log("Next Point");
                                    }
                                    else
                                    {
                                        nextPoint = previous;
                                        //Debug.Log("Previous Point");
                                    }

                                    lastTarget = path[nextPoint];

                                }
                            }


                        }
                    }
                    else
                    {
                        //Debug.Log("nothing");
                    }


                }

                //100.Move towards lastTarget.
                lastTarget.y = enemyTransform.position.y;
                //Debug.Log(lastTarget);
                //targetObj.transform.position = lastTarget + new Vector3(0,2,0);
                enemyTransform.LookAt(lastTarget);
                //enemyTransform.Translate(Vector3.forward * runSpeed * deltaTime);
            }
            else
            {
                base.FindPath();
            }






        }


    }
}