using UnityEngine;
using System.Collections;
using Zombie3D;

class NewItemUIPosition
{
    public Rect DialogImage = new Rect(175, 180, 610, 376);

    public Rect NewItemLabel = new Rect(300, 464, 360, 76);
    public Rect NewItemLabelText = new Rect(406, 442, 260, 76);
    public Rect RetryButton = new Rect(540, 60, 356, 116);
    public Rect QuitButton = new Rect(60, 60, 356, 116);


    public Rect WeaponLogo = new Rect(380, 320, 194, 112);
    public Rect FirstLineText = new Rect(0, 200, 960, 188);
    

    public Rect CashText = new Rect(400, 550, 202, 87);
    

}

public class NewItemUI : UIPanel, UIHandler
{
    protected Rect[] buttonRect;
    
    protected Material gameuiMaterial;

    protected UIImage dialogImage;
    protected UIImage newitemLabel;
    protected UIText newitemLabelText;
    protected UIImage unlockWeaponImage;
    protected UITextButton retryButton;
    protected UITextButton quitButton;

    protected UIText newEquipmentText;
    protected UIText returnText;

    protected UIText surviveTimeText;
    protected UIText firstLineText;
    protected UIText scoreText;
    protected UIText cashText;



    private NewItemUIPosition uiPos;

    protected float screenRatioX;
    protected float screenRatioY;

    protected bool uiInited = false;


    protected GameState gameState;
    protected Weapon selectedWeapon;

    // Use this for initialization
    public NewItemUI()
    {



        uiPos = new NewItemUIPosition();

        gameState = GameApp.GetInstance().GetGameState();
        selectedWeapon = gameState.GetWeapons()[0];

        gameuiMaterial = UIResourceMgr.GetInstance().GetMaterial("GameUI");
        Material buttonsMaterial = UIResourceMgr.GetInstance().GetMaterial("Buttons");



        /*
        retryButton = new UITextButton();

        retryButton.Rect = uiPos.RetryButton;
        retryButton.SetTexture(UIButtonBase.State.Normal, buttonsMaterial, ButtonsTexturePosition.ButtonNormal);
        retryButton.SetTexture(UIButtonBase.State.Pressed, buttonsMaterial, ButtonsTexturePosition.ButtonPressed);
        retryButton.SetText(ConstData.FONT_NAME1, "RETRY", ColorName.fontColor_orange);

        quitButton = new UITextButton();

        quitButton.Rect = uiPos.QuitButton;
        quitButton.SetTexture(UIButtonBase.State.Normal, buttonsMaterial, ButtonsTexturePosition.ButtonNormal);
        quitButton.SetTexture(UIButtonBase.State.Pressed, buttonsMaterial, ButtonsTexturePosition.ButtonPressed);
        quitButton.SetText(ConstData.FONT_NAME1, "QUIT", ColorName.fontColor_orange);
        */

        Material dialogMaterial = UIResourceMgr.GetInstance().GetMaterial("Dialog");


        dialogImage = new UIImage();
        dialogImage.SetTexture(dialogMaterial,

DialogTexturePosition.Dialog, AutoRect.AutoSize(DialogTexturePosition.Dialog));

        dialogImage.Rect = AutoRect.AutoPos(uiPos.DialogImage);

        unlockWeaponImage = new UIImage();
        unlockWeaponImage.Rect = AutoRect.AutoPos(uiPos.WeaponLogo);

        newitemLabel = new UIImage();
        newitemLabel.SetTexture(buttonsMaterial,

ButtonsTexturePosition.Label, AutoRect.AutoSize(ButtonsTexturePosition.Label));

        newitemLabel.Rect = AutoRect.AutoPos(uiPos.NewItemLabel);

        newitemLabelText = new UIText();
        newitemLabelText.Set(ConstData.FONT_NAME2, " NEW ITEM", ColorName.fontColor_orange);
        newitemLabelText.Rect = AutoRect.AutoPos(uiPos.NewItemLabelText);


        cashText = new UIText();
        cashText.Set(ConstData.FONT_NAME1, "Cash", ColorName.fontColor_yellow);
        cashText.Rect = AutoRect.AutoPos(uiPos.CashText);


        /*
        surviveTimeText = new UIText();
        surviveTimeText.Set(ConstData.FONT_NAME2, "TIME ", ColorName.fontColor_darkorange);
        surviveTimeText.Rect = uiPos.SurviveTimeText;
        */

        firstLineText = new UIText();
        firstLineText.Set(ConstData.FONT_NAME2, "", ColorName.fontColor_darkorange);
        firstLineText.AlignStyle = UIText.enAlignStyle.center;
        firstLineText.Rect = AutoRect.AutoPos(uiPos.FirstLineText);

        /*
        scoreText = new UIText();
        scoreText.Set(ConstData.FONT_NAME2, "KILLS   "+GameApp.GetInstance().GetGameScene().Killed, ColorName.fontColor_darkorange);
        scoreText.Rect = uiPos.ScoreText;
        */
       
        
        this.Add(dialogImage);
        this.Add(newitemLabel);
        this.Add(newitemLabelText);
        this.Add(unlockWeaponImage);
        this.Add(firstLineText);
        //this.Add(scoreText);
        //this.Add(retryButton);
        //this.Add(quitButton);
        
       
        this.SetUIHandler(this);

        uiInited = true;
       
    }


    public void SetUnlockWeapon(Weapon w)
    {
        if (w != null)
        {
            Material material = UIResourceMgr.GetInstance().GetMaterial("GameUI");
            int weaponLogoIndex = GameApp.GetInstance().GetGameState().GetWeaponIndex(w);
            Rect weaponlogoRect = GameUITexturePosition.GetWeaponLogoRect(weaponLogoIndex);
            unlockWeaponImage.SetTexture(material, weaponlogoRect);
        }
    }



    public override void Show()
    {
        //GameApp.GetInstance().GetGameScene().BonusWeapon = GameApp.GetInstance().GetGameState().GetWeapons()[2];
        SetUnlockWeapon(GameApp.GetInstance().GetGameScene().BonusWeapon);
        firstLineText.Set(ConstData.FONT_NAME2, GameApp.GetInstance().GetGameScene().BonusWeapon.Name+" IS AVAILABLE FOR PURCHASE!", ColorName.fontColor_darkorange);
        base.Show();
    }

    public void DisplayBattleEndUI()
    {
        //m_UIManager.active = true;
        
    }


    public void HandleEvent(UIControl control, int command, float wparam, float lparam)
    {

        if (control == retryButton)
        {
            GameApp.GetInstance().Save();
            Application.LoadLevel(SceneName.SCENE_ARENA);
        }
        else if (control == quitButton)
        {
            UIResourceMgr.GetInstance().UnloadAllUIMaterials();
            Application.LoadLevel(SceneName.MAP);
            GameApp.GetInstance().Save();
        }
    }

}
