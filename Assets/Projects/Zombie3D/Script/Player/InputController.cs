using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Zombie3D
{

    public class InputInfo
    {
        public bool fire = false;
        public bool stopFire = false;
        public Vector3 moveDirection = Vector3.zero;

        public bool IsMoving()
        {
            if (moveDirection.x != 0 || moveDirection.z != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
               
        }

    }


    public abstract class InputController
    {
        //Input data: only process two fingers
        protected Touch[] lastTouch = new Touch[2];
        protected Vector2 cameraRotation = new Vector2(0, 0);
        protected Vector2 deflection;
        //Thumb for moving input
        protected Vector2 thumbCenter;
        protected Vector2 thumbCenterToScreen;

        protected Vector2 shootThumbCenter;
        protected Vector2 shootThumbCenterToScreen;
        protected Vector2 lastShootTouch = new Vector2();


        protected float touchX = 0;
        protected float touchY = 0;
        protected float thumbRadius;

        protected int thumbTouchFingerId = -1;
        protected int shootingTouchFingerId = -1;
        protected int moveTouchFingerId = -1;
        protected int moveTouchFingerId2 = -1;
        protected string phaseStr = ".";

        protected Vector3 moveDirection = Vector3.zero;
        protected GameScene gameScene;
        protected Player player;

        public bool EnableMoveInput { get; set; }
        public bool EnableTurningAround { get; set; }
        public bool EnableShootingInput { get; set; }


        public int GetMoveTouchFingerID()
        {
            return thumbTouchFingerId;
        }

        public int GetShootingTouchFingerID()
        {
            return shootingTouchFingerId;
        }

        public string PhaseStr
        {
            get
            {
                return phaseStr;
            }
        }


        public Vector2 LastTouchPos
        {
            get
            {
                return new Vector2(thumbCenterToScreen.x + touchX * thumbRadius, thumbCenterToScreen.y + touchY * thumbRadius);
            }
        }

        public Vector2 LastShootTouch
        {
            get
            {
                return new Vector2(lastShootTouch.x, lastShootTouch.y);
            }
        }

        public Vector2 ThumbCenter
        {
            get
            {
                return thumbCenter;
            }
        }

        public Vector2 ShootThumbCenter
        {
            get
            {
                return shootThumbCenter;
            }
        }

        public float ThumbRadius
        {
            get
            {
                return thumbRadius;
            }
        }

        public Vector2 CameraRotation
        {
            get
            {
                return cameraRotation;
            }
            set
            {
                cameraRotation = value;
            }
        }

        public Vector2 Deflection
        {
            get
            {
                return deflection;
            }
        }




        public void Init()
        {
            thumbCenter.x = AutoRect.AutoX(110);
            thumbCenter.y = AutoRect.AutoY(530) ;
            thumbRadius = AutoRect.AutoValue(85) ;
           

            shootThumbCenter.x = AutoRect.AutoX(852) ;
            shootThumbCenter.y = AutoRect.AutoY(530);

            if (AutoRect.GetPlatform() == Platform.IPad)
            {
                thumbCenter.x = AutoRect.AutoX(66);
                shootThumbCenter.x = AutoRect.AutoX(896);
                thumbCenter.y = AutoRect.AutoY(500);
                shootThumbCenter.y = AutoRect.AutoY(500);
            }



            thumbCenterToScreen = new Vector2(thumbCenter.x, Screen.height - thumbCenter.y);
            shootThumbCenterToScreen = new Vector2(shootThumbCenter.x, Screen.height - shootThumbCenter.y);

            lastShootTouch = shootThumbCenterToScreen;
            for (int i = 0; i < 2; i++)
            {
                lastTouch[i] = new Touch();
            }

            gameScene = GameApp.GetInstance().GetGameScene();
            player = gameScene.GetPlayer();

            EnableMoveInput = true;
            EnableShootingInput = true;
            EnableTurningAround = true;

        }

        public abstract void ProcessInput(float deltaTime, InputInfo inputInfo);


    }
}