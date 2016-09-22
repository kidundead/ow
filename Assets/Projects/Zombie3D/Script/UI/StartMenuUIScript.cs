using UnityEngine;
using System.Collections;
using System.IO;
using Zombie3D;
using UnityEngine.SceneManagement;

class StartMenuUIPosition
{
    public Rect Background = new Rect(0, 0, 960, 640);

    /*
    public Rect StartButton = new Rect(600, 280, 291, 217);
    public Rect ContinueButton = new Rect(600, 150, 277, 217);
    public Rect OptionButton = new Rect(600, 20, 277, 227);
    */
    public Rect StartButton = new Rect(22, 74, 356, 116);
    public Rect ContinueButton = new Rect(480 - (22 + 356 - 480), 74, 356, 116);
   

}


public class StartMenuUIScript : MonoBehaviour, UIHandler, UIDialogEventHandler
{
    protected Rect[] buttonRect;

    public UIManager m_UIManager = null;
    public string m_ui_material_path;  //最后带反斜杠
    protected Material startMenuMaterial;
    protected Material startMenu2Material;


    protected UIImage background;
    protected UITextButton startButton;
    protected UITextButton continueButton;

    protected UIClickButton optionsButton;
    protected GameDialog gameDialog;

    private StartMenuUIPosition uiPos;
    //private StartMenuTexturePosition texPos;

    protected float screenRatioX;
    protected float screenRatioY;



    protected Timer fadeTimer = new Timer();



    void Awake()
    {

		//! 实例化 ResourceConfig
        if (GameObject.Find("ResourceConfig") == null)
        {
            GameObject resourceConfig = Object.Instantiate(Resources.Load("ResourceConfig")) as GameObject;
            resourceConfig.name = "ResourceConfig";
            DontDestroyOnLoad(resourceConfig);
        }

		//! 添加Music 对象 管理音乐播放

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
        ResolutionConstant.R = ((float)Screen.width) / 960f;
        UIResourceMgr.GetInstance().LoadStartMenuUIMaterials();    //! 加载开始菜单需要的材质

        
        uiPos = new StartMenuUIPosition();
        //texPos = new StartMenuTexturePosition();

		//! UIManager
        m_UIManager = gameObject.AddComponent<UIManager>() as UIManager;
        m_UIManager.SetParameter(8, 1, false);
		m_UIManager.SetUIHandler(this);             //! HandleEvent
        m_UIManager.CLEAR = true;

        startMenuMaterial = UIResourceMgr.GetInstance().GetMaterial("StartMenu");   //! StartMenu 材质
        //startMenu2Material = UIResourceMgr.GetInstance().GetMaterial("StartMenu2");
        Material buttonsMaterial = UIResourceMgr.GetInstance().GetMaterial("Buttons");  //! Button材质
        background = new UIImage();
        background.SetTexture(startMenuMaterial, StartMenuTexturePosition.Background, AutoRect.AutoSize(StartMenuTexturePosition.Background));
        background.Rect = AutoRect.AutoPos(uiPos.Background);


        startButton = new UITextButton();
        startButton.SetTexture(UIButtonBase.State.Normal, buttonsMaterial,

ButtonsTexturePosition.ButtonNormal, AutoRect.AutoSize(ButtonsTexturePosition.MiddleSizeButton));
        startButton.SetTexture(UIButtonBase.State.Pressed, buttonsMaterial,

ButtonsTexturePosition.ButtonPressed, AutoRect.AutoSize(ButtonsTexturePosition.MiddleSizeButton));

        startButton.Rect = AutoRect.AutoPos(uiPos.StartButton);
        startButton.SetText(ConstData.FONT_NAME1, " NEW GAME", ColorName.fontColor_orange);

        continueButton = new UITextButton();
        continueButton.SetTexture(UIButtonBase.State.Normal, buttonsMaterial,

ButtonsTexturePosition.ButtonNormal, AutoRect.AutoSize(ButtonsTexturePosition.MiddleSizeButton));
        continueButton.SetTexture(UIButtonBase.State.Pressed, buttonsMaterial,

ButtonsTexturePosition.ButtonPressed, AutoRect.AutoSize(ButtonsTexturePosition.MiddleSizeButton));

        continueButton.Rect = AutoRect.AutoPos(uiPos.ContinueButton);

        continueButton.SetText(ConstData.FONT_NAME1, " CONTINUE", ColorName.fontColor_orange);


     



        gameDialog = new GameDialog(UIDialog.DialogMode.YES_OR_NO);
        gameDialog.SetText(ConstData.FONT_NAME2, "\n\nAre You Sure You Want To Erase Your Progress And Start A New Game?", ColorName.fontColor_darkorange);
        gameDialog.SetDialogEventHandler(this);


        m_UIManager.Add(background);
        m_UIManager.Add(startButton);
        m_UIManager.Add(continueButton);
  
        m_UIManager.Add(gameDialog);

        GameApp.GetInstance().Init();
        GameCenterInterface.Login();

        string path = Application.dataPath + "/../../Documents/";
        if (File.Exists(path + "MySavedGame.game"))
        {
        }
        else
        {
            continueButton.Visible = false;
            continueButton.Enable = false;
        }

        FlurryTAd.ShowTAd(true);

       

    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Camera.mainCamera != null)
        {
            if (!Camera.mainCamera.audio.isPlaying)
            {
                Camera.mainCamera.audio.Play();
            }
        }
        */

       


        if (FadeAnimationScript.GetInstance().FadeOutComplete())
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

        if (fadeTimer.Ready())
        {
            if (FadeAnimationScript.GetInstance().FadeInComplete())
            {

                if (fadeTimer.Name == "Start")
                {
                    UIResourceMgr.GetInstance().UnloadAllUIMaterials();
                    //GameApp.GetInstance().GetGameState().MenuMusicTime = Camera.mainCamera.audio.time;
                    //! Application.LoadLevel(SceneName.MAP);
					SceneManager.LoadScene(SceneName.MAP);
                }
                else if (fadeTimer.Name == "Continue")
                {

                    if (!GameApp.GetInstance().Load())
                    {
                        GameApp.GetInstance().GetGameState().InitWeapons();  //! 初始化武器
                    }
                    UIResourceMgr.GetInstance().UnloadAllUIMaterials();
                    //GameApp.GetInstance().GetGameState().MenuMusicTime = Camera.mainCamera.audio.time;
                    //! Application.LoadLevel(SceneName.MAP);
					SceneManager.LoadScene(SceneName.MAP);
				}
                fadeTimer.Do();
            }

        }


    }


    public void HandleEvent(UIControl control, int command, float wparam, float lparam)
    {

        if (control == startButton)
        {
            string path = Application.dataPath + "/../../Documents/";
            if (File.Exists(path + "MySavedGame.game"))
            {
                gameDialog.Show();
            }
            else
            {
                Yes();
            }

            AudioPlayer.PlayAudio(GetComponent<AudioSource>());

        }
        else if (control == continueButton)
        {
            AudioPlayer.PlayAudio(GetComponent<AudioSource>());


            FadeAnimationScript.GetInstance().FadeInBlack();
            fadeTimer.Name = "Continue";
            fadeTimer.SetTimer(0.5f, false);

        }
        

    }

    public void Yes()
    {
        GameApp.GetInstance().GetGameState().ClearState();
        GameApp.GetInstance().GetGameState().InitWeapons();

        FadeAnimationScript.GetInstance().FadeInBlack();


        fadeTimer.Name = "Start";
        fadeTimer.SetTimer(0.5f, false);
    }
    public void No()
    {
        gameDialog.Hide();
    }


}
