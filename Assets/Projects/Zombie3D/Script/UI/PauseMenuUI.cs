using UnityEngine;
using System.Collections;
using Zombie3D;


class PauseMenuUIPosition
{

    public Rect Background = new Rect(186, 160, 588, 444);

    public Rect PauseLabel = new Rect(350, 504, 260, 76);

    public Rect SoundLabel = new Rect(336, 382, 260, 76);

    public Rect MusicButtonOn = new Rect(276, 310, 148, 80);
    public Rect MusicButtonOff = new Rect(522, 310, 148, 80);
    


    //public Rect CreditsButton = new Rect(300, 220, 350, 101);

    public Rect ResumeButton = new Rect(510, 140, 250, 80);
    public Rect ReturnButton = new Rect(200, 140, 250, 80);
    public Rect Mask = new Rect(0, 0, 1024, 768);
}


public class PauseMenuUI : UIPanel, UIHandler
{
    protected Rect[] buttonRect;

    public UIManager m_UIManager = null;

    public string m_ui_material_path;

    protected Material buttonsMaterial;
    protected Material gameuiMaterial;

    protected UIImage background;
    protected UITextImage pauseLabel;
    protected UITextImage soundLabel;

    protected UITextSelectButton musicButtonOff;
    protected UITextSelectButton musicButtonOn;
    protected UITextButton resumeButton;
    protected UITextButton returnButton;
    protected UIImage mask;
   
    

    private PauseMenuUIPosition uiPos;

    protected float screenRatioX;
    protected float screenRatioY;
    protected GameState gameState;
    protected GameUIScript ui;
    
    public PauseMenuUI()
    {


        uiPos = new PauseMenuUIPosition();
       
        gameState = GameApp.GetInstance().GetGameState();
        

        buttonsMaterial = UIResourceMgr.GetInstance().GetMaterial("Buttons");
        gameuiMaterial = UIResourceMgr.GetInstance().GetMaterial("GameUI");
        background = new UIImage();
        background.SetTexture(gameuiMaterial,

GameUITexturePosition.Dialog, AutoRect.AutoSize(GameUITexturePosition.DialogSize));
        background.Rect = AutoRect.AutoPos(uiPos.Background);


        pauseLabel = new UITextImage();
        pauseLabel.SetTexture(buttonsMaterial,

ButtonsTexturePosition.Label, AutoRect.AutoSize(ButtonsTexturePosition.Label));
        pauseLabel.Rect = AutoRect.AutoPos(uiPos.PauseLabel);
        pauseLabel.SetText(ConstData.FONT_NAME1, " PAUSE", ColorName.fontColor_orange);


        soundLabel = new UITextImage();
        soundLabel.SetTexture(buttonsMaterial,

ButtonsTexturePosition.Label, AutoRect.AutoSize(ButtonsTexturePosition.Label));
        soundLabel.Rect = AutoRect.AutoPos(uiPos.SoundLabel);
        soundLabel.SetText(ConstData.FONT_NAME1, " SOUND", ColorName.fontColor_orange);




        resumeButton = new UITextButton();
        resumeButton.SetTexture(UIButtonBase.State.Normal, buttonsMaterial,

ButtonsTexturePosition.ButtonNormal,AutoRect.AutoSize(ButtonsTexturePosition.SmallSizeButton));
        resumeButton.SetTexture(UIButtonBase.State.Pressed, buttonsMaterial,

ButtonsTexturePosition.ButtonPressed, AutoRect.AutoSize(ButtonsTexturePosition.SmallSizeButton));
        resumeButton.Rect = AutoRect.AutoPos(uiPos.ResumeButton);
        resumeButton.SetText(ConstData.FONT_NAME1, " RESUME", ColorName.fontColor_orange);


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




        returnButton = new UITextButton();
        returnButton.SetTexture(UIButtonBase.State.Normal, buttonsMaterial,

ButtonsTexturePosition.ButtonNormal, AutoRect.AutoSize(ButtonsTexturePosition.SmallSizeButton));
        returnButton.SetTexture(UIButtonBase.State.Pressed, buttonsMaterial,

ButtonsTexturePosition.ButtonPressed, AutoRect.AutoSize(ButtonsTexturePosition.SmallSizeButton));
        returnButton.Rect = AutoRect.AutoPos(uiPos.ReturnButton);
        returnButton.SetText(ConstData.FONT_NAME1, " QUIT", ColorName.fontColor_orange);


        mask = new UIImage();
        mask.SetTexture(gameuiMaterial, GameUITexturePosition.Mask, AutoRect.AutoSize(uiPos.Mask));
        mask.Rect = AutoRect.AutoValuePos(uiPos.Mask);



        this.Add(mask);
        this.Add(background);

        //this.Add(pauseLabel);

        this.Add(soundLabel);

        
       
        this.Add(musicButtonOff);
        this.Add(musicButtonOn);

        this.Add(resumeButton);
        this.Add(returnButton);



        this.SetUIHandler(this);
        
        Hide();

    }

    public void SetGameUIScript(GameUIScript guis)
    {
        ui = guis;
    }


    public void HandleEvent(UIControl control, int command, float wparam, float lparam)
    {
        

        if (control == resumeButton)
        {
            Time.timeScale = 1;
            //AudioPlayer.PlayAudio(GameUIScript.GetGameUIScript().audio);
            //gameObject.SetActiveRecursively(false);
            
            Hide();
        }


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
            //AudioPlayer.PlayAudio(ArenaMenuUI.GetInstance().audio);
        }

        /*
        else if (control == musicButton)
        {

            if (gameState.MusicOn)
            {
                musicLogoButton.SetTexture(UIButtonBase.State.Normal, buttonsMaterial,

texPos.MusicOffLogoButtonNormal);
                musicLogoButton.SetTexture(UIButtonBase.State.Pressed, buttonsMaterial,

        texPos.MusicOffLogoButtonPressed);
                gameState.MusicOn = false;
            }
            else
            {
                musicLogoButton.SetTexture(UIButtonBase.State.Normal, buttonsMaterial,

texPos.MusicOnLogoButtonNormal);
                musicLogoButton.SetTexture(UIButtonBase.State.Pressed, buttonsMaterial,

        texPos.MusicOnLogoButtonPressed);
                gameState.MusicOn = true;
                //AudioPlayer.PlayAudio(GameUIScript.GetGameUIScript().audio);
            }

        }
        else if (control == musicLogoButton)
        {

            if (gameState.MusicOn)
            {
                musicLogoButton.SetTexture(UIButtonBase.State.Normal, buttonsMaterial,

texPos.MusicOffLogoButtonNormal);
                musicLogoButton.SetTexture(UIButtonBase.State.Pressed, buttonsMaterial,

        texPos.MusicOffLogoButtonPressed);
                gameState.MusicOn = false;
                AudioListener.volume = 0;

            }
            else
            {
                musicLogoButton.SetTexture(UIButtonBase.State.Normal, buttonsMaterial,

texPos.MusicOnLogoButtonNormal);
                musicLogoButton.SetTexture(UIButtonBase.State.Pressed, buttonsMaterial,

        texPos.MusicOnLogoButtonPressed);
                gameState.MusicOn = true;
                //AudioPlayer.PlayAudio(GameUIScript.GetGameUIScript().audio);
                AudioListener.volume = 100;
            }
        }*/
        else if (control == returnButton)
        {
            GameApp.GetInstance().GetGameScene().PlayingState = PlayingState.GameQuit;
            Time.timeScale = 1;
            //AudioPlayer.PlayAudio(GameUIScript.GetGameUIScript().audio);
            UIResourceMgr.GetInstance().UnloadAllUIMaterials();
            GameApp.GetInstance().Load();
            Application.LoadLevel(SceneName.MAP);
            /*
            Hide();
            ui.GetPanel(GameUIName.QUIT).Show();
            */
        }

    }
}
