using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Zombie3D
{
    public class NearestEnemyInfo
    {
        public Transform transform;
        public Vector2 screenPos;
        public Vector2 currentScreenPos;
        public float distance;
    }

    public class Sniper : Weapon
    {
        protected float trimWidth = 25.0f;
        protected float trimHeight = 25.0f;
        protected List<NearestEnemyInfo> nearestEnemyInfoList;
        protected int maxLocks = 5;
        protected bool locked = false;
        protected float flySpeed;
        protected static int sbulletCount;
        public static Rect lockAreaRect = AutoRect.AutoPos(new Rect(230, 200, 500, 250));

        public int MaxLocks
        {
            get
            {
                return maxLocks;
            }
        }

        public override int BulletCount
        {
            get
            {
                return Sniper.sbulletCount;
            }
            set
            {
                sbulletCount = value;
            }
        }




        public List<NearestEnemyInfo> GetNearestEnemyInfoList()
        {
            return nearestEnemyInfoList;
        }


        public override WeaponType GetWeaponType()
        {
            return WeaponType.Sniper;
        }

        public override void Init()
        {
            base.Init();


            hitForce = 60f;
            maxCapacity = 9999;

            nearestEnemyInfoList = new List<NearestEnemyInfo>();
            TimerManager.GetInstance().SetTimer(TimerName.AUTOLOCK, attackFrenquency, false);
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

        public override void changeReticle()
        {

        }

        public override void CreateGun()
        {

            gun = (GameObject)GameObject.Instantiate(rConf.sniper, player.GetTransform().position, player.GetTransform().rotation);

        }

        public override void AutoAim(float deltaTime)
        {

            foreach (NearestEnemyInfo info in nearestEnemyInfoList)
            {
                if (info.transform != null)
                {
                    Vector2 screenPos = cameraComponent.WorldToScreenPoint(info.transform.position);

                    info.screenPos = new Vector2(screenPos.x, Screen.height - screenPos.y);
                    float x = Mathf.Lerp(info.currentScreenPos.x, info.screenPos.x, deltaTime * 10f);
                    float y = Mathf.Lerp(info.currentScreenPos.y, info.screenPos.y, deltaTime * 10f);
                    info.currentScreenPos = new Vector2(x, y);
                }
            }


            if (TimerManager.GetInstance().Ready(TimerName.AUTOLOCK))
            {

                for (int i = nearestEnemyInfoList.Count - 1; i >= 0; i--)
                {
                    NearestEnemyInfo info = nearestEnemyInfoList[i];
                    if (info.transform != null)
                    {
                        Vector3 screenPos = cameraComponent.WorldToScreenPoint(info.transform.position);

                        if (screenPos.z < 0
                           || screenPos.x < lockAreaRect.xMin
                           || screenPos.x > lockAreaRect.xMax
                           || screenPos.y < lockAreaRect.yMin
                           || screenPos.y > lockAreaRect.yMax
                        )
                        {
                            nearestEnemyInfoList.Remove(info);

                            if (info.transform.gameObject.layer == PhysicsLayer.WOODBOX_AIM)
                            {
                                GameObject.Destroy(info.transform.gameObject);
                            }
                        }

                    }
                }


                if (nearestEnemyInfoList.Count < maxLocks)
                {

                    float minSqrDistance = 99999.0f;
                    NearestEnemyInfo newInfo = null;

                    foreach (Enemy enemy in gameScene.GetEnemies().Values)
                    {

                        if (enemy.GetState() == Enemy.DEAD_STATE)
                        {
                            continue;
                        }

                        Transform t = enemy.GetAimedTransform();

                        Vector3 screenPos = cameraComponent.WorldToScreenPoint(t.position);
                        if (screenPos.z < 0)
                        {
                            continue;
                        }

                        if (screenPos.x > lockAreaRect.xMin
                           && screenPos.x < lockAreaRect.xMax
                           && screenPos.y > lockAreaRect.yMin
                           && screenPos.y < lockAreaRect.yMax
                        )
                        {

                            Ray ray = new Ray(rightGun.position, t.position - rightGun.position);
                            RaycastHit hit;
                            if (Physics.Raycast(ray, out hit, 1000, (1 << PhysicsLayer.ENEMY) | (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.FLOOR)))
                            {
                                if (!hit.collider.gameObject.name.StartsWith(ConstData.ENEMY_NAME))
                                {
                                    continue;
                                }

                            }

                            float dist = (rightGun.position - t.position).sqrMagnitude;


                            bool alreadyLocked = false;

                            foreach (NearestEnemyInfo info in nearestEnemyInfoList)
                            {
                                if (info.transform == t)
                                {
                                    alreadyLocked = true;
                                }
                            }

                            if (!alreadyLocked)
                            {

                                if (dist < minSqrDistance)
                                {
                                    minSqrDistance = dist;
                                    newInfo = new NearestEnemyInfo();
                                    newInfo.transform = t;
                                    newInfo.screenPos = new Vector2(screenPos.x, Screen.height - screenPos.y);
                                    newInfo.distance = dist;
                                    newInfo.currentScreenPos = gameScene.GetCamera().ReticlePosition;
                                }
                            }

                        }

                    }
                    if (newInfo != null)
                    {
                        nearestEnemyInfoList.Add(newInfo);
                        if (shootAudio != null)
                        {
                            AudioPlayer.PlayAudio(shootAudio);
                        }
                    }
                    else
                    {

                        GameObject[] woodboxes = gameScene.GetWoodBoxes();
                        foreach (GameObject woodbox in woodboxes)
                        {
                            if (woodbox != null)
                            {
                                Transform t = woodbox.transform;
                                Vector3 screenPos = cameraComponent.WorldToScreenPoint(t.position + Vector3.up * 0.6f);
                                if (screenPos.z < 0)
                                {
                                    continue;
                                }

                                if (screenPos.x > lockAreaRect.xMin
                                   && screenPos.x < lockAreaRect.xMax
                                   && screenPos.y > lockAreaRect.yMin
                                   && screenPos.y < lockAreaRect.yMax
                                )
                                {
                                    Ray ray = new Ray(rightGun.position, t.position + Vector3.up * 0.6f - rightGun.position);
                                    RaycastHit hit;
                                    if (Physics.Raycast(ray, out hit, 1000, (1 << PhysicsLayer.WOODBOX) | (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.FLOOR)))
                                    {
                                        if (hit.collider.gameObject.layer != PhysicsLayer.WOODBOX)
                                        {
                                            continue;
                                        }

                                    }
                                    float dist = (rightGun.position - t.position).sqrMagnitude;


                                    bool alreadyLocked = false;

                                    foreach (NearestEnemyInfo info in nearestEnemyInfoList)
                                    {
                                        if (t.GetChildCount() > 0)
                                        {
                                            alreadyLocked = true;
                                        }

                                    }

                                    if (!alreadyLocked)
                                    {
                                        if (dist < minSqrDistance)
                                        {
                                            minSqrDistance = dist;
                                            newInfo = new NearestEnemyInfo();
                                            GameObject o = new GameObject();
                                            o.transform.position = t.position + Vector3.up * 0.6f;
                                            o.transform.parent = t;
                                            o.layer = PhysicsLayer.WOODBOX_AIM;
                                            newInfo.transform = o.transform;
                                            newInfo.screenPos = new Vector2(screenPos.x, Screen.height - screenPos.y);
                                            newInfo.distance = dist;
                                            newInfo.currentScreenPos = gameScene.GetCamera().ReticlePosition;
                                        }
                                    }
                                }
                            }
                        }

                        if (newInfo != null)
                        {
                            nearestEnemyInfoList.Add(newInfo);
                            if (shootAudio != null)
                            {
                                AudioPlayer.PlayAudio(shootAudio);
                            }
                        }


                    }
                    TimerManager.GetInstance().Do(TimerName.AUTOLOCK);

                }
            }

        }

        public bool AimedTarget()
        {
            if (nearestEnemyInfoList.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }


        }


        public override void Fire(float deltaTime)
        {

            foreach (NearestEnemyInfo info in nearestEnemyInfoList)
            {
                if (info.transform != null)
                {
                    Vector3 dir = (info.transform.position - rightGun.position).normalized;
                    GameObject proObj = Object.Instantiate(projectile, rightGun.position + Vector3.up, Quaternion.LookRotation(-dir)) as GameObject;
                    ProjectileScript p = proObj.GetComponent<ProjectileScript>();
                    p.dir = dir;
                    p.life = 10.0f;
                    p.damage = damage;
                    p.flySpeed = flySpeed;
                    p.hitForce = hitForce;
                    p.targetTransform = info.transform;
                    p.GunType = WeaponType.Sniper;
                    p.explodeRadius = range;
                    lastShootTime = Time.time;
                    sbulletCount--;
                    sbulletCount = Mathf.Clamp(sbulletCount, 0, maxCapacity);
                }

            }
            nearestEnemyInfoList.Clear();

            locked = false;
        }

        public override void StopFire()
        {
            if (nearestEnemyInfoList != null)
            {
                nearestEnemyInfoList.Clear();
            }
        }

    }
}