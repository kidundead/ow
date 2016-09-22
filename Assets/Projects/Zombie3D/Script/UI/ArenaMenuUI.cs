using UnityEngine;
using System.Collections;
using Zombie3D;
using System.IO;


class ArenaMenuUIPosition
{

    public Rect Background = new Rect(0, 0, 960, 640);

    public Rect UpgradeButton = new Rect(556, 420, 356, 90);
    public Rect EquipmentButton = new Rect(556, 300, 356, 90);
    public Rect AvatarButton = new Rect(556, 180, 356, 90);


    public Rect BattleButton = new Rect(290, 6, 356, 116);

    public Rect OptionsButton = new Rect(798, 6, 130, 74);
    public Rect ReturnButton = new Rect(24, 6, 130, 70);

    public Rect DaysPanel = new Rect(26, 640 - 80, 358, 76);
    public Rect AchievementButton = new Rect(22, 0, 104 * 1.3f, 60 * 1.3f);
    public Rect LeaderBoardButton = new Rect(800, 0, 104 * 1.3f, 60 * 1.3f);
}

public class Avatar3DFrame : UI3DFrame
{

    protected Vector3 scale;
    protected float lastMotionTime;
    public Avatar3DFrame(Rect rect, Vector3 pos, Vector3 scale)
        : base(rect, pos)
    {
        this.scale = scale;
        ChangeAvatar(GameApp.GetInstance().GetGameState().Avatar);

    }

    public void ChangeAvatar(AvatarType aType)
    {
        ClearModels();
        GameObject avatarObj = AvatarFactory.GetInstance().CreateAvatar(aType);  //! 改变人物

        avatarObj.transform.rotation = Quaternion.Euler(0f, 200f, 0f);

        ResourceConfigScript rConf = GameApp.GetInstance().GetResourceConfig();

        Weapon w = GameApp.GetInstance().GetGameState().GetBattleWeapons()[0];
        string firstWeaponName = w.Name;
        string wNameEnd = Weapon.GetWeaponNameEnd(w.GetWeaponType());

        GameObject weapon = WeaponFactory.GetInstance().CreateWeaponModel(firstWeaponName, avatarObj.transform.position, avatarObj.transform.rotation);   //! 创建武器
        Transform weaponBoneTrans = avatarObj.transform.Find(BoneName.WEAPON_PATH);   //! 获取人物上挂载武器的节点
        weapon.transform.parent = weaponBoneTrans;                                    //! 设置武器的父节点
        avatarObj.transform.localScale = scale;
        avatarObj.GetComponent<UnityEngine.Animation>()[AnimationName.PLAYER_IDLE + wNameEnd].wrapMode = WrapMode.Loop;
        avatarObj.GetComponent<UnityEngine.Animation>().Play(AnimationName.PLAYER_IDLE + wNameEnd);

        SetModel(avatarObj);
        lastMotionTime = Time.time;
    }

    public void UpdateAnimation()
    {
        GameObject avatarObj = GetModel();
        Weapon w = GameApp.GetInstance().GetGameState().GetBattleWeapons()[0];
        string wNameEnd = Weapon.GetWeaponNameEnd(w.GetWeaponType());
        if (avatarObj != null)
        {

            if (w.GetWeaponType() == WeaponType.RocketLauncher || w.GetWeaponType() == WeaponType.Sniper)
            {
                if (Time.time - lastMotionTime > 7.0f)
                {
                    string aniName = "";
                    if (avatarObj.GetComponent<UnityEngine.Animation>().IsPlaying(AnimationName.PLAYER_RUN + wNameEnd))
                    {
                        aniName = AnimationName.PLAYER_IDLE + wNameEnd;
                    }
                    else
                    {
                        aniName = AnimationName.PLAYER_RUN + wNameEnd;
                    }

                    avatarObj.GetComponent<UnityEngine.Animation>()[aniName].wrapMode = WrapMode.Loop;
                    avatarObj.GetComponent<UnityEngine.Animation>().CrossFade(aniName);
                    lastMotionTime = Time.time;
                }
            }
            else if (w.GetWeaponType() == WeaponType.Saw)
            {
                if (Time.time - lastMotionTime > 7.0f)
                {
                    avatarObj.GetComponent<UnityEngine.Animation>()[AnimationName.PLAYER_SHOT + "_Saw2"].wrapMode = WrapMode.ClampForever;
                    avatarObj.GetComponent<UnityEngine.Animation>().CrossFade(AnimationName.PLAYER_SHOT + "_Saw");
                    avatarObj.GetComponent<UnityEngine.Animation>().CrossFadeQueued(AnimationName.PLAYER_SHOT + "_Saw2");
                    lastMotionTime = Time.time;
                }

                if (avatarObj.GetComponent<UnityEngine.Animation>().IsPlaying(AnimationName.PLAYER_SHOT + "_Saw2") && (Time.time - lastMotionTime > avatarObj.GetComponent<UnityEngine.Animation>()[AnimationName.PLAYER_SHOT + "_Saw2"].clip.length * 2))
                {
                    avatarObj.GetComponent<UnityEngine.Animation>().CrossFade(AnimationName.PLAYER_IDLE + wNameEnd);
                }
            }
            else
            {
                if (Time.time - lastMotionTime > 7.0f)
                {
                    avatarObj.GetComponent<UnityEngine.Animation>()[AnimationName.PLAYER_STANDBY].wrapMode = WrapMode.ClampForever;
                    avatarObj.GetComponent<UnityEngine.Animation>().CrossFade(AnimationName.PLAYER_STANDBY);
                    lastMotionTime = Time.time;
                }

                if (avatarObj.GetComponent<UnityEngine.Animation>()[AnimationName.PLAYER_STANDBY].time > avatarObj.GetComponent<UnityEngine.Animation>()[AnimationName.PLAYER_STANDBY].clip.length)
                {
                    avatarObj.GetComponent<UnityEngine.Animation>().CrossFade(AnimationName.PLAYER_IDLE + wNameEnd);
                }

            }


        }

    }


}

public class ArenaMenuPanel : UIPanel, UIHandler
{

    protected UIImage background;
    protected UITextImage avatarPanel;
    protected UITextImage daysPanel;
    protected CashPanel cashPanel;

    protected UITextButton upgradeButton;
    protected UITextButton equipmentButton;

    protected UITextButton battleButton;
    protected UITextButton avatarButton;

    protected UIClickButton returnButton;
    protected UIClickButton optionsButton;

    protected UIClickButton leaderButton;
    protected UIClickButton achieveButton;

    protected Avatar3DFrame avatar3DFrame;

    private ArenaMenuUIPosition uiPos;
    protected ArenaMenuUI ui;
    protected ReviewDialog reviewDialog;
    protected Timer fadeTimer = new Timer();
    public bool BattlePressed { get; set; }

    protected float startTime;

    public ArenaMenuPanel()
    {
        uiPos = new ArenaMenuUIPosition();
        BattlePressed = false;
        Material arenaMenuMaterial = UIResourceMgr.GetInstance().GetMaterial("ArenaMenu");
        Material buttonsMaterial = UIResourceMgr.GetInstance().GetMaterial("Buttons");

        background = new UIImage();
        background.SetTexture(arenaMenuMaterial,

ArenaMenuTexturePosition.Background, AutoRect.AutoSize(ArenaMenuTexturePosition.Background));
        background.Rect = AutoRect.AutoPos(uiPos.Background);


        /*
        avatarPanel = new UITextImage();
        avatarPanel.SetTexture(arenaMenuMaterial,

ArenaMenuTexturePosition.AvatarImage);
        avatarPanel.Rect = uiPos.AvatarPanel;
        avatarPanel.SetText(ConstData.FONT_NAME1, AvatarInfo.AVATAR_NAME[(int)GameApp.GetInstance().GetGameState().Avatar], ColorName.fontColor_darkred);
        */

        Material shopMaterial = UIResourceMgr.GetInstance().GetMaterial("ShopUI");

        daysPanel = new UITextImage();
        daysPanel.SetTexture(shopMaterial,

ShopTexturePosition.DayLargePanel, AutoRect.AutoSize(ShopTexturePosition.DayLargePanel));
        daysPanel.Rect = AutoRect.AutoPos(uiPos.DaysPanel);
        daysPanel.SetText(ConstData.FONT_NAME0, "DAY " + GameApp.GetInstance().GetGameState().LevelNum, ColorName.fontColor_darkred);


        cashPanel = new CashPanel();
        
        upgradeButton = new UITextButton();
        upgradeButton.SetTexture(UIButtonBase.State.Normal, buttonsMaterial,

ButtonsTexturePosition.ButtonNormal, AutoRect.AutoSize(ButtonsTexturePosition.MiddleSizeButton));
        upgradeButton.SetTexture(UIButtonBase.State.Pressed, buttonsMaterial,

ButtonsTexturePosition.ButtonPressed, AutoRect.AutoSize(ButtonsTexturePosition.MiddleSizeButton));
        upgradeButton.Rect = AutoRect.AutoPos(uiPos.UpgradeButton);
        upgradeButton.SetText(ConstData.FONT_NAME1, " ARMORY", ColorName.fontColor_orange);


        equipmentButton = new UITextButton();
        equipmentButton.SetTexture(UIButtonBase.State.Normal, buttonsMaterial,

ButtonsTexturePosition.ButtonNormal, AutoRect.AutoSize(ButtonsTexturePosition.MiddleSizeButton));
        equipmentButton.SetTexture(UIButtonBase.State.Pressed, buttonsMaterial,

ButtonsTexturePosition.ButtonPressed, AutoRect.AutoSize(ButtonsTexturePosition.MiddleSizeButton));
        equipmentButton.Rect = AutoRect.AutoPos(uiPos.EquipmentButton);
        equipmentButton.SetText(ConstData.FONT_NAME1, " EQUIP", ColorName.fontColor_orange);



        avatarButton = new UITextButton();
        avatarButton.SetTexture(UIButtonBase.State.Normal, buttonsMaterial,

ButtonsTexturePosition.ButtonNormal, AutoRect.AutoSize(ButtonsTexturePosition.MiddleSizeButton));

        avatarButton.SetTexture(UIButtonBase.State.Pressed, buttonsMaterial,

ButtonsTexturePosition.ButtonPressed, AutoRect.AutoSize(ButtonsTexturePosition.MiddleSizeButton));
        avatarButton.Rect = AutoRect.AutoPos(uiPos.AvatarButton);
        avatarButton.SetText(ConstData.FONT_NAME1, " CHARACTER", ColorName.fontColor_orange);



        battleButton = new UITextButton();
        battleButton.SetTexture(UIButtonBase.State.Normal, shopMaterial,

ShopTexturePosition.MapButtonNormal, AutoRect.AutoSize(ShopTexturePosition.MapButtonNormal));
        battleButton.SetTexture(UIButtonBase.State.Pressed, shopMaterial,

ShopTexturePosition.MapButtonPressed, AutoRect.AutoSize(ShopTexturePosition.MapButtonPressed));
        battleButton.Rect = AutoRect.AutoPos(uiPos.BattleButton);
        battleButton.SetText(ConstData.FONT_NAME0, " MAP", ColorName.fontColor_orange);





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

        Material startMenuMaterial = UIResourceMgr.GetInstance().GetMaterial("StartMenu");

        leaderButton = new UITextButton();
        leaderButton.SetTexture(UIButtonBase.State.Normal, startMenuMaterial,

StartMenuTexturePosition.AcheivementButtonNormal, AutoRect.AutoSize(StartMenuTexturePosition.AcheivementButtonNormal) * 1.3f);
        leaderButton.SetTexture(UIButtonBase.State.Pressed, startMenuMaterial,

StartMenuTexturePosition.AcheivementButtonPressed, AutoRect.AutoSize(StartMenuTexturePosition.AcheivementButtonPressed)*1.3f);

        leaderButton.Rect = AutoRect.AutoPos(uiPos.AchievementButton);


        achieveButton = new UITextButton();
        achieveButton.SetTexture(UIButtonBase.State.Normal, startMenuMaterial,

StartMenuTexturePosition.LeaderBoardsButtonNormal, AutoRect.AutoSize(StartMenuTexturePosition.LeaderBoardsButtonNormal) * 1.3f);
        achieveButton.SetTexture(UIButtonBase.State.Pressed, startMenuMaterial,

StartMenuTexturePosition.LeaderBoardsButtonPressed, AutoRect.AutoSize(StartMenuTexturePosition.LeaderBoardsButtonPressed) * 1.3f);

        achieveButton.Rect = AutoRect.AutoPos(uiPos.LeaderBoardButton);



        if (AutoRect.GetPlatform() == Platform.IPad)
        {
            avatar3DFrame = new Avatar3DFrame(AutoRect.AutoPos(new Rect(0, 10, 500, 600)), new Vector3(-1.299798f * 0.9f, -0.9672753f * 1f, 3.420711f), new Vector3(1.5f, 1.5f, 1.5f) * 0.9f);

        }
        else
        {
            avatar3DFrame = new Avatar3DFrame(AutoRect.AutoPos(new Rect(0, 10, 500, 600)), new Vector3(-1.299798f, -1.0672753f, 3.420711f), new Vector3(1.5f, 1.5f, 1.5f));

        }

        ui = GameObject.Find("ArenaMenuUI").GetComponent<ArenaMenuUI>();

        this.Add(background);
        //this.Add(avatarPanel);
        this.Add(daysPanel);
        this.Add(cashPanel);
        this.Add(upgradeButton);
        this.Add(equipmentButton);
        //this.Add(optionsButton);
        this.Add(battleButton);
        this.Add(avatarButton);
        //this.Add(returnButton);
        this.Add(leaderButton);
        this.Add(achieveButton);
        this.Add(avatar3DFrame);
       
        SetUIHandler(this);

        startTime = Time.time;
    }

    public override void Hide()
    {
        base.Hide();
        avatar3DFrame.Hide();
        cashPanel.Hide();
    }

    public override void Show()
    {

        base.Show();
        avatar3DFrame.ChangeAvatar(GameApp.GetInstance().GetGameState().Avatar);
        //avatarPanel.SetText(ConstData.FONT_NAME1, AvatarInfo.AVATAR_NAME[(int)GameApp.GetInstance().GetGameState().Avatar], ColorName.fontColor_darkred);
        cashPanel.SetCash(GameApp.GetInstance().GetGameState().GetCash());
        cashPanel.Show();
        avatar3DFrame.Show();
    }

    public void Update()
    {
        /*
        if (Camera.mainCamera != null)
        {
            if (!Camera.mainCamera.audio.isPlaying && !BattlePressed)
            {
                Camera.mainCamera.audio.Play();
            }
        }
        */

        if (Time.time - startTime > 1.5f)
        {
            
            if (!GameApp.GetInstance().GetGameState().AlreadyCountered)
            {
                GameApp.GetInstance().GetGameState().AddScore(1);
                GameApp.GetInstance().GetGameState().AlreadyCountered = true;

            }
            
            if (!GameApp.GetInstance().GetGameState().AlreadyPopReview)
            {
                if (GameApp.GetInstance().GetGameState().GetScore() == 3 || GameApp.GetInstance().GetGameState().GetScore() == 6 || GameApp.GetInstance().GetGameState().GetScore() == 9)
                {
                    Debug.Log("Pop Review " + GameApp.GetInstance().GetGameState().GetScore());
                    ReviewDialogUI.GetInstance().ShowDialog();

                    GameApp.GetInstance().GetGameState().AlreadyPopReview = true;
                }
            }

        }


        if (avatar3DFrame != null)
        {
            avatar3DFrame.UpdateAnimation();
        }


        if (fadeTimer.Ready())
        {

            if (fadeTimer.Name == "StartMenu")
            {
                UIResourceMgr.GetInstance().UnloadAllUIMaterials();
                //GameApp.GetInstance().GetGameState().MenuMusicTime = Camera.mainCamera.audio.time;
                Application.LoadLevel(SceneName.START_MENU);

            }
            else
            {
                UIResourceMgr.GetInstance().UnloadAllUIMaterials();
                //GameApp.GetInstance().GetGameState().FirstTimeGame = false;
                GameApp.GetInstance().GetGameState().FromShopMenu = true;
                
                Application.LoadLevel(SceneName.MAP);
                /*
                if (GameApp.GetInstance().GetGameState().FirstTimeGame)
                {
                    Application.LoadLevel(SceneName.SCENE_TUTORIAL);
                }
                else
                {
                    int rnd = Random.RandomRange(0, 100);
                    int mod = GameApp.GetInstance().GetGameState().LevelNum % 2;
                    if (mod == 0)
                    {
                        Application.LoadLevel(SceneName.SCENE_HOSPITAL);
                    }
                    else
                    {
                        Application.LoadLevel(SceneName.SCENE_ARENA);
                    }
                    
                    //else
                    {
                    //    Application.LoadLevel(SceneName.SCENE_PARKING);
                    }
                }
                */
            }

            fadeTimer.Do();

        }
    }


    public void HandleEvent(UIControl control, int command, float wparam, float lparam)
    {
       

        if (BattlePressed)
        {
            return;
        }
        if (control == upgradeButton)
        {
            ArenaMenuUI.GetInstance().GetAudioPlayer().PlayAudio("Button");
            //AudioPlayer.PlayAudio(ArenaMenuUI.GetInstance().audio);
            Hide();
            ui.GetPanel(MenuName.UPGRADE).Show();
        }
        else if (control == equipmentButton)
        {
            ArenaMenuUI.GetInstance().GetAudioPlayer().PlayAudio("Button");
            Hide();
            ui.GetPanel(MenuName.EQUIPMENT).Show();
        }
        else if (control == battleButton)
        {
            BattlePressed = true;
            //GameObject.Find("Music").audio.Stop();
            //GameObject.Destroy(GameObject.Find("Music"));
            //ArenaMenuUI.GetInstance().GetAudioPlayer().PlayAudio("Battle");
            FadeAnimationScript.GetInstance().FadeInBlack(0.3f);
            fadeTimer.Name = "Continue";
            fadeTimer.SetTimer(0.3f, false);
            GameApp.GetInstance().Save();

        }
        else if (control == avatarButton)
        {
            ArenaMenuUI.GetInstance().GetAudioPlayer().PlayAudio("Button");
            Hide();
            ui.GetPanel(MenuName.AVATAR).Show();
        }
        else if (control == optionsButton)
        {
            //this.Hide();
            //ui.GetPanel(MenuName.OPTIONS).Show();

            //ArenaMenuUI.GetInstance().GetAudioPlayer().PlayAudio("Button");

        }
        else if (control == returnButton)
        {
            ArenaMenuUI.GetInstance().GetAudioPlayer().PlayAudio("Button");

            FadeAnimationScript.GetInstance().FadeInBlack();
            fadeTimer.Name = "StartMenu";
            fadeTimer.SetTimer(2f, false);


        }
        else if (control == leaderButton)
        {
            ArenaMenuUI.GetInstance().GetAudioPlayer().PlayAudio("Button");
            GameCenterInterface.OpenLeaderboard();
            /*
            if (GameCenterInterface.IsLogin())
            {
                GameCenterInterface.OpenLeaderboard();
            }
            else
            {
                GameCenterInterface.Login();
            }
            */
        }
        else if (control == achieveButton)
        {
            ArenaMenuUI.GetInstance().GetAudioPlayer().PlayAudio("Button");
            GameCenterInterface.OpenAchievement();
            /*
            if (GameCenterInterface.IsLogin())
            {
                GameCenterInterface.OpenAchievement();
            }
            else
            {
                GameCenterInterface.Login();
            }
            */
        }
    }



}

public class MenuName
{
    public const int ARENA = 0;
    public const int UPGRADE = 1;
    public const int EQUIPMENT = 2;
    public const int AVATAR = 3;
    public const int SHOP = 4;
    public const int MENU_COUNT = 5;
}

public class LoadingPanel : UIPanel
{
    protected UIText m_LoadingText;
    protected UIText m_desc;
    public LoadingPanel()
    {
        m_LoadingText = new UIText();
        m_LoadingText.Set(ConstData.FONT_NAME1, "LOADING...", ColorName.fontColor_darkorange);
        m_LoadingText.AlignStyle = UIText.enAlignStyle.center;
        m_LoadingText.Rect = AutoRect.AutoPos(new Rect(0, 70, 960, 100));
       



        m_desc = new UIText();
        m_desc.Set(ConstData.FONT_NAME2, "YOUR TOWN HAS BEEN INFECTED...\n\nALL YOUR FAMILY, FRIENDS, AND CO-WORKERS HAVE TURNED INTO ZOMBIES.\n\nFIGHT FOR YOUR LIFE.\n\nFIGHT FOR AS LONG AS YOU CAN...", Color.white);
        m_desc.AlignStyle = UIText.enAlignStyle.center;
        m_desc.Rect = AutoRect.AutoPos(new Rect(0, 120, 960, 640));
        string path = Application.dataPath + "/../../Documents/";
        this.Add(m_desc);
        if (GameApp.GetInstance().GetGameState().FirstTimeGame && Application.loadedLevelName != SceneName.ARENA_MENU)
        {
            
            
        }
        else
        {
            this.Add(m_LoadingText);
            int size = AvatarInfo.TIPS_INO.Length;
            int rnd = Random.RandomRange(0, size);
            m_desc.Rect = AutoRect.AutoPos(new Rect(0, 0, 960, 640));
            m_desc.SetText(AvatarInfo.TIPS_INO[rnd]);
        }
       



    }

    public override void Show()
    {
        base.Show();
        m_LoadingText.Visible = true;
        m_desc.Visible = true;
    }

    public override void Hide()
    {
        base.Hide();
        m_LoadingText.Visible = false;
        m_desc.Visible = false;
    }


}


public class ArenaMenuUI : MonoBehaviour, UIHandler
{


    protected Rect[] buttonRect;

    public UIManager m_UIManager = null;
    //public string m_ui_material_path;  //最后带反斜杠
    //protected Material arenaMenuMaterial;


    //private ArenaMenuUIPosition uiPos;
    //private ArenaMenuTexturePosition texPos;

    protected float screenRatioX;
    protected float screenRatioY;

    ArenaMenuPanel arenaMenuPanel;
    OptionsMenuUI optionPanel;
    protected UIPanel[] panels = new UIPanel[MenuName.MENU_COUNT];

    protected AudioPlayer audioPlayer = new AudioPlayer();
    protected LoadingPanel loadingPanel;

    protected bool init = false;
    protected bool setAudioTime = false;

    protected float startTime;
    public UIPanel GetPanel(int panelID)
    {
        return panels[panelID];
    }

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


        FlurryTAd.ShowTAd(false);

        loadingPanel = new LoadingPanel();

        loadingPanel.Show();


        m_UIManager = gameObject.AddComponent<UIManager>() as UIManager;
        m_UIManager.SetParameter(8, 1, false);
        m_UIManager.SetUIHandler(this);

        m_UIManager.CLEAR = true;


        m_UIManager.Add(loadingPanel);

        startTime = Time.time;
        StartCoroutine("Init");

    }

    IEnumerator Init()
    {
        yield return 1;

        UIResourceMgr.GetInstance().LoadAllUIMaterials();
        //if (GameApp.GetInstance().GetGameState().FirstTimeGame)
        {
            if (Time.time - startTime < 1.5f)
            {
                yield return new WaitForSeconds(1.5f - (Time.time - startTime));
            }
        }

        GameApp.GetInstance().ClearScene();

        //uiPos = new ArenaMenuUIPosition();
        //texPos = new ArenaMenuTexturePosition();



        panels[MenuName.ARENA] = new ArenaMenuPanel();
        //panels[MenuName.OPTIONS] = new OptionsMenuUI();
        panels[MenuName.UPGRADE] = new WeaponUpgradeUI();
        panels[MenuName.EQUIPMENT] = new EquipmentUI();
        panels[MenuName.AVATAR] = new AvatarUI();
        panels[MenuName.SHOP] = new ShopUI();
        for (int i = 0; i < MenuName.MENU_COUNT; i++)
        {

            m_UIManager.Add(panels[i]);
        }

        panels[MenuName.ARENA].Show();

        Transform audioFolderTrans = transform.Find("Audio");
        audioPlayer.AddAudio(audioFolderTrans, "Button");
        audioPlayer.AddAudio(audioFolderTrans, "Upgrade");
        audioPlayer.AddAudio(audioFolderTrans, "Battle");
        GameApp.GetInstance().GetGameState().Achievement.SubmitAllToGameCenter();
    }

    // Update is called once per frame
    void Update()
    {
        if (Camera.main != null && !setAudioTime)
        {
            //Camera.mainCamera.audio.time = GameApp.GetInstance().GetGameState().MenuMusicTime;
            setAudioTime = true;
        }


        ArenaMenuPanel panel = panels[MenuName.ARENA] as ArenaMenuPanel;
        if (panel != null)
        {
            panel.Update();
        }

        for (int i = 0; i < MenuName.MENU_COUNT; i++)
        {
            if (panels[i] != null)
            {
                panels[i].UpdateLogic();
            }
        }



        ShopUI shopUI = panels[MenuName.SHOP] as ShopUI;
        if (shopUI != null)
        {
            shopUI.GetPurchaseStatus();
        }

        if (!ReviewDialogUI.GetInstance().IsVisible())
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

    public static ArenaMenuUI GetInstance()
    {
        return GameObject.Find("ArenaMenuUI").GetComponent<ArenaMenuUI>();
    }

    public AudioPlayer GetAudioPlayer()
    {
        return audioPlayer;
    }





    public void HandleEvent(UIControl control, int command, float wparam, float lparam)
    {


    }

}
