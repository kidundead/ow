using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Zombie3D
{

    public abstract class PlayerState
    {
        public virtual void NextState(Player player, float deltaTime) { }

        public virtual void OnHit(Player player, float damage)
        {

            if (player.HP <= 0)
            {
                player.StopFire();
                player.OnDead();
                player.SetState(Player.DEAD_STATE);
            }
            else if(player.CouldGetAnotherHit())
            {
                player.CreateScreenBlood(damage);
                if (player.GetWeapon().GetWeaponType() != WeaponType.Saw)
                {
                    player.Animate(AnimationName.PLAYER_GOTHIT, WrapMode.Once);
                    player.StopFire();
                    player.SetState(Player.GOTHIT_STATE);
                }
               
            }
        
        }
    }
}