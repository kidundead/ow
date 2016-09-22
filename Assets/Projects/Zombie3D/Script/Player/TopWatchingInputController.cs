using UnityEngine;
using System.Collections;
using System.Collections.Generic;



namespace Zombie3D
{
    public class TopWatchingInputController : InputController
    {

        public override void ProcessInput(float deltaTime, InputInfo inputInfo)
        {
            //process input from both keyboard/mouse and touch screen
            Weapon weapon = player.GetWeapon();
            GameObject playerObject = player.PlayerObject;
            Vector3 getHitFlySpeed = player.GetHitFlySpeed;
            List<Weapon> weaponList = GameApp.GetInstance().GetGameState().GetWeapons();
            Transform respawnTrans = player.GetRespawnTransform();
            

            if (Application.platform != RuntimePlatform.IPhonePlayer)
            {
                //In Editor
                if (Input.GetButton("Fire1"))
                {
                    player.Fire(deltaTime);
                }
                else
                {
                    player.StopFire();
                }
                moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            }
            else
            {

                //Touch screen Input processing: Infinite Input Style

                touchX = 0;
                touchY = 0;
                lastShootTouch = shootThumbCenterToScreen;
                cameraRotation.x = 0;
                cameraRotation.y = 0;

                bool fired = false;

                for (int i = 0; i < Input.touchCount; i++)
                {
                    if (i == 2)
                    {
                        break;
                    }

                    Touch touch = Input.GetTouch(i);
                    phaseStr = touch.phase.ToString() + touch.fingerId + " p:" + touch.position.x + "," + touch.position.y;


                    /*
                    if (touch.phase != TouchPhase.Stationary)
                    {
                        stationaryStartTime[i] = -1f;
                    }
                     * */
                    Vector2 diff = touch.position - thumbCenterToScreen;

                    bool inThumb = diff.sqrMagnitude < thumbRadius * thumbRadius;
                    bool stillThumbFinger = (touch.fingerId == thumbTouchFingerId);
                    if (touch.phase == TouchPhase.Stationary)
                    {
                        /*
                        if (lastTouch[i].phase == TouchPhase.Began)
                        {
                            stationaryStartTime[i] = Time.time;
                        }
                        **/

                        if (inThumb || stillThumbFinger)
                        {

                            if (inThumb)
                            {
                                touchX = diff.x / thumbRadius;
                                touchY = diff.y / thumbRadius;

                            }
                            else
                            {
                                touchX = diff.x / thumbRadius;
                                touchY = diff.y / thumbRadius;

                                if (Mathf.Abs(touchX) > Mathf.Abs(touchY))
                                {
                                    touchY = touchY / Mathf.Abs(touchX);
                                    touchX = (touchX > 0) ? 1 : -1;
                                }
                                else
                                {
                                    if (touchY != 0)
                                    {
                                        touchX = touchX / Mathf.Abs(touchY);
                                        touchY = (touchY > 0) ? 1 : -1;
                                    }
                                    else
                                    {
                                        touchX = 0;
                                        touchY = 0;
                                    }


                                }


                            }
                            thumbTouchFingerId = touch.fingerId;


                            /*
                            if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
                            {
                                touchX = (diff.x > 0) ? 1 : -1;
                            }
                            else
                            {
                                touchY = (diff.y > 0) ? 1 : -1;
                            }
                            */


                        }
                        else
                        {

                            Vector2 diffShootTouch = touch.position - shootThumbCenterToScreen;

                            if (diffShootTouch.sqrMagnitude < thumbRadius * thumbRadius)
                            {
                                player.Fire(deltaTime);


                                shootingTouchFingerId = touch.fingerId;


                                fired = true;
                                lastShootTouch = touch.position;
                            }


                        }
                    }
                    else if (touch.phase == TouchPhase.Moved)
                    {

                        if (inThumb || stillThumbFinger)
                        {
                            if (inThumb)
                            {
                                touchX = diff.x / thumbRadius;
                                touchY = diff.y / thumbRadius;

                            }
                            else
                            {
                                touchX = diff.x / thumbRadius;
                                touchY = diff.y / thumbRadius;

                                if (Mathf.Abs(touchX) > Mathf.Abs(touchY))
                                {
                                    touchY = touchY / Mathf.Abs(touchX);
                                    touchX = (touchX > 0) ? 1 : -1;

                                }
                                else
                                {
                                    if (touchY != 0)
                                    {
                                        touchX = touchX / Mathf.Abs(touchY);
                                        touchY = (touchY > 0) ? 1 : -1;
                                    }
                                    else
                                    {
                                        touchX = 0;
                                        touchY = 0;
                                    }


                                }


                            }

                            thumbTouchFingerId = touch.fingerId;

                        }
                        else
                        {


                            cameraRotation.x = touch.deltaPosition.x * 0.2f;
                            cameraRotation.y = touch.deltaPosition.y * 0.2f;
                            Vector2 diffShootTouch = touch.position - shootThumbCenterToScreen;
                            bool inShootThumb = diffShootTouch.sqrMagnitude < thumbRadius * thumbRadius;
                            if (shootingTouchFingerId == touch.fingerId || inShootThumb)
                            {
                                player.Fire(deltaTime);

                                fired = true;
                                if (inShootThumb)
                                {
                                    lastShootTouch = touch.position;
                                }

                            }
                            else
                            {

                            }

                        }

                    }
                    else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                    {
                        if (touch.fingerId == thumbTouchFingerId)
                        {
                            thumbTouchFingerId = -1;
                        }

                        if (touch.fingerId == shootingTouchFingerId)
                        {
                            shootingTouchFingerId = -1;
                        }
                    }


                    lastTouch[i] = touch;
                }

                if (!fired)
                {
                    player.StopFire();
                }

                touchX = Mathf.Clamp(touchX, -1, 1);
                touchY = Mathf.Clamp(touchY, -1, 1);
                //phaseStr += "touchXY " + touchX + "," + touchY;

                moveDirection = new Vector3(touchX, 0, touchY);
            }


            //move
            moveDirection = respawnTrans.TransformDirection(moveDirection);
            moveDirection += Physics.gravity * deltaTime;

            getHitFlySpeed.x = Mathf.Lerp(getHitFlySpeed.x, 0, 5.0f * Time.deltaTime);
            getHitFlySpeed.y = Mathf.Lerp(getHitFlySpeed.y, 0, -Physics.gravity.y * Time.deltaTime);
            getHitFlySpeed.z = Mathf.Lerp(getHitFlySpeed.z, 0, 5.0f * Time.deltaTime);



            for (int i = 1; i <= 3; i++)
            {
                if (Input.GetButton("Weapon" + i))
                {
                    if (weaponList[i - 1] != null)
                    {

                        player.ChangeWeapon(weaponList[i - 1]);

                    }

                }

            }


            if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0 || touchX != 0 || touchY != 0)
            {
                player.Run();
            }
            else
            {
                player.StopRun();
            }

        }
    }
}