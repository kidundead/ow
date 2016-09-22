using UnityEngine;
using System.Collections;

namespace Zombie3D
{
    /*  Long distance enemy
     */

    public class Swat : Enemy
    {

        protected GameObject hitParticles;
        protected LineRenderer lineR;
        public override void Init(GameObject gObject)
        {
            base.Init(gObject);
            hitParticles = rConfig.hitparticles;
            lineR = enemyObject.GetComponent<Renderer>() as LineRenderer;

            attackRange = 14.0f;

            MonsterConfig mConf = gConfig.GetMonsterConfig("Swat");
            hp = mConf.hp * gameScene.GetDifficultyHpFactor;
            attackDamage = mConf.damage * gameScene.GetDifficultyDamageFactor;
            attackFrequency = mConf.attackRate;
            runSpeed = mConf.walkSpeed;
            lootCash = mConf.lootCash;
            RandomRunAnimation();


            if (IsElite)
            {
                hp *= 1.5f;
                runSpeed += 2f;
                attackDamage *= 1.5f;
                animation[runAnimationName].speed = 1.2f;
            }

            TimerManager.GetInstance().SetTimer(TimerName.NURSE_AUDIO, 12.0f, true);
            //animation[runAnimationName].speed = 1.5f;
        }


        public override bool CouldEnterAttackState()
        {
            if (base.CouldEnterAttackState())
            {
                if (Mathf.Abs(enemyTransform.position.y - player.GetTransform().position.y)< 2f)
                {

                    return true;
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
        protected void RandomRunAnimation()
        {

            runAnimationName = AnimationName.ENEMY_RUN;
            /*
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
            */

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


        public override void CheckHit()
        {
            if (!attacked && IsAnimationPlayedPercentage(AnimationName.ENEMY_ATTACK, 0.6f))
            {

                //Physics:
                //v0 * t + 1/2 * g * t * t = h;
                //v1* t = dis;

                Vector3 enemyOnFloorPosition = new Vector3(enemyTransform.position.x, enemyTransform.position.y, enemyTransform.position.z);

                float h = -1f;

                Vector3 disVector = new Vector3(target.position.x, enemyTransform.position.y, target.position.z) - enemyOnFloorPosition;
                float dis = disVector.magnitude;
                float flySpeed = 5.0f;
                float t = dis / flySpeed;

                float v0 = (h - 0.5f * Physics.gravity.y * (0.5f)* t * t) / t;

                Vector3 dir = Vector3.up * v0 + disVector.normalized * flySpeed;

                GameObject bombObj = Object.Instantiate(rConfig.copBomb, enemyOnFloorPosition + Vector3.up * (-h), Quaternion.LookRotation(-dir)) as GameObject;
                CopBombScript cs = bombObj.GetComponent<CopBombScript>();
                cs.damage = attackDamage;
                bombObj.GetComponent<Rigidbody>().AddForce(dir, ForceMode.Impulse);

                attacked = true;

            }
        }

        
        public override void OnAttack()
        {

            base.OnAttack();

            Animate(AnimationName.ENEMY_ATTACK, WrapMode.ClampForever);

            enemyTransform.LookAt(target);

            attacked = false;
            lastAttackTime = Time.time;
            /*
            ProjectileScript p = proObj.GetComponent<ProjectileScript>();
            p.dir = dir;
            p.speed = dir;
            p.explodeRadius = 2.0f;
            p.hitForce = 0;
            p.GunType = WeaponType.NurseSaliva;
            p.damage = attackDamage;
            
            Object.Instantiate(rConfig.salivaExplosion, enemyTransform.position + Vector3.up * 0.5f, Quaternion.identity);
            */


        }


    }

}