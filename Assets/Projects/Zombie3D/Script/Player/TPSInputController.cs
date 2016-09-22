using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Zombie3D
{

    public  class TouchInfo
    {
        public Vector2 position = Vector2.zero;
        public TouchPhase phase = TouchPhase.Began;
    }
    public class TPSInputController : InputController
    {

        protected TouchInfo lastMoveTouch = new TouchInfo();
        protected TouchInfo lastMoveTouch2 = new TouchInfo();

        public override void ProcessInput(float deltaTime, InputInfo inputInfo)
        {
            //process input from both keyboard/mouse and touch screen
            Weapon weapon = player.GetWeapon();
            GameObject playerObject = player.PlayerObject;
            Transform playerTransform = player.GetTransform();
            Vector3 getHitFlySpeed = player.GetHitFlySpeed;
            List<Weapon> weaponList = GameApp.GetInstance().GetGameState().GetBattleWeapons();




            if (Application.platform != RuntimePlatform.IPhonePlayer)
            {
                //In Editor
                if (EnableShootingInput)
                {
                    if (Input.GetButton("Fire1"))
                    {
                        inputInfo.fire = true;
                        //player.Fire();
                    }
                    else
                    {
                        inputInfo.stopFire = true;
                        //player.StopFire();
                    }
                }

                if (EnableMoveInput)
                {
                    moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                }

                if (EnableTurningAround)
                {
                    cameraRotation.x = player.InputController.CameraRotation.x;//Input.GetAxis("Mouse X");
                    cameraRotation.y = player.InputController.CameraRotation.y;//Input.GetAxis("Mouse Y");
                }
            }
            else
            {

                //Touch screen Input processing: Infinite Input Style

                touchX = 0;
                touchY = 0;

                cameraRotation.x = 0;
                cameraRotation.y = 0;

                bool fired = false;
                if (Input.touchCount == 0)
                {
                    thumbTouchFingerId = -1;
                    shootingTouchFingerId = -1;
                    lastShootTouch = shootThumbCenterToScreen;
                }


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

                    if (touch.phase == TouchPhase.Began)
                    {
                        /*
                        if (AutoRect.GetPlatform() == Platform.IPad)
                        {
                            if (touch.position.x < Screen.width * 0.5f)
                            {
                                thumbCenter = new Vector2(touch.position.x, Screen.height - touch.position.y);


                            }
                            else
                            {
                                shootThumbCenter = new Vector2(touch.position.x, Screen.height - touch.position.y);
                            }

                            thumbCenterToScreen = new Vector2(thumbCenter.x, Screen.height - thumbCenter.y);
                            shootThumbCenterToScreen = new Vector2(shootThumbCenter.x, Screen.height - shootThumbCenter.y);

                        }
                        */


                    }
                    else if (touch.phase == TouchPhase.Stationary)
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
                            if (EnableShootingInput)
                            {
                                Vector2 diffShootTouch = touch.position - shootThumbCenterToScreen;
                                bool inShootThumb = diffShootTouch.sqrMagnitude < thumbRadius * thumbRadius;


                                if (inShootThumb || shootingTouchFingerId == touch.fingerId)
                                {


                                    if (inShootThumb)
                                    {
                                        cameraRotation.x = Mathf.Clamp(diffShootTouch.x, -thumbRadius, thumbRadius) * 0.005f;
                                        lastShootTouch = touch.position;
                                    }
                                    else
                                    {
                                        cameraRotation.x = Mathf.Sign(diffShootTouch.x) * thumbRadius * 0.01f;
                                        Vector2 d = (touch.position - shootThumbCenterToScreen).normalized;
                                        lastShootTouch = shootThumbCenterToScreen + d * thumbRadius;
                                    }

                                    //cameraRotation.y = Mathf.Sign(diffShootTouch.y) * thumbRadius * 0.02f;

                                    inputInfo.fire = true;

                                    shootingTouchFingerId = touch.fingerId;

                                    fired = true;

                                }
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

                            if (EnableTurningAround)
                            {
                                if (lastMoveTouch.phase == TouchPhase.Moved)
                                {
                                    if (touch.fingerId == moveTouchFingerId)
                                    {
                                        cameraRotation.x = (touch.position.x - lastMoveTouch.position.x) * 0.3f;
                                        cameraRotation.y = (touch.position.y - lastMoveTouch.position.y) * 0.16f;
                                    }
                                    else if (touch.fingerId == moveTouchFingerId2)
                                    {
                                        cameraRotation.x = (touch.position.x - lastMoveTouch2.position.x) * 0.3f;
                                        cameraRotation.y = (touch.position.y - lastMoveTouch2.position.y) * 0.16f;
                                    }
                                }

                                if (moveTouchFingerId == -1)
                                {
                                    moveTouchFingerId = touch.fingerId;
                                    
                                }
                                if (moveTouchFingerId != -1 && touch.fingerId != moveTouchFingerId)
                                {
                                    moveTouchFingerId2 = touch.fingerId;
                                }

                                if (touch.fingerId == moveTouchFingerId)
                                {
                                    lastMoveTouch.phase = TouchPhase.Moved;
                                    lastMoveTouch.position = touch.position;
                                }

                                if (touch.fingerId == moveTouchFingerId2)
                                {
                                    lastMoveTouch2.phase = TouchPhase.Moved;
                                    lastMoveTouch2.position = touch.position;
                                }


                            }
                            Vector2 diffShootTouch = touch.position - shootThumbCenterToScreen;
                            bool inShootThumb = diffShootTouch.sqrMagnitude < thumbRadius * thumbRadius;

                            if (EnableShootingInput)
                            {
                                if (shootingTouchFingerId == touch.fingerId || inShootThumb)
                                {
                                    //player.Fire();
                                    inputInfo.fire = true;
                                    fired = true;

                                    if (inShootThumb)
                                    {
                                        cameraRotation.x += Mathf.Clamp(diffShootTouch.x, -thumbRadius, thumbRadius) * 0.002f;
                                        lastShootTouch = touch.position;
                                    }
                                    else
                                    {

                                        Vector2 d = (touch.position - shootThumbCenterToScreen).normalized;
                                        lastShootTouch = shootThumbCenterToScreen + d * thumbRadius;
                                        cameraRotation.x += Mathf.Sign(diffShootTouch.x) * thumbRadius * 0.006f;
                                        //cameraRotation.y = Mathf.Sign(diffShootTouch.y) * thumbRadius * 0.02f;
                                    }

                                    shootingTouchFingerId = touch.fingerId;

                                }
                                else
                                {

                                }
                            }
                        }
                    }
                    else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                    {
                        if (touch.fingerId == thumbTouchFingerId)
                        {
                            thumbTouchFingerId = -1;
                        }
                        //GameApp.GetInstance().DebugInfo = touch.phase + "," + touch.fingerId + "," + shootingTouchFingerId;
                        if (touch.fingerId == shootingTouchFingerId)
                        {
                            shootingTouchFingerId = -1;
                            lastShootTouch = shootThumbCenterToScreen;
                        }

                        if (touch.fingerId == moveTouchFingerId)
                        {
                            moveTouchFingerId = -1;
                            lastMoveTouch.phase = TouchPhase.Ended;
                        }
                        if (touch.fingerId == moveTouchFingerId2)
                        {
                            moveTouchFingerId2 = -1;
                            lastMoveTouch2.phase = TouchPhase.Ended;
                        }

                    }



                    lastTouch[i] = touch;
                }

                if (!fired)
                {
                    //player.StopFire();
                    inputInfo.stopFire = true;
                }

                touchX = Mathf.Clamp(touchX, -1, 1);
                touchY = Mathf.Clamp(touchY, -1, 1);
                //phaseStr += "touchXY " + touchX + "," + touchY;

                moveDirection = new Vector3(touchX, 0, touchY);
            }

            //move
            moveDirection = playerTransform.TransformDirection(moveDirection);
            if (!EnableMoveInput)
            {
                moveDirection = Vector3.zero;
            }
            if (!EnableShootingInput)
            {
                inputInfo.fire = false;
            }

            moveDirection += Physics.gravity * deltaTime * 10.0f;

            /*
            getHitFlySpeed.x = Mathf.Lerp(getHitFlySpeed.x, 0, 5.0f * deltaTime);
            getHitFlySpeed.y = Mathf.Lerp(getHitFlySpeed.y, 0, -Physics.gravity.y * deltaTime);
            getHitFlySpeed.z = Mathf.Lerp(getHitFlySpeed.z, 0, 5.0f * deltaTime);
            */
            inputInfo.moveDirection = moveDirection;



            for (int i = 1; i <= weaponList.Count; i++)
            {
                if (Input.GetButton("Weapon" + i))
                {
                    player.ChangeWeapon(weaponList[i - 1]);
                }

            }

            if (Input.GetButton("K"))
            {
                player.OnHit(player.GetMaxHp());
            }

            if (Input.GetButton("H"))
            {
                player.GetHealed((int)player.GetMaxHp());
            }


            if (Input.GetButtonDown("N"))
            {
                GameApp.GetInstance().GetGameState().LevelNum++;
                Debug.Log(GameApp.GetInstance().GetGameState().LevelNum);
                GameApp.GetInstance().Save();
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