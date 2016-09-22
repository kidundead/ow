using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


namespace Zombie3D
{

    /*    Game Scene Class: Playing for One Game Scene
     *    Manage All Game Objects in one game
     * 
     * 
     */

    public enum PlayingState
    {
        GamePlaying,
        GameWin,
        GameLose,
        GameQuit
    }


    public class GameScene
    {
        protected Player player;
        protected BaseCameraScript camera;
        protected Hashtable enemyList;
        protected Quest quest;
        protected List<BombSpot> bombSpotList;
        protected List<TriggerScript> triggerList;
        protected GameObject[] woodboxList;
        protected Vector3[] path;

        //protected GameParametersScript gameParameters;

        protected ObjectPool hitBloodObjectPool = new ObjectPool();
        protected ObjectPool[] deadBodyObjectPool = new ObjectPool[6];
        protected ObjectPool[] enemyObjectPool = new ObjectPool[6];


        public Weapon BonusWeapon { get; set; }

        protected string sceneName;
        protected int infectionRate;
        protected int sceneIndex;
        protected int enemyNum;

        protected PlayingState playingState;

        protected float difficultyDamageFactor = 1.0f;
        protected float difficultyHpFactor = 1.0f;
        protected float difficultyCashDropFactor = 1.0f;
        protected int killed;
        protected int triggerCount;
        protected int enemyID;
        protected float spawnWoodBoxesTime = -30.0f;
        protected float lastPrintTime = 0.0f;

        protected ArenaTriggerFromConfigScript arenaTrigger;
        public ArenaTriggerFromConfigScript ArenaTrigger
        {
            get
            {
                return arenaTrigger;
            }
            set
            {
                arenaTrigger = value;
            }
        }

        public void Init(int index)
        {
            GameApp.GetInstance().DebugInfo = "";
            sceneIndex = index;
            //! sceneName = Application.loadedLevelName.Substring(9);
			sceneName = SceneManager.GetSceneAt(0).name;       //! 保存场景名字

            //infectionRate = GameApp.GetInstance().GetGameState().GetInfectionRate(index);
			//! 读取 ai 路标 箱子列表
            CreateSceneData();

			//! 打中时流血的例子粒子效果
            hitBloodObjectPool.Init("HitBlood", GameApp.GetInstance().GetResourceConfig().hitBlood, 3, 0.4f);

            for (int i = 0; i < deadBodyObjectPool.Length; i++)
            {
                deadBodyObjectPool[i] = new ObjectPool();
                enemyObjectPool[i] = new ObjectPool();
            }

			//! 死亡时的mesh网格
            deadBodyObjectPool[0].Init("DeadBody_Zombie", GameApp.GetInstance().GetResourceConfig().deadbody[0], 10, 2.0f);  //! zombie 
            deadBodyObjectPool[1].Init("DeadBody_Nurse", GameApp.GetInstance().GetResourceConfig().deadbody[1], 5, 2.0f);    //! 护 士
            deadBodyObjectPool[4].Init("DeadBody_Boomer", GameApp.GetInstance().GetResourceConfig().deadbody[4], 5, 2.0f);   //! 炸弹 人
            deadBodyObjectPool[5].Init("DeadBody_Swat", GameApp.GetInstance().GetResourceConfig().deadbody[5], 5, 2.0f);     //! 特种兵

			//! 敌人mesh
            enemyObjectPool[0].Init("Zombies", GameApp.GetInstance().GetResourceConfig().enemy[0], 10, 0f);
            enemyObjectPool[1].Init("Nurses", GameApp.GetInstance().GetResourceConfig().enemy[1], 5, 0f);
            enemyObjectPool[2].Init("Tanks", GameApp.GetInstance().GetResourceConfig().enemy[2], 2, 0f);
            enemyObjectPool[3].Init("Hunters", GameApp.GetInstance().GetResourceConfig().enemy[3], 2, 0f);
            enemyObjectPool[4].Init("Boomers", GameApp.GetInstance().GetResourceConfig().enemy[4], 5, 0f);
            enemyObjectPool[5].Init("Swats", GameApp.GetInstance().GetResourceConfig().enemy[5], 5, 0f);

            
            camera = GameObject.Find("Main Camera").GetComponent<TPSSimpleCameraScript>();

            if (camera == null)
            {
                camera = GameObject.Find("Main Camera").GetComponent<TopWatchingCameraScript>();
            }
            else if (!camera.enabled)
            {
                camera = GameObject.Find("Main Camera").GetComponent<TopWatchingCameraScript>();
            }

			//! 创建Player
            player = new Player();
            player.Init();
            camera.Init();

            enemyList = new Hashtable();

            playingState = PlayingState.GamePlaying;


            GameApp.GetInstance().GetGameState().Achievement.CheckAchievemnet_NeverGiveUp();

            enemyNum = 0;
            killed = 0;
            triggerCount = 0;
            enemyID = 0;


            Color[] colors = {
                                 Color.white,
                                 Color.red,
                                 Color.blue,
                                 Color.yellow,
                                 Color.magenta,
                                 Color.gray,
                                 Color.grey,
                                 Color.cyan
                             };

            int cIndex= Random.Range(0, colors.Length);
            RenderSettings.ambientLight = colors[cIndex];
        }

        public void AddNetworkComponents()
        {
            NetworkView networkView = player.PlayerObject.AddComponent<NetworkView>();
            player.PlayerObject.AddComponent<PlayerNetworkViewScript>();
        }

        public Player GetPlayer()
        {
            return player;
        }

        public BaseCameraScript GetCamera()
        {
            return camera;
        }


        public Hashtable GetEnemies()
        {
            return enemyList;
        }

        public Enemy GetEnemyByID(string enemyID)
        {
            return (Enemy)(enemyList[enemyID]);
        }

        public PlayingState PlayingState
        {
            get
            {
                return playingState;
            }
            set
            {
                playingState = value;
            }
        }


        public int EnemyNum
        {
            get
            {
                return enemyNum;
            }
        }

        public int Killed
        {
            get
            {
                return killed;
            }
            set
            {
                killed = value;
            }
        }

        public int EnemyID
        {
            get
            {
                return enemyID;
            }

        }

        public int GetNextTriggerID()
        {
            triggerCount++;
            return triggerCount;
        }

        public int GetNextEnemyID()
        {
            enemyID++;
            return enemyID;
        }

        public int IncreaseKills()
        {
            killed++;
            return killed;
        }

        public ObjectPool HitBloodObjectPool
        {
            get
            {
                return hitBloodObjectPool;
            }
        }

        public ObjectPool GetDeadBodyPool(EnemyType eType)
        {
            return deadBodyObjectPool[(int)eType];
        }


        public ObjectPool GetEnemyPool(EnemyType eType)
        {
            return enemyObjectPool[(int)eType];
        }




        public void CalculateDifficultyFactor(int wave)
        {
            int d = (wave - 1)/3;
            difficultyDamageFactor = 1.0f + d * 0.1f;
            difficultyHpFactor = 1.0f;
            for (int i = 0; i < d; i++)
            {
                difficultyHpFactor *= (1 + 0.3f);
            }
           
            difficultyCashDropFactor = 1.0f + d * 0.05f;
        }


        public float GetDifficultyDamageFactor
        {

            get
            {
                return difficultyDamageFactor;
            }
            set
            {
                difficultyDamageFactor = value;
            }

        }

        public float GetDifficultyCashDropFactor
        {
            get
            {
                return difficultyCashDropFactor;
            }
            set

            {
                difficultyCashDropFactor = value;
            }
        }

        public float GetDifficultyHpFactor
        {

            get
            {
                return difficultyHpFactor;
            }
            set
            {
                difficultyHpFactor = value;
            }

        }



        public void ModifyEnemyNum(int num)
        {
            enemyNum += num;
        }


        public int GetSceneIndex()
        {
            return sceneIndex;
        }

        public int GetInfectionRate()
        {
            return infectionRate;
        }

        public string GetSceneName()
        {
            return sceneName;

        }

        public void AddTrigger(TriggerScript trigger)
        {
            triggerList.Add(trigger);
        }


        //If all enemies already spawned
        public bool TriggersAllMaxSpawned()
        {
            bool allSpawned = true;
            if (triggerList.Count == 0)
            {
                allSpawned = false;
            }

            foreach (TriggerScript t in triggerList)
            {
                if (!t.AlreadyMaxSpawned)
                {
                    allSpawned = false;
                }
            }
            return allSpawned;

        }

        public void DoLogic(float deltaTime)
        {
			if ( null != player ) 
           	    player.DoLogic(deltaTime);

            object[] keys = new object[enemyList.Count];


            enemyList.Keys.CopyTo(keys, 0);
            for (int i = 0; i < keys.Length; i++)
            {
                Enemy enemy = enemyList[keys[i]] as Enemy;
                enemy.DoLogic(deltaTime);                        //! 敌人的逻辑

               
            }

            /*
            if (Time.time - lastPrintTime > 10.0f)
            {
                for (int i = 0; i < keys.Length; i++)
                {
                    Enemy enemy = enemyList[keys[i]] as Enemy;
                    Debug.Log("Enemy Name:" + enemy.Name + "," + enemy.GetState());

                }

                
                lastPrintTime = Time.time;
            }
            */
            /*
            foreach (Enemy enemy in enemyList.Values)
            {
                enemy.DoLogic(deltaTime);
            }
            */

            /*
            quest.DoLogic();
            if (quest.QuestCompleted)
            {
                playingState = PlayingState.GameWin;
                player.GetWeapon().StopFire();

            }
            */

            hitBloodObjectPool.AutoDestruct();

            
            deadBodyObjectPool[0].AutoDestruct();
            deadBodyObjectPool[1].AutoDestruct();
            deadBodyObjectPool[4].AutoDestruct();
            deadBodyObjectPool[5].AutoDestruct();


        }

        public void CreateSceneData()
        {
            triggerList = new List<TriggerScript>();

			//! AI路标
            //prepare AI path points
            GameObject[] pathPoints = GameObject.FindGameObjectsWithTag("Path");
            path = new Vector3[pathPoints.Length];
            for (int i = 0; i < pathPoints.Length; i++)
            {
                path[i] = pathPoints[i].transform.position;
            }

			//! 会掉落物品的箱子
            //Get Item Wood Boxes
            woodboxList = GameObject.FindGameObjectsWithTag("WoodBox");

            /*
            //Create Quest
            QuestType qt = gameParameters.questType;

            //create mission
            switch (qt)
            {
                case QuestType.Bomb:
                    quest = new BombQuest();

                    CreateBombSpots();
                    quest.Init();
                    break;
                case QuestType.KillAll:
                    quest = new KillAllQuest();
                    quest.Init();
                    break;
                case QuestType.Survival:
                    quest = new SurvivalQuest();
                    quest.Init();
                    break;
            }
            */





        }

        /*
        public void UpdateDifficultyLevel(float difficultyFactor)
        {
            //simple level difficulty formula

            //int theday = GameApp.GetInstance().GetGameState().TheDays;
            //difficultyFactor = ((theday - 1) * 2) / 100.0f;

            gameParameters.ZombieAttack *= (1 + difficultyFactor);
            gameParameters.ZombieHP *= (1 + difficultyFactor);
            gameParameters.ZombieAttackFrenquency /= (1 + difficultyFactor);


            gameParameters.NurseAttack *= (1 + difficultyFactor);
            gameParameters.NurseHP *= (1 + difficultyFactor);
            gameParameters.NurseAttackFrenquency /= (1 + difficultyFactor);

            gameParameters.FatZombieAttack *= (1 + difficultyFactor);
            gameParameters.FatZombieHP *= (1 + difficultyFactor);
            gameParameters.FatZombieRushingFrenquency /= (1 + difficultyFactor);
            gameParameters.FatZombieRushingDamage *= (1 + difficultyFactor);
            gameParameters.FatZombieRushingAttackDamage *= (1 + difficultyFactor);

        }
        */

        public void ClearAllSceneResources()
        {
            player = null;
            enemyList.Clear();
            woodboxList = null;
            path = null;
            this.triggerList = null;
            camera = null;
        }

        public void CreateBombSpots()
        {
            //Add BombSpot into list
            bombSpotList = new List<BombSpot>();
            GameObject[] bsl = GameObject.FindGameObjectsWithTag("BombSpot");
            List<int> indexList = new List<int>();
            for (int i = 0; i < bsl.Length; i++)
            {
                indexList.Add(i);
            }


            //Random select 3 of the spots 
            if (indexList.Count > 0)
            {
                int rnd = Random.Range(0, indexList.Count);
                int no1 = indexList[rnd];
                indexList.Remove(no1);
                rnd = Random.Range(0, indexList.Count - 1);
                int no2 = indexList[rnd];
                indexList.Remove(no2);
                rnd = Random.Range(0, indexList.Count - 2);
                int no3 = indexList[rnd];
                for (int i = 0; i < bsl.Length; i++)
                {
                    if (i != no1 && i != no2 && i != no3)
                    {
                        GameObject.Destroy(bsl[i]);
                    }
                    else
                    {
                        BombSpot bs = new BombSpot();
                        bs.bombSpotObj = bsl[i];
                        bsl[i].active = true;
                        bs.Init();
                        bombSpotList.Add(bs);
                    }
                }
            }
        }

        public List<BombSpot> GetBombSpots()
        {
            return bombSpotList;
        }

        public Vector3[] GetPath()
        {
            return path;
        }
        public Quest GetQuest()
        {
            return quest;
        }

        public GameObject[] GetWoodBoxes()
        {
            return woodboxList;
        }

        public void RefreshWoodBoxes()
        {
            Object.Instantiate(GameApp.GetInstance().GetResourceConfig().woodBoxes);
            woodboxList = GameObject.FindGameObjectsWithTag("WoodBox");
            spawnWoodBoxesTime = Time.time;
        }



    }

}