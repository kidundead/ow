using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//Mission: Survival Mode


namespace Zombie3D
{
    public class SurvivalQuest : Quest
    {

        protected int enemyKilled;
        protected float survivalTime;
        protected float startedTime;
        protected int timeSurvive;

        // Use this for initialization
        public override void Init()
        {
            base.Init();

            //GameParametersScript gParam = gameScene.GetGameParameters();
            //survivalTime = gParam.SurviveTime;
            questType = QuestType.KillAll;
            startedTime = Time.time;

        }

        public override void DoLogic()
        {
            base.DoLogic();

            Player player = gameScene.GetPlayer();

            /*
            if (player.GetState() != EPlayerState.Dead)
            {
                timeSurvive = (int)(Time.time - startedTime);
            }
            */

        }

        public override string GetQuestInfo()
        {


            string timeStr = string.Format("{0:00}", timeSurvive / 60) + ":" + string.Format("{0:00}", timeSurvive % 60);
            //string questInfo = "Mission: Survived " + timeStr +"   Kills " + gameScene.Killed;
            
            
            string questInfo = timeStr;
           
            
            /*
            if (questCompleted)
            {
                questInfo = "Mission: Time's Up";
            }
             */
            return questInfo;
        }

    }
}