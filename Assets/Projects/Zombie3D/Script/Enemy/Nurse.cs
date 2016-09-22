using UnityEngine;
using System.Collections;

namespace Zombie3D
{
    /*  Long distance enemy
     */

    public class Nurse : Enemy
    {

        protected GameObject hitParticles;
        protected LineRenderer lineR;
        public override void Init(GameObject gObject)
        {
            base.Init(gObject);
            hitParticles = rConfig.hitparticles;
            lineR = enemyObject.GetComponent<Renderer>() as LineRenderer;

            attackRange = 10.0f;

            MonsterConfig mConf = gConfig.GetMonsterConfig("Nurse");
            hp = mConf.hp * gameScene.GetDifficultyHpFactor;
            attackDamage = mConf.damage * gameScene.GetDifficultyDamageFactor;
            attackFrequency = mConf.attackRate;
            runSpeed = mConf.walkSpeed;
            lootCash = mConf.lootCash;
            RandomRunAnimation();
            if (IsElite)
            {
                hp *= 2;
                runSpeed += 1.5f;
                attackDamage *= 3;
                animation[runAnimationName].speed = 1.2f;
            }


            
            TimerManager.GetInstance().SetTimer(TimerName.NURSE_AUDIO, 12.0f, true);
            //animation[runAnimationName].speed = 1.5f;
        }

        protected void RandomRunAnimation()
        {
            int rnd = Random.Range(0, 100);
            if (rnd < 50)
            {
                runAnimationName = AnimationName.ENEMY_RUN;
            }
            else if (rnd < 75)
            {
                runAnimationName = AnimationName.ENEMY_RUN01;
            }
            else
            {
                runAnimationName = AnimationName.ENEMY_RUN02;
            }


        }

        public override void DoLogic(float deltaTime)
        {

            base.DoLogic(deltaTime);
            if (TimerManager.GetInstance().Ready(TimerName.NURSE_AUDIO))
            {
                audio.PlayAudio(AudioName.SHOUT);
                TimerManager.GetInstance().Do(TimerName.NURSE_AUDIO);
            }

        }



        public override bool CouldEnterAttackState()
        {
            if (base.CouldEnterAttackState())
            {
                if(Mathf.Abs(enemyTransform.position.y - player.GetTransform().position.y)< 2f)
                {

                    Vector3 tpoint = player.GetTransform().position;
                    //tpoint.y = enemyTransform.position.y;
                    Ray ray = new Ray(enemyTransform.position + new Vector3(0, 0.5f, 0), tpoint - (enemyTransform.position + new Vector3(0, 0.5f, 0)));
                    return true;
                    if (Physics.Raycast(ray, out rayhit, Mathf.Sqrt(SqrDistanceFromPlayer), 1 << PhysicsLayer.WALL | 1 << PhysicsLayer.TRANSPARENT_WALL | 1 << PhysicsLayer.SCENE_OBJECT | 1 << PhysicsLayer.PLAYER))
                    {
                        if (rayhit.collider.gameObject.name == "Player")
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    }
                    return false;
                }
                else
                {
                    return false;
                }


            }
            else
            {
                return false;
            }

        }



        public override void OnAttack()
        {

            base.OnAttack();

            Animate(AnimationName.ENEMY_ATTACK, WrapMode.ClampForever);

            enemyTransform.LookAt(target);


            //Physics:
            //v0 * t + 1/2 * g * t * t = h;
            //v1* t = dis;

            Vector3 enemyOnFloorPosition = new Vector3(enemyTransform.position.x, enemyTransform.position.y , enemyTransform.position.z);

            float h = -1.0f;

            Vector3 disVector = new Vector3(target.position.x, enemyTransform.position.y, target.position.z) - enemyOnFloorPosition;
            float dis = disVector.magnitude;
            float flySpeed = 12.0f;
            float t = dis / flySpeed;

            float v0 = (h - 0.5f * Physics.gravity.y * t * t) / t;

            Vector3 dir = Vector3.up * v0 + disVector.normalized * flySpeed;

            GameObject proObj = Object.Instantiate(rConfig.nurseSalivaProjectile, enemyOnFloorPosition + Vector3.up * (-h), Quaternion.LookRotation(-dir)) as GameObject;
            ProjectileScript p = proObj.GetComponent<ProjectileScript>();
            p.dir = dir;
            p.speed = dir;
            p.explodeRadius = 2.0f;
            p.hitForce = 0;
            p.GunType = WeaponType.NurseSaliva;
            p.damage = attackDamage;
            lastAttackTime = Time.time;
            Object.Instantiate(rConfig.salivaExplosion, enemyTransform.position + Vector3.up * 0.5f, Quaternion.identity);



        }


    }

}