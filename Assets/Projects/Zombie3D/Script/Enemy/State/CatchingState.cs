using UnityEngine;
using System.Collections;
namespace Zombie3D
{



    /*   
     * 
     * 
     */

    public class CatchingState : EnemyState
    {
        public override void NextState(Enemy enemy, float deltaTime, Player player)
        {
            if (enemy.HP <= 0)
            {
                enemy.OnDead();
                enemy.SetState(Enemy.DEAD_STATE);
                return;
            }

            enemy.FindPath();
            //enemy.PathFinding();

            enemy.DoMove(deltaTime);

            enemy.Animate(enemy.RunAnimationName, WrapMode.Loop);

            EnemyState specialState = enemy.EnterSpecialState(deltaTime);
            if (specialState != null)
            {
                enemy.SetState(specialState);
            }
            else
            {               
                if (enemy.CouldEnterAttackState())       //! 进入攻击范围
                {
                    
                    enemy.SetState(Enemy.ATTACK_STATE);
                }
            }
        }

    }
}

