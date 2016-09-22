using UnityEngine;
using System.Collections;
using Zombie3D;



class OptionsMenuUIPosition
{

    public Rect Background = new Rect(0, 0, 960, 640);
    public Rect SoundPanel = new Rect(336, 460, 260, 76);

    public Rect MusicButtonOn = new Rect(280, 380, 148, 80);
    public Rect MusicButtonOff = new Rect(512, 380, 148, 80);

    public Rect CreditsButton = new Rect(300, 276, 356, 90);

    public Rect ShareButton = new Rect(60, 120, 250, 80);
    public Rect ReviewButton = new Rect(350, 120, 250, 80);
    public Rect SupportButton = new Rect(640, 120, 250, 80);


    public Rect ReturnButton = new Rect(24, 6, 130, 70);
    public Rect DaysPanel = new Rect(106, 640 - 70, 264, 64);
}





public class OptionsMenuUI : UIPanel, UIHandler
{
    protected Rect[] buttonRect;

    //public UIManager m_UIManager = null;
    protected CreditsMenuUI cm;
    public string m_ui_material_path;

    protected Material buttonsMaterial;
    protected Material arenaMenuMaterial;

    protected UIImage background;

    protected UITextImage soundPanel;
    protected UITextSelectButton musicButtonOff;
    protected UITextSelectButton musicButtonOn;
    protected UITextButton creditsButton;
    protected UITextButton shareButton;
    protected UITextButton reviewButton;
    protected UITextButton supportButton;
    protected UIClickButton returnButton;
    protected UITextImage daysPanel;
    protected CashPanel cashPanel;


    private OptionsMenuUIPosition uiPos;
    private OptionsMenuTexturePosition texPos;

    protected float screenRatioX;
    protected float screenRatioY;
    protected GameState gameState;
    protected CreditsMenuUI creditsPanel;

    protected MapUI ui;



    // Use this for initialization
    public OptionsMenuUI()
    {

        uiPos = new OptionsMenuUIPosition();
        texPos = new OptionsMenuTexturePosition();
        gameState = GameApp.GetInstance().GetGameState();


        buttonsMaterial = UIResourceMgr.GetInstance().GetMaterial("Buttons");
        arenaMenuMaterial = UIResourceMgr.GetInstance().GetMaterial("ArenaMenu");


        background = new UIImage();
        background.SetTexture(arenaMenuMaterial,

ArenaMenuTexturePosition.Background, AutoRect.AutoSize(ArenaMenuTexturePosition.Background));
        background.Rect = AutoRect.AutoPos(uiPos.Background);



        daysPanel = new UITextImage();
        daysPanel.SetTexture(arenaMenuMaterial,

ArenaMenuTexturePosition.Panel, AutoRect.AutoSize(ArenaMenuTexturePosition.Panel));
        daysPanel.Rect = AutoRect.AutoPos(uiPos.DaysPanel);
        daysPanel.SetText(ConstData.FONT_NAME1, "DAY " + GameApp.GetInstance().GetGameState().LevelNum, ColorName.fontColor_darkorange);


        cashPanel = new CashPanel();


        soundPanel = new UITextImage();
        soundPanel.SetTexture(buttonsMaterial,

ButtonsTexturePosition.Label, AutoRect.AutoSize(ButtonsTexturePosition.Label));
        soundPanel.Rect = AutoRect.AutoPos(uiPos.SoundPanel);
        soundPanel.SetText(ConstData.FONT_NAME1, " SOUND", ColorName.fontColor_darkorange);

        musicButtonOff = new UITextSelectButton();
        musicButtonOff.SetTexture(UIButtonBase.State.Normal, buttonsMaterial,

ButtonsTexturePosition.SoundButtonNormal, AutoRect.AutoSize(ButtonsTexturePosition.SoundButtonNormal));
        musicButtonOff.SetTexture(UIButtonBase.State.Pressed, buttonsMaterial,

ButtonsTexturePosition.SoundButtonPressed, AutoRect.AutoSize(ButtonsTexturePosition.SoundButtonPressed));
        musicButtonOff.Rect = AutoRect.AutoPos(uiPos.MusicButtonOff);
        musicButtonOff.SetText(ConstData.FONT_NAME1, " OFF", ColorName.fontColor_orange);



        musicButtonOn = new UITextSelectButton();
        musicButtonOn.SetTexture(UIButtonBase.State.Normal, buttonsMaterial,

ButtonsTexturePosition.SoundButtonNormal, AutoRect.AutoSize(ButtonsTexturePosition.SoundButtonNormal));
        musicButtonOn.SetTexture(UIButtonBase.State.Pressed, buttonsMaterial,

ButtonsTexturePosition.SoundButtonPressed, AutoRect.AutoSize(ButtonsTexturePosition.SoundButtonPressed));
        musicButtonOn.Rect = AutoRect.AutoPos(uiPos.MusicButtonOn);
        musicButtonOn.SetText(ConstData.FONT_NAME1, " ON", ColorName.fontColor_orange);

        if (gameState.MusicOn)
        {
            musicButtonOn.Set(true);
            musicButtonOff.Set(false);
        }
        else
        {
            musicButtonOn.Set(false);
            musicButtonOff.Set(true);
        }




        creditsButton = new UITextButton();
        creditsButton.SetTexture(UIButtonBase.State.Normal, buttonsMaterial,

ButtonsTexturePosition.ButtonNormal, AutoRect.AutoSize(ButtonsTexturePosition.MiddleSizeButton));
        creditsButton.SetTexture(UIButtonBase.State.Pressed, buttonsMaterial,

ButtonsTexturePosition.ButtonPressed, AutoRect.AutoSize(ButtonsTexturePosition.MiddleSizeButton));

        creditsButton.Rect = AutoRect.AutoPos(uiPos.CreditsButton);
        creditsButton.SetText(ConstData.FONT_NAME1, " CREDITS", ColorName.fontColor_orange);



        shareButton = new UITextButton();
        shareButton.SetTexture(UIButtonBase.State.Normal, buttonsMaterial,

ButtonsTexturePosition.ButtonNormal, AutoRect.AutoSize(ButtonsTexturePosition.SmallSizeButton));
        shareButton.SetTexture(UIButtonBase.State.Pressed, buttonsMaterial,

ButtonsTexturePosition.ButtonPressed, AutoRect.AutoSize(ButtonsTexturePosition.SmallSizeButton));

        shareButton.Rect = AutoRect.AutoPos(uiPos.ShareButton);
        shareButton.SetText(ConstData.FONT_NAME1, " SHARE", ColorName.fontColor_darkorange);


        reviewButton = new UITextButton();
        reviewButton.SetTexture(UIButtonBase.State.Normal, buttonsMaterial,

ButtonsTexturePosition.ButtonNormal,AutoRect.AutoSize( ButtonsTexturePosition.SmallSizeButton));
        reviewButton.SetTexture(UIButtonBase.State.Pressed, buttonsMaterial,

ButtonsTexturePosition.ButtonPressed, AutoRect.AutoSize(ButtonsTexturePosition.SmallSizeButton));

        reviewButton.Rect = AutoRect.AutoPos(uiPos.ReviewButton);
        reviewButton.SetText(ConstData.FONT_NAME1, " REVIEW", ColorName.fontColor_darkorange);



        supportButton = new UITextButton();
        supportButton.SetTexture(UIButtonBase.State.Normal, buttonsMaterial,

ButtonsTexturePosition.ButtonNormal, AutoRect.AutoSize(ButtonsTexturePosition.SmallSizeButton));
        supportButton.SetTexture(UIButtonBase.State.Pressed, buttonsMaterial,

ButtonsTexturePosition.ButtonPressed, AutoRect.AutoSize(ButtonsTexturePosition.SmallSizeButton));

        supportButton.Rect = AutoRect.AutoPos(uiPos.SupportButton);
        supportButton.SetText(ConstData.FONT_NAME1, " SUPPORT", ColorName.fontColor_darkorange);

        returnButton = new UIClickButton();
        returnButton.SetTexture(UIButtonBase.State.Normal, arenaMenuMaterial,

ArenaMenuTexturePosition.ReturnButtonNormal,AutoRect.AutoSize(ArenaMenuTexturePosition.ReturnButtonNormal));
        returnButton.SetTexture(UIButtonBase.State.Pressed, arenaMenuMaterial,

ArenaMenuTexturePosition.ReturnButtonPressed,AutoRect.AutoSize(ArenaMenuTexturePosition.ReturnButtonPressed));

        returnButton.Rect = AutoRect.AutoPos(uiPos.ReturnButton);






        creditsPanel = new CreditsMenuUI();

        this.Add(background);
        this.Add(daysPanel);
        this.Add(cashPanel);
        this.Add(soundPanel);
        this.Add(musicButtonOff);
        this.Add(musicButtonOn);
        this.Add(creditsButton);
        this.Add(shareButton);
        this.Add(reviewButton);
        this.Add(supportButton);
        this.Add(returnButton);

        this.Add(creditsPanel);

        ui = MapUI.GetInstance();
        this.SetUIHandler(this);
    }



    /*
    // Update is called once per frame
    void Update()
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
    */

    public override void Show()
    {
        base.Show();

        cashPanel.SetCash(GameApp.GetInstance().GetGameState().GetCash());
        cashPanel.Show();
    }
    public void EnableUI()
    {
        //m_UIManager.enabled = true;
    }
    public void HandleEvent(UIControl control, int command, float wparam, float lparam)
    {

       
        if (control == musicButtonOff)
        {
            musicButtonOn.Set(false);
            AudioListener.volume = 0;
            gameState.MusicOn = false;
        }
        else if (control == musicButtonOn)
        {
            musicButtonOff.Set(false);
            AudioListener.volume = 100;
            gameState.MusicOn = true;
            MapUI.GetInstance().GetAudioPlayer().PlayAudio("Button");
        }

        else if (control == creditsButton)
        {
            MapUI.GetInstance().GetAudioPlayer().PlayAudio("Button");
            creditsPanel.Show();
        }
        else if (control == returnButton)
        {
            MapUI.GetInstance().GetAudioPlayer().PlayAudio("Button");
            this.Hide();
            ui.GetMapUI().Show();
        }
        else if (control == shareButton)
        {
            string htm = GameApp.GetInstance().GetResourceConfig().shareHtm.text;

            Utils.ToSendMail("", "Call Of Mini: Zombies", htm);
        }
        else if (control == reviewButton)
        {
            Application.OpenURL("http://www.trinitigame.com/callofminizombies/review/");
        }
        else if(control == supportButton)
        {
            Utils.ToSendMail("support@trinitigame.com", "Call Of Mini: Zombies", "");
        }

    }

}
