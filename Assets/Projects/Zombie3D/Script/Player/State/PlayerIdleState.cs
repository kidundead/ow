using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Zombie3D
{

    public class PlayerIdleState: PlayerState
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
            player.Animate(AnimationName.PLAYER_IDLE + player.WeaponNameEnd, WrapMode.Loop);
            player.Move(inputInfo.moveDirection * (deltaTime * player.WalkSpeed));

            if (inputInfo.fire && !inputInfo.IsMoving())
            {
                player.SetState(Player.SHOOT_STATE);
            }
            else if (inputInfo.fire && inputInfo.IsMoving())
            {
                player.SetState(Player.RUNSHOOT_STATE);
            }
            else if (!inputInfo.fire && inputInfo.IsMoving())
            {
                player.SetState(Player.RUN_STATE);
            }


        }

    }
}