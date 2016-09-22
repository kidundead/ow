using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Zombie3D
{
    public class Saw : Weapon
    {

        protected GameObject fireTrail;

        protected ObjectPool bulletsObjectPool;
        protected ObjectPool firelineObjectPool;

        protected ObjectPool sparksObjectPool;

        protected static float sbulletCount;

        public override WeaponType GetWeaponType()
        {
            return WeaponType.Saw;
        }

        public override int BulletCount
        {
            get
            {
                return (int)Saw.sbulletCount;
            }
            set
            {
                sbulletCount = value;
            }
        }


        public Saw()
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
            gun = (GameObject)GameObject.Instantiate(rConf.saw, player.GetTransform().position, player.GetTransform().rotation);

        }

        public override void DoLogic()
        {

            bulletsObjectPool.AutoDestruct();
            firelineObjectPool.AutoDestruct();

            sparksObjectPool.AutoDestruct();
        }

        public override void FireUpdate(float deltaTime)
        {
            /*
            sbulletCount -= (20f * deltaTime);
            sbulletCount = Mathf.Clamp(sbulletCount, 0, maxCapacity);

            if (CouldMakeNextShoot())
            {
                if (player.IsAnimationPlayedPercentage(AnimationName.PLAYER_SHOT + player.WeaponNameEnd, 0.7f)
                    || player.IsAnimationPlayedPercentage(AnimationName.PLAYER_RUNFIRE + player.WeaponNameEnd, 0.7f))
                {


                    //player.LastHitPosition = hit.point;

                    Hashtable enemyList = gameScene.GetEnemies();
                    foreach (Enemy enemy in enemyList.Values)
                    {
                        Collider c = enemy.GetCollider();
                        if (gun.collider.bounds.Intersects(c.bounds))
                        {
                            DamageProperty dp = new DamageProperty();
                            dp.damage = damage;
                            enemy.OnHit(dp, WeaponType.Saw, true);

                        }

                    }


                    GameObject[] woodboxes = gameScene.GetWoodBoxes();
                    foreach (GameObject woodbox in woodboxes)
                    {
                        if (woodbox != null)
                        {
                            Collider c = woodbox.collider;
                            if (gun.collider.bounds.Intersects(c.bounds))
                            {
                                WoodBoxScript ws = woodbox.GetComponent<WoodBoxScript>();
                                ws.OnHit(damage * player.PowerBuff);
                            }



                        }

                    }


                    lastShootTime = Time.time;

                }
            }
            */
        }

        public override bool HaveBullets()
        {
            return true;
        }

        public override void Fire(float deltaTime)
        {

            //gunfire.renderer.enabled = true;
            if (shootAudio != null)
            {
                if (!shootAudio.isPlaying)
                {
                    AudioPlayer.PlayAudio(shootAudio);
                }
            }


            Hashtable enemyList = gameScene.GetEnemies();
            foreach (Enemy enemy in enemyList.Values)
            {
                Collider c = enemy.GetCollider();
                if (gun.GetComponent<Collider>().bounds.Intersects(c.bounds))
                {
                    DamageProperty dp = new DamageProperty();
                    dp.damage = damage;
                    enemy.OnHit(dp, WeaponType.Saw, true);

                }

            }


            GameObject[] woodboxes = gameScene.GetWoodBoxes();
            foreach (GameObject woodbox in woodboxes)
            {
                if (woodbox != null)
                {
                    Collider c = woodbox.GetComponent<Collider>();
                    if (gun.GetComponent<Collider>().bounds.Intersects(c.bounds))
                    {
                        WoodBoxScript ws = woodbox.GetComponent<WoodBoxScript>();
                        ws.OnHit(damage * player.PowerBuff);
                    }



                }

            }


            lastShootTime = Time.time;


            

            
        }


        

        public override void GunOn()
        {
            GameObject model1 = gun.transform.Find("Saw01").gameObject;
            GameObject model2 = gun.transform.Find("Saw02").gameObject;
            if (model1.GetComponent<Renderer>() != null)
            {
                model1.GetComponent<Renderer>().enabled = true;
            }
            if (model2.GetComponent<Renderer>() != null)
            {
                model2.GetComponent<Renderer>().enabled = true;
            }

        }

        public override void GunOff()
        {

            GameObject model1 = gun.transform.Find("Saw01").gameObject;
            GameObject model2 = gun.transform.Find("Saw02").gameObject;
            if (model1.GetComponent<Renderer>() != null)
            {
                model1.GetComponent<Renderer>().enabled = false;
            }
            if (model2.GetComponent<Renderer>() != null)
            {
                model2.GetComponent<Renderer>().enabled = false;
            }

            StopFire();

        }


        public override void StopFire()
        {
            if (shootAudio != null)
            {
                //shootAudio.Stop();
            }
            if (gunfire != null)
            {
                gunfire.GetComponent<Renderer>().enabled = false;
            }
        }

    }
}