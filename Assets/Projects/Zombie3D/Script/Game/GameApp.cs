using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;

namespace Zombie3D
{


    /*  Application Class
     * 
     * 
     * 
     */

    public class GameApp
    {

        protected static GameApp instance;
        protected ResourceConfigScript resourceConfig;
        protected GameConfig gameConfig = new GameConfig();
        protected GameState gameState = new GameState();
        protected GameScene scene;
        public DeviceOrientation PreviousOrientation { get; set; }
           
        protected GameScript script;
        public string DebugInfo { get; set; }

        protected GameApp()
        {
        }

        public static GameApp GetInstance()
        {
            if (instance == null)
            {
                instance = new GameApp();
                instance.PreviousOrientation = DeviceOrientation.Portrait;
                //FlurryTAd.Init("1.0", "SVUM3MKWJ9LRFPFQJ1F4");
            }
            return instance;
        }


        public void Save()
        {
            string path = Application.dataPath + "/../../Documents/";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            Stream stream = File.Open(path + "MySavedGame.game", FileMode.Create);

            BinaryWriter bw = new BinaryWriter(stream);
            gameState.SaveData(bw);
            bw.Close();
            stream.Close();

        }

        public bool Load()
        {
            string path = Application.dataPath + "/../../Documents/";
            if (File.Exists(path + "MySavedGame.game"))
            {
                Stream stream = File.Open(path + "MySavedGame.game", FileMode.Open);
                BinaryReader br = new BinaryReader(stream);
                gameState.LoadData(br);
                br.Close();
                stream.Close();
                return true;
            }
            else
            {
                return false;
            }
        }


        public void Init()
        {
            LoadResource();   //! 装载资源
            LoadConfig();     //! 装载配置文件
            InitGameState();
        }

        public void LoadResource()
        {
            //Debug.Log("Load Game Resources");
            resourceConfig = GameObject.Find("ResourceConfig").GetComponent<ResourceConfigScript>();
        }

        public void LoadConfig()
        {

            if (gameConfig.monsterConfTable.Count == 0)
            {
                //Debug.Log("Load Game Config");
                if (Application.isEditor || Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    gameConfig.LoadFromXML(null);
                }
                else
                {
                    gameConfig.LoadFromXML("/");
                }

            }
        }

        public void InitGameState()
        {
            gameState.Init();
        }


        public void CreateScene()
        {
            script = GameObject.Find("GameApp").GetComponent<GameScript>();
            scene = new GameScene();
            //! scene.Init(Application.loadedLevel - 1);
			scene.Init(SceneManager.GetSceneAt(0).buildIndex-1);
			//! SceneManager.LoadScene(SceneManager.GetSceneAt(0).buildIndex, LoadSceneMode.Single);
		}

        public void AddMultiplayerComponents()
        {
            scene.AddNetworkComponents();
        }

        public void ClearScene()
        {
            scene = null;
        }

        //game loop
        public void Loop(float deltaTime)
        {
            scene.DoLogic(deltaTime);
        }

        public ResourceConfigScript GetResourceConfig()
        {
            return resourceConfig;
        }

        public GameScene GetGameScene()
        {
            return scene;
        }

        public GameState GetGameState()
        {
            return gameState;
        }

        public GameConfig GetGameConfig()
        {
            return gameConfig;
        }
    }
}