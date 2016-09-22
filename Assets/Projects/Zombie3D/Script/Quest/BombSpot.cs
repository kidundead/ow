using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Zombie3D
{
    public class BombSpot
    {

        public enum BombSpotState
        {
            UnInstalled,
            Installing,
            Installed
        }

        public GameScene gameScene;
        public GameObject bombSpotObj;
        public float spotRadius = 5.0f;
        public float installTimeTakes = 5.0f;
        protected float lastInstallTime = 0;
        protected BombSpotState bss = BombSpotState.UnInstalled;
        protected Transform spotTransform;

        public BombSpotState GetSpotState()
        {
            return bss;
        }
        // Use this for initialization
        public void Init()
        {

            spotTransform = bombSpotObj.transform;
            gameScene = GameApp.GetInstance().GetGameScene();
        }

        // Update is called once per frame
        public void DoLogic()
        {
            //spotTransform.Rotate(0f, 145f * Time.deltaTime, 0f);
            if (bss == BombSpotState.Installing && Time.time - lastInstallTime > installTimeTakes)
            {
                bss = BombSpotState.Installed;
                //bombSpotObj.renderer.enabled = false;
                bombSpotObj.transform.Find("glow").GetComponent<Renderer>().enabled = true;
                BombQuest bq = gameScene.GetQuest() as BombQuest;
                bq.CheckAllBombComplete();

            }

            if (bss == BombSpotState.Installing)
            {
                Hashtable enemies = gameScene.GetEnemies();
                foreach (Enemy enemy in enemies.Values)
                {
                    if (enemy.GetState() != Enemy.DEAD_STATE)
                    {
                        if ((enemy.GetPosition() - spotTransform.position).sqrMagnitude < spotRadius * spotRadius)
                        {
                            bss = BombSpotState.UnInstalled;
                            //Debug.Log("UnInstalled..");
                            break;
                        }
                    }

                }
            }
        }

        public bool CheckInSpot()
        {
            if (bss != BombSpotState.UnInstalled)
            {
                return false;
            }
            Player player = gameScene.GetPlayer();
            float dis = (player.GetTransform().position - spotTransform.position).sqrMagnitude;


            if (dis < spotRadius * spotRadius)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public void Install()
        {
            lastInstallTime = Time.time;
            bss = BombSpotState.Installing;
        }

        public bool isInstalling()
        {
            return (bss == BombSpotState.Installing);
        }

        public float GetInstallingProgress()
        {
            return (Time.time - lastInstallTime) / installTimeTakes;
        }
    }

}
