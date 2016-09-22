using UnityEngine;
using System.Collections;
using Zombie3D;


class AvatarUIPosition
{
    public Rect Background = new Rect(0, 0, 960, 640);
    public Rect AvatarImage = new Rect(90, 560, 267, 71);
    public Rect TextBackround = new Rect(20, 80, 396, 131);
    public Rect ReturnButton = new Rect(24, 6, 130, 70);


    public Rect AvatarInfoText = new Rect(50, 60, 320, 127);

    public Rect BuyButton = new Rect(598, 0, 356, 116);

    public Rect CashPanel = new Rect(400, 550, 310, 79);

    public Rect CostPanel = new Rect(655, 550, 294, 69);
    public Rect GetMoreMoneyButton = new Rect(42, 558, 392, 82);

    public Rect AvatarInfoPanel = new Rect(450, 150, 490, 176);
}

public class AvatarInfoPanel : UIPanel
{
    protected UIImage background;

    protected UIText infoText;
    public AvatarInfoPanel()
    {
        AvatarUIPosition uiPos = new AvatarUIPosition();
        Material dialogMaterial = UIResourceMgr.GetInstance().GetMaterial("Dialog");
        background = new UIImage();
        background.SetTexture(dialogMaterial, DialogTexturePosition.TextBox, AutoRect.AutoSize(DialogTexturePosition.TextBox));
        background.Rect = AutoRect.AutoPos(uiPos.AvatarInfoPanel);
        background.Enable = false;
        Add(background);




        infoText = new UIText();
        infoText.Set(ConstData.FONT_NAME3, "", ColorName.fontColor_darkorange);

        infoText.Rect = AutoRect.AutoPos(new Rect(uiPos.AvatarInfoPanel.x + 50, uiPos.AvatarInfoPanel.y + 10, uiPos.AvatarInfoPanel.width-70, uiPos.AvatarInfoPanel.height - 40));



        Add(infoText);
    }

    public void SetText(string text)
    {

        infoText.SetText(text);
    }


}


public class AvatarUI : UIPanel, UIHandler, UIDialogEventHandler
{
    protected Rect[] buttonRect;

    protected Material arenaMenuMaterial;
    protected Material avatarLogoMaterial;


    protected const int BUTTON_NUM = 8;

    protected UIImage background;
    protected UIClickButton getMoreMoneyButton;
    protected UIImage avatarImage;
    protected UIImage textBackground;
    protected UIImage avatarClickImage;
    protected UIClickButton returnButton;
    protected CashPanel cashPanel;
    protected UIImageScroller avatarScroller;
    protected UIText avatarInfoText;
    protected IAPDialog iapDialog;
    
    protected UIText returnText;

    protected UITextButton buyButton;

    private AvatarUIPosition uiPos;
    protected AvatarInfoPanel avatarInfoPanel;

    protected Avatar3DFrame avatarFrame;

    protected int currentSelectionIndex = 0;
    // Use this for initialization
    public AvatarUI()
    {

        uiPos = new AvatarUIPosition();

        /*
        for (int i = 0; i < AvatarTexturePosition.AvatarLogo.Length; i++)
        {
            int x = i % 2;
            int y = i / 2;
            AvatarTexturePosition.AvatarLogo[i] = new Rect(x * 446, y * 200, 446, 200);
        }
        */
        AvatarTexturePosition.InitLogosTexturePos();

        arenaMenuMaterial = UIResourceMgr.GetInstance().GetMaterial("ArenaMenu");
        avatarLogoMaterial = UIResourceMgr.GetInstance().GetMaterial("Avatar");
        Material buttonsMaterial = UIResourceMgr.GetInstance().GetMaterial("Buttons");
        background = new UIImage();
        background.SetTexture(arenaMenuMaterial,

ArenaMenuTexturePosition.Background, AutoRect.AutoSize(ArenaMenuTexturePosition.Background));
        background.Rect = AutoRect.AutoPos(uiPos.Background);

        

        returnButton = new UIClickButton();
        returnButton.SetTexture(UIButtonBase.State.Normal, arenaMenuMaterial,

ArenaMenuTexturePosition.ReturnButtonNormal, AutoRect.AutoSize(ArenaMenuTexturePosition.ReturnButtonNormal));
        returnButton.SetTexture(UIButtonBase.State.Pressed, arenaMenuMaterial,

ArenaMenuTexturePosition.ReturnButtonPressed,AutoRect.AutoSize(ArenaMenuTexturePosition.ReturnButtonPressed));
        returnButton.Rect = AutoRect.AutoPos(uiPos.ReturnButton);


        buyButton = new UITextButton();
        buyButton.SetTexture(UIButtonBase.State.Normal, buttonsMaterial, ButtonsTexturePosition.ButtonNormal, AutoRect.AutoSize(ButtonsTexturePosition.ButtonNormal));
        buyButton.SetTexture(UIButtonBase.State.Pressed, buttonsMaterial, ButtonsTexturePosition.ButtonPressed, AutoRect.AutoSize(ButtonsTexturePosition.ButtonPressed));
        buyButton.Rect = AutoRect.AutoPos(uiPos.BuyButton);
        buyButton.SetText(ConstData.FONT_NAME0, " SELECT", ColorName.fontColor_orange);
        SetBuyButtonText();
        cashPanel = new CashPanel();

        getMoreMoneyButton = new UITextButton();
        getMoreMoneyButton.SetTexture(UIButtonBase.State.Normal, arenaMenuMaterial,

ArenaMenuTexturePosition.GetMoneyButtonNormal, AutoRect.AutoSize(ArenaMenuTexturePosition.GetMoneyButtonSmallSize));
        getMoreMoneyButton.SetTexture(UIButtonBase.State.Pressed, arenaMenuMaterial,

ArenaMenuTexturePosition.GetMoneyButtonPressed, AutoRect.AutoSize(ArenaMenuTexturePosition.GetMoneyButtonSmallSize));
        getMoreMoneyButton.Rect = AutoRect.AutoPos(uiPos.GetMoreMoneyButton);


        avatarInfoPanel = new AvatarInfoPanel();
        avatarInfoPanel.SetText("ffff");
       
        
        this.Add(background);
        //this.Add(avatarImage);
        this.Add(buyButton);

        
        avatarScroller = new UIImageScroller( AutoRect.AutoPos(new Rect(450, 0, 500, 640)),  AutoRect.AutoPos(new Rect(442, 216, 500, 400)), 1,  AutoRect.AutoSize(AvatarTexturePosition.AvatarLogoSize), ScrollerDir.Vertical, true);
        avatarScroller.SetImageSpacing(AutoRect.AutoSize(AvatarTexturePosition.AvatarLogoSpacing));
        Material shopMaterial = UIResourceMgr.GetInstance().GetMaterial("ShopUI");
        avatarScroller.AddOverlay(shopMaterial, ShopTexturePosition.SmallBuyLogo);
        
        for (int i = 0; i < BUTTON_NUM; i++)
        {
            UIImage aImage = new UIImage();
            aImage.SetTexture(avatarLogoMaterial, AvatarTexturePosition.AvatarLogo[BUTTON_NUM -1 - i]);
            avatarScroller.Add(aImage);
        }
        avatarScroller.SetMaskImage(avatarLogoMaterial, AvatarTexturePosition.Mask);
        this.Add(returnButton);
        //this.Add(textBackground);
        //this.Add(avatarInfoText);
        

       
        avatarScroller.SetCenterFrameTexture(avatarLogoMaterial, AvatarTexturePosition.Frame);

        avatarScroller.EnableScroll();




        this.Add(avatarScroller);
        this.Add(avatarInfoPanel);
        this.Add(cashPanel);
        UpdateAvatarIcon();

        avatarScroller.Show();
        avatarInfoPanel.Show();
        if (AutoRect.GetPlatform() == Platform.IPad)
        {

            avatarFrame = new Avatar3DFrame(AutoRect.AutoPos(new Rect(0, 10, 400, 600)), new Vector3(-1.589703f * 0.8f, -1.1672753f * 0.9f, 4.420711f), new Vector3(1.5f, 1.5f, 1.5f) * 0.9f);

        }
        else
        {

            avatarFrame = new Avatar3DFrame(AutoRect.AutoPos(new Rect(0, 10, 400, 600)), new Vector3(-1.589703f, -1.1672753f, 4.420711f), new Vector3(1.5f, 1.5f, 1.5f));

        }


        this.Add(avatarFrame);
        this.Add(getMoreMoneyButton);

        UpdateCashPanel();

        iapDialog = new IAPDialog(UIDialog.DialogMode.YES_OR_NO);

        iapDialog.SetDialogEventHandler(this);

        this.Add(iapDialog);

       


        this.SetUIHandler(this);
        Hide();

    }

    public override void UpdateLogic()
    {
        if (avatarFrame != null)
        {
            avatarFrame.UpdateAnimation();
        }
    }



    public void SetBuyButtonText()
    {
        if (GameApp.GetInstance().GetGameState().GetAvatarData((AvatarType)(BUTTON_NUM - 1- currentSelectionIndex)) == AvatarState.Avaliable)
        {
            buyButton.SetText(" SELECT");
        }
        else
        {
            buyButton.SetText(" BUY");
        }
    }

    public override void Hide()
    {
        avatarFrame.Hide();
        iapDialog.Hide();
        cashPanel.Hide();
        base.Hide();
    }

    public override void Show()
    {
        cashPanel.SetCostPanelPosition(new Rect(650, 110, 314, 60));
        avatarFrame.ChangeAvatar(GameApp.GetInstance().GetGameState().Avatar);
        avatarScroller.SetSelection(BUTTON_NUM - 1 - (int)GameApp.GetInstance().GetGameState().Avatar);
        currentSelectionIndex = BUTTON_NUM -1 -(int)GameApp.GetInstance().GetGameState().Avatar;
        buyButton.SetText(" SELECT");
        avatarInfoPanel.SetText(AvatarInfo.AVATAR_INFO[(int)GameApp.GetInstance().GetGameState().Avatar]);
        UpdateCashPanel();
        avatarFrame.Show();
        cashPanel.Show();
        base.Show();
    }

    public void UpdateAvatarIcon()
    {
        for (int i = 0; i < BUTTON_NUM; i++)
        {
            if (GameApp.GetInstance().GetGameState().GetAvatarData((AvatarType)(BUTTON_NUM - 1 - i)) == AvatarState.ToBuy)
            {
                avatarScroller.SetOverlay(i, 0);
            }
            else if (GameApp.GetInstance().GetGameState().GetAvatarData((AvatarType)(BUTTON_NUM - 1 - i)) == AvatarState.Avaliable)
            {
                avatarScroller.SetOverlay(i, -1);
            }
        }
    }


    public void ChangeAvatarModel(int index)
    {
        //avatarInfoText.Set(ConstData.FONT_NAME3, AvatarInfo.AVATAR_INFO[index], ColorName.fontColor_darkorange);

        avatarFrame.ChangeAvatar((AvatarType)index);

    }

    public void UpdateCashPanel()
    {
        if (GameApp.GetInstance().GetGameState().GetAvatarData((AvatarType)(BUTTON_NUM - 1 - currentSelectionIndex)) == AvatarState.Avaliable)
        {
            cashPanel.DisableCost();
        }
        else
        {
            GameConfig gConf = GameApp.GetInstance().GetGameConfig();
            cashPanel.SetCost(gConf.GetAvatarConfig(BUTTON_NUM - 1 - currentSelectionIndex).price);
        }

        cashPanel.SetCash(GameApp.GetInstance().GetGameState().GetCash());
    }

    public void HandleEvent(UIControl control, int command, float wparam, float lparam)
    {
        if (control == returnButton)
        {
            AudioPlayer.PlayAudio(ArenaMenuUI.GetInstance().GetComponent<AudioSource>());
            Hide();
            ArenaMenuUI ui = GameObject.Find("ArenaMenuUI").GetComponent<ArenaMenuUI>();
            ui.GetPanel(MenuName.ARENA).Show();
            GameApp.GetInstance().Save();
        }

        if (control == avatarScroller && command == (int)UIImageScroller.Command.ScrollSelect)
        {
            currentSelectionIndex = (int)wparam;

            SetBuyButtonText();
            ChangeAvatarModel(BUTTON_NUM -1 - currentSelectionIndex);
            avatarInfoPanel.SetText(AvatarInfo.AVATAR_INFO[BUTTON_NUM - 1 - currentSelectionIndex]);
            UpdateCashPanel();
        }

        if (control == buyButton)
        {
            AudioPlayer.PlayAudio(ArenaMenuUI.GetInstance().GetComponent<AudioSource>());
            if (GameApp.GetInstance().GetGameState().GetAvatarData((AvatarType)(BUTTON_NUM - 1 - currentSelectionIndex)) == AvatarState.ToBuy)
            {
                
                GameConfig gConf = GameApp.GetInstance().GetGameConfig();
                if (GameApp.GetInstance().GetGameState().BuyAvatar((AvatarType)(BUTTON_NUM - 1 - currentSelectionIndex), gConf.GetAvatarConfig(BUTTON_NUM - 1 - currentSelectionIndex).price))
                {
                    SetBuyButtonText();
                }
                else
                {
                    TopDialogUI.GetInstance().ShowDialog();

                }
                UpdateAvatarIcon();
                UpdateCashPanel();
            }
            else if (GameApp.GetInstance().GetGameState().GetAvatarData((AvatarType)(BUTTON_NUM - 1 - currentSelectionIndex)) == AvatarState.Avaliable)
            {

                GameApp.GetInstance().GetGameState().Avatar = (AvatarType)(BUTTON_NUM - 1 - currentSelectionIndex);
            }

        }

        else if (control == getMoreMoneyButton)
        {
            AudioPlayer.PlayAudio(ArenaMenuUI.GetInstance().GetComponent<AudioSource>());
            Hide();
            ShopUI shopUI = GameObject.Find("ArenaMenuUI").GetComponent<ArenaMenuUI>().GetPanel(MenuName.SHOP) as ShopUI;
            shopUI.SetFromPanel(this);
            shopUI.Show();
        }
    }

    public void Yes()
    {
        Hide();
        ShopUI shopUI = ArenaMenuUI.GetInstance().GetPanel(MenuName.SHOP) as ShopUI;
        shopUI.SetFromPanel(this);
        shopUI.Show();
    }


    public void No()
    {
        iapDialog.Hide();
    }



}
