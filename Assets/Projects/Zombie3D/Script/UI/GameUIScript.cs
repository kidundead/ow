using UnityEngine;
using System.Collections;
using Zombie3D;

class UIPosition
{
    public Rect PlayerLogo = new Rect(-16, 566, 116, 81);
    public Rect PlayerLogoBackground = new Rect(0, 556, 134, 88);
    public Rect HPBackground = new Rect(94, 590 - 12, 288, 50);
    public Rect HPImage = new Rect(94, 590 - 12, 288, 50);
    public Rect WeaponLogoBackground = new Rect(960 - 148, 640 - 88, 148, 88);
    public Rect WeaponLogo = new Rect(960 - 180, 640 - 88, 194, 112);
    public Rect BulletsLogo = new Rect(960 - 420, 640 - 88 - 10, 194, 112);

    public Rect WeaponSwitchButtonLeft = new Rect(700, 500, 184, 183);
    public Rect WeaponSwitchButtonRight = new Rect(860, 500, 184, 183);
    public Rect PauseButton = new Rect(408, 584-64, 160, 166);
    public Rect MoveJoystick = new Rect(0, 0, 0, 0);
    public Rect MoveJoystickThumb = new Rect(0, 0, 0, 0);
    public Rect TutorialOKButton = new Rect(600, 0, 164, 163);
    public Rect TutorialText = new Rect(260, 0, 360, 100);
    public Rect WeaponInfo = new Rect(656, 558, 100, 64);
    public Rect CashText = new Rect(0, 576, 960, 64);
    public Rect LevelInfo = new Rect(10, 456, 480, 128);

    public Rect DayClear = new Rect(0, 200, 960, 280);
    public Rect Mask = new Rect(0, 0, 960, 640);
    public Rect RightSemiMask = new Rect(480, 0, 480, 640);
    public Rect Switch = new Rect(960 - 268, 640 - 90 + 12, 148, 88);
}

public class GameUIName
{
    public const int PAUSE = 0;
    public const int GAME_OVER = 1;
    public const int NEW_ITEM = 2;
    public const int UI_COUNT = 3;
}


public class UnlockPanel : UIPanel
{

    protected UIImage unlockWeaponImage;
    protected UIText unlockWeaponText;
    protected UIImage unlockAvatarImage;
    protected UIText unlockAvatarText;

    public UnlockPanel()
    {

        unlockWeaponText = new UIText();
        unlockAvatarImage = new UIImage();
        unlockAvatarText = new UIText();

        unlockWeaponImage = new UIImage();
        unlockWeaponImage.Rect = new Rect(240, 220, 194, 112);
        unlockWeaponText.Set(ConstData.FONT_NAME2, " IS AVAILABLE!", ColorName.fontColor_yellow);
        unlockWeaponText.AlignStyle = UIText.enAlignStyle.center;
        unlockWeaponText.Rect = new Rect(440, 220, 294, 112);
        unlockWeaponText.Visible = false;
        this.Add(unlockWeaponImage);
        this.Add(unlockWeaponText);
        this.Add(unlockAvatarImage);
        this.Add(unlockAvatarText);


    }

    public override void Show()
    {
        base.Show();
        //unlockWeaponText.Visible = true;
        unlockWeaponImage.Visible = true;
    }

    public static GameUIScript GetInstance()
    {
        return GameObject.Find("SceneGUI").GetComponent<GameUIScript>();
    }

    public void SetUnlockWeapon(Weapon w)
    {
        if (w != null)
        {
            Material material = UIResourceMgr.GetInstance().GetMaterial("GameUI");
            int weaponLogoIndex = GameApp.GetInstance().GetGameState().GetWeaponIndex(w);
            Rect weaponlogoRect = new Rect(weaponLogoIndex % 5 * 194, weaponLogoIndex / 5 * 112 + 512, 194, 112);
            unlockWeaponImage.SetTexture(material, weaponlogoRect);
            unlockWeaponText.Visible = true;
        }
    }

}



public class DayInfoPanel : UIPanel
{
    public Rect Day = new Rect(480 - 180, 286, 210, 108);
    protected UIImage dayImg;
    protected UIImage[] numberImg = new UIImage[3];
    protected Material material;
    protected float aniStartTime;
    protected Rect GetNumberPos(int index)
    {
        return AutoRect.AutoPos(new Rect(480 + index * 59, 300, 182, 76));
    }
    public DayInfoPanel()
    {
        dayImg = new UIImage();
        material = UIResourceMgr.GetInstance().GetMaterial("GameUI");
        Material buttonMaterial = UIResourceMgr.GetInstance().GetMaterial("Buttons");

        dayImg.SetTexture(buttonMaterial, ButtonsTexturePosition.Day, AutoRect.AutoSize(ButtonsTexturePosition.Day));
        dayImg.Rect = AutoRect.AutoPos(Day);
        dayImg.Enable = false;
        Day = AutoRect.AutoPos(Day);
        Add(dayImg);
        for (int i = 0; i < 3; i++)
        {
            numberImg[i] = new UIImage();
            numberImg[i].Rect = GetNumberPos(i);
            numberImg[i].Enable = false;
            Add(numberImg[i]);
        }

    }


    public void SetDay(int day)
    {
        if (day < 10)
        {
            numberImg[0].SetTexture(material, GameUITexturePosition.GetNumberRect(day));
            numberImg[1].SetTexture(material, GameUITexturePosition.GetNumberRect(-1));
            numberImg[2].SetTexture(material, GameUITexturePosition.GetNumberRect(-1));
        }
        else if (day < 100)
        {
            int digit1 = day / 10;
            int digit2 = day % 10;
            numberImg[0].SetTexture(material, GameUITexturePosition.GetNumberRect(digit1));
            numberImg[1].SetTexture(material, GameUITexturePosition.GetNumberRect(digit2));
            numberImg[2].SetTexture(material, GameUITexturePosition.GetNumberRect(-1));
        }
        else
        {
            int digit1 = day / 100;
            day = day - digit1 * 100;
            int digit2 = day / 10;
            int digit3 = day % 10;
            numberImg[0].SetTexture(material, GameUITexturePosition.GetNumberRect(digit1));
            numberImg[1].SetTexture(material, GameUITexturePosition.GetNumberRect(digit2));
            numberImg[2].SetTexture(material, GameUITexturePosition.GetNumberRect(digit3));
        }
    }
    public override void Show()
    {
        if (Application.loadedLevelName != SceneName.SCENE_TUTORIAL)
        {
            base.Show();
            
        }
        aniStartTime = Time.time;
        
    }

    public void UpdateAnimation()
    {
        float timeDiff = Time.time - aniStartTime;
        if (timeDiff < 15.0f)
        {
            float dayTimeDiff = ((0.5f - timeDiff) > 0) ? (0.5f - timeDiff) : 0;
            dayImg.Rect = new Rect(Day.x + AutoRect.AutoX(2000 * dayTimeDiff), Day.y, Day.width, Day.height);
            if (dayTimeDiff == 0)
            {
                float scaleTime = timeDiff - 0.5f;
                if (scaleTime >= 0 && scaleTime <= 0.1f)
                {
                    dayImg.SetTextureSize(new Vector2(ButtonsTexturePosition.Day.width * (1 + scaleTime * 5), ButtonsTexturePosition.Day.height * (1 + scaleTime * 5)));
                }
                else if (scaleTime > 0.1f && scaleTime <= 0.2f)
                {
                    dayImg.SetTextureSize(new Vector2(ButtonsTexturePosition.Day.width * (2f - scaleTime * 5), ButtonsTexturePosition.Day.height * (2f - scaleTime * 5)));
                }
            }


            for (int i = 0; i < 3; i++)
            {
                float numTimeDiff = ((0.5f + (i + 1) * 0.5f - timeDiff) > 0) ? (0.5f + (i + 1) * 0.5f - timeDiff) : 0;
                Rect r = GetNumberPos(i);
                numberImg[i].Rect = new Rect(r.x + AutoRect.AutoX(2000 * numTimeDiff), r.y, r.width, r.height);

                if (numTimeDiff == 0)
                {
                    float scaleTime = timeDiff - (0.5f + (i + 1) * 0.5f);
                    Rect texSize = GameUITexturePosition.GetNumberRect(0);
                    if (scaleTime >= 0 && scaleTime <= 0.1f)
                    {
                        numberImg[i].SetTextureSize(new Vector2(texSize.width * (1 + scaleTime * 5), texSize.height * (1 + scaleTime * 5)));
                    }
                    else if (scaleTime > 0.1f && scaleTime <= 0.2f)
                    {
                        numberImg[i].SetTextureSize(new Vector2(texSize.width * (2f - scaleTime * 5), texSize.height * (2f - scaleTime * 5)));
                    }
                }


            }

            if (timeDiff > 4.0f)
            {
                dayImg.Rect = new Rect(Day.x - AutoRect.AutoX(2000 * (timeDiff - 4.0f)), Day.y, Day.width, Day.height);
                for (int i = 0; i < 3; i++)
                {
                    Rect r = GetNumberPos(i);
                    numberImg[i].Rect = new Rect(r.x - AutoRect.AutoX(2000 * (timeDiff - 4.0f)), r.y, r.width, r.height);
                }
            }
        }
    }

}

 



public class GameDialog : UIDialog
{



    public GameDialog(DialogMode mode)
        : base(mode)
    {
        Material dialogUIMaterial = UIResourceMgr.GetInstance().GetMaterial("Dialog");
        Material buttonsMaterial = UIResourceMgr.GetInstance().GetMaterial("Buttons");
        SetBackgroundTexture(dialogUIMaterial, DialogTexturePosition.Dialog, AutoRect.AutoPos(new Rect((960 - 574) / 2f, (640 - 306) / 2f, 574f, 306f)));
        SetTextAreaOffset(AutoRect.AutoValuePos(new Rect(80, 70, -100, -90)));
        if (mode == DialogMode.YES_OR_NO)
        {
            SetText(ConstData.FONT_NAME2, "", ColorName.fontColor_darkorange);
            SetButtonTexture(buttonsMaterial, ButtonsTexturePosition.ButtonNormal, ButtonsTexturePosition.ButtonPressed, AutoRect.AutoSize(ButtonsTexturePosition.TinySizeButton));

            SetYesButtonOffset(AutoRect.AutoValuePos(new Vector2(70, 0)), AutoRect.AutoSize(ButtonsTexturePosition.TinySizeButton));
            SetNoButtonOffset(AutoRect.AutoValuePos(new Vector2(300, 0)), AutoRect.AutoSize(ButtonsTexturePosition.TinySizeButton));
            SetYesButtonText(ConstData.FONT_NAME2, "YES", ColorName.fontColor_orange);
            SetNoButtonText(ConstData.FONT_NAME2, "NO", ColorName.fontColor_orange);
        }
        else if (mode == DialogMode.TAP_TO_DISMISS)
        {
            SetTipTextOffset(AutoRect.AutoValuePos(new Rect(180, -260, 0, 0)));
            SetTipText(ConstData.FONT_NAME2, "TAP TO DISMISS", ColorName.fontColor_darkorange);
        }

    }

}


public class ReviewDialog : UIDialog
{

    public ReviewDialog()
        : base(DialogMode.YES_OR_NO)
    {
    
        Material dialogUIMaterial = UIResourceMgr.GetInstance().GetMaterial("Dialog");
        Material buttonsMaterial = UIResourceMgr.GetInstance().GetMaterial("Buttons");
        SetBackgroundTexture(dialogUIMaterial, DialogTexturePosition.Dialog, AutoRect.AutoPos(new Rect((960 - 574) / 2f, (640 - 306) / 2f, 574f, 306f)));
        SetTextAreaOffset(AutoRect.AutoValuePos(new Rect(80, 70, -100, -90)));
 
            SetText(ConstData.FONT_NAME2, "\n                REVIEW US \nLIKE THIS GAME? WANT MORE UPDATES? PLEASE REVIEW US.", ColorName.fontColor_darkorange);
            SetButtonTexture(buttonsMaterial, ButtonsTexturePosition.ButtonNormal, ButtonsTexturePosition.ButtonPressed, AutoRect.AutoSize(ButtonsTexturePosition.TinySizeButton));

            SetYesButtonOffset(AutoRect.AutoValuePos(new Vector2(70, 0)), AutoRect.AutoSize(ButtonsTexturePosition.TinySizeButton));
            SetNoButtonOffset(AutoRect.AutoValuePos(new Vector2(300, 0)), AutoRect.AutoSize(ButtonsTexturePosition.TinySizeButton));
            SetYesButtonText(ConstData.FONT_NAME2, "YES", ColorName.fontColor_orange);
            SetNoButtonText(ConstData.FONT_NAME2, "NO,THANKS", ColorName.fontColor_orange);
        


    }

}

public class GameUIScript : MonoBehaviour, UIHandler, ITutorialGameUI, UIDialogEventHandler
{
    protected Rect[] buttonRect;

    public UIManager m_UIManager = null;
    public string m_ui_material_path;
    protected Material gameuiMaterial;

    protected UIImage playerLogoImage;
    protected UIImage hpBackground;
    protected UIImage hpImage;
    protected UIImage weaponBackground;
    protected UIClickButton weaponLogo;
    protected UIImage joystickImage;
    protected UIImage joystickThumb;
    protected UIImage shootjoystickImage;
    protected UIImage shootjoystickThumb;
    protected UIImage playerLogoBackgroundImage;
    protected UIImage bulletsLogo;
    protected UIText cashText;
    protected UIText weaponInfoText;
    protected UIText fpsText;
    protected UIImage dayclear;
    protected UIImage mask;
    protected UIImage semiMask;
    protected UIImage switchImg;
    protected UIClickButton pauseButton;
    protected DayInfoPanel dayInfoPanel;
    /*
    protected UIText wavesText;
    protected UIText hpText;
    
    
    
    protected UIText bonusInfoText;
    protected UIText difficultyText;
    protected UIText tutorialText;

    protected UIClickButton switchWeaponButtonLeft;
    protected UIClickButton switchWeaponButtonRight;
    protected UIClickButton winButton;
    protected UIClickButton loseButton;
    protected UIClickButton bombButton;
   
    protected UIClickButton tutorialOKButton;
    */

    protected UIPanel[] panels = new UIPanel[GameUIName.UI_COUNT];

    protected GameDialog dialog;
    UnlockPanel unlockPanel;

    protected ITutorialUIEvent tutorialUIEvent;

    //public GUISkin guiSkin;

    protected float frames;
    protected float updateInterval = 2.0f;
    protected float timeLeft;
    protected string fpsStr;
    protected float accum;
    protected int count = 0;

    protected GameScene gameScene;
    protected Player player;
    private UIPosition uiPos;


    protected float screenRatioX;
    protected float screenRatioY;

    protected float lastUpdateTime;
    protected bool uiInited = false;
    protected float startTime = 0;

    void Awake()
    {

        if (GameObject.Find("ResourceConfig") == null)
        {
            GameObject resourceConfig = Object.Instantiate(Resources.Load("ResourceConfig")) as GameObject;
            resourceConfig.name = "ResourceConfig";
            DontDestroyOnLoad(resourceConfig);
        }


        UIResourceMgr.GetInstance().LoadAllGameUIMaterials();
        gameuiMaterial = UIResourceMgr.GetInstance().GetMaterial("GameUI");

        if (Application.loadedLevelName == SceneName.SCENE_TUTORIAL)
        {
            dialog = new GameDialog(UIDialog.DialogMode.TAP_TO_DISMISS);
            dialog.SetText(ConstData.FONT_NAME2, "", ColorName.fontColor_darkorange);
            dialog.SetDialogEventHandler(this);
        }
    }


    public void SetTutorialUIEvent(ITutorialUIEvent tEvent)
    {
        tutorialUIEvent = tEvent;
    }

    public void SetTutorialText(string text)
    {

    }

    public static GameUIScript GetGameUIScript()
    {
        return GameObject.Find("SceneGUI").GetComponent<GameUIScript>();
    }

    public Material FGetGameUIMaterial()
    {
        return gameuiMaterial;
    }


    // Use this for initialization
    IEnumerator Start()
    {


        yield return 0;

        uiPos = new UIPosition();
        //texPos = new GameUITexturePosition();

        float screenRatioX = ((float)Screen.width) / 960.00f;

        buttonRect = new Rect[4];
        buttonRect[ButtonNames.WEAPON_SWITCH] = new Rect(650, 540, 205, 89);
        buttonRect[ButtonNames.BOMB] = new Rect(0.4f * Screen.width, 0.25f * Screen.height, 0.24f * Screen.width, 0.08f * Screen.height);
        buttonRect[ButtonNames.CONTINUE] = new Rect(0.4f * Screen.width, 0.25f * Screen.height, 0.14f * Screen.width, 0.14f * Screen.height);
        buttonRect[ButtonNames.START_OVER] = new Rect(0.4f * Screen.width, 0.25f * Screen.height, 0.14f * Screen.width, 0.14f * Screen.height);


        if (AutoRect.GetPlatform() == Platform.IPad)
        {

            uiPos.PlayerLogo = new Rect(-16 - 32, 566 + 64, 116, 81);
            uiPos.PlayerLogoBackground = new Rect(0 - 32, 556 + 64, 134, 88);
            uiPos.HPBackground = new Rect(94 - 32, 590-12 + 64, 288, 50);
            uiPos.HPImage = new Rect(94 - 32, 590-12 + 64, 288, 50);
            uiPos.WeaponLogoBackground = new Rect(960 - 148 + 32, 640 - 74 + 54, 148, 88);
            uiPos.WeaponLogo = new Rect(960 - 180 + 32, 640 - 84 + 64, 194, 112);
            uiPos.BulletsLogo = new Rect(960 - 420 + 32, 640 - 94 + 64, 194, 112);
            uiPos.WeaponInfo = new Rect(656 +32, 558 + 64, 100, 64);
            uiPos.PauseButton = new Rect(408, 588, 160, 166);
            uiPos.CashText = new Rect(0, 576 + 64, 1024, 64);
            uiPos.Mask = new Rect(0, 0, 1024, 768);
            uiPos.Switch = new Rect(960 - 268 + 32, 640 - 90 +12+64, 148, 88);
                                    
        }



        gameScene = GameApp.GetInstance().GetGameScene();
        player = gameScene.GetPlayer();

        m_UIManager = gameObject.AddComponent<UIManager>() as UIManager;
        m_UIManager.SetParameter(8, 1, false);
        m_UIManager.SetUIHandler(this);




        int avatarLogoIndex = (int)player.GetAvatarType();
        Rect logoRect = GameUITexturePosition.GetAvatarLogoRect(avatarLogoIndex);
        //Player Logo
        playerLogoImage = new UIImage();
        playerLogoImage.Rect = AutoRect.AutoPos(uiPos.PlayerLogo);

        playerLogoImage.SetTexture(gameuiMaterial, logoRect, AutoRect.AutoSize(logoRect));
        //playerLogoImage.SetTextureSize(new Vector2(texPos.PlayerLogo.width, texPos.PlayerLogo.height));

        //HP
        hpBackground = new UIImage();
        hpBackground.SetTexture(gameuiMaterial, GameUITexturePosition.HPBackground, AutoRect.AutoSize(GameUITexturePosition.HPBackground));
        hpBackground.Rect = AutoRect.AutoPos(uiPos.HPBackground);


        dayclear = new UIImage();
        dayclear.SetTexture(gameuiMaterial, GameUITexturePosition.DayClear, AutoRect.AutoSize(GameUITexturePosition.DayClear));
        dayclear.Rect = AutoRect.AutoPos(uiPos.DayClear);
        dayclear.Visible = false;
        dayclear.Enable = false;
        hpImage = new UIImage();
        hpImage.SetTexture(gameuiMaterial, GameUITexturePosition.HPImage, AutoRect.AutoSize(GameUITexturePosition.HPImage));


        playerLogoBackgroundImage = new UIImage();
        playerLogoBackgroundImage.SetTexture(gameuiMaterial, GameUITexturePosition.PlayerLogoBackground, AutoRect.AutoSize(GameUITexturePosition.PlayerLogoBackground));
        playerLogoBackgroundImage.Rect = AutoRect.AutoPos(uiPos.PlayerLogoBackground);

        //Weapon Switch
        weaponBackground = new UIImage();
        weaponBackground.Rect = AutoRect.AutoPos(uiPos.WeaponLogoBackground);
        weaponBackground.SetTexture(gameuiMaterial, GameUITexturePosition.WeaponLogoBackground, AutoRect.AutoSize(GameUITexturePosition.WeaponLogoBackground));


        int weaponLogoIndex = GameApp.GetInstance().GetGameState().GetWeaponIndex(player.GetWeapon());
        Rect weaponlogoRect = GameUITexturePosition.GetWeaponLogoRect(weaponLogoIndex);

        weaponLogo = new UIClickButton();
        weaponLogo.Rect = AutoRect.AutoPos(uiPos.WeaponLogo);
        weaponLogo.SetTexture(UIButtonBase.State.Normal, gameuiMaterial, weaponlogoRect, AutoRect.AutoSize(weaponlogoRect));
        weaponLogo.SetTexture(UIButtonBase.State.Pressed, gameuiMaterial, weaponlogoRect, AutoRect.AutoSize(weaponlogoRect));


        switchImg = new UIImage();
        switchImg.Rect = AutoRect.AutoPos(uiPos.Switch);
        switchImg.SetTexture(gameuiMaterial, GameUITexturePosition.Switch, AutoRect.AutoSize(GameUITexturePosition.Switch));
        switchImg.Enable = true;

        Material buttonsMaterial = UIResourceMgr.GetInstance().GetMaterial("Buttons");
        bulletsLogo = new UIImage();
        bulletsLogo.Rect = AutoRect.AutoPos(uiPos.BulletsLogo);
        Rect bulletlogoRect = ButtonsTexturePosition.GetBulletsLogoRect((int)player.GetWeapon().GetWeaponType());
        bulletsLogo.SetTexture(buttonsMaterial, bulletlogoRect, AutoRect.AutoSize(bulletlogoRect));
        bulletsLogo.Enable = false;



        InputController inputController = player.InputController;
        //Move Joystick
        Vector2 thumbCenter = inputController.ThumbCenter;
        joystickImage = new UIImage();
        joystickImage.Rect = new Rect((thumbCenter.x - inputController.ThumbRadius), ((Screen.height - thumbCenter.y) - inputController.ThumbRadius), AutoRect.AutoValue(169), AutoRect.AutoValue(168));
        joystickImage.SetTexture(gameuiMaterial, GameUITexturePosition.MoveJoystick, AutoRect.AutoSize(GameUITexturePosition.MoveJoystick));

        joystickThumb = new UIImage();
        joystickThumb.SetTexture(gameuiMaterial, GameUITexturePosition.MoveJoystickThumb, AutoRect.AutoSize(GameUITexturePosition.MoveJoystickThumb));

        thumbCenter = inputController.ShootThumbCenter;
        shootjoystickImage = new UIImage();
        shootjoystickImage.Rect = new Rect((thumbCenter.x - inputController.ThumbRadius), ((Screen.height - thumbCenter.y) - inputController.ThumbRadius), AutoRect.AutoValue(169), AutoRect.AutoValue(168));
        shootjoystickImage.SetTexture(gameuiMaterial, GameUITexturePosition.ShootJoystick, AutoRect.AutoSize(GameUITexturePosition.ShootJoystick));
        shootjoystickImage.SetRotation(Mathf.Deg2Rad * 180);

        shootjoystickThumb = new UIImage();
        shootjoystickThumb.SetTexture(gameuiMaterial, GameUITexturePosition.ShootJoystickThumb, AutoRect.AutoSize(GameUITexturePosition.ShootJoystickThumb));


        pauseButton = new UIClickButton();
        pauseButton.Rect = AutoRect.AutoPos(uiPos.PauseButton);
        pauseButton.SetTexture(UIButtonBase.State.Normal, gameuiMaterial, GameUITexturePosition.PauseButtonNormal, AutoRect.AutoSize(GameUITexturePosition.PauseButtonNormal));
        pauseButton.SetTexture(UIButtonBase.State.Pressed, gameuiMaterial, GameUITexturePosition.PauseButtonPressed, AutoRect.AutoSize(GameUITexturePosition.PauseButtonPressed));


        //Cash
        cashText = new UIText();
        //cashText.Rect = new Rect(0.05f * Screen.width, 0.75f * Screen.height, 400, 50);
        cashText.AlignStyle = UIText.enAlignStyle.center;
        cashText.Rect = AutoRect.AutoPos(uiPos.CashText);
        cashText.Set(ConstData.FONT_NAME1, "$" + GameApp.GetInstance().GetGameState().GetCash().ToString("N0"), ColorName.fontColor_orange);




        //Weapon Info
        weaponInfoText = new UIText();
        weaponInfoText.AlignStyle = UIText.enAlignStyle.left;

        weaponInfoText.Rect = AutoRect.AutoPos(uiPos.WeaponInfo);
        weaponInfoText.Set(ConstData.FONT_NAME2, fpsStr, ColorName.fontColor_darkorange);



        fpsText = new UIText();
        fpsText.AlignStyle = UIText.enAlignStyle.left;
        fpsText.Rect = AutoRect.AutoPos(uiPos.LevelInfo);
        fpsText.Set(ConstData.FONT_NAME3, "", Color.white);

        dayInfoPanel = new DayInfoPanel();
        dayInfoPanel.SetDay(GameApp.GetInstance().GetGameState().LevelNum);

        mask = new UIImage();
        mask.SetTexture(gameuiMaterial, GameUITexturePosition.Mask, AutoRect.AutoSize(uiPos.Mask));
        mask.Rect = AutoRect.AutoValuePos(uiPos.Mask);

        Vector2 size = AutoRect.AutoSize(GameUITexturePosition.SemiMaskSize);
        Rect pos = AutoRect.AutoPos(uiPos.RightSemiMask); 
        if (AutoRect.GetPlatform() == Platform.IPad)
        {
            size = new Vector2(512, 768);
            pos = new Rect(512, 0, 512, 768);
        }
        semiMask = new UIImage();
        semiMask.SetTexture(gameuiMaterial, GameUITexturePosition.Mask, size);
        semiMask.Rect = pos;

        

        unlockPanel = new UnlockPanel();


        m_UIManager.Add(dayInfoPanel);
        m_UIManager.Add(hpBackground);
        m_UIManager.Add(hpImage);
        m_UIManager.Add(playerLogoBackgroundImage);
        m_UIManager.Add(playerLogoImage);
        m_UIManager.Add(joystickImage);
        m_UIManager.Add(joystickThumb);
        m_UIManager.Add(shootjoystickImage);
        m_UIManager.Add(shootjoystickThumb);
        m_UIManager.Add(weaponBackground);
        m_UIManager.Add(weaponLogo);
        m_UIManager.Add(switchImg);
        m_UIManager.Add(pauseButton);
        m_UIManager.Add(semiMask);
        if (Application.loadedLevelName == SceneName.SCENE_TUTORIAL)
        {
            m_UIManager.Add(dialog);
        }
        m_UIManager.Add(bulletsLogo);
        m_UIManager.Add(weaponInfoText);
        //m_UIManager.Add(cashText);
        m_UIManager.Add(mask);
        
        m_UIManager.Add(dayclear);
        //m_UIManager.Add(fpsText);
        
        m_UIManager.Add(unlockPanel);


        semiMask.Enable = false;
        semiMask.Visible = false;
        dayInfoPanel.Show();
        uiInited = true;

        EnableTutorialOKButton(false);
        mask.Enable = false;
        mask.Visible = false;

        SetWeaponLogo(player.GetWeapon().GetWeaponType());

        panels[GameUIName.PAUSE] = new PauseMenuUI();
        ((PauseMenuUI)panels[GameUIName.PAUSE]).SetGameUIScript(this);

        panels[GameUIName.GAME_OVER] = new GameOverUI();
        panels[GameUIName.NEW_ITEM] = new NewItemUI();

        //unlockPanel.Show();
        for (int i = 0; i < GameUIName.UI_COUNT; i++)
        {
            m_UIManager.Add(panels[i]);
        }
        startTime = Time.time;
    }

    

    public UIPanel GetPanel(int menuID)
    {
        return panels[menuID];
    }

    public void EnableTutorialOKButton(bool enable)
    {

    }

    public void ClearLevelInfo()
    {
    }

    // Update is called once per frame
    void Update()
    {

        timeLeft -= Time.deltaTime;

        accum += Time.timeScale / Time.deltaTime;
        frames++;

        if (timeLeft <= 0)
        {
            fpsStr = "FPS:" + (accum / frames).ToString();
            frames = 0;
            accum = 0;
            timeLeft = updateInterval;
        }

        if (uiInited)
        {
            fpsText.SetText(fpsStr);
            dayInfoPanel.UpdateAnimation();
            panels[GameUIName.GAME_OVER].UpdateLogic();

            if (FadeAnimationScript.GetInstance().FadeOutComplete())
            {
                foreach (UITouchInner touch in iPhoneInputMgr.MockTouches())
                {
                    if (m_UIManager.HandleInput(touch))
                    {
                        continue;
                    }
                }

            }


            if (gameScene.PlayingState == PlayingState.GameWin)
            {
                dayclear.Visible = true;
            }

            if (!GameApp.GetInstance().GetGameScene().GetPlayer().InputController.EnableShootingInput)
            {
                semiMask.Visible = true;
            }
            else
            {
                semiMask.Visible = false;
            }
        }

        if (Time.time - lastUpdateTime < 0.03f || !uiInited)
        {
            return;
        }

        lastUpdateTime = Time.time;


        if (player != null)
        {
            InputController inputController = player.InputController;

            float guihp = player.GetGuiHp();
            float guihpWidth = uiPos.HPImage.width * guihp / player.GetMaxHp();


            int g = (int)guihpWidth;
            if (g % 2 != 0)
            {
                g += 1;
            }
            if (hpImage != null)
            {
                hpImage.Rect = AutoRect.AutoPos(new Rect(uiPos.HPImage.xMin, uiPos.HPImage.yMin, g, uiPos.HPImage.height));
                hpImage.SetTexture(gameuiMaterial, GameUITexturePosition.GetHPTextureRect(g), AutoRect.AutoSize(GameUITexturePosition.GetHPTextureRect(g)));
            }

            cashText.SetText("$" + GameApp.GetInstance().GetGameState().GetCash().ToString("N0"));

            Weapon weapon = player.GetWeapon();
            if (weapon.GetWeaponType() == WeaponType.Saw)
            {
                weaponInfoText.SetText("");
            }
            else
            {
                weaponInfoText.SetText(" x" + weapon.BulletCount);
            }

            PlayingState playingState = gameScene.PlayingState;
            Vector2 lastTouchPos = inputController.LastTouchPos;

            joystickThumb.Rect = new Rect(lastTouchPos.x - AutoRect.AutoValue(0.5f * GameUITexturePosition.MoveJoystickThumb.width), lastTouchPos.y - AutoRect.AutoValue(0.5f * GameUITexturePosition.MoveJoystickThumb.height), AutoRect.AutoValue(GameUITexturePosition.MoveJoystickThumb.width), AutoRect.AutoValue(GameUITexturePosition.MoveJoystickThumb.height));

            shootjoystickThumb.Rect = new Rect(inputController.LastShootTouch.x - AutoRect.AutoValue(0.5f * GameUITexturePosition.ShootJoystickThumb.width), inputController.LastShootTouch.y - AutoRect.AutoValue(0.5f * GameUITexturePosition.ShootJoystickThumb.height), AutoRect.AutoValue(GameUITexturePosition.ShootJoystickThumb.width), AutoRect.AutoValue(GameUITexturePosition.ShootJoystickThumb.height));
            /*
             * 
             *  
             * 
            Vector2 thumbCenter = inputController.ThumbCenter;
            joystickImage.Rect = new Rect((thumbCenter.x - inputController.ThumbRadius), ((Screen.height - thumbCenter.y) - inputController.ThumbRadius), AutoRect.AutoValue(169), AutoRect.AutoValue(168));
            thumbCenter = inputController.ShootThumbCenter;
            shootjoystickImage.Rect = new Rect((thumbCenter.x - inputController.ThumbRadius), ((Screen.height - thumbCenter.y) - inputController.ThumbRadius), AutoRect.AutoValue(169), AutoRect.AutoValue(168));
        

            if (inputController.GetMoveTouchFingerID() == -1)
            {
                joystickImage.Visible = false;
                joystickThumb.Visible = false;
            }
            else
            {
                joystickImage.Visible = true;
                joystickThumb.Visible = true;
            }

            if (inputController.GetShootingTouchFingerID() == -1)
            {
                shootjoystickImage.Visible = false;
                shootjoystickThumb.Visible = false;
            }
            else
            {
                shootjoystickImage.Visible = true;
                shootjoystickThumb.Visible = true;
            }
            */

        }


    }



    public void SetWeaponLogo(WeaponType weaponType)
    {
        if (uiInited)
        {

            int weaponLogoIndex = GameApp.GetInstance().GetGameState().GetWeaponIndex(player.GetWeapon());
            Rect weaponlogoRect = GameUITexturePosition.GetWeaponLogoRect(weaponLogoIndex);
            weaponLogo.SetTexture(UIButtonBase.State.Normal, gameuiMaterial, weaponlogoRect, AutoRect.AutoSize(weaponlogoRect));
            weaponLogo.SetTexture(UIButtonBase.State.Pressed, gameuiMaterial, weaponlogoRect, AutoRect.AutoSize(weaponlogoRect));

            Material buttonsMaterial = UIResourceMgr.GetInstance().GetMaterial("Buttons");

            Rect bulletlogoRect = ButtonsTexturePosition.GetBulletsLogoRect((int)player.GetWeapon().GetWeaponType());
            bulletsLogo.SetTexture(buttonsMaterial, bulletlogoRect, AutoRect.AutoSize(bulletlogoRect));

        }

    }

    public void HandleEvent(UIControl control, int command, float wparam, float lparam)
    {

        if (control == weaponLogo || control == switchImg)
        {
            player.NextWeapon();
           
        }
        else if (control == pauseButton)
        {
            if (GameApp.GetInstance().GetGameScene().PlayingState == PlayingState.GamePlaying)
            {
                Time.timeScale = 0;
                //mask.Visible = true;
                panels[GameUIName.PAUSE].Show();
            }
        }


    }

    public GameDialog GetDialog()
    {
        return dialog;
    }

    public void Yes()
    {
        tutorialUIEvent.OK(player);
    }
    public void No()
    {

    }


}
