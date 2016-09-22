using UnityEngine;
using System.Collections;

namespace Zombie3D
{
    public enum QuestType
    {
        Bomb,
        KillAll,
        Survival
    }

    public abstract class Quest
    {

        protected bool questCompleted;
        protected QuestType questType;
        protected GameScene gameScene;

        public QuestType GetQuestType()
        {
            return questType;
        }

        public bool QuestCompleted
        {
            get
            {
                return questCompleted;
            }
        }

        // Use this for initialization
        public virtual void Init()
        {

            questCompleted = false;
            gameScene = GameApp.GetInstance().GetGameScene();
        }

        public virtual void DoLogic()
        {

        }

        public abstract string GetQuestInfo();
    }
}