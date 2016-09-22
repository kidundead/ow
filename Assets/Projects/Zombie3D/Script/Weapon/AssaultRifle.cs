using UnityEngine;
using System.Collections;

namespace Zombie3D
{
    public class AssaultRifle : Weapon
    {

        protected GameObject fireTrail;

        protected ObjectPool bulletsObjectPool;
        protected ObjectPool firelineObjectPool;

        protected ObjectPool sparksObjectPool;

        protected static int sbulletCount;
        protected float lastPlayAudioTime;
        public override WeaponType GetWeaponType()
        {
            return WeaponType.AssaultRifle;
        }

        public override int BulletCount
        {
            get
            {
                return AssaultRifle.sbulletCount;
            }
            set
            {
                sbulletCount = value;
            }
        }


        public AssaultRifle()
        {
            maxCapacity = 9999;
            IsSelectedForBattle = false;
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

        public override void Init()
        {
            base.Init();

            hitForce = 20f;
            fireTrail = GameObject.Find("muzzleFlash");


            gunfire = gun.transform.Find("gun_fire_new").gameObject;

            bulletsObjectPool = new ObjectPool();
            firelineObjectPool = new ObjectPool();

            sparksObjectPool = new ObjectPool();



            bulletsObjectPool.Init("Bullets", rConf.bullets, 6, 1);
            firelineObjectPool.Init("Firelines", rConf.fireline, 4, 0.5f);

            sparksObjectPool.Init("Sparks", rConf.hitparticles, 3, 0.22f);



        }

        public override void changeReticle()
        {

        }

        public override void CreateGun()
        {
            switch (Name)
            {
                case GunName.M4:
                    gun = (GameObject)GameObject.Instantiate(rConf.m4, player.GetTransform().position, player.GetTransform().rotation);
                    break;
                case GunName.AK47:
                    gun = (GameObject)GameObject.Instantiate(rConf.ak47, player.GetTransform().position, player.GetTransform().rotation);
                    break;
                case GunName.MP5:
                    gun = (GameObject)GameObject.Instantiate(rConf.mp5, player.GetTransform().position, player.GetTransform().rotation);
                    break;
                case GunName.AUG:
                    gun = (GameObject)GameObject.Instantiate(rConf.aug, player.GetTransform().position, player.GetTransform().rotation);
                    break;
                case GunName.P90:
                    gun = (GameObject)GameObject.Instantiate(rConf.p90, player.GetTransform().position, player.GetTransform().rotation);
                    break;

            }
        }

        public override void DoLogic()
        {

            bulletsObjectPool.AutoDestruct();
            firelineObjectPool.AutoDestruct();

            sparksObjectPool.AutoDestruct();
        }

        public override void Fire(float deltaTime)
        {

            gunfire.GetComponent<Renderer>().enabled = true;


            Ray ray = new Ray();
            if (gameCamera.GetCameraType() == CameraType.TPSCamera)
            {
                Vector3 tempTargetPoint = cameraComponent.ScreenToWorldPoint(new Vector3(gameCamera.ReticlePosition.x, Screen.height - gameCamera.ReticlePosition.y, 0.1f));

                ray = new Ray(cameraTransform.position, (tempTargetPoint - cameraTransform.position));

            }
            else if (gameCamera.GetCameraType() == CameraType.TopWatchingCamera)
            {

                ray = new Ray(player.GetTransform().position + Vector3.up * 0.5f, player.GetTransform().TransformDirection(Vector3.forward));
            }


            RaycastHit hit = new RaycastHit();
            RaycastHit[] hits;
            float tan = Mathf.Tan(Mathf.Deg2Rad *60.0f);
            hits = Physics.RaycastAll(ray, 1000, 1 << PhysicsLayer.ENEMY | 1 << PhysicsLayer.WALL | 1 << PhysicsLayer.FLOOR | 1 << PhysicsLayer.WOODBOX);

           
            float minDistance = 1000000f;
            for (int i = 0; i<hits.Length;i++ )
            {
                Vector3 relativePos = Vector3.zero;
                if (hits[i].collider.gameObject.layer == PhysicsLayer.ENEMY)
                {
                    relativePos = gunfire.transform.InverseTransformPoint(hits[i].collider.transform.position);
                }
                else
                {
                    relativePos = gunfire.transform.InverseTransformPoint(hits[i].point);
                }

                if (relativePos.z < 1)// || Mathf.Abs(relativePos.z / relativePos.x) > tan
                {
                    float dis = (hits[i].point - gunfire.transform.position).sqrMagnitude;
                    if (dis < minDistance)
                    {
                       
                        hit = hits[i];
                        minDistance = dis;
                    }
                   
                }
            }


            if (hit.collider != null)
            //if (Physics.Raycast(ray, out hit, 1000, 1 << PhysicsLayer.ENEMY | 1 << PhysicsLayer.WALL | 1 << PhysicsLayer.FLOOR | 1 << PhysicsLayer.WOODBOX))
            {

                //Debug.Log(hit.collider.name);

                aimTarget = hit.point;
                Vector3 dir = (aimTarget - rightGun.position).normalized;

                Vector3 relativeDir = player.GetTransform().InverseTransformPoint(aimTarget);
                if (relativeDir.z > Constant.SPARK_MIN_DISTANCE)
                {

                    GameObject fireLineObj = firelineObjectPool.CreateObject(gunfire.transform.position + dir * 2, dir);
                    fireLineObj.transform.Rotate(180,0,0);
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
                        Debug.Log(hitObject.name);
                        WoodBoxScript ws = hitObject.GetComponent<WoodBoxScript>();
                        ws.OnHit(damage * player.PowerBuff);
                    }

                }

                if (Time.time - lastPlayAudioTime >= shootAudio.clip.length)
                {
                    lastPlayAudioTime = Time.time;
                    AudioPlayer.PlayAudio(shootAudio);
                }
                sbulletCount--;
                sbulletCount = Mathf.Clamp(sbulletCount, 0, maxCapacity);

                player.LastHitPosition = hit.point;

                lastShootTime = Time.time;

            }
            else
            {
                aimTarget = cameraTransform.TransformPoint(0, 0, 1000);
                Vector3 dir = (aimTarget - rightGun.position).normalized;

                bulletsObjectPool.CreateObject(rightGun.position, dir);



                GameObject fireLineObj = firelineObjectPool.CreateObject(gunfire.transform.position + dir * 2, dir);
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



                if (!shootAudio.isPlaying)
                {
                    AudioPlayer.PlayAudio(shootAudio);
                }
                sbulletCount--;
                sbulletCount = Mathf.Clamp(sbulletCount, 0, maxCapacity);

                player.LastHitPosition = hit.point;

                lastShootTime = Time.time;

            }



        }

        public override void StopFire()
        {
            if (shootAudio != null)
            {
                shootAudio.Stop();
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