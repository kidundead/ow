using UnityEngine;
using System.Collections;

namespace Zombie3D
{
    public class ShotGun : Weapon
    {
       
        protected static int sbulletCount;
        protected Timer shotgunFireTimer;
        public override WeaponType GetWeaponType()
        {
            return WeaponType.ShotGun;
        }

        public ShotGun()
        {

            maxCapacity = 9999;




            IsSelectedForBattle = false;
            shotgunFireTimer = new Timer();
            
        }

        public override int BulletCount
        {
            get
            {
                return ShotGun.sbulletCount;
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
        }


        public override void Init()
        {
            base.Init();
            hitForce = 20f;
            gunfire = gun.transform.Find("gun_fire_new").gameObject;

        }

        public void PlayPumpAnimation()
        {

        }

        public override void changeReticle()
        {

        }

        public override void CreateGun()
        {

            switch (Name)
            {
                case GunName.WINCHESTER1200:
                    gun = (GameObject)GameObject.Instantiate(rConf.winchester1200, player.GetTransform().position, player.GetTransform().rotation);
                    break;
                case GunName.REMINGTON870:
                    gun = (GameObject)GameObject.Instantiate(rConf.remington870, player.GetTransform().position, player.GetTransform().rotation);
                    break;
                case GunName.XM1014:
                    gun = (GameObject)GameObject.Instantiate(rConf.xm1014, player.GetTransform().position, player.GetTransform().rotation);
                    break;

            }


        }

        public override void FireUpdate(float deltaTime)
        {
            base.FireUpdate(deltaTime);
            if (shotgunFireTimer.Ready())
            {
                if (gunfire != null)
                    gunfire.GetComponent<Renderer>().enabled = false;
                shotgunFireTimer.Do();
            }
        }

        public override void Fire(float deltaTime)
        {

            AudioPlayer.PlayAudio(shootAudio);
            
            gun.GetComponent<UnityEngine.Animation>()[AnimationName.SHOTGUN_RELOAD].wrapMode = WrapMode.Once;
            gun.GetComponent<UnityEngine.Animation>().Play(AnimationName.SHOTGUN_RELOAD);


            gunfire.GetComponent<Renderer>().enabled = true;
            shotgunFireTimer.SetTimer(0.2f, false);

            Object.Instantiate(rConf.shotgunBullet, rightGun.position, player.GetTransform().rotation);
            player.LastHitPosition = player.GetTransform().position;

            GameObject shotgunfireObj = Object.Instantiate(rConf.shotgunfire, gunfire.transform.position, player.GetTransform().rotation) as GameObject;
            shotgunfireObj.transform.parent = player.GetTransform();

            float tan60 = Mathf.Tan(Mathf.Deg2Rad * 60.0f);

            int oneShotKills = 0;
            foreach (Enemy enemy in gameScene.GetEnemies().Values)
            {
                if (enemy.GetState() == Enemy.DEAD_STATE)
                {
                    continue;
                }
                Vector3 relativeEnemyPos = player.GetTransform().InverseTransformPoint(enemy.GetPosition());
                float dis = (enemy.GetPosition() - player.GetTransform().position).sqrMagnitude;
                float radiusSqr = range * range;

                if (relativeEnemyPos.z > 0)
                {

                    if (Mathf.Abs(relativeEnemyPos.z / relativeEnemyPos.x) > tan60)
                    {
                        DamageProperty dp = new DamageProperty();
                        dp.damage = damage * player.PowerBuff;
                        if (dis < radiusSqr)
                        {
                            enemy.OnHit(dp, GetWeaponType(), true);
                        }
                        else if (dis < radiusSqr * 2 * 2)
                        {
                            int rnd = Random.Range(0, 100);
                            if (rnd < accuracy)
                            {
                                enemy.OnHit(dp, GetWeaponType(), true);
                            }

                        }
                        else if (dis < radiusSqr * 3 * 3)
                        {
                            int rnd = Random.Range(0, 100);
                            if (rnd < accuracy/2)
                            {
                                enemy.OnHit(dp, GetWeaponType(), true);
                            }

                        }
                        else if (dis < radiusSqr * 4 * 4)
                        {
                            int rnd = Random.Range(0, 100);
                            if (rnd < accuracy/4)
                            {
                                enemy.OnHit(dp, GetWeaponType(), true);
                            }

                        }
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
                    Vector3 relativeBoxPos = player.GetTransform().InverseTransformPoint(woodbox.transform.position);
                    float dis = (woodbox.transform.position - player.GetTransform().position).sqrMagnitude;
                    float radiusSqr = range * range;
                    if ((dis < radiusSqr * 2*2) && relativeBoxPos.z > 0)
                    {
                        WoodBoxScript ws = woodbox.GetComponent<WoodBoxScript>();
                        ws.OnHit(damage * player.PowerBuff);
                    }
                }
            }


            lastShootTime = Time.time;
            sbulletCount--;
            sbulletCount = Mathf.Clamp(sbulletCount, 0, maxCapacity);
        
        }



        public override void StopFire()
        {
            if(gunfire!=null)
            gunfire.GetComponent<Renderer>().enabled = false;
        }

        public override void GunOff()
        {
            base.GunOff();
        }
    }
}