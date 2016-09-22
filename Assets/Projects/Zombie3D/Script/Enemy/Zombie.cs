using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Zombie3D
{

    /*  
     * 
     * 
     */

    public class Zombie : Enemy
    {

        protected Collider handCollider;

        protected Vector3 targetPosition;

        protected Vector3[] p = new Vector3[4];




        protected void RandomRunAnimation()
        {
            int rnd = Random.Range(0, 10);
            if (rnd < 7)
            {
                runAnimationName = AnimationName.ENEMY_RUN;
            }
            else if (rnd == 7)
            {
                runAnimationName = AnimationName.ENEMY_RUN01;
            }
            else if (rnd == 8)
            {
                runAnimationName = AnimationName.ENEMY_RUN02;
            }
            else if (rnd == 9)
            {
                runAnimationName = AnimationName.ENEMY_RUN03;
            }

            if (IsElite)
            {
                runAnimationName = AnimationName.ENEMY_RUN;
            }

        }

        // Use this for initialization
        public override void Init(GameObject gObject)
        {

            base.Init(gObject);
            handCollider = enemyTransform.Find(BoneName.ENEMY_HAND).gameObject.GetComponent<Collider>();
            lastTarget = Vector3.zero;
            MonsterConfig mConf = gConfig.GetMonsterConfig("Zombie");
            hp = mConf.hp * gameScene.GetDifficultyHpFactor;
            attackDamage = mConf.damage * gameScene.GetDifficultyDamageFactor;
            attackFrequency = mConf.attackRate;
            runSpeed = mConf.walkSpeed;
            lootCash = mConf.lootCash;


            RandomRunAnimation();

            if (IsElite)
            {
                hp *= 3;
                runSpeed += 2;
                attackDamage *= 2;
                animation[runAnimationName].speed = 1.5f;
            }


            TimerManager.GetInstance().SetTimer(TimerName.ZOMBIE_AUDIO, 8.0f, true);

            //animation[runAnimationName].speed = 1.5f;

        }


        public override void CheckHit()
        {
            if (!attacked && IsAnimationPlayedPercentage(AnimationName.ENEMY_ATTACK, 0.4f))
            {
                Collider pcollider = player.GetCollider();
                if (pcollider != null)
                {

                    if (handCollider.bounds.Intersects(pcollider.bounds))
                    {
                        player.OnHit(attackDamage);
                        attacked = true;
                    }
                }
            }
        }



        public override void DoLogic(float deltaTime)
        {

            base.DoLogic(deltaTime);
            if (TimerManager.GetInstance().Ready(TimerName.ZOMBIE_AUDIO))
            {
                audio.PlayAudio(AudioName.SHOUT);
                TimerManager.GetInstance().Do(TimerName.ZOMBIE_AUDIO);
            }

        }



        public override void OnAttack()
        {

            base.OnAttack();

            Animate(AnimationName.ENEMY_ATTACK, WrapMode.ClampForever);
            attacked = false;

            /*
            Collider pcollider = player.GetCollider();
            if (pcollider != null)
            {
                if (handCollider.bounds.Intersects(pcollider.bounds))
                {
                    player.OnHit(attackDamage);

                }
            }
             */
            lastAttackTime = Time.time;
        }

    }

}