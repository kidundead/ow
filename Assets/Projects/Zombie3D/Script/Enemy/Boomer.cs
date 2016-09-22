using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Zombie3D
{

    /*  
     * 
     * 
     */

    public class Boomer : Enemy
    {

        public static EnemyState EXPLODE_STATE = new BoomerExplodeState();

        protected Collider handCollider;

        protected Vector3 targetPosition;

        protected Vector3[] p = new Vector3[4];

        protected GameObject explodeObject;
        // Use this for initialization
        public override void Init(GameObject gObject)
        {

            base.Init(gObject);
            handCollider = enemyTransform.Find(BoneName.ENEMY_HAND).gameObject.GetComponent<Collider>();
            lastTarget = Vector3.zero;

            MonsterConfig mConf = gConfig.GetMonsterConfig("Boomer");

            hp = mConf.hp * gameScene.GetDifficultyHpFactor;
            attackDamage = mConf.damage * gameScene.GetDifficultyDamageFactor;
            attackFrequency = mConf.attackRate;
            runSpeed = mConf.walkSpeed;

            lootCash = mConf.lootCash;
            //animation[runAnimationName].speed = 1.5f;

            explodeObject = rConfig.boomerExplosion;

            if (IsElite)
            {
                hp *= 2;
                attackDamage *= 1.5f;
            }

        }

        public void Explode()
        {
            
            float sqrDis = (enemyTransform.position - player.GetTransform().position).sqrMagnitude;
            if (sqrDis < 5 * 5)
            {
                player.OnHit(attackDamage);
            }
        }
        
        public override void OnDead()
        {
            Object.Instantiate(explodeObject, enemyTransform.position, Quaternion.identity);
            criticalAttacked = true;
            base.OnDead();

        }

        public override void OnAttack()
        {

            base.OnAttack();
            
            DamageProperty dp = new DamageProperty();
            dp.damage = hp;

            Explode();
            OnHit(dp, WeaponType.NoGun, true);

        }


        public override EnemyState EnterSpecialState(float deltaTime)
        {
            EnemyState state = null;
            if (SqrDistanceFromPlayer < AttackRange * AttackRange)
            {
                Animate(AnimationName.ENEMY_ATTACK, WrapMode.Loop);
                state = EXPLODE_STATE;
            }
            return state;

        }






    }

}