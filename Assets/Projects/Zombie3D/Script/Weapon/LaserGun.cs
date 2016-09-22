using UnityEngine;
using System.Collections;

namespace Zombie3D
{
    public class LaserGun : Weapon
    {

        protected float flySpeed;
        protected static float sbulletCount;
        GameObject laserObj = null;
        protected Vector3 laserStartScale;
        protected float lastLaserHitInitiatTime;
        public override void Init()
        {

            base.Init();
            maxCapacity = 9999;

            gunfire = gun.transform.Find("gun_fire_new").gameObject;

        }

        public override int BulletCount
        {
            get
            {
                return (int)LaserGun.sbulletCount;
            }
            set
            {
                sbulletCount = value;
            }
        }


        public override void CreateGun()
        {

            gun = (GameObject)GameObject.Instantiate(rConf.lasergun, player.GetTransform().position, player.GetTransform().rotation);

        }

        public override void LoadConfig()
        {
            base.LoadConfig();
            WeaponConfig wConf = gConfig.GetWeaponConfig(Name);
            damage = wConf.damageConf.baseData;
            attackFrenquency = wConf.attackRateConf.baseData;
            speedDrag = wConf.moveSpeedDrag;
            range = wConf.range;
            accuracy = wConf.accuracyConf.baseData;
            maxGunLoad = wConf.initBullet;
            sbulletCount = maxGunLoad;
            flySpeed = wConf.flySpeed;
        }

        public void PlayShootAudio()
        {
            if (shootAudio != null)
            {
                //if (!shootAudio.isPlaying)
                {
                    AudioPlayer.PlayAudio(shootAudio);
                }
            }
        }

        public void SetShootTimeNow()
        {
            lastShootTime = Time.time;
        }

        public override void FireUpdate(float deltaTime)
        {
            if (laserObj != null)
            {
                sbulletCount -= (20f * deltaTime);
                sbulletCount = Mathf.Clamp(sbulletCount, 0, maxCapacity);

                Vector3 tempTargetPoint = cameraComponent.ScreenToWorldPoint(new Vector3(gameCamera.ReticlePosition.x, Screen.height - gameCamera.ReticlePosition.y, 50));
                Ray ray = new Ray(cameraTransform.position, (tempTargetPoint - cameraTransform.position));
                RaycastHit hit;
                //aimTarget = cameraTransform.TransformPoint(0, 0, 20);

                if (Physics.Raycast(ray, out hit, 1000, 1 << PhysicsLayer.WALL | 1 << PhysicsLayer.FLOOR))
                {
                    aimTarget = hit.point;

                }
                else
                {
                    aimTarget = cameraTransform.TransformPoint(0, 0, 1000);
                }

                Vector3 dir = (aimTarget - gunfire.transform.position).normalized;
                float dis = (aimTarget - gunfire.transform.position).magnitude;
                laserObj.transform.position = (gunfire.transform.position);
                laserObj.transform.LookAt(aimTarget);
                if (hit.collider != null)
                {
                    laserObj.transform.localScale = new Vector3(laserObj.transform.localScale.x, laserObj.transform.localScale.y, dis * 0.5f / 30);
                }


                if (Time.time - lastLaserHitInitiatTime > 0.03f)
                {
                    if ((aimTarget - dir - cameraTransform.position).sqrMagnitude > 3.0f * 3.0f)
                    {
                        Object.Instantiate(rConf.laserHit, aimTarget - dir, Quaternion.identity);
                        lastLaserHitInitiatTime = Time.time;
                    }
                }

                if (CouldMakeNextShoot())
                {
                    /*
                    foreach (Enemy enemy in gameScene.GetEnemies().Values)
                    {
                        if (enemy.GetState() == Enemy.DEAD_STATE)
                        {
                            continue;
                        }

                        if (laserObj.collider.bounds.Intersects(enemy.GetCollider().bounds))
                        {
                            Object.Instantiate(rConf.laserHit, enemy.GetPosition(), Quaternion.identity);
                            DamageProperty dp = new DamageProperty();
                            dp.hitForce = (dir + new Vector3(0f, 0.18f, 0f)) * hitForce;
                            dp.damage = damage;
                            enemy.OnHit(dp, GetWeaponType(), false);

                        }


                    }
                     */
                    GameObject[] woodboxes = gameScene.GetWoodBoxes();
                    foreach (GameObject woodbox in woodboxes)
                    {
                        if (woodbox != null)
                        {
                            if (laserObj.GetComponent<Collider>().bounds.Intersects(woodbox.GetComponent<Collider>().bounds))
                            {
                                WoodBoxScript ws = woodbox.GetComponent<WoodBoxScript>();
                                ws.OnHit(damage);

                            }
                        }
                    }

                   

                    if (shootAudio != null)
                    {
                        if (!shootAudio.isPlaying)
                        {
                            AudioPlayer.PlayAudio(shootAudio);
                        }
                    }
                    lastShootTime = Time.time;



                }
            }
        }

        public override void Fire(float deltaTime)
        {

            gunfire.GetComponent<Renderer>().enabled = true;
            Vector3 tempTargetPoint = cameraComponent.ScreenToWorldPoint(new Vector3(gameCamera.ReticlePosition.x, Screen.height - gameCamera.ReticlePosition.y, 50));

            Ray ray = new Ray(cameraTransform.position, (tempTargetPoint - cameraTransform.position));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000, 1 << PhysicsLayer.ENEMY | 1 << PhysicsLayer.WALL | 1 << PhysicsLayer.FLOOR))
            {
                aimTarget = hit.point;
            }
            else
            {
                aimTarget = cameraTransform.TransformPoint(0, 0, 1000);
            }
            Vector3 dir = (aimTarget - gunfire.transform.position).normalized;
            if (laserObj == null)
            {
                laserObj = Object.Instantiate(rConf.laser, gunfire.transform.position, Quaternion.LookRotation(dir)) as GameObject;
                laserStartScale = laserObj.transform.localScale;
                ProjectileScript p = laserObj.GetComponent<ProjectileScript>();
                p.dir = dir;
                p.flySpeed = flySpeed;
                p.explodeRadius = 0.0f;
                p.hitForce = hitForce;
                p.life = 8;
                p.damage = damage;
                p.GunType = WeaponType.LaserGun;

            }
            else
            {


            }


            lastShootTime = Time.time;

        }
        public override void StopFire()
        {
            if (laserObj != null)
            {
                GameObject.Destroy(laserObj);

                laserObj = null;
            }
            if (shootAudio != null)
            {
                shootAudio.Stop();
            }
            if (gunfire != null)
            {
                gunfire.GetComponent<Renderer>().enabled = false;
            }
        }




        public override WeaponType GetWeaponType()
        {
            return WeaponType.LaserGun;
        }

        public override void changeReticle()
        {

        }

    }
}