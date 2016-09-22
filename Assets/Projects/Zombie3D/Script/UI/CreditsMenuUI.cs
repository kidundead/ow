using UnityEngine;
using System.Collections;
using Zombie3D;


class CreditsMenuUIPosition
{

    public Rect Background = new Rect(0, 0, 960, 640);

    public Rect Dialog = new Rect(110, 220, 736, 376);
    public Rect TitleImage = new Rect(90, 560, 267, 71);
    public Rect ReturnButton = new Rect(24, 6, 130, 70);

    public Rect RightButton = new Rect(670, 190, 131, 80);

    public Rect DesignerText = new Rect(300 + 80, 400 - 30, 279, 91);
    public Rect ArtistText = new Rect(300 + 80, 350 - 30, 279, 91);
    public Rect ProgrammerText = new Rect(300 + 80, 300 - 30, 279, 91);
    public Rect QAText = new Rect(300 + 80, 250 - 30, 279, 91);
    public Rect ReturnText = new Rect(400, 0, 279, 91);
}

public class CreditsMenuUI : UIPanel, UIHandler
{
    protected Rect[] buttonRect;

    protected Material creditsMenuMaterial;
    protected Material backgroundMenuMaterial;

    protected UIImage background;
    protected UIImage dialog;
    protected UIImage titleImage;
    protected UIClickButton returnButton;
    protected UIClickButton okButton;

    protected UIText designerText;
    protected UIText artistText;
    protected UIText programmerText;
    protected UIText qaText;


    protected UIImage creditsBackground;
    protected UIClickButton creditsReturnButton;

    private CreditsMenuUIPosition uiPos;
    private CreditsMenuTexturePosition texPos;

    protected float screenRatioX;
    protected float screenRatioY;
    protected GameState gameState;
    protected GameObject optionsUI;
    protected bool enableBackground = true;
    protected ArenaMenuUI ui;
    // Use this for initialization

   
    void Awake()
    {
    }

    public void RemoveBackground()
    {
        enableBackground = false;

    }

    public CreditsMenuUI()
    {
        uiPos = new CreditsMenuUIPosition();
        texPos = new CreditsMenuTexturePosition();
        gameState = GameApp.GetInstance().GetGameState();



        
        if (enableBackground)
        {
            backgroundMenuMaterial = UIResourceMgr.GetInstance().GetMaterial("ArenaMenu");
            Material creditsMaterial = UIResourceMgr.GetInstance().GetMaterial("Credits");

            background = new UIImage();
            background.SetTexture(creditsMaterial,

    CreditsMenuTexturePosition.Background, AutoRect.AutoSize(CreditsMenuTexturePosition.Background));
            background.Rect = AutoRect.AutoPos(uiPos.Background);


            titleImage = new UIImage();
            titleImage.SetTexture(backgroundMenuMaterial, texPos.TitleImage);
            titleImage.Rect = uiPos.TitleImage;



            returnButton = new UIClickButton();
            returnButton.SetTexture(UIButtonBase.State.Normal, backgroundMenuMaterial,

    ArenaMenuTexturePosition.ReturnButtonNormal, AutoRect.AutoSize(ArenaMenuTexturePosition.ReturnButtonNormal));
            returnButton.SetTexture(UIButtonBase.State.Pressed, backgroundMenuMaterial,

    ArenaMenuTexturePosition.ReturnButtonPressed, AutoRect.AutoSize(ArenaMenuTexturePosition.ReturnButtonPressed));

            returnButton.Rect = AutoRect.AutoPos(uiPos.ReturnButton);


        }

        /*
        dialog = new UIImage();
        dialog.SetTexture(creditsMenuMaterial,

texPos.Dialog);
        dialog.Rect = uiPos.Dialog;
        */



        okButton = new UIClickButton();
        okButton.SetTexture(UIButtonBase.State.Normal, creditsMenuMaterial,

texPos.RightButtonNormal);
        okButton.SetTexture(UIButtonBase.State.Pressed, creditsMenuMaterial,

texPos.RightButtonPressed);


        okButton.Rect = uiPos.RightButton;



        designerText = new UIText();
        designerText.Set(ConstData.FONT_NAME1, "DESIGNER", ColorName.fontColor_orange);
        designerText.Rect = uiPos.DesignerText;



        artistText = new UIText();
        artistText.Set(ConstData.FONT_NAME1, "ARTIST", ColorName.fontColor_orange);
        artistText.Rect = uiPos.ArtistText;

        programmerText = new UIText();
        programmerText.Set(ConstData.FONT_NAME1, "PROGRAMMER", ColorName.fontColor_orange);
        programmerText.Rect = uiPos.ProgrammerText;


        qaText = new UIText();
        qaText.Set(ConstData.FONT_NAME1, "QA", ColorName.fontColor_orange);
        qaText.Rect = uiPos.QAText;


        if (enableBackground)
        {
            this.Add(background);

            this.Add(returnButton);
            //this.Add(titleImage);
            //this.Add(dialog);
        }
        else
        {
            //this.Add(dialog);
            this.Add(okButton);
        }


        //this.Add(designerText);
        //this.Add(artistText);
        //this.Add(programmerText);
        //this.Add(qaText);
        Hide();
        

        GameObject obj = GameObject.Find("ArenaMenuUI");
        if(obj!=null)
        {
            ui = obj.GetComponent<ArenaMenuUI>();
        }
        this.SetUIHandler(this);
        //gameObject.SetActiveRecursively(false);
    }

    public void SetOptionsUI(GameObject obj)
    {
        optionsUI = obj;
    }



    public void HandleEvent(UIControl control, int command, float wparam, float lparam)
    {
        
        if (control == returnButton || control == okButton)
        {
            MapUI.GetInstance().GetAudioPlayer().PlayAudio("Button");
            //gameObject.SetActiveRecursively(false);
            this.Hide();

            //if (ui != null)
            {
                MapUI.GetInstance().GetOptionsMenuUI().Show();
            }
        }

    }

}
