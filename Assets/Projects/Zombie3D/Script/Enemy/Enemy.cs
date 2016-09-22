using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Zombie3D
{



    /*   Base Class for all enemies
     * 
     * 
     */



    public abstract class Enemy
    {
        public static EnemyState GRAVEBORN_STATE = new GraveBornState();
        public static EnemyState IDLE_STATE = new IdleState();
        public static EnemyState CATCHING_STATE = new CatchingState();
        public static EnemyState GOTHIT_STATE = new GotHitState();
        public static EnemyState PATROL_STATE = new PatrolState();
        public static EnemyState ATTACK_STATE = new AttackState();
        public static EnemyState DEAD_STATE = new DeadState();

        //U3D Components
        protected GameObject enemyObject;
        protected Transform enemyTransform;
        protected Animation animation;
        protected Rigidbody rigidbody;
        protected Transform aimedTransform;
        protected Transform target;
        protected Vector3 spawnCenter;
        protected Vector3 patrolTarget;
        protected EnemySpawnScript spawn;
        protected SceneUIScript sceneGUI;
        protected Collider collider;
        protected ResourceConfigScript rConfig;
        protected GameConfig gConfig;
        protected EnemyType enemyType;
        protected Vector3 lastTarget;
        protected GameScene gameScene;
        protected Player player;
        protected CharacterController controller;
        protected Vector3 dir;
        protected AudioPlayer audio;
        protected IPathFinding pathFinding;


        protected float hp;
        protected float runSpeed;
        protected bool beWokeUp = false;
        protected float deadTime = 0;
        
        protected string name;
        protected bool visible = false;
        protected ObjectPool hitBloodObjectPool;
        protected bool moveWithCharacterController = false;

        protected float lastUpdateTime;
        protected float lastPathFindingTime;
        protected float lastSearchPathTime;

        protected float lastReCheckPathTime;
        protected EnemyState state;
        

        //enemy attack distance
        protected float attackRange;
        //the distance when the enemy could see player

        protected float detectionRange;

        //the min distance from player the enemy could walk 
        protected float minRange;
        //interval time between two attacks
        protected float attackFrequency;

        protected float attackDamage;

        protected float idlePeriod = 1.5f;

        protected float aiRadius = 100.0f;
        protected int score = 0;
        protected int lootCash = 0;

        protected float gotHitTime = 0;
        protected float idleStartTime;
        protected float lastAttackTime = -100.0f;
        protected float lookAroundStartTime;

        protected int nextPoint = -1;
        protected string runAnimationName = AnimationName.ENEMY_RUN;
        protected GameObject targetObj;
        protected float onhitRate = 100;
        protected bool criticalAttacked;
        protected bool attacked = false;
        protected Quaternion deadRotation;
        protected Vector3 deadPosition;
        protected Vector3[] path;

        public bool IsElite { get; set; }

        public Ray ray;
        public RaycastHit rayhit;

        public float lastStateTime;

        public AudioPlayer Audio
        {
            get
            {
                return audio;
            }
        }


        public string RunAnimationName
        {
            get
            {
                return runAnimationName;
            }
        }

        public bool MoveWithCharacterController
        {
            get
            {
                return moveWithCharacterController;
            }
            set
            {
                moveWithCharacterController = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public float HP
        {
            get
            {
                return hp;
            }
        }

        public Transform GetTransform()
        {
            return enemyTransform;
        }

        public float DetectionRange
        {
            get
            {
                return detectionRange;
            }
        }

        public float AttackRange
        {
            get
            {
                return attackRange;
            }
        }



        public EnemyType EnemyType
        {
            get
            {
                return enemyType;
            }
            set
            {
                enemyType = value;
            }
        }

        public EnemySpawnScript Spawn
        {
            set
            {
                spawn = value;
            }
        }

        public Vector3 LastTarget
        {
            get
            {
                return lastTarget;
            }
        }

        public bool CouldMakeNextAttack()
        {
            if (Time.time - lastAttackTime >= attackFrequency)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public virtual bool CouldEnterAttackState()
        {
            if (SqrDistanceFromPlayer < AttackRange * AttackRange)
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

        public bool AttackAnimationEnds()
        {
            //if (enemyObject.animation[AnimationName.ENEMY_ATTACK].time >= enemyObject.animation[AnimationName.ENEMY_ATTACK].clip.length)
            if (Time.time - lastAttackTime > enemyObject.GetComponent<UnityEngine.Animation>()[AnimationName.ENEMY_ATTACK].length)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool GotHitAnimationEnds()
        {
            //if (enemyObject.animation[AnimationName.ENEMY_GOTHIT].time >= enemyObject.animation[AnimationName.ENEMY_GOTHIT].clip.length)

            if (Time.time - gotHitTime >= animation[AnimationName.ENEMY_GOTHIT].clip.length)
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

            animation[animationName].wrapMode = wrapMode;

            if (!animation.IsPlaying(AnimationName.ENEMY_GOTHIT))
            {

                if ((wrapMode == WrapMode.Loop) || (!animation.IsPlaying(animationName) && animationName != AnimationName.ENEMY_GOTHIT))
                {
                    animation.CrossFade(animationName);
                }
                else
                {
                    animation.Stop();
                    animation.Play(animationName);

                }
            }
        }

        public void SetInGrave(bool inGrave)
        {
            if (inGrave)
            {
                SetState(GRAVEBORN_STATE);
                enemyTransform.Translate(Vector3.down * 2.0f);
                enemyObject.layer = PhysicsLayer.SCENE_OBJECT;
                GameObject.DestroyImmediate(enemyObject.GetComponent<Rigidbody>());
                enemyObject.AddComponent<Rigidbody>();
                enemyTransform.GetComponent<Rigidbody>().freezeRotation = true;
                enemyTransform.GetComponent<Rigidbody>().useGravity = false;
                //enemyTransform.rigidbody.isKinematic = true;
            }
            else
            {
                enemyObject.layer = PhysicsLayer.ENEMY;
                enemyTransform.GetComponent<Rigidbody>().useGravity = true;
                //enemyTransform.rigidbody.isKinematic = false;
            }
        }

        public bool MoveFromGrave(float deltaTime)
        {
            enemyTransform.Translate(Vector3.up * deltaTime * 2.0f);
            if (enemyTransform.position.y >= Constant.FLOORHEIGHT)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        // Use this for initialization
        public virtual void Init(GameObject gObject)
        {
            gameScene = GameApp.GetInstance().GetGameScene();
            player = gameScene.GetPlayer();
            enemyObject = gObject;
            enemyTransform = enemyObject.transform;
            animation = enemyObject.GetComponent<UnityEngine.Animation>();
            aimedTransform = enemyTransform.Find(BoneName.ENEMY_AIMED_PATH);
            rigidbody = enemyObject.GetComponent<Rigidbody>();
            //collider = enemyObject.transform.Find(BoneName.ENEMY_BODY).collider;
            collider = enemyObject.transform.GetComponent<Collider>();
            //sceneGUI = GameObject.Find("SceneGUI").GetComponent<SceneGUI>();
            rConfig = GameApp.GetInstance().GetResourceConfig();


            gConfig = GameApp.GetInstance().GetGameConfig();
            controller = enemyObject.GetComponent<Collider>() as CharacterController;
            detectionRange = 150.0f;
            attackRange = 1.5f;
            minRange = 1.5f;
            lootCash = 0;
            criticalAttacked = false;
            spawnCenter = enemyTransform.position;

            target = GameObject.Find("Player").transform;

            //rigidbody.centerOfMass = Vector3.up;

            audio = new AudioPlayer();
            Transform audioFolderTrans = enemyTransform.Find("Audio");
            audio.AddAudio(audioFolderTrans, "Attack");
            audio.AddAudio(audioFolderTrans, "Walk");
            audio.AddAudio(audioFolderTrans, "Dead");
            audio.AddAudio(audioFolderTrans, "Special");
            audio.AddAudio(audioFolderTrans, "Shout");

            hitBloodObjectPool = GameApp.GetInstance().GetGameScene().HitBloodObjectPool;

            pathFinding = new GraphPathFinding();

            animation.wrapMode = WrapMode.Loop;
            animation.Play(AnimationName.ENEMY_IDLE);

            state = IDLE_STATE;

            lastUpdateTime = Time.time;
            lastPathFindingTime = Time.time;

            idleStartTime = -2;
            enemyObject.GetComponent<UnityEngine.Animation>()[AnimationName.ENEMY_ATTACK].wrapMode = WrapMode.ClampForever;

            path = GameApp.GetInstance().GetGameScene().GetPath();

        }

        public void SetState(EnemyState newState)
        {
            //Debug.Log(newState);
            state = newState;
        }

        public EnemyState GetState()
        {
            return state;
        }

        public virtual void CheckHit()
        {
        }

        public Transform GetAimedTransform()
        {
            return aimedTransform;
        }


        public Vector3 GetPosition()
        {
            return enemyTransform.position;
        }

        public Collider GetCollider()
        {
            return collider;
        }

        public float SqrDistanceFromPlayer
        {
            get
            {
                return (player.GetTransform().position - enemyTransform.position).sqrMagnitude;
            }
        }

        //enemy on hit
        public virtual void OnHit(DamageProperty dp, WeaponType weaponType, bool criticalAttack)
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
                Object.Instantiate(rConfig.hitBlood, enemyTransform.position+ Vector3.up*1f, Quaternion.identity);
                gotHitTime = Time.time;
                hp -= dp.damage;
                this.criticalAttacked = criticalAttack;
                state.OnHit(this);
            }
        }



        public virtual void OnAttack()
        {
            audio.PlayAudio(AudioName.ATTACK);
        }

        public virtual void PlayDeadEffects()
        {
            if (enemyObject)
            {
                if (enemyObject.active)
                {
                    PlayBloodEffect();
                    PlayBodyExlodeEffect();

                    GameObject enemyDeadHeadPrefab = rConfig.deadhead[(int)EnemyType];
                    GameObject deadhead = (GameObject)GameObject.Instantiate(enemyDeadHeadPrefab, enemyTransform.position + new Vector3(0, 2, 0), enemyTransform.rotation);

                    deadhead.GetComponent<Rigidbody>().AddForce(Random.Range(-5, 5), Random.Range(-5, 0), Random.Range(-5, 5), ForceMode.Impulse);

                    gameScene.GetEnemies().Remove(enemyObject.name);
                    enemyObject.SetActiveRecursively(false);
                }
                //GameObject.Destroy(enemyObject);

            }

        }

        public virtual void PlayBloodEffect()
        {
            if (enemyObject)
            {
                if (enemyObject.active)
                {
                    GameObject enemyDeadBloodPrefab = rConfig.deadBlood;
                    GameObject enemyDeadFloorBloodPrefab;
                    int r = Random.Range(0, 100);
                    float fby = Constant.FLOORHEIGHT + 0.02f;
                    if (r > 50)
                    {
                        enemyDeadFloorBloodPrefab = rConfig.deadFoorblood;
                    }
                    else
                    {
                        enemyDeadFloorBloodPrefab = rConfig.deadFoorblood2;
                        fby = Constant.FLOORHEIGHT + 0.01f;
                    }

                    GameObject.Instantiate(enemyDeadBloodPrefab, enemyTransform.position + new Vector3(0, 0.5f, 0), Quaternion.Euler(0, 0, 0));
                    
                    GameObject floorBlood = GameObject.Instantiate(enemyDeadFloorBloodPrefab, new Vector3(enemyTransform.position.x, fby, enemyTransform.position.z), Quaternion.Euler(270, 0, 0)) as GameObject;
                    floorBlood.transform.rotation = deadRotation * floorBlood.transform.rotation;

                    floorBlood.transform.position = deadPosition;


                }
            }
        }

        public void PlayBloodExplodeEffect(Vector3 pos)
        {
            GameObject enemyDeadBloodPrefab = rConfig.deadBlood;
            GameObject.Instantiate(enemyDeadBloodPrefab, pos, Quaternion.Euler(0, 0, 0));
        }


        public virtual void PlayBodyExlodeEffect()
        {
            if (enemyObject)
            {
                if (enemyObject.active)
                {
                    Quaternion rotation = Quaternion.Euler(enemyTransform.rotation.eulerAngles.x, Random.Range(0, 360), enemyTransform.rotation.eulerAngles.z);
                    //GameObject enemyDeadBodyPrefab = rConfig.deadbody[(int)EnemyType];
                    //GameObject.Instantiate(enemyDeadBodyPrefab, enemyTransform.position + new Vector3(0, 0.2f, 0), rotation);
                    ObjectPool bodyPool = GameApp.GetInstance().GetGameScene().GetDeadBodyPool(enemyType);
                    GameObject body = bodyPool.CreateObject(enemyTransform.position + new Vector3(0, 0.2f, 0), rotation);
                    float fby = Constant.FLOORHEIGHT + 0.02f;
                    body.transform.rotation = deadRotation * body.transform.rotation;

                }
            }
        }

        public virtual void FindPath()
        {

            Vector3 tpoint = target.position;                     //! Player 的坐标
            if (Time.time - lastPathFindingTime > (0.5f/runSpeed))
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

                ray = new Ray(enemyTransform.position + new Vector3(0, 0.5f, 0), target.position+new Vector3(0, 0.5f, 0) - (enemyTransform.position + new Vector3(0, 0.5f, 0)));  //! 敌人指向Player的一条射线
                //if (Physics.CapsuleCast(enemyTransform.position, enemyTransform.position + Vector3.up, 1, target.position + new Vector3(0, 0.5f, 0) - (enemyTransform.position + new Vector3(0, 0.5f, 0)), out rayhit, 100, 1 << PhysicsLayer.WALL | 1 << PhysicsLayer.TRANSPARENT_WALL | 1 << PhysicsLayer.SCENE_OBJECT | 1 << PhysicsLayer.PLAYER | 1 << PhysicsLayer.FLOOR))
                    
                if (Physics.Raycast(ray, out rayhit, 100, 1 << PhysicsLayer.WALL | 1 << PhysicsLayer.TRANSPARENT_WALL | 1 << PhysicsLayer.SCENE_OBJECT | 1 << PhysicsLayer.PLAYER|1<<PhysicsLayer.FLOOR))
                {
					//! 检测到Player
                    if (rayhit.collider.gameObject.name == "Player" && Mathf.Abs(enemyTransform.position.y - player.GetTransform().position.y) < 0.5f)
                    {
                        lastTarget = tpoint;
                        pathFinding.ClearPath();
                        //Debug.Log("See Player");
                    }
                    else
                    {
                        /*
                        if (!pathFinding.HavePath())
                        {
                            Transform t = pathFinding.GetNextWayPoint(enemyTransform.position, player.GetTransform().position);
                            if (t != null)
                            {
                                lastTarget = t.position;
                                //Debug.Log("New Path, Next:" + t.name + " " + lastTarget);
                            }
                        }
                        else*/
                        {
                            if (Time.time - lastReCheckPathTime > 10.0f)
                            {

                                ray = new Ray(enemyTransform.position + new Vector3(0, 0.5f, 0), lastTarget - (enemyTransform.position + new Vector3(0, 0.5f, 0)));
                                if (Physics.Raycast(ray, out rayhit, 100, 1 << PhysicsLayer.WALL | 1 << PhysicsLayer.TRANSPARENT_WALL))
                                {
                                    pathFinding.ClearPath();
                                    Transform t = pathFinding.GetNextWayPoint(enemyTransform.position, player.GetTransform().position);
                                    if (t != null)
                                    {
                                        lastTarget = t.position;
                                        //Debug.Log("New Path, Next:" + t.name + " " + lastTarget);
                                    }
                                }
                                lastReCheckPathTime = Time.time;
                            }

                        }
                        
                    }
                }

                //Debug.Log((enemyTransform.position - lastTarget).sqrMagnitude);
                //already on the path, reached on point, then compare previous with next point
                if ((enemyTransform.position - lastTarget).sqrMagnitude < 1 * 1)
                {
                    pathFinding.PopNode();
                    Transform t = pathFinding.GetNextWayPoint(enemyTransform.position, player.GetTransform().position);
                    if (t != null)
                    {
                        lastTarget = t.position;
                        //Debug.Log("Reach Point, Next:" + t.name + " "+lastTarget);
                    }

                }

                //100.Move towards lastTarget.
                /*
                if (lastTarget.y < Constant.FLOORHEIGHT + 2.0f)
                {
                    lastTarget.y = enemyTransform.position.y;
                }
                */
                enemyTransform.LookAt(new Vector3(lastTarget.x, enemyTransform.position.y, lastTarget.z));
                dir = (lastTarget - enemyTransform.position).normalized;
            }

        }


       


        public virtual void Patrol(float deltaTime)
        {

            /*
            Random.seed = (int)Time.time + (int)(enemyTransform.position.x * enemyTransform.position.z);
            idlePeriod = Random.Range(0.5f, 2f);

            if (status == Status.Idle && Time.time - idleStartTime > idlePeriod)
            {
                status = Status.Patrol;
                //Debug.Log("patrol start");
                if ((enemyTransform.position - spawnCenter).sqrMagnitude < 1)
                {

                    float x = Random.Range(-3, 3);
                    float z = Random.Range(-3, 3);
                    if (Mathf.Abs(x) < 2f)
                    {
                        x += Mathf.Sign(x) * 2f;
                    }
                    if (Mathf.Abs(z) < 2)
                    {
                        z += Mathf.Sign(z) * 2f;
                    }
                    patrolTarget = new Vector3(x, 0, z);
                    patrolTarget = patrolTarget + spawnCenter;

                }
                else
                {
                    patrolTarget = spawnCenter;
                }
            }
            else if (status == Status.Patrol)
            {
                animation.CrossFade(AnimationName.ENEMY_PATROL);
                animation[AnimationName.ENEMY_PATROL].speed = 0.5f;
                enemyTransform.LookAt(new Vector3(patrolTarget.x, enemyTransform.position.y, patrolTarget.z));
                if (moveWithCharacterController)
                {
                    controller.Move((enemyTransform.TransformDirection(Vector3.forward) + Physics.gravity * deltaTime) * runSpeed * 0.2f * deltaTime);
                }
                else
                {
                    enemyTransform.Translate(Vector3.forward * runSpeed * 0.2f * deltaTime);

                }
                if ((enemyTransform.position - patrolTarget).sqrMagnitude < 1 || Time.time - idleStartTime > 9.0f)
                {
                    status = Status.Idle;
                    idleStartTime = Time.time;
                }
            }
            */
        }

        public void RemoveDeadBodyTimer()
        {
            if (Time.time - deadTime > 3.0f)
            {
                gameScene.GetEnemies().Remove(enemyObject.name);
                //GameObject.Destroy(enemyObject);
                enemyObject.SetActiveRecursively(false);
            }

        }

        public virtual void OnDead()
        {

            deadTime = Time.time;
            if (spawn != null)
            {
                spawn.gameObject.SendMessage("OnResetSpawnTrigger", SendMessageOptions.DontRequireReceiver);
            }

            gameScene.IncreaseKills();
            gameScene.ModifyEnemyNum(-1);

            GameApp.GetInstance().GetGameState().Achievement.KillEnemy();

            GameApp.GetInstance().GetGameState().AddCash((int)(lootCash * gameScene.GetDifficultyCashDropFactor));
            enemyObject.SendMessage("OnLoot");


            if (IsElite && enemyType != global::EnemyType.E_BOOMER)
            {
                criticalAttacked = false;
            }

            deadRotation = Quaternion.identity;
            deadPosition = enemyTransform.position;
            deadPosition.y = Constant.FLOORHEIGHT + 0.02f;
            if (enemyTransform.position.y > Constant.FLOORHEIGHT + 0.5f)
            {
                RaycastHit hitfloor;
                Ray ray = new Ray(enemyTransform.position + Vector3.up * 0.5f, -Vector3.up);
                if (Physics.Raycast(ray, out hitfloor, 50.0f, 1 << PhysicsLayer.FLOOR))
                {
                    deadRotation = Quaternion.FromToRotation(Vector3.up, hitfloor.normal);
                    deadPosition =  hitfloor.point + Vector3.up * 0.01f;
                }
            }




            if (criticalAttacked)
            {
                PlayDeadEffects();
            }
            else
            {
                if (animation)
                {
                    animation[AnimationName.ENEMY_DEATH1].wrapMode = WrapMode.ClampForever;
                    animation[AnimationName.ENEMY_DEATH1].speed = 1f;
                    animation.CrossFade(AnimationName.ENEMY_DEATH1);
                }
                
                if (enemyObject)
                {
                    if (enemyObject.active)
                    {
                        enemyTransform.rotation = deadRotation * enemyTransform.rotation;
                        enemyObject.layer = PhysicsLayer.DEADBODY;
                    }
                }

                PlayBloodEffect();

            }

        }

        public virtual bool OnSpecialState(float deltaTime)
        {

            return false;
        }

        public virtual EnemyState EnterSpecialState(float deltaTime)
        {
            return null;
        }

        public virtual void DoMove(float deltaTime)
        {

            enemyTransform.Translate(dir * runSpeed * deltaTime, Space.World);

        }


        public float GetSqrDistanceFromPlayer()
        {
            return (enemyTransform.position - player.GetTransform().position).sqrMagnitude;
        }

        public virtual void DoLogic(float deltaTime)
        {

            float dis = (enemyTransform.position - player.GetTransform().position).sqrMagnitude;


            state.NextState(this, deltaTime, player);

            RemoveExceptionPositionEnemy();

        }


        protected void RemoveExceptionPositionEnemy()
        {
            if (enemyTransform.position.y < Constant.FLOORHEIGHT - 20.0f)
            {
                DamageProperty dp = new DamageProperty();
                dp.damage = HP;
                OnHit(dp, WeaponType.NoGun, false);
                Debug.Log("~~~~~~~~~~~~~~~~Remove Kill~~~~~~~~~~~~~~~"+ Name);
            }
        }

    }
}
