using UnityEngine;
using System.Collections;

namespace Zombie3D
{
    public class MachineGun : Weapon
    {

        protected ObjectPool bulletsObjectPool;
        protected ObjectPool firelineObjectPool;

        protected ObjectPool sparksObjectPool;
        protected static int sbulletCount;
        public override WeaponType GetWeaponType()
        {
            return WeaponType.MachineGun;
        }

        public override void Init()
        {
            base.Init();
            maxCapacity = 9999;
            attackFrenquency = 0.25f;
            hitForce = 20f;
            maxDeflection = 4f;

            //fireTrail = GameObject.Find("muzzleFlash");


            gunfire = gun.transform.Find("gun_fire_new").gameObject;
            bulletsObjectPool = new ObjectPool();
            firelineObjectPool = new ObjectPool();

            sparksObjectPool = new ObjectPool();

            bulletsObjectPool.Init("Bullets", rConf.bullets, 6, 1);
            firelineObjectPool.Init("Firelines", rConf.fireline, 20, 0.5f);

            sparksObjectPool.Init("Sparks", rConf.hitparticles, 3, 0.22f);

        }

        public override void changeReticle()
        {
        }

        public override void CreateGun()
        {
            gun = (GameObject)GameObject.Instantiate(rConf.gatlin, player.GetTransform().position, player.GetTransform().rotation);

        }

        public override int BulletCount
        {
            get
            {
                return MachineGun.sbulletCount;
            }
            set
            {
                sbulletCount = value;
            }
        }
        public override void LoadConfig()
        {

            base.LoadConfig();

            WeaponConfig wConf = gConfig.GetWeaponConfig(Name);
            damage = wConf.damageConf.baseData;
            attackFrenquency = wConf.attackRateConf.baseData;
            accuracy = wConf.accuracyConf.baseData;
            speedDrag = wConf.moveSpeedDrag;
            range = wConf.range;
            maxGunLoad = wConf.initBullet;
            sbulletCount = maxGunLoad;

        }

        public override void FireUpdate(float deltaTime)
        {
            deflection.x += Random.Range(-0.5f, 0.5f);
            deflection.y += Random.Range(-0.5f, 0.5f);
            deflection.x = Mathf.Clamp(deflection.x, -maxDeflection, maxDeflection);
            deflection.y = Mathf.Clamp(deflection.y, -maxDeflection, maxDeflection);
        }


        public override void Fire(float deltaTime)
        {

            gunfire.GetComponent<Renderer>().enabled = true;


            Ray ray = new Ray();
            if (gameCamera.GetCameraType() == CameraType.TPSCamera)
            {
                Vector3 tempTargetPoint = cameraComponent.ScreenToWorldPoint(new Vector3(gameCamera.ReticlePosition.x, Screen.height - gameCamera.ReticlePosition.y, 50));

                ray = new Ray(cameraTransform.position, (tempTargetPoint - cameraTransform.position));
            }
            else if (gameCamera.GetCameraType() == CameraType.TopWatchingCamera)
            {
                ray = new Ray(player.GetTransform().position + Vector3.up * 0.5f, player.GetTransform().TransformDirection(Vector3.forward));
            }


            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000, 1 << PhysicsLayer.ENEMY | 1 << PhysicsLayer.WALL | 1 << PhysicsLayer.FLOOR | 1 << PhysicsLayer.WOODBOX))
            {
                aimTarget = hit.point;

                Vector3 dir = Vector3.zero;

                Vector3 relativeDir = player.GetTransform().InverseTransformPoint(aimTarget);
                if (relativeDir.z > Constant.SPARK_MIN_DISTANCE)
                {


                    //float disSqr = (aimTarget - gunfire.transform.position).sqrMagnitude;
                    for (int i = -2; i <= 2; i++)
                    {

                        Vector3 startPoint = Vector3.zero;
                        if (i % 2 == 0)
                        {
                            startPoint = gunfire.transform.TransformPoint(Vector3.left * i * 1.5f);
                        }
                        else
                        {
                            startPoint = gunfire.transform.TransformPoint(Vector3.up * i * 1.5f);
                        }

                        dir = (aimTarget - gunfire.transform.position).normalized;


                        GameObject fireLineObj = firelineObjectPool.CreateObject(startPoint + dir * (Mathf.Abs(i) + 2), dir);
                        fireLineObj.transform.Rotate(180,0,0);
                        //fireLineObj.transform.Rotate(new Vector3(0, 0, 0), Space.Self);
                        if (fireLineObj == null)
                        {
                            Debug.Log("fire line obj null");
                        }
                        else
                        {
                            FireLineScript f = fireLineObj.GetComponent<FireLineScript>();
                            f.transform.Rotate(90, 0, 0);
                            f.beginPos = rightGun.position;
                            f.endPos = hit.point;

                        }
                    }
                }

                bulletsObjectPool.CreateObject(rightGun.position, dir);

                GameObject hitObject = hit.collider.gameObject;

                if (hitObject.name.StartsWith(ConstData.ENEMY_NAME))
                {
                    Enemy enemy = gameScene.GetEnemyByID(hitObject.name);
                    if (enemy.GetState() != Enemy.DEAD_STATE)
                    {
                        if (relativeDir.z > Constant.SPARK_MIN_DISTANCE)
                        {
                            sparksObjectPool.CreateObject(hit.point, -ray.direction);

                        }
                        DamageProperty dp = new DamageProperty();
                        dp.hitForce = ray.direction * hitForce;
                        dp.damage = damage * player.PowerBuff;
                        bool criticalAttack = false;

                        int rnd = Random.Range(0, 100);
                        if (rnd < 70)
                        {
                            criticalAttack = true;
                        }

                        float dis = (enemy.GetPosition() - player.GetTransform().position).sqrMagnitude;
                        float radiusSqr = range * range;
                        if (dis < radiusSqr)
                        {
                            enemy.OnHit(dp, GetWeaponType(), criticalAttack);
                        }
                        else
                        {
                            if (rnd < accuracy)
                            {
                                enemy.OnHit(dp, GetWeaponType(), criticalAttack);
                            }
                        }


                    }


                }
                else
                {
                    if (relativeDir.z > Constant.SPARK_MIN_DISTANCE)
                    {
                        sparksObjectPool.CreateObject(hit.point, -ray.direction);
                    }
                    if (hitObject.layer == PhysicsLayer.WOODBOX)
                    {
                        WoodBoxScript ws = hitObject.GetComponent<WoodBoxScript>();
                        ws.OnHit(damage * player.PowerBuff);
                    }

                }

                if (shootAudio != null)
                {
                    if (!shootAudio.isPlaying)
                    {
                        AudioPlayer.PlayAudio(shootAudio);
                    }
                }
                sbulletCount--;
                sbulletCount = Mathf.Clamp(sbulletCount, 0, maxCapacity);


                player.LastHitPosition = hit.point;

                lastShootTime = Time.time;

            }
            else
            {
                aimTarget = cameraTransform.TransformPoint(0, 0, 1000);
            }


        }

        public override void DoLogic()
        {

            bulletsObjectPool.AutoDestruct();
            firelineObjectPool.AutoDestruct();

            sparksObjectPool.AutoDestruct();
        }


        public override void StopFire()
        {
            deflection = Vector2.zero;
            if (shootAudio != null)
            {
                //shootAudio.Stop();
            }
            if (gunfire != null)
            {
                gunfire.GetComponent<Renderer>().enabled = false;
            }
        }


        public override void GunOff()
        {

            base.GunOff();
        }


    }
}