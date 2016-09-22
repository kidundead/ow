using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Zombie3D
{

    public class PlayerShootState : PlayerState
    {
        public override void NextState(Player player, float deltaTime)
        {
            InputController inputController = player.InputController;

            InputInfo inputInfo = new InputInfo();
            inputController.ProcessInput(deltaTime, inputInfo);
            player.ZoomIn(deltaTime);

            Weapon weapon = player.GetWeapon();
            
            if (weapon != null)
            {


                if (weapon.GetWeaponType() == WeaponType.Sniper)
                {
                    Sniper autoMissle = weapon as Sniper;
                    if (!autoMissle.HaveBullets())
                    {
                        player.SetState(Player.IDLE_STATE);
                    }
                    else
                    {
                        if (inputInfo.fire)
                        {
                            player.AutoAim(deltaTime);
                        }
                        else
                        {
                            if (autoMissle.AimedTarget())
                            {
                                player.Fire(deltaTime);
                            }
                        }
                    }

                    
                }
                else
                {
                    if (!weapon.HaveBullets())
                    {
                        player.SetState(Player.IDLE_STATE);
                    }
                    else if (!weapon.CouldMakeNextShoot())
                    {
                        weapon.FireUpdate(deltaTime);
                    }
                    else
                    {
                        //¹¥»÷
                        if (inputInfo.fire)
                        {
                            weapon.FireUpdate(deltaTime);
                            player.Fire(deltaTime);
                            WeaponType wType = weapon.GetWeaponType();
                            switch (wType)
                            {
                                case WeaponType.ShotGun:
                                case WeaponType.RocketLauncher:
                                    player.Animate(AnimationName.PLAYER_SHOT + player.WeaponNameEnd, WrapMode.Once);
                                    break;
                                case WeaponType.Saw:
                                    player.RandomSawAnimation();
                                    player.Animate(AnimationName.PLAYER_RUNFIRE + player.WeaponNameEnd, WrapMode.Loop);
                                    break;
                                default:
                                    player.Animate(AnimationName.PLAYER_SHOT + player.WeaponNameEnd, WrapMode.Loop);
                                    break;
                            }
                        }

                    }
                }


              

            }


            bool isPlayingAnimation = player.IsPlayingAnimation(AnimationName.PLAYER_SHOT + player.WeaponNameEnd);
            bool animationEnds = player.AnimationEnds(AnimationName.PLAYER_SHOT + player.WeaponNameEnd);

            if ((isPlayingAnimation && animationEnds) || !isPlayingAnimation)
            {
                
                if (!inputInfo.fire && !inputInfo.IsMoving())
                {                  
                    player.SetState(Player.IDLE_STATE);
                    player.StopFire();
                }
                else if (inputInfo.fire && inputInfo.IsMoving())
                {
                    player.SetState(Player.RUNSHOOT_STATE);
                }
                else if (!inputInfo.fire && inputInfo.IsMoving())
                {
                    player.SetState(Player.RUN_STATE);
                    player.StopFire();
                }
                else
                {
                    //¹¥»÷¼ä¸ô   
                    WeaponType wType = weapon.GetWeaponType();
                    if (wType == WeaponType.AssaultRifle 
                        || weapon.GetWeaponType() == WeaponType.MachineGun 
                        || weapon.GetWeaponType() == WeaponType.LaserGun
                        || weapon.GetWeaponType() == WeaponType.Saw
                        )
                    {
                        player.Animate(AnimationName.PLAYER_SHOT + player.WeaponNameEnd, WrapMode.Loop);
                    }
                    else
                    {
                        player.Animate(AnimationName.PLAYER_IDLE + player.WeaponNameEnd, WrapMode.Loop);
                    }
                }
            }

        
        }

    }
}