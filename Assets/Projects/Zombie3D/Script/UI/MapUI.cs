using UnityEngine;
using System.Collections;
using Zombie3D;
using System.IO;
using UnityEngine.SceneManagement;


class MapUIPosition
{

    public Rect Background = new Rect(0, 0, 960, 640);

    public Rect FactoryButton = new Rect(132, 370, 266, 202);
    public Rect HospitalButton = new Rect(406, 286, 164, 244);
    public Rect ParkingButton = new Rect(354, 72, 230, 184);
    public Rect VillageButton = new Rect(627, 298, 186, 260);

    public Rect ShopButton = new Rect(624, 58, 204, 192);

    public Rect OptionsButton = new Rect(808, 0, 130, 74);
    public Rect ReturnButton = new Rect(34, 0, 130, 70);

    public Rect DaysPanel = new Rect(70, 570, 264, 64);

}

public class MapUIPanel : UIPanel, UIHandler
{

    protected Rect[] buttonRect;

    //public string m_ui_material_path;  //最后带反斜杠
    //protected Material arenaMenuMaterial;
    protected UITextImage daysPanel;
    protected CashPanel cashPanel;
    protected UIClickButton returnButton;
    protected UIClickButton optionsButton;
    private MapUIPosition uiPos;
    protected bool buttonPressed = false;
    //private ArenaMenuTexturePosition texPos;

    protected float screenRatioX;
    protected float screenRatioY;


  
    protected bool init = false;
    protected bool setAudioTime = false;

    protected float startTime;
    protected Timer fadeTimer = new Timer();

    protected const int MAP_COUNT = 4;
    protected UIImage background;
    protected UIColoredButton[] mapButtons = new UIColoredButton[MAP_COUNT];
    protected UIClickButton shopButton = new UIClickButton();
    protected UIAnimatedImage[] zombieAnimations = new UIAnimatedImage[MAP_COUNT];

    protected int[] infection = new int[MAP_COUNT];
    

    // Use this for initialization
    public void Start()
    {
		//! for test
		GameApp.GetInstance().Init();
		GameApp.GetInstance().GetGameState().ClearState();
		GameApp.GetInstance().GetGameState ().InitWeapons();

	
        FlurryTAd.ShowTAd(false);
        startTime = Time.time;

        for (int i = 0; i < MAP_COUNT; i++)
        {
            infection[i] = i;
        }

        if (GameApp.GetInstance().GetGameState().LevelNum == 1)
        {
            infection[1] = -1;
            infection[2] = -1;
            infection[3] = -1;
        }
        else
        {
            //Random.seed = GameApp.GetInstance().GetGameState().LevelNum;
            Algorithem<int>.RandomSort(infection);
            int infectionNum = Random.Range(1, 3);
            if (infectionNum == 2)
            {
                infection[1] = -1;
                infection[2] = -1;
                infection[3] = -1;
            }
            else if (infectionNum == 1)
            {
                infection[2] = -1;
                infection[3] = -1;
            }
        }
        

        //int Random.Range(0, 2);

        Init();
        //StartCoroutine("Init");

    }

    void Init()
    {
        //yield return 1;


        GameApp.GetInstance().ClearScene();    //! 清除 GameScene

        SetUIHandler(this);
        uiPos = new MapUIPosition();
        //texPos = new ArenaMenuTexturePosition();
        Material mapMaterial = UIResourceMgr.GetInstance().GetMaterial("Map");      //! 装载地图材质
        background = new UIImage();
        background.SetTexture(mapMaterial, MapUITexturePosition.Background, AutoRect.AutoSize(MapUITexturePosition.Background));
        background.Rect = AutoRect.AutoPos(uiPos.Background);
        SetUIHandler(this);
        this.Add(background);
        for (int i = 0; i < MAP_COUNT; i++)
        {
            mapButtons[i] = new UIColoredButton();
            mapButtons[i].SetAnimatedColor(new Color(179.0f / 255f, 0, 0));            //! 场景按钮
            this.Add(mapButtons[i]);

            zombieAnimations[i] = new UIAnimatedImage();
            this.Add(zombieAnimations[i]);

			//! 僵尸动画
            zombieAnimations[i].SetAnimationFrameRate(5);
            zombieAnimations[i].AddAnimation(mapMaterial, MapUITexturePosition.ZombieAnimation1, AutoRect.AutoSize(MapUITexturePosition.ZombieAnimation1));
            zombieAnimations[i].AddAnimation(mapMaterial, MapUITexturePosition.ZombieAnimation2, AutoRect.AutoSize(MapUITexturePosition.ZombieAnimation2));
            zombieAnimations[i].AddAnimation(mapMaterial, MapUITexturePosition.ZombieAnimation3, AutoRect.AutoSize(MapUITexturePosition.ZombieAnimation3));

            mapButtons[i].Enable = false;
            zombieAnimations[i].Visible = false;
            //Debug.Log(i);

        }

        for (int i = 0; i < MAP_COUNT; i++)
        {
            int index = infection[i];
            if (index != -1)
            {
                mapButtons[index].Enable = true;
                zombieAnimations[index].Visible = true;

            }
        }



        mapButtons[0].SetTexture(UIButtonBase.State.Normal, mapMaterial, MapUITexturePosition.FactoryImg, AutoRect.AutoSize(MapUITexturePosition.FactoryImg));
        mapButtons[1].SetTexture(UIButtonBase.State.Normal, mapMaterial, MapUITexturePosition.HospitalImg, AutoRect.AutoSize(MapUITexturePosition.HospitalImg));
        mapButtons[2].SetTexture(UIButtonBase.State.Normal, mapMaterial, MapUITexturePosition.ParkingImg, AutoRect.AutoSize(MapUITexturePosition.ParkingImg));
        mapButtons[3].SetTexture(UIButtonBase.State.Normal, mapMaterial, MapUITexturePosition.Village, AutoRect.AutoSize(MapUITexturePosition.Village));

        mapButtons[0].SetTexture(UIButtonBase.State.Pressed, mapMaterial, MapUITexturePosition.FactoryImg, AutoRect.AutoSize(MapUITexturePosition.FactoryImg) * 2);
        mapButtons[1].SetTexture(UIButtonBase.State.Pressed, mapMaterial, MapUITexturePosition.HospitalImg, AutoRect.AutoSize(MapUITexturePosition.HospitalImg) * 2);
        mapButtons[2].SetTexture(UIButtonBase.State.Pressed, mapMaterial, MapUITexturePosition.ParkingImg, AutoRect.AutoSize(MapUITexturePosition.ParkingImg) * 2);
        mapButtons[3].SetTexture(UIButtonBase.State.Pressed, mapMaterial, MapUITexturePosition.Village, AutoRect.AutoSize(MapUITexturePosition.Village) * 2);


        mapButtons[0].Rect = AutoRect.AutoPos(uiPos.FactoryButton);
        mapButtons[1].Rect = AutoRect.AutoPos(uiPos.HospitalButton);
        mapButtons[2].Rect = AutoRect.AutoPos(uiPos.ParkingButton);
        mapButtons[3].Rect = AutoRect.AutoPos(uiPos.VillageButton);


        Rect offset = new Rect(66, -30, 0, 0);
        Rect offset2 = new Rect(76, -30, 0, 0);
        Rect offset3 = new Rect(46, -30, 0, 0);
        zombieAnimations[0].Rect = AutoRect.AutoPos(Math.AddRect(uiPos.FactoryButton, offset3));
        zombieAnimations[1].Rect = AutoRect.AutoPos(Math.AddRect(uiPos.HospitalButton, offset));
        zombieAnimations[2].Rect = AutoRect.AutoPos(Math.AddRect(uiPos.ParkingButton, offset));
        zombieAnimations[3].Rect = AutoRect.AutoPos(Math.AddRect(uiPos.VillageButton, offset2));


        shopButton.SetTexture(UIButtonBase.State.Normal, mapMaterial, MapUITexturePosition.ShopImg, AutoRect.AutoSize(MapUITexturePosition.ShopImg));
        shopButton.SetTexture(UIButtonBase.State.Pressed, mapMaterial, MapUITexturePosition.ShopImg, AutoRect.AutoSize(MapUITexturePosition.ShopImg) * 2);

        shopButton.Rect = AutoRect.AutoPos(uiPos.ShopButton);
        this.Add(shopButton);


        Material shopMaterial = UIResourceMgr.GetInstance().GetMaterial("ShopUI");

        daysPanel = new UITextImage();
        daysPanel.SetTexture(shopMaterial,

ShopTexturePosition.DayLargePanel, AutoRect.AutoSize(ShopTexturePosition.DayLargePanel));
        daysPanel.Rect = AutoRect.AutoPos(uiPos.DaysPanel);
        daysPanel.SetText(ConstData.FONT_NAME0, "DAY " + GameApp.GetInstance().GetGameState().LevelNum, ColorName.fontColor_darkred);
        cashPanel = new CashPanel();
        this.Add(daysPanel);
        this.Add(cashPanel);
        cashPanel.SetCash(GameApp.GetInstance().GetGameState().GetCash());
        cashPanel.Show();

        Material arenaMenuMaterial = UIResourceMgr.GetInstance().GetMaterial("ArenaMenu");

        returnButton = new UIClickButton();
        returnButton.SetTexture(UIButtonBase.State.Normal, arenaMenuMaterial,

ArenaMenuTexturePosition.ReturnButtonNormal, AutoRect.AutoSize(ArenaMenuTexturePosition.ReturnButtonNormal));
        returnButton.SetTexture(UIButtonBase.State.Pressed, arenaMenuMaterial,

ArenaMenuTexturePosition.ReturnButtonPressed, AutoRect.AutoSize(ArenaMenuTexturePosition.ReturnButtonPressed));
        returnButton.Rect = AutoRect.AutoPos(uiPos.ReturnButton);



        optionsButton = new UIClickButton();
        optionsButton.SetTexture(UIButtonBase.State.Normal, arenaMenuMaterial,

ArenaMenuTexturePosition.OptionsButton, AutoRect.AutoSize(ArenaMenuTexturePosition.OptionsButton));
        optionsButton.SetTexture(UIButtonBase.State.Pressed, arenaMenuMaterial,

ArenaMenuTexturePosition.OptionsButtonPressed, AutoRect.AutoSize(ArenaMenuTexturePosition.OptionsButtonPressed));
        optionsButton.Rect = AutoRect.AutoPos(uiPos.OptionsButton);
        this.Add(optionsButton);
        this.Add(returnButton);

       



    }

    public override void Hide()
    {
        base.Hide();

    }

    public override void Show()
    {

        base.Show();
        buttonPressed = false;
    }


	public override void Update()
    {
        if (fadeTimer.Ready())
        {
			string strSceneName = "";

            if (fadeTimer.Name == "0")
            {
                if (false && GameApp.GetInstance().GetGameState().FirstTimeGame)
                {
                    //! Application.LoadLevel(SceneName.SCENE_TUTORIAL);
					strSceneName = SceneName.SCENE_TUTORIAL;
                }
                else
                {
                    //! Application.LoadLevel(SceneName.SCENE_ARENA);

					strSceneName = SceneName.SCENE_ARENA;
                }
            }
            else if (fadeTimer.Name == "1")
            {
				//! Application.LoadLevel(SceneName.SCENE_HOSPITAL);

				strSceneName = SceneName.SCENE_HOSPITAL;
            }
            else if (fadeTimer.Name == "2")
            {
				//! Application.LoadLevel(SceneName.SCENE_PARKING);

				strSceneName = SceneName.SCENE_PARKING;
            }
            else if (fadeTimer.Name == "3")
            {
				//! Application.LoadLevel(SceneName.SCENE_VILLAGE);

				strSceneName = SceneName.SCENE_VILLAGE;
            }
            else if (fadeTimer.Name == "shop")
            {
				//! Application.LoadLevel(SceneName.ARENA_MENU);

				strSceneName = SceneName.ARENA_MENU;
            }
            else if (fadeTimer.Name == "return")
            {
				//! Application.LoadLevel(SceneName.START_MENU);

				strSceneName = SceneName.START_MENU;
            }

			SceneManager.LoadScene (strSceneName);

            fadeTimer.Do();
        }
    }



    public void HandleEvent(UIControl control, int command, float wparam, float lparam)
    {

        if (buttonPressed)
            return;

        if (control == mapButtons[0])
        {
            GameObject.Destroy(GameObject.Find("Music"));
            MapUI.GetInstance().GetAudioPlayer().PlayAudio("Battle");
            FadeAnimationScript.GetInstance().FadeInBlack();
            fadeTimer.Name = "0";
            fadeTimer.SetTimer(2f, false);
            GameApp.GetInstance().Save();
            buttonPressed = true;

        }
        else if (control == mapButtons[1])
        {
            GameObject.Destroy(GameObject.Find("Music"));
            MapUI.GetInstance().GetAudioPlayer().PlayAudio("Battle");
            FadeAnimationScript.GetInstance().FadeInBlack();
            fadeTimer.Name = "1";
            fadeTimer.SetTimer(2f, false);
            GameApp.GetInstance().Save();
            buttonPressed = true;
        }
        else if (control == mapButtons[2])
        {
            GameObject.Destroy(GameObject.Find("Music"));
            MapUI.GetInstance().GetAudioPlayer().PlayAudio("Battle");
            FadeAnimationScript.GetInstance().FadeInBlack();
            fadeTimer.Name = "2";
            fadeTimer.SetTimer(2f, false);
            GameApp.GetInstance().Save();
            buttonPressed = true;

        }
        else if (control == mapButtons[3])
        {
            GameObject.Destroy(GameObject.Find("Music"));
            MapUI.GetInstance().GetAudioPlayer().PlayAudio("Battle");
            FadeAnimationScript.GetInstance().FadeInBlack();
            fadeTimer.Name = "3";
            fadeTimer.SetTimer(2f, false);
            GameApp.GetInstance().Save();
            buttonPressed = true;

        }
        else if (control == shopButton)
        {
            FadeAnimationScript.GetInstance().FadeInBlack(0.5f);
            fadeTimer.Name = "shop";
            fadeTimer.SetTimer(0.5f, false);
            buttonPressed = true;
        }
        else if (control == returnButton)
        {
            FadeAnimationScript.GetInstance().FadeInBlack(1);
            fadeTimer.Name = "return";
            fadeTimer.SetTimer(1f, false);
            buttonPressed = true;
           

        }
        else if (control == optionsButton)
        {
            this.Hide();
            MapUI.GetInstance().GetOptionsMenuUI().Show();
        }

    }


}



public class MapUI : MonoBehaviour
{
    public UIManager m_UIManager = null;
    protected MapUIPanel mapPanel;
    protected OptionsMenuUI optionsUI;
    protected AudioPlayer audioPlayer = new AudioPlayer();
    protected LoadingPanel loadingPanel;
    protected float startTime;
   


    void Awake()
    {
        if (GameObject.Find("ResourceConfig") == null)
        {
            GameObject resourceConfig = Object.Instantiate(Resources.Load("ResourceConfig")) as GameObject;
            resourceConfig.name = "ResourceConfig";
            DontDestroyOnLoad(resourceConfig);
        }

        if (GameObject.Find("Music") == null)
        {
            GameApp.GetInstance().Init();
            GameObject musicObj = new GameObject("Music");
            DontDestroyOnLoad(musicObj);
            musicObj.transform.position = new Vector3(0, 1, -10);
            AudioSource audioSource = musicObj.AddComponent<AudioSource>();
            audioSource.clip = GameApp.GetInstance().GetResourceConfig().menuAudio;
            musicObj.AddComponent<MenuMusicScript>();
            audioSource.loop = true;
            audioSource.bypassEffects = true;
            audioSource.rolloffMode = AudioRolloffMode.Linear;
            audioSource.Play();
        }
    }

    // Use this for initialization
    void Start()
    {
        loadingPanel = new LoadingPanel();

        loadingPanel.Show();
       

        startTime = Time.time;
        


        m_UIManager = gameObject.AddComponent<UIManager>() as UIManager;
        m_UIManager.SetParameter(8, 1, false);
        //m_UIManager.SetUIHandler(this);

        m_UIManager.CLEAR = true;

        Transform audioFolderTrans = transform.Find("Audio");
        audioPlayer.AddAudio(audioFolderTrans, "Button");
        audioPlayer.AddAudio(audioFolderTrans, "Battle");
        GameApp.GetInstance().GetGameState().Achievement.SubmitAllToGameCenter();
        if (!GameApp.GetInstance().GetGameState().FromShopMenu)
        {
            m_UIManager.Add(loadingPanel);
        }
        StartCoroutine("Init");


    }


    IEnumerator Init()
    {
        yield return 1;

        UIResourceMgr.GetInstance().LoadMapUIMaterials();
        //if (GameApp.GetInstance().GetGameState().FirstTimeGame)
        {
            if (Time.time - startTime < 3.0f && !GameApp.GetInstance().GetGameState().FromShopMenu)
            {
                yield return new WaitForSeconds(3.0f - (Time.time - startTime));
            }
        }
        FadeAnimationScript.GetInstance().FadeOutBlack();
        GameApp.GetInstance().GetGameState().FromShopMenu = false;

        mapPanel = new MapUIPanel();
        m_UIManager.Add(mapPanel);
        mapPanel.Start();
        mapPanel.Show();


        optionsUI = new OptionsMenuUI();
        m_UIManager.Add(optionsUI);

    }


    // Update is called once per frame
    void Update()
    {
        if (mapPanel != null)
        {
            mapPanel.Update();
        }
        //if (FadeAnimationScript.GetInstance().FadeOutComplete())
        {
            foreach (UITouchInner touch in iPhoneInputMgr.MockTouches())
            {
                if (m_UIManager != null)
                {
                    if (m_UIManager.HandleInput(touch))
                    {
                        continue;
                    }
                }
            }
        }
    }

    public static MapUI GetInstance()
    {
        return GameObject.Find("MapUI").GetComponent<MapUI>();
    }

    public OptionsMenuUI GetOptionsMenuUI()
    {
        return optionsUI;
    }
    public MapUIPanel GetMapUI()
    {
        return mapPanel;
    }

    public AudioPlayer GetAudioPlayer()
    {
        return audioPlayer;
    }


}
