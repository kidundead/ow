using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Mission: Install the bombs and escape

namespace Zombie3D
{
    public class BombQuest : Quest
    {

        protected bool bombCompleted = false;
        protected Vector3 exitPosition;
        protected Renderer exitGlowRenderer;
        protected float radius = 2;
        protected int bombTotal;
        protected int bombLeft;

        // Use this for initialization
        public override void Init()
        {
            base.Init();

            Transform exitTrans = GameObject.Find("Exit").transform;
            exitPosition = exitTrans.position;
            exitGlowRenderer = exitTrans.Find("glow").GetComponent<Renderer>();
            exitGlowRenderer.enabled = false;

            questType = QuestType.Bomb;

            bombTotal = GameApp.GetInstance().GetGameScene().GetBombSpots().Count;
            bombLeft = bombTotal;
        }

        public override void DoLogic()
        {
            base.DoLogic();
            Player p = gameScene.GetPlayer();
            if (bombCompleted && (p.GetTransform().position - exitPosition).sqrMagnitude < radius * radius)
            {
                questCompleted = true;
            }

        }

        public void CheckAllBombComplete()
        {
            List<BombSpot> bsl = gameScene.GetBombSpots();
            bombLeft = bombTotal;
            foreach (BombSpot bs in bsl)
            {
                if (bs.GetSpotState() == BombSpot.BombSpotState.Installed)
                {
                    bombLeft--;
                }
            }

            if (bombLeft == 0)
            {
                bombCompleted = true;
                if (!exitGlowRenderer.enabled)
                {
                    exitGlowRenderer.enabled = true;
                }
            }
        }

        public override string GetQuestInfo()
        {
            string questInfo = "Mission: " + questType.ToString() + " " + bombLeft + "/" + bombTotal;
            if (bombCompleted && !questCompleted)
            {
                questInfo = "Mission: Bomb Complete, Get to The Exit!";
            }

            if (questCompleted)
            {
                questInfo = "Mission Complete";
            }

            return questInfo;
        }
    }
}