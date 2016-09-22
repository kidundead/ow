using UnityEngine;
using System.Collections;

namespace Zombie3D
{
    public class GrenadeRifle : Weapon
    {

        public override WeaponType GetWeaponType()
        {
            return WeaponType.GrenadeRifle;
        }


        public override void Init()
        {
            base.Init();
            attackFrenquency = 1.5f;
            hitForce = 10f;
            maxCapacity = 100;
            capacity = 10;
            maxGunLoad = 10;
            bulletCount = maxGunLoad;
        }

        public override void changeReticle()
        {
        }

        public override void CreateGun()
        {
            gun = (GameObject)GameObject.Instantiate(rConf.m4, weaponBoneTrans.position, weaponBoneTrans.rotation);

        }

        public override void Fire(float deltaTime)
        {
            if (Time.time - lastShootTime < attackFrenquency || bulletCount == 0)
            {
                return;
            }

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
            aimTarget.y *= 2;
            Vector3 dir = (aimTarget - rightGun.position).normalized;
            GameObject proObj = Object.Instantiate(projectile, rightGun.position + Vector3.up * 0.5f + dir * 0.1f, Quaternion.LookRotation(-dir)) as GameObject;
            ProjectileScript p = proObj.GetComponent<ProjectileScript>();
            p.dir = dir;
            p.flySpeed = 50.0f;
            p.explodeRadius = 5.0f;
            p.hitForce = hitForce;
            p.GunType = WeaponType.GrenadeRifle;

            lastShootTime = Time.time;
            //bulletCount--;
            bulletCount = Mathf.Clamp(bulletCount, 0, maxCapacity);



        }


        public override void StopFire()
        {
        }


    }
}