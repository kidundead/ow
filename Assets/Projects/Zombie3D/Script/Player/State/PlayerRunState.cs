using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Zombie3D
{

    public class PlayerRunState: PlayerState
    {
        public override void NextState(Player player, float deltaTime) 
        {
            InputController inputController = player.InputController;

            InputInfo inputInfo = new InputInfo();
            inputController.ProcessInput(deltaTime, inputInfo);
            if (!inputInfo.fire)
            {
                player.ZoomOut(deltaTime);
            }

            player.ResetSawAnimation();
            player.Animate(AnimationName.PLAYER_RUN + player.WeaponNameEnd, WrapMode.Loop);

            player.Move(inputInfo.moveDirection * (deltaTime * player.WalkSpeed));

            Weapon weapon = player.GetWeapon();
            if (!inputInfo.fire && !inputInfo.IsMoving())
            {
                player.SetState(Player.IDLE_STATE);
            }
            else if (inputInfo.fire && inputInfo.IsMoving())
            {
                if (weapon.HaveBullets())
                {
                    player.SetState(Player.RUNSHOOT_STATE);
                }
            }
            else if (inputInfo.fire && !inputInfo.IsMoving())
            {

                player.SetState(Player.SHOOT_STATE);
            }
        }

    }
}