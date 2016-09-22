using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;


namespace Zombie3D
{



    public class AchievementInfo
    {
        public string id;
        public bool submitting = false;
        public bool complete = false;
        
    }

    public class ScoreInfo
    {
        public string id = "com.trinitigame.callofminizombies.l1";
        public int score = 0;
    }

    public class AchievementState
    {
        protected int score = 0;
        protected int newWeaponsGot = 0;
        protected int newAvatarGot = 0;
        protected int enemyKills = 0;
        protected int sawKills = 0;
        protected int loseTimes = 0;
        protected int upgradeTenTimes = 0;

        protected const int ACHIEVEMENT_COUNT = 16;
        protected AchievementInfo[] acheivements = new AchievementInfo[ACHIEVEMENT_COUNT];
        protected ScoreInfo scoreInfo = new ScoreInfo();

        public AchievementState()
        {
            GameCenterInterface.Initialize();
            for (int i = 0; i < ACHIEVEMENT_COUNT; i++)
            {
                acheivements[i] = new AchievementInfo();
                acheivements[i].id = "com.trinitigame.callofminizombies.a" + (i+1);
            }
        }

        public void SubmitScore(int score)
        {

            scoreInfo.score = score;

        }

        public void SubmitAllToGameCenter()
        {

            if (GameCenterInterface.IsLogin())
            {

                for (int i = 0; i < ACHIEVEMENT_COUNT; i++)
                {
                    if (acheivements[i].submitting)
                    {
                        if (GameCenterInterface.SubmitAchievement(acheivements[i].id, 100))
                        {
                            acheivements[i].submitting = false;
                        }
                    }

                }

                if (scoreInfo.score != 0)
                {
                    if (GameCenterInterface.SubmitScore(scoreInfo.id, scoreInfo.score))
                    {
                        scoreInfo.score = 0;
                    }
                }
            }
        }
       
        

        public void GotNewWeapon()
        {
            newWeaponsGot++;
            CheckAchievemnet_NewBattleAbility();
            CheckAchievemnet_WeaponHouseware();
            CheckAchievemnet_WeaponCollector();
        }


        public void GotNewAvatar()
        {
            newAvatarGot++;
            CheckAchievemnet_Avatar();
            CheckAchievemnet_AvatarMaster();
        }

        public void UpgradeTenTimes()
        {
            upgradeTenTimes++;
            CheckAchievemnet_Upgrade();
            CheckAchievemnet_UpgradeMaster();
        }

        public void KillEnemy()
        {
            enemyKills++;
            if (GameApp.GetInstance().GetGameScene().GetPlayer().GetWeapon().GetWeaponType() == WeaponType.Saw)
            {
                sawKills++;
                CheckAchievemnet_SawKillers();
            }

            CheckAchievemnet_TookAShoot();
            CheckAchievemnet_Killer();

        }

        public void LoseGame()
        {
            loseTimes++;

        }

        public void AddScore(int scoreAdd)
        {
            score += scoreAdd;
        }



        public void Save(BinaryWriter bw)
        {
            bw.Write(score);
            bw.Write(newWeaponsGot);
            bw.Write(enemyKills);
            bw.Write(loseTimes);
            bw.Write(newAvatarGot);
            bw.Write(upgradeTenTimes);

            for (int i = 0; i < ACHIEVEMENT_COUNT; i++)
            {
                bw.Write(acheivements[i].submitting);
                bw.Write(acheivements[i].complete);
            }
        }

        public void Load(BinaryReader br)
        {
            score = br.ReadInt32();
            newWeaponsGot = br.ReadInt32();
            enemyKills = br.ReadInt32();
            loseTimes = br.ReadInt32();
            newAvatarGot = br.ReadInt32();
            upgradeTenTimes = br.ReadInt32();
            for (int i = 0; i < ACHIEVEMENT_COUNT; i++)
            {
                acheivements[i].submitting = br.ReadBoolean();
                acheivements[i].complete = br.ReadBoolean();
            }
        }


        public void CheckAchievemnet_NewBattleAbility()
        {
            if (acheivements[0].complete)
            {
                return;
            }
            if (newWeaponsGot == 1)
            {
                Debug.Log("Achievement: NewBattleAbility!");
                acheivements[0].submitting = true;
                acheivements[0].complete = true;
            }

        }

        public void CheckAchievemnet_WeaponHouseware()
        {
            if (acheivements[1].complete)
            {
                return;
            }
            if (newWeaponsGot == 5)
            {
                Debug.Log("Achievement: Weapon Store!");
                acheivements[1].submitting = true;
                acheivements[1].complete = true;

            }
        }

        public void CheckAchievemnet_SawKillers()
        {
            if (acheivements[2].complete)
            {
                return;
            }
            if (sawKills == 300)
            {
                Debug.Log("Achievement: SawKillers!");
                acheivements[2].submitting = true;
                acheivements[2].complete = true;
            }

        }

        public void CheckAchievemnet_TookAShoot()
        {
            if (acheivements[3].complete)
            {
                return;
            }
            if (enemyKills == 10)
            {
                Debug.Log("Achievement: Took A Shoot!");
                acheivements[3].submitting = true;
                acheivements[3].complete = true;
            }
        }

        public void CheckAchievemnet_BraveHeart()
        {
            if (acheivements[4].complete)
            {
                return;
            }
            Debug.Log("Achievement: BraveHeart!");
            acheivements[4].submitting = true;
            acheivements[4].complete = true;
        }

        public void CheckAchievemnet_Killer()
        {
            if (acheivements[5].complete)
            {
                return;
            }

            if (enemyKills == 3000)
            {
                Debug.Log("Achievement: Killer!");
                acheivements[5].submitting = true;
                acheivements[5].complete = true;
            }
        }

        public void CheckAchievemnet_RichMan(int cash)
        {
            if (acheivements[6].complete)
            {
                return;
            }
            if (cash > 100000)
            {
                Debug.Log("Achievement: Rich Man!");

                acheivements[6].submitting = true;
                acheivements[6].complete = true;
            }
        }




        public void CheckAchievemnet_Survivior(int level)
        {
            if (acheivements[7].complete)
            {
                return;
            }
            if (level == 30)
            {
                Debug.Log("Achievement: Survivior!");
                acheivements[7].submitting = true;
                acheivements[7].complete = true;
            }
        }


        public void CheckAchievemnet_LastSurvivior(int level)
        {
            if (acheivements[8].complete)
            {
                return;
            }
            if (level == 50)
            {
                Debug.Log("Achievement: LastSurvivior!");
                acheivements[8].submitting = true;
                acheivements[8].complete = true;
            }
        }

        public void CheckAchievemnet_Avatar()
        {
            if (acheivements[9].complete)
            {
                return;
            }
            if (newAvatarGot == 1)
            {
                Debug.Log("Achievement: Avatar!");
                acheivements[9].submitting = true;
                acheivements[9].complete = true;
            }
        }

        public void CheckAchievemnet_AvatarMaster()
        {
            if (acheivements[10].complete)
            {
                return;
            }
            if (newAvatarGot == 4)
            {
                Debug.Log("Achievement: AvatarMaster!");
                acheivements[10].submitting = true;
                acheivements[10].complete = true;
            }
        }


        public void CheckAchievemnet_UpgradeMaster()
        {
            if (acheivements[11].complete)
            {
                return;
            }
            if (upgradeTenTimes == 3)
            {
                Debug.Log("Achievement: UpgradeMaster!");
                acheivements[11].submitting = true;
                acheivements[11].complete = true;
            }
        }


        public void CheckAchievemnet_Upgrade()
        {
            if (acheivements[12].complete)
            {
                return;
            }
            Debug.Log("Achievement: Upgrade!");
            acheivements[12].submitting = true;
            acheivements[12].complete = true;
        }


      

        public void CheckAchievemnet_WeaponMaster()
        {
            if (acheivements[13].complete)
            {
                return;
            }
            Debug.Log("Achievement: WeaponMaster!");
            acheivements[13].submitting = true;
            acheivements[13].complete = true;
        }

        public void CheckAchievemnet_NeverGiveUp()
        {
            if (acheivements[14].complete)
            {
                return;
            }
            if (loseTimes == 100)
            {
                Debug.Log("Achievement: NeverGiveUp!");
                acheivements[14].submitting = true;
                acheivements[14].complete = true;
            }
        }




        public void CheckAchievemnet_WeaponCollector()
        {
            if (acheivements[15].complete)
            {
                return;
            }
            if (GameApp.GetInstance().GetGameState().GotAllWeapons())
            {
                Debug.Log("Achievement: WeaponCollector!");
                acheivements[15].submitting = true;
                acheivements[15].complete = true;
            }

        }



    }

}