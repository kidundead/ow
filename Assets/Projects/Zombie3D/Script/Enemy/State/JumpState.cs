using UnityEngine;
using System.Collections;
namespace Zombie3D
{



    /*   
     * 
     * 
     */

    public class JumpState : EnemyState
    {

        public override void NextState(Enemy enemy, float deltaTime, Player player)
        {
            if (enemy.HP <= 0)
            {
                enemy.OnDead();
                enemy.SetState(Enemy.DEAD_STATE);
                return;
            }
            
            Transform enemyTransform = enemy.GetTransform();

            Hunter hunter = enemy as Hunter;

            if (hunter != null)
            {
                if (hunter.Jump(deltaTime))
                {
                    hunter.SetState(Hunter.IDLE_STATE);
                }
                else
                {
                    if (!hunter.JumpEnded)
                    {
                        hunter.Animate(AnimationName.ENEMY_JUMPING, WrapMode.Loop);
                    }
                }

            }

        }

        public override void OnHit(Enemy enemy)
        {

        }
    }
}