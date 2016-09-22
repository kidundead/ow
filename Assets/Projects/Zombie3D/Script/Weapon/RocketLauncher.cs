using UnityEngine;
using System.Collections;

namespace Zombie3D
{
    public class RocketLauncher : Weapon
    {
        protected const float shootLastingTime = 0.5f;
        protected float rocketFlySpeed;
        protected static int sbulletCount;

        public override WeaponType GetWeaponType()
        {
            return WeaponType.RocketLauncher;
        }

        public RocketLauncher()
        {
            maxCapacity = 9999;

            IsSelectedForBattle = false;
        }

        public override int BulletCount
        {
            get
            {
                return RocketLauncher.sbulletCount;
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
            speedDrag = wConf.moveSpeedDrag;
            range = wConf.range;
            accuracy = wConf.accuracyConf.baseData;
            maxGunLoad = wConf.initBullet;
            sbulletCount = maxGunLoad;
            rocketFlySpeed = wConf.flySpeed;
        }

        public override void Init()
        {
            base.Init();

            hitForce = 30f;



            //gunfire.renderer.enabled = false;
        }



        public override void changeReticle()
        {
        }

        public override void CreateGun()
        {
            gun = (GameObject)GameObject.Instantiate(rConf.rpgGun, player.GetTransform().position, player.GetTransform().rotation);

        }

        public override void Fire(float deltaTime)
        {
            

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
            if (Physics.Raycast(ray, out hit, 1000, 1 << PhysicsLayer.ENEMY | 1 << PhysicsLayer.WALL | 1 << PhysicsLayer.FLOOR))
            {
                aimTarget = hit.point;

            }
            else
            {
                aimTarget = cameraTransform.TransformPoint(0, 0, 1000);
            }

            Vector3 dir = (aimTarget - rightGun.position).normalized;
            GameObject proObj = Object.Instantiate(projectile, rightGun.position, Quaternion.LookRotation(dir)) as GameObject;
            ProjectileScript p = proObj.GetComponent<ProjectileScript>();
            p.dir = dir;
            p.flySpeed = rocketFlySpeed;
            p.explodeRadius = range;
            p.hitForce = hitForce;
            p.life = 8.0f;
            p.damage = damage;
            p.GunType = WeaponType.RocketLauncher;

            lastShootTime = Time.time;
            sbulletCount--;
            sbulletCount = Mathf.Clamp(sbulletCount, 0, maxCapacity);

            /*
            if (shootAudio != null)
            {
                if (!shootAudio.isPlaying)
                {
                    AudioPlayer.PlayAudio(shootAudio);
                }
            }
            */

        }

        public override void StopFire()
        {
            //gunfire.renderer.enabled = false;
        }


    }
}