using UnityEngine;
using System.Collections;
namespace Zombie3D
{



    /*   
     * 
     * 
     */

    public class BoomerExplodeState : EnemyState
    {

        public override void NextState(Enemy enemy, float deltaTime, Player player)
        {
            if (enemy.HP <= 0)
            {
                enemy.OnDead();
                enemy.SetState(Enemy.DEAD_STATE);
                return;
            }

            Boomer boomer = enemy as Boomer;

            if (boomer.IsAnimationPlayedPercentage(AnimationName.ENEMY_ATTACK, 1.0f))
            {
                boomer.OnAttack();
            }
            
            

        }

        

    }
}