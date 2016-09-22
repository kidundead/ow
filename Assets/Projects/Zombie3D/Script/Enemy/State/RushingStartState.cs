using UnityEngine;
using System.Collections;
namespace Zombie3D
{



    /*   
     * 
     * 
     */

    public class RushingStartState : EnemyState
    {

        public override void NextState(Enemy enemy, float deltaTime, Player player)
        {
            if (enemy.HP <= 0)
            {
                enemy.OnDead();
                enemy.SetState(Enemy.DEAD_STATE);
                return;
            }

            Tank tank = enemy as Tank;
            if (tank != null)
            {
                tank.Audio.PlayAudio(AudioName.SHOUT);
                if (tank.IsAnimationPlayedPercentage(AnimationName.ENEMY_RUSHINGSTART, 1.0f))
                {
                    

                    tank.SetState(Tank.RUSHING_STATE);
                    tank.Animate(AnimationName.ENEMY_RUSHING, WrapMode.Loop);
                }
            }
            

        }

        public override void OnHit(Enemy enemy)
        {

        }
    }
}