using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Zombie3D
{
    public class KillAllQuest : Quest
    {

        protected int enemyLeft;

        // Use this for initialization
        public override void Init()
        {
            base.Init();
            questType = QuestType.KillAll;

        }

        public override void DoLogic()
        {
            base.DoLogic();
            enemyLeft = gameScene.EnemyNum;


            if (enemyLeft == 0 && gameScene.TriggersAllMaxSpawned())
            {
                questCompleted = true;
            }

        }

        public override string GetQuestInfo()
        {
            string monsterNum = "";
            if (enemyLeft < 10)
            {
                monsterNum = enemyLeft.ToString();
            }
            else
            {
                monsterNum = "???";
            }

            string questInfo = "Mission: Kill Them All  " + monsterNum;

            if (questCompleted)
            {
                questInfo = "Mission Complete";
            }
            return questInfo;
        }

    }
}