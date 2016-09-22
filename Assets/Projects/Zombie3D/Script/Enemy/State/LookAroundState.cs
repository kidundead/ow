using UnityEngine;
using System.Collections;
namespace Zombie3D
{



    /*   
     * 
     * 
     */

    public class LookAroundState : EnemyState
    {

        public override void NextState(Enemy enemy, float deltaTime, Player player)
        {
            if (enemy.HP <= 0)
            {
                enemy.OnDead();
                enemy.SetState(Enemy.DEAD_STATE);
                return;
            }

            Hunter hunter = enemy as Hunter;

            enemy.Animate(AnimationName.ENEMY_IDLE, WrapMode.Loop);
            enemy.GetTransform().LookAt(player.GetTransform());
            //hunter.Patrol(deltaTime);
            if (hunter.LookAroundTimOut())
            {

                if (hunter.ReadyForJump())
                {
                    int rnd = Random.Range(0, 100);
                    if (rnd < 100)
                    {
                        hunter.StartJump();

                        hunter.SetState(Hunter.JUMP_STATE);
                    }
                    else
                    {
                        hunter.SetState(Hunter.CATCHING_STATE);
                    }
                }
                else
                {
                    hunter.SetState(Hunter.CATCHING_STATE);
                }

            }

        }

        

    }
}