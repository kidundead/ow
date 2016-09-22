using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Zombie3D
{

    public class PlayerGotHitState : PlayerState
    {
        public override void NextState(Player player, float deltaTime)
        {
            //player.ZoomOut(deltaTime);
            if (!player.IsPlayingAnimation(AnimationName.PLAYER_GOTHIT))
            {
                player.SetState(Player.IDLE_STATE);
            }

            

        }


    }
}