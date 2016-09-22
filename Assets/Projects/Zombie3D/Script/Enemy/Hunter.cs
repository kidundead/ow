using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Zombie3D
{
    /*      Hunter Zombie Class
     * 
     * 
     */
    public class Hunter : Enemy
    {

        public static EnemyState JUMP_STATE = new JumpState();

        public static EnemyState LOOKAROUND_STATE = new LookAroundState();
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
        public Vector3 speed;
        protected Collider leftHandCollider;
        protected bool jumpended;

        

        // Use this for initialization
        public override void Init(GameObject gObject)
        {

            base.Init(gObject);
            handCollider = enemyTransform.Find(BoneName.ENEMY_HAND).GetComponent<Collider>();
            leftHandCollider = enemyTransform.Find(BoneName.ENEMY_LEFTHAND).GetComponent<Collider>();


            collider = enemyTransform.GetComponent<Collider>();
            controller = enemyTransform.GetComponent<Collider>() as CharacterController;
            lastTarget = Vector3.zero;
            attackRange = 3f;

            onhitRate = 10f;
            lastRushingTime = -99;
            startAttacking = false;
            rushingCollided = false;
            rushingAttacked = false;

            detectionRange = 120.0f;
            MonsterConfig mConf = gConfig.GetMonsterConfig("Hunter");

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
                hp *= 1.5f;
            }



            animation[AnimationName.ENEMY_RUN].speed = 1.5f;
            //animation[AnimationName.ENEMY_GOTHIT].speed = 0.1f;
            animation[AnimationName.ENEMY_JUMPSTART].wrapMode = WrapMode.ClampForever;
            animation[AnimationName.ENEMY_JUMPING].wrapMode = WrapMode.Loop;
            animation[AnimationName.ENEMY_JUMPGEND].wrapMode = WrapMode.ClampForever;
            
        }

        public override void OnAttack()
        {

            base.OnAttack();

            Animate(AnimationName.ENEMY_ATTACK, WrapMode.ClampForever);
            attacked = false;
            /*
            Collider pcollider = player.GetCollider();
            if (pcollider != null)
            {
                if (handCollider.bounds.Intersects(pcollider.bounds))
                {
                    player.OnHit(attackDamage);

                }
            }
            */
            lastAttackTime = Time.time;
        }

        public override void OnDead()
        {
            gameScene.IncreaseKills();
            PlayBloodEffect();
            deadTime = Time.time;
            animation[AnimationName.ENEMY_DEATH1].wrapMode = WrapMode.ClampForever;
            animation.Play(AnimationName.ENEMY_DEATH1);
            enemyObject.layer = PhysicsLayer.DEADBODY;
            enemyObject.SendMessage("OnLoot");

            GameApp.GetInstance().GetGameState().Achievement.KillEnemy();
        }


        public override void CheckHit()
        {
            if (CouldMakeNextAttack())
            {
                Collider pcollider = player.GetCollider();
                if (pcollider != null)
                {

                    if (handCollider.bounds.Intersects(pcollider.bounds))
                    {
                        player.OnHit(rushingDamage);
                        lastAttackTime = Time.time;
                    }
                }
            }
            else if (!attacked && IsAnimationPlayedPercentage(AnimationName.ENEMY_ATTACK, 0.6f))
            {
                Collider pcollider = player.GetCollider();
                if (pcollider != null)
                {

                    if (handCollider.bounds.Intersects(pcollider.bounds))
                    {
                        player.OnHit(attackDamage);
                        attacked = true;
                    }
                }
            }
        }

        public bool Jump(float deltaTime)
        {


            //speed += Physics.gravity.y * Vector3.up * deltaTime;
            //speed += Physics.gravity.y * Vector3.up * deltaTime;

            //Vector3 moveDirection = enemyTransform.TransformDirection(Vector3.forward);



            //enemyTransform.Translate(speed * deltaTime, Space.World);


            //enemyTransform.LookAt(new Vector3(player.GetTransform().position.x, enemyTransform.position.y, player.GetTransform().position.z));
            if ((Time.time - lastRushingTime > 0.5f && enemyTransform.position.y <= (Constant.FLOORHEIGHT + 0.2f)) || Time.time - lastRushingTime > 4.0f)
            {

                CheckHit();


            }
            else
            {
                speed += Physics.gravity * deltaTime;
                controller.Move(speed * deltaTime);
            }


            if ((Time.time - lastRushingTime > 0.5f && enemyTransform.position.y <= (Constant.FLOORHEIGHT + 1.6f)) || Time.time - lastRushingTime > 2f || controller.isGrounded)
            {
                
                if (!jumpended)
                {
                    animation.CrossFade(AnimationName.ENEMY_JUMPGEND);
                    jumpended = true;
                }
                
                if(IsAnimationPlayedPercentage(AnimationName.ENEMY_JUMPGEND, 1f))            
                {
                    return true;
                }

            }

            return false;
        }

        public bool JumpEnded
        {
            get
            {
                return jumpended;
            }
        }

        public override void DoLogic(float deltaTime)
        {
            base.DoLogic(deltaTime);

            if (state == DEAD_STATE)
            {
                speed = Physics.gravity * 10;
                controller.Move(speed * deltaTime);
            }
        }

        public bool ReadyForJump()
        {

            if (Time.time - lastRushingTime > 5.5f && (enemyTransform.position - target.position).sqrMagnitude > 8 * 8 && (enemyTransform.position - target.position).sqrMagnitude < 15 * 15)
            {
                Vector3 tpoint = player.GetTransform().position;
                tpoint.y = enemyTransform.position.y;
                Ray ray = new Ray(enemyTransform.position + new Vector3(0, 0.5f, 0), tpoint - (enemyTransform.position + new Vector3(0, 0.0f, 0)));
                return true;
                if (Physics.Raycast(ray, out rayhit, Mathf.Sqrt(SqrDistanceFromPlayer), 1 << PhysicsLayer.WALL | 1 << PhysicsLayer.TRANSPARENT_WALL | 1 << PhysicsLayer.SCENE_OBJECT | 1 << PhysicsLayer.PLAYER))
                {
                    if (rayhit.collider.gameObject.name == "Player")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public void StartJump()
        {
            lastRushingTime = Time.time;

            Vector3 enemyOnFloorPosition = new Vector3(enemyTransform.position.x, Constant.FLOORHEIGHT, enemyTransform.position.z);


            //Physics:
            //v0 * t + 1/2 * g * t * t = h;
            //v1* t = dis;


            float h = 0f;

            Vector3 disVector = new Vector3(player.GetTransform().position.x, Constant.FLOORHEIGHT, player.GetTransform().position.z) - enemyOnFloorPosition;
            float dis = disVector.magnitude;
            float flySpeed = 10.0f;
            float t = dis / flySpeed;

            float v0 = (h - 0.5f * Physics.gravity.y * t * t) / t;

            speed = Vector3.up * v0 + disVector.normalized * flySpeed;


            //float aniLenth = animation[AnimationName.ENEMY_ATTACK].clip.length;
            //float ratio = aniLenth / t;

            //animation[AnimationName.ENEMY_RUN].speed = ratio;
            //animation[AnimationName.ENEMY_RUN].wrapMode = WrapMode.Once;
            animation.CrossFade(AnimationName.ENEMY_JUMPSTART);
            
            audio.PlayAudio("Special");
            jumpended = false;
        }

        public bool LookAroundTimOut()
        {
            if (Time.time - lookAroundStartTime > 2.0f)
            {
                return true;
            }
            else
            {
                return false;
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


                Object.Instantiate(rConfig.hitBlood, enemyTransform.position + Vector3.up * 0.5f, Quaternion.identity);
                
                hp -= dp.damage;

                if (weaponType == WeaponType.AssaultRifle || weaponType == WeaponType.Saw)
                {
                    if (Time.time - gotHitTime > 0.3f)
                    {
                        gotHitTime = Time.time;
                        state.OnHit(this);
                    }
                }
                else
                {
                    gotHitTime = Time.time;
                    state.OnHit(this);
                }
            }
        }



        public override void DoMove(float deltaTime)
        {
            Vector3 moveDirection = enemyTransform.TransformDirection(Vector3.forward);
            moveDirection += Physics.gravity * 0.2f;
            controller.Move(moveDirection * runSpeed * deltaTime);
            audio.PlayAudio(AudioName.WALK);
        }



        public override EnemyState EnterSpecialState(float deltaTime)
        {
            EnemyState state = null;

            target = player.GetTransform();
            if (Time.time - lastRushingTime > 5.0f && Time.time - lookAroundStartTime > 10.0f)
            {

                int rnd = Random.Range(0, 100);
                if (rnd < 10)
                {
                    state = new LookAroundState();
                    lookAroundStartTime = Time.time;
                    spawnCenter = enemyTransform.position;
                }
                else
                {

                    if (ReadyForJump())
                    {
                        StartJump();
                        state = new JumpState();
                    }
                }
            }


            return state;
        }



    }


}