using UnityEngine;
using System.Collections;
using Zombie3D;
[AddComponentMenu("TPS/ProjectileScript")]
public class ProjectileScript : MonoBehaviour
{

    //U3D Object
    protected GameObject resObject;
    protected GameObject explodeObject;
    protected GameObject smallExplodeObject;
    protected GameObject laserHitObject;
    public Transform targetTransform;
    protected Transform proTransform;
    protected WeaponType gunType;
    protected ResourceConfigScript rConf;

    public Vector3 dir;
    public float hitForce;
    public float explodeRadius = 0.0f;
    public float flySpeed;
    public Vector3 speed;
    public float life = 2.0f;
    public float damage;
    protected float createdTime;
    protected float lastTriggerTime;
    protected float gravity = 16.0f;
    protected float downSpeed = 0f;
    protected float deltaTime = 0f;
    protected Vector3 targetPos;
    protected Vector3 lastPos;
    protected float initAngel = 40.0f;
    protected float lastCheckPosTime;
    // Use this for initialization
    public void Start()
    {
        rConf = GameApp.GetInstance().GetResourceConfig();
        resObject = rConf.projectile;
        smallExplodeObject = rConf.salivaExplosion;
        explodeObject = rConf.rocketExlposion;
        proTransform = transform;
        laserHitObject = rConf.laserHit;
        createdTime = Time.time;
        lastCheckPosTime = Time.time;
        lastPos = proTransform.position;
        //dir = (targetTransform.position - proTransform.position).normalized;

        //dir = Vector3.Cross((targetTransform.position - proTransform.position), Vector3.up).normalized;
        //proTransform.LookAt(targetTransform.position);
    }

    // Update is called once per frame
    public void Update()
    {

        deltaTime += Time.deltaTime;

        if (deltaTime < 0.03f)
        {
            return;
        }

        if (gunType == WeaponType.Sniper)
        {
            if (targetTransform != null)
            {
                if (targetTransform.gameObject.active)
                {
                    dir = (targetTransform.position - proTransform.position).normalized;
                    targetPos = targetTransform.position;
                    //dir = Vector3.Cross((targetTransform.position - proTransform.position), Vector3.up).normalized;
                    //if(targetTransform.
                    
                }
                else
                {
                    targetTransform = null;
                }
                


            }
            else
            {
                
            }
            proTransform.LookAt(targetPos);
            initAngel -= deltaTime * 80.0f;
            if (initAngel <= 0)
            {
                initAngel = 0;
            }
            proTransform.rotation = Quaternion.AngleAxis(initAngel, -1 * proTransform.right) * proTransform.rotation;
            dir = proTransform.forward;
            if (Time.time - lastCheckPosTime > 0.3f)
            {
                lastCheckPosTime = Time.time;
                if ((proTransform.position - lastPos).sqrMagnitude < 2.0f)
                {

                    Object.DestroyObject(gameObject);
                    return;
                }
                lastPos = proTransform.position;

            }
        }



        if (gunType == WeaponType.GrenadeRifle || gunType == WeaponType.NurseSaliva)
        {
            speed += Physics.gravity.y * Vector3.up * deltaTime;
            proTransform.Translate(speed * deltaTime, Space.World);
            proTransform.LookAt(proTransform.position + speed * 10f);

        }
        else
        {
            proTransform.Translate(flySpeed * dir * deltaTime, Space.World);

            if (gunType == WeaponType.RocketLauncher )
            {
                proTransform.Rotate(Vector3.forward, deltaTime * 900, Space.Self);
            }
        }

        if (Time.time - createdTime > life)
        {
            Object.DestroyObject(gameObject);
        }

        deltaTime = 0f;

    }


    void OnTriggerStay(Collider other)
    {
       
        if (gunType == WeaponType.LaserGun)
        {
          
            
            GameScene gameScene = GameApp.GetInstance().GetGameScene();
            Player player = gameScene.GetPlayer();
            Weapon w = player.GetWeapon();
            if (w.GetWeaponType() == WeaponType.LaserGun)
            {

                LaserGun lg = w as LaserGun;
                if (lg.CouldMakeNextShoot())
                {
                    
                    if (other.gameObject.layer == PhysicsLayer.ENEMY)
                    {
                        
                        Enemy enemy = gameScene.GetEnemyByID(other.gameObject.name);
                        if (enemy != null)
                        {
                            if (enemy.GetState() != Enemy.DEAD_STATE)
                            {
                                Object.Instantiate(laserHitObject, enemy.GetPosition(), Quaternion.identity);
                                DamageProperty dp = new DamageProperty();
                                dp.hitForce = (dir + new Vector3(0f, 0.18f, 0f)) * hitForce;
                                dp.damage = damage;
                                enemy.OnHit(dp, gunType, false);
                            }
                        }
                    }
                    /*
                    else if (other.gameObject.layer == PhysicsLayer.WOODBOX)
                    {
                        WoodBoxScript ws = other.gameObject.GetComponent<WoodBoxScript>();
                        ws.OnHit(damage);
                    }*/
                }
            }
            
        }
    }


    void OnTriggerEnter(Collider other)
    {

        GameScene gameScene = GameApp.GetInstance().GetGameScene();
        Player player = gameScene.GetPlayer();

        /*
        if (gunType == WeaponType.Sniper)
        {

            Object.Instantiate(rConf.rocketExlposion, proTransform.position, Quaternion.identity);
            Object.DestroyObject(gameObject);


            if (other.gameObject.layer == PhysicsLayer.ENEMY)
            {
                Enemy enemy = gameScene.GetEnemyByID(other.gameObject.name);
                DamageProperty dp = new DamageProperty();
                //dp.hitForce = (dir + new Vector3(0f, 0.18f, 0f)) * hitForce;
                dp.damage = damage;
                enemy.OnHit(dp, gunType, true);
            }


        }
        */

        if (gunType == WeaponType.RocketLauncher || gunType == WeaponType.GrenadeRifle || gunType == WeaponType.Sniper)
        {
            GameObject.Instantiate(rConf.rpgFloor, new Vector3(proTransform.position.x, Constant.FLOORHEIGHT + 0.1f, proTransform.position.z), Quaternion.Euler(270, 0, 0));
            Object.Instantiate(explodeObject, proTransform.position, Quaternion.identity);
            Object.DestroyObject(gameObject);

            player.LastHitPosition = proTransform.position;

            int oneShotKills = 0;

            foreach (Enemy enemy in gameScene.GetEnemies().Values)
            {
                if (enemy.GetState() == Enemy.DEAD_STATE)
                {
                    continue;
                }

                float dis = (enemy.GetPosition() - proTransform.position).sqrMagnitude;
                float radiusSqr = explodeRadius * explodeRadius;
                if (dis < radiusSqr)
                {
                    DamageProperty dp = new DamageProperty();
                    dp.hitForce = (dir + new Vector3(0f, 0.18f, 0f)) * hitForce;

                    if (dis * 4 < radiusSqr)
                    {
                        dp.damage = damage * player.PowerBuff;
                        enemy.OnHit(dp, gunType, true);
                    }
                    else
                    {
                        dp.damage = damage / 2 * player.PowerBuff;
                        enemy.OnHit(dp, gunType, false);
                    }


                }

                if (enemy.HP <= 0)
                {
                    oneShotKills++;
                }
            }

            if (oneShotKills >= 4)
            {
                GameApp.GetInstance().GetGameState().Achievement.CheckAchievemnet_WeaponMaster();
            }

            GameObject[] woodboxes = gameScene.GetWoodBoxes();
            foreach (GameObject woodbox in woodboxes)
            {
                if (woodbox != null)
                {

                    float dis = (woodbox.transform.position - proTransform.position).sqrMagnitude;
                    float radiusSqr = explodeRadius * explodeRadius;
                    if ((dis < radiusSqr))
                    {
                        WoodBoxScript ws = woodbox.GetComponent<WoodBoxScript>();
                        ws.OnHit(damage * player.PowerBuff);
                    }
                }

            }
            return;

        }
        else if (gunType == WeaponType.LaserGun)
        {
            if (other.gameObject.layer == PhysicsLayer.ENEMY)
            {

                Enemy enemy = gameScene.GetEnemyByID(other.gameObject.name);
                if (enemy != null)
                {
                    if (enemy.GetState() != Enemy.DEAD_STATE)
                    {

                        //Object.Instantiate(laserHitObject, enemy.GetPosition(), Quaternion.identity);
                        DamageProperty dp = new DamageProperty();
                        dp.hitForce = (dir + new Vector3(0f, 0.18f, 0f)) * hitForce;
                        dp.damage = damage;
                        enemy.OnHit(dp, gunType, false);
                    }
                }
            }
            else if (other.gameObject.layer == PhysicsLayer.WOODBOX)
            {
                //Debug.Log("hit wood");
                WoodBoxScript ws = other.gameObject.GetComponent<WoodBoxScript>();
                ws.OnHit(damage);
            }
            //Object.Instantiate(laserHitObject, proTransform.position + dir * 2, Quaternion.identity);
        }
        else if (gunType == WeaponType.NurseSaliva)
        {
            if (other.gameObject.layer != PhysicsLayer.ENEMY)
            {
                Ray ray = new Ray(proTransform.position+Vector3.up*1.0f, Vector3.down);
                RaycastHit hit;
                float floorY = Constant.FLOORHEIGHT;
                if (Physics.Raycast(ray, out hit, 100, 1 << PhysicsLayer.FLOOR))
                {
                    floorY = hit.point.y;
                }

                Object.Instantiate(smallExplodeObject, new Vector3(proTransform.position.x, floorY + 0.1f, proTransform.position.z), Quaternion.identity);
                GameObject salivaObj = Object.Instantiate(rConf.nurseSaliva, new Vector3(proTransform.position.x, floorY + 0.1f, proTransform.position.z), Quaternion.Euler(0, 0, 0)) as GameObject;
                salivaObj.transform.Rotate(270, 0, 0);
                Object.DestroyObject(gameObject);
                if ((player.GetTransform().position - proTransform.position).sqrMagnitude < explodeRadius * explodeRadius)
                {
                    player.OnHit(damage);
                }

            }

        }

    }


    public WeaponType GunType
    {
        set
        {
            gunType = value;
        }
    }

}



