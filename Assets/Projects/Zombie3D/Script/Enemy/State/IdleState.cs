using UnityEngine;
using System.Collections;
namespace Zombie3D
{



    /*   
     * 
     * 
     */

    public class IdleState : EnemyState
    {

        public override void NextState(Enemy enemy, float deltaTime, Player player)
        {
            if (enemy.HP <= 0)
            {
                enemy.OnDead();
                enemy.SetState(Enemy.DEAD_STATE);
                return;
            }

            enemy.Animate(AnimationName.ENEMY_IDLE, WrapMode.Loop);


            float distance = (enemy.GetTransform().position - player.GetTransform().position).sqrMagnitude;

            if (distance < enemy.DetectionRange * enemy.DetectionRange)
            {
                enemy.SetState(Enemy.CATCHING_STATE);
            }

        }
    }
}