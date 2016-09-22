using UnityEngine;
using System.Collections;
namespace Zombie3D
{



    /*   
     * 
     * 
     */

    public class GotHitState : EnemyState
    {

        public override void NextState(Enemy enemy, float deltaTime, Player player)
        {
            if (enemy.HP <= 0)
            {
                enemy.OnDead();
                enemy.SetState(Enemy.DEAD_STATE);
            }
            else
            {
                enemy.Animate(AnimationName.ENEMY_GOTHIT, WrapMode.Once);

                if (enemy.GotHitAnimationEnds())
                {
                    enemy.SetState(Enemy.IDLE_STATE);
                }
            }

        }

    }
}