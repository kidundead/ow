using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Zombie3D
{

    public class PlayerRunAndShootState : PlayerState
    {
        public override void NextState(Player player, float deltaTime)
        {
            InputController inputController = player.InputController;

            InputInfo inputInfo = new InputInfo();
            inputController.ProcessInput(deltaTime, inputInfo);

            player.ZoomIn(deltaTime);
            player.Move(inputInfo.moveDirection * (deltaTime * player.WalkSpeed * Constant.PLAYING_WALKINGSPEED_DISCOUNT_WHEN_SHOOTING));


            Weapon weapon = player.GetWeapon();
            if (weapon != null)
            {

                if (weapon.GetWeaponType() == WeaponType.Sniper)
                {
                    Sniper autoMissle = weapon as Sniper;
                    if (!autoMissle.HaveBullets())
                    {
                        player.SetState(Player.RUN_STATE);
                    }
                    else
                    {
                        if (inputInfo.fire)
                        {
                            autoMissle.AutoAim(deltaTime);
                            player.Animate(AnimationName.PLAYER_RUN + player.WeaponNameEnd, WrapMode.Loop);
                        }
                        else
                        {

                            if (autoMissle.AimedTarget())
                            {
                                player.Fire(deltaTime);
                                player.Animate(AnimationName.PLAYER_RUNFIRE + player.WeaponNameEnd, WrapMode.Once);

                            }

                        }
                    }
                }
                else
                {
                    weapon.FireUpdate(deltaTime);
                    if (inputInfo.fire)
                    {
                        if (!weapon.HaveBullets())
                        {
                            player.SetState(Player.RUN_STATE);
                        }
                        else if (!weapon.CouldMakeNextShoot())
                        {
                            //weapon.FireUpdate(deltaTime);
                        }
                        else
                        {
                            //weapon.FireUpdate(deltaTime);
                            player.Fire(deltaTime);
                            WeaponType wType = weapon.GetWeaponType();
                            switch (wType)
                            {
                                case WeaponType.RocketLauncher:
                                    player.Animate(AnimationName.PLAYER_RUNFIRE + player.WeaponNameEnd, WrapMode.Once);
                                    break;
                                case WeaponType.ShotGun:
                                    player.Animate(AnimationName.PLAYER_RUNFIRE + player.WeaponNameEnd, WrapMode.ClampForever);
                                    break;
                                case WeaponType.Saw:
                                    player.RandomSawAnimation();
                                    player.Animate(AnimationName.PLAYER_RUNFIRE + player.WeaponNameEnd, WrapMode.Loop);
                                    break;
                                default:
                                    player.Animate(AnimationName.PLAYER_RUNFIRE + player.WeaponNameEnd, WrapMode.Loop);
                                    break;
                            }


                        }
                    }

                }

            }

            bool isPlayingAnimation = player.IsPlayingAnimation(AnimationName.PLAYER_RUNFIRE + player.WeaponNameEnd);
            bool animationEnds = player.AnimationEnds(AnimationName.PLAYER_RUNFIRE + player.WeaponNameEnd);

            if ((isPlayingAnimation && animationEnds) || !isPlayingAnimation)
            {
                /*
                if (isPlayingAnimation && animationEnds)
                {
                    Debug.Log("animation ends");
                }
                else if (!isPlayingAnimation)
                {
                    Debug.Log("animation stops");
                }
                */
                if (!inputInfo.fire && !inputInfo.IsMoving())
                {
                    player.StopFire();
                    player.SetState(Player.IDLE_STATE);
                }
                else if (!inputInfo.fire && inputInfo.IsMoving())
                {
                    player.StopFire();
                    player.SetState(Player.RUN_STATE);
                }
                else if (inputInfo.fire && !inputInfo.IsMoving())
                {
                    player.SetState(Player.SHOOT_STATE);
                }
                else
                {
                    if (weapon.GetWeaponType() == WeaponType.RocketLauncher)
                    {
                        player.Animate(AnimationName.PLAYER_RUN + player.WeaponNameEnd, WrapMode.Loop);
                    }
                    else if (weapon.GetWeaponType() == WeaponType.ShotGun)
                    {
                        player.Animate(AnimationName.PLAYER_RUN02 + player.WeaponNameEnd, WrapMode.Loop);
                    }
                    else if (weapon.GetWeaponType() == WeaponType.AssaultRifle
                        || weapon.GetWeaponType() == WeaponType.MachineGun
                        || weapon.GetWeaponType() == WeaponType.LaserGun)
                    {
                        player.Animate(AnimationName.PLAYER_RUNFIRE + player.WeaponNameEnd, WrapMode.Loop);
                    }

                }
            }


        }

    }
}