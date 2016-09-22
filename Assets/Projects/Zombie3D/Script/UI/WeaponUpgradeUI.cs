using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zombie3D;



public class CashPanel : UIPanel
{
    public UIImage backPanel;
    public UIImage costPanel;
    public UIText cashText;
    public UIText costText;

    public CashPanel()
    {
        Material arenaMaterial = UIResourceMgr.GetInstance().GetMaterial("ArenaMenu");

        WeaponUpgradeUIPosition uiPos = new WeaponUpgradeUIPosition();
        backPanel = new UITextImage();
        backPanel.SetTexture(arenaMaterial, ArenaMenuTexturePosition.CashPanel, AutoRect.AutoSize(ArenaMenuTexturePosition.CashPanel));
        backPanel.Rect = AutoRect.AutoPos(uiPos.CashPanel);

        costPanel = new UITextImage();
        costPanel.SetTexture(arenaMaterial, ArenaMenuTexturePosition.CashPanel, AutoRect.AutoSize(ArenaMenuTexturePosition.CashPanel));
        costPanel.Rect = AutoRect.AutoPos(uiPos.CostPanel);

        cashText = new UIText();
        cashText.Set(ConstData.FONT_NAME2, "", ColorName.fontColor_darkorange);
        cashText.AlignStyle = UIText.enAlignStyle.left;
        cashText.Rect = AutoRect.AutoPos(new Rect(uiPos.CashPanel.x + 40, uiPos.CashPanel.y, uiPos.CashPanel.width * 0.6f, uiPos.CashPanel.height - 10));

        costText = new UIText();
        costText.Set(ConstData.FONT_NAME2, "", ColorName.fontColor_red);
        costText.AlignStyle = UIText.enAlignStyle.center;
        costText.Rect = AutoRect.AutoPos(new Rect(uiPos.CostPanel.x + 40, uiPos.CostPanel.y, uiPos.CostPanel.width * 0.6f, uiPos.CostPanel.height - 10));
        costPanel.Visible = false;
        costPanel.Enable = false;
        this.Add(backPanel);
        this.Add(costPanel);
        this.Add(cashText);
        this.Add(costText);

    }

    public void SetCostPanelPosition(Rect pos)
    {
        WeaponUpgradeUIPosition uiPos = new WeaponUpgradeUIPosition();
        uiPos.CostPanel = pos;
       
        costPanel.Rect = AutoRect.AutoPos(uiPos.CostPanel);
        costText.Rect = AutoRect.AutoPos(new Rect(uiPos.CostPanel.x + 40, uiPos.CostPanel.y, uiPos.CostPanel.width * 0.6f, uiPos.CostPanel.height - 10));
    }

    public void SetCash(int cash)
    {
        cashText.SetText("$" + cash.ToString("N0"));

    }

    public void SetCost(int cost)
    {
        costPanel.Visible = true;
        costText.Visible = true;
        costText.SetText("-$" + cost.ToString("N0"));
    }

    public void DisableCost()
    {
        costPanel.Visible = false;
        costText.Visible = false;
    }


}

public class UpgradePanel : UIPanel
{
    protected UISelectButton selectPanelButton;
    protected UIText buttonText;
    protected UIText currentValueText;
    protected UIText nextValueText;
    protected UIImage[] starsBackground = new UIImage[10];
    protected UIImage[] stars = new UIImage[10];
    protected UIImage arrowImage;
    protected Material arenaMaterial;
    protected UIImage bulletLogo;

    public UpgradePanel(Rect rect, int index)
    {
        arenaMaterial = UIResourceMgr.GetInstance().GetMaterial("ArenaMenu");


        selectPanelButton = new UISelectButton();
        selectPanelButton.SetTexture(UIButtonBase.State.Normal, arenaMaterial,

ArenaMenuTexturePosition.UpgradeButtonNormal, AutoRect.AutoSize(ArenaMenuTexturePosition.UpgradeButtonNormal));
        selectPanelButton.SetTexture(UIButtonBase.State.Pressed, arenaMaterial,

ArenaMenuTexturePosition.UpgradeButtonPressed, AutoRect.AutoSize(ArenaMenuTexturePosition.UpgradeButtonPressed));

        selectPanelButton.SetTexture(UIButtonBase.State.Disabled, arenaMaterial,

ArenaMenuTexturePosition.UpgradeButtonNormal, AutoRect.AutoSize(ArenaMenuTexturePosition.UpgradeButtonNormal));


        selectPanelButton.Rect = AutoRect.AutoPos(rect);

        arrowImage = new UIImage();
        arrowImage.SetTexture(arenaMaterial, ArenaMenuTexturePosition.Arrow, AutoRect.AutoSize(ArenaMenuTexturePosition.Arrow));
        arrowImage.Rect = AutoRect.AutoPos(new Rect(rect.x + 276, rect.y + 62, 36, 26));


        Material buttonsMaterial = UIResourceMgr.GetInstance().GetMaterial("Buttons");

        bulletLogo = new UIImage();
        Rect brect = ButtonsTexturePosition.GetBulletsLogoRect(1);
        bulletLogo.SetTexture(buttonsMaterial, brect, AutoRect.AutoSize(brect));
        bulletLogo.Rect = AutoRect.AutoPos(new Rect(rect.x + 156, rect.y + 46, 44, 52));
        bulletLogo.Visible = false;
        bulletLogo.Enable = false;



        buttonText = new UIText();
        buttonText.Set(ConstData.FONT_NAME2, "", ColorName.fontColor_orange);
        buttonText.Rect = AutoRect.AutoPos(new Rect(rect.x + 56, rect.y + 25, 175, 68));


        currentValueText = new UIText();
        currentValueText.Set(ConstData.FONT_NAME2, "", ColorName.fontColor_orange);
        currentValueText.Rect = AutoRect.AutoPos(new Rect(688, 526 - 100 * index, 92, 32));
        currentValueText.AlignStyle = UIText.enAlignStyle.right;

        nextValueText = new UIText();
        nextValueText.Set(ConstData.FONT_NAME2, "", ColorName.fontColor_orange);
        nextValueText.Rect = AutoRect.AutoPos(new Rect(810, 526 - 100 * index, 92, 32));
        nextValueText.AlignStyle = UIText.enAlignStyle.left;


        for (int i = 0; i < 10; i++)
        {
            starsBackground[i] = new UIImage();


            starsBackground[i].SetTexture(arenaMaterial, ArenaMenuTexturePosition.StarEmpty, AutoRect.AutoSize(ArenaMenuTexturePosition.StarEmpty));

            stars[i] = new UIImage();
            stars[i].SetTexture(arenaMaterial, ArenaMenuTexturePosition.StarFull, AutoRect.AutoSize(ArenaMenuTexturePosition.StarFull));

            int x = 572 + i * 24;
            int y = 490 - index * 100;

            starsBackground[i].Rect = AutoRect.AutoPos(new Rect(x, y, 24, 22));
            starsBackground[i].Enable = false;

            stars[i].Rect = AutoRect.AutoPos(new Rect(x, y, 24, 22));
            stars[i].Enable = false;

        }


        Add(selectPanelButton);
        Add(arrowImage);
        Add(bulletLogo);
        Add(buttonText);
        Add(currentValueText);
        Add(nextValueText);

        for (int i = 0; i < 10; i++)
        {
            Add(starsBackground[i]);
        }

        for (int i = 0; i < 10; i++)
        {
            Add(stars[i]);
        }
    }

    public void SetButtonText(string text)
    {
        buttonText.SetText(text);
    }

    public void SetCurrentValue(float number)
    {


    }

    public void SetNextValue(float number)
    {


    }

    public void VisibleAll()
    {
        selectPanelButton.Visible = true;
        buttonText.Visible = true;
        currentValueText.Visible = true;
        nextValueText.Visible = true;
        arrowImage.Visible = true;
    }

    public void HideArrow()
    {
        arrowImage.Visible = false;
    }

    public void EnableAll()
    {
        selectPanelButton.Enable = true;
    }

    public void HideAll()
    {
        selectPanelButton.Visible = false;
        buttonText.Visible = false;
        currentValueText.Visible = false;
        nextValueText.Visible = false;
        arrowImage.Visible = false;
        bulletLogo.Visible = false;
        UpdateStar(0);
        UpdateStarBackground(0);
    }

    public void DisableUpgrade()
    {
        selectPanelButton.Enable = false;
        nextValueText.Visible = false;
        arrowImage.Visible = false;
        UpdateStar(0);
        UpdateStarBackground(0);
    }

    public void DisableNumbers()
    {
        currentValueText.Visible = false;
        nextValueText.Visible = false;

    }

    public void Select(bool bSelect)
    {

        selectPanelButton.Set(bSelect);
        if (bSelect)
        {
            buttonText.SetColor(ColorName.fontColor_yellow);
            currentValueText.SetColor(ColorName.fontColor_yellow);
            nextValueText.SetColor(ColorName.fontColor_yellow);
            arrowImage.SetTexture(arenaMaterial, ArenaMenuTexturePosition.ArrowSelected, AutoRect.AutoSize(ArenaMenuTexturePosition.ArrowSelected));
            for (int i = 0; i < 10; i++)
            {
                starsBackground[i].SetTexture(arenaMaterial, ArenaMenuTexturePosition.StarEmptySelected, AutoRect.AutoSize(ArenaMenuTexturePosition.StarEmptySelected));
                stars[i].SetTexture(arenaMaterial, ArenaMenuTexturePosition.StarFullSelected, AutoRect.AutoSize(ArenaMenuTexturePosition.StarFullSelected));
            }
        }
        else
        {
            buttonText.SetColor(ColorName.fontColor_orange);
            currentValueText.SetColor(ColorName.fontColor_orange);
            nextValueText.SetColor(ColorName.fontColor_orange);
            arrowImage.SetTexture(arenaMaterial, ArenaMenuTexturePosition.Arrow, AutoRect.AutoSize(ArenaMenuTexturePosition.Arrow));
            for (int i = 0; i < 10; i++)
            {
                starsBackground[i].SetTexture(arenaMaterial, ArenaMenuTexturePosition.StarEmpty, AutoRect.AutoSize(ArenaMenuTexturePosition.StarEmpty));
                stars[i].SetTexture(arenaMaterial, ArenaMenuTexturePosition.StarFull, AutoRect.AutoSize(ArenaMenuTexturePosition.StarFull));
            }
        }


    }

    protected void UpdateStar(int level)
    {

        for (int i = 0; i < 10; i++)
        {
            if (i < level)
            {
                stars[i].Visible = true;
            }
            else
            {
                stars[i].Visible = false;
            }
        }
    }



    protected void UpdateStarBackground(int maxlevel)
    {

        for (int i = 0; i < 10; i++)
        {
            if (i < maxlevel)
            {
                starsBackground[i].Visible = true;
            }
            else
            {
                starsBackground[i].Visible = false;
            }
        }
    }


    //ammo
    public void UpdateInfo(string cStr, string nStr, int weaponTypeIndex)
    {
        Material buttonsMaterial = UIResourceMgr.GetInstance().GetMaterial("Buttons");
        Rect brect = ButtonsTexturePosition.GetBulletsLogoRect(weaponTypeIndex);
        bulletLogo.SetTexture(buttonsMaterial, brect, AutoRect.AutoSize(brect));
        arrowImage.Visible = false;
        bulletLogo.Visible = true;
        currentValueText.SetText(cStr);
        nextValueText.SetText(nStr);
        UpdateStar(0);
        UpdateStarBackground(0);

    }

    //damage,.....
    public void UpdateInfo(float cValue, float nValue, int maxLevel, int level, int uType)
    {

        currentValueText.SetText(Math.SignificantFigures(cValue, 4).ToString());
       

        if (level == maxLevel)
        {
            nextValueText.Visible = false;
            arrowImage.Visible = false;
        }
        else
        {
            arrowImage.Visible = true;
            nextValueText.Visible = true;
            nextValueText.SetText(Math.SignificantFigures(nValue, 4).ToString());
        }

        if (uType == 1)
        {
            currentValueText.SetText(currentValueText.GetText()+"s");
            nextValueText.SetText(nextValueText.GetText()+"s");
        }
        else if (uType == 2)
        {
            currentValueText.SetText(currentValueText.GetText() + "%");
            nextValueText.SetText(nextValueText.GetText() + "%");
        }

        UpdateStarBackground(maxLevel);
        UpdateStar(level);


    }

    //armor
    public void UpdateInfo(string cStr, string nStr, int maxLevel, int level)
    {

        currentValueText.SetText(cStr);

        nextValueText.SetText(nStr);

        UpdateStarBackground(maxLevel);
        UpdateStar(level);


    }


}



class WeaponUpgradeUIPosition
{

    public Rect Background = new Rect(0, 0, 960, 640);
    public Rect TitleImage = new Rect(10, 560, 391, 117);


    public Rect UpgradePowerButton = new Rect(500, 435, 424, 108);
    public Rect UpgradeFrequencyButton = new Rect(500, 435 - 100, 424, 108);
    public Rect UpgradeAccuracyButton = new Rect(500, 435 - 200, 424, 108);
    public Rect BuyAmmoButton = new Rect(500, 435 - 300, 424, 108);


    public Rect UpgradePowerButtonText = new Rect(556, 460, 175, 68);
    public Rect UpgradeFrequencyButtonText = new Rect(556, 460 - 100, 175, 68);

    public Rect UpgradeAccuracyButtonText = new Rect(556, 460 - 200, 175, 68);

    public Rect BuyAmmoText = new Rect(556, 460 - 300, 175, 68);

    public Rect ReturnButton = new Rect(24, 6, 130, 70);

    public Rect CashPanel = new Rect(650, 560 + 10, 314, 60);

    public Rect CostPanel = new Rect(688, 46 + 10, 314, 60);

    public Rect UpgradeButton = new Rect(598, 0, 356, 116);

    public Rect GetMoreMoneyButton = new Rect(42, 558, 392, 82);
}



public class IAPDialog : GameDialog
{
    public const string NOT_ENOUGH_CASH = "\n\n\n  SHORT ON MONEY!";
    public const string NOT_AVAILABLE = "\n\n\nUNAVAILABLE WEAPON!";

    public IAPDialog(DialogMode mode)
        : base(mode)
    {
        SetTextAreaOffset(AutoRect.AutoValuePos(new Rect(80, 80, -20, -106)));
        SetText(ConstData.FONT_NAME1, "", ColorName.fontColor_darkorange);
        Material material = UIResourceMgr.GetInstance().GetMaterial("ArenaMenu");
        this.SetText(NOT_ENOUGH_CASH);
        SetButtonTexture(material, ArenaMenuTexturePosition.GetMoneyButtonNormal, ArenaMenuTexturePosition.GetMoneyButtonPressed, AutoRect.AutoSize(ArenaMenuTexturePosition.GetMoneyButtonPressed));

        SetYesButtonOffset(AutoRect.AutoValuePos(new Vector2(26, -42)), AutoRect.AutoSize(ArenaMenuTexturePosition.GetMoneyButtonPressed));
        this.DisableNoButton();
        this.SetYesButtonText("");
        this.SetNoButtonText("");

        this.SetCloseButtonTexture(material, ArenaMenuTexturePosition.CloseButtonNormal, ArenaMenuTexturePosition.CloseButtonPressed);
        this.SetCloseButtonOffset(AutoRect.AutoValuePos(new Vector2(-16, 224)), AutoRect.AutoSize(ArenaMenuTexturePosition.CloseButtonNormal));
    }
}


public class WeaponUpgradeUI : UIPanel, UIHandler, UIDialogEventHandler
{
    protected Rect[] buttonRect;
    public AudioSource upgradeSucceed;
    protected Material arenaMaterial;

    protected UIImage background;
    protected CashPanel cashPanel;

    protected UIClickButton returnButton;
    protected UIClickButton getMoreMoneyButton;
    protected UITextButton upgradeButton;

    protected UpgradePanel[] upgradePanels = new UpgradePanel[4];

    private WeaponUpgradeUIPosition uiPos;


    protected float screenRatioX;
    protected float screenRatioY;

    protected bool uiInited = false;


    protected GameState gameState;
    protected Weapon selectedWeapon;

    protected List<Weapon> weaponList;
    protected List<GameObject> weaponModles;
    protected int currentWeaponIndex;
    protected int upgradeSelection = 1;

    protected UIImageScroller weaponScroller;
    protected IAPDialog iapDialog;
    Material weaponUI;
    Material weaponUI2;
    // Use this for initialization
    public WeaponUpgradeUI()
    {

        uiPos = new WeaponUpgradeUIPosition();

        GameApp.GetInstance().Init();
        gameState = GameApp.GetInstance().GetGameState();
        currentWeaponIndex =-1;
        selectedWeapon = null;


        weaponList = GameApp.GetInstance().GetGameState().GetWeapons();


        arenaMaterial = UIResourceMgr.GetInstance().GetMaterial("ArenaMenu");



        background = new UIImage();
        background.SetTexture(arenaMaterial, ArenaMenuTexturePosition.Background, AutoRect.AutoSize(ArenaMenuTexturePosition.Background));
        background.Rect = AutoRect.AutoPos(uiPos.Background);





        for (int i = 0; i < 4; i++)
        {
            upgradePanels[i] = new UpgradePanel(new Rect(500, 465 - i * 100, 424, 108), i);
            upgradePanels[i].Show();

        }
        upgradePanels[0].SetButtonText("DAMAGE");
        upgradePanels[1].SetButtonText("FIRE RATE");
        upgradePanels[2].SetButtonText("ACCURACY");
        upgradePanels[3].SetButtonText("AMMO");



        returnButton = new UIClickButton();
        returnButton.SetTexture(UIButtonBase.State.Normal, arenaMaterial,
ArenaMenuTexturePosition.ReturnButtonNormal, AutoRect.AutoSize(ArenaMenuTexturePosition.ReturnButtonNormal));
        returnButton.SetTexture(UIButtonBase.State.Pressed, arenaMaterial,

ArenaMenuTexturePosition.ReturnButtonPressed, AutoRect.AutoSize(ArenaMenuTexturePosition.ReturnButtonPressed));
        returnButton.Rect = AutoRect.AutoPos(uiPos.ReturnButton);



        Material buttonsMaterial = UIResourceMgr.GetInstance().GetMaterial("Buttons");

        upgradeButton = new UITextButton();
        upgradeButton.SetTexture(UIButtonBase.State.Normal, buttonsMaterial,

ButtonsTexturePosition.ButtonNormal, AutoRect.AutoSize(ButtonsTexturePosition.ButtonNormal));
        upgradeButton.SetTexture(UIButtonBase.State.Pressed, buttonsMaterial,

ButtonsTexturePosition.ButtonPressed, AutoRect.AutoSize(ButtonsTexturePosition.ButtonPressed));
        upgradeButton.Rect = AutoRect.AutoPos(uiPos.UpgradeButton);


        getMoreMoneyButton = new UITextButton();
        getMoreMoneyButton.SetTexture(UIButtonBase.State.Normal, arenaMaterial,

ArenaMenuTexturePosition.GetMoneyButtonNormal, AutoRect.AutoSize(ArenaMenuTexturePosition.GetMoneyButtonSmallSize));
        getMoreMoneyButton.SetTexture(UIButtonBase.State.Pressed, arenaMaterial,

ArenaMenuTexturePosition.GetMoneyButtonPressed, AutoRect.AutoSize(ArenaMenuTexturePosition.GetMoneyButtonSmallSize));
        getMoreMoneyButton.Rect = AutoRect.AutoPos(uiPos.GetMoreMoneyButton);


        cashPanel = new CashPanel();





        upgradeButton.SetText(ConstData.FONT_NAME0, " UPGRADE", ColorName.fontColor_orange);

        UpdateWeaponInfo();
        InitSelection();



        this.Add(background);

       




        this.Add(returnButton);
        this.Add(getMoreMoneyButton);
        this.Add(upgradeButton);

        for (int i = 0; i < 4; i++)
        {
            this.Add(upgradePanels[i]);

        }

        weaponUI = UIResourceMgr.GetInstance().GetMaterial("Weapons");
        weaponUI2 = UIResourceMgr.GetInstance().GetMaterial("Weapons2");

        weaponScroller = new UIImageScroller(AutoRect.AutoPos(new Rect(0, 0, 500, 640)), AutoRect.AutoPos(new Rect(10, 120, 500, 440)), 1, AutoRect.AutoSize(WeaponsLogoTexturePosition.WeaponLogoSize), ScrollerDir.Vertical, true);
        weaponScroller.SetImageSpacing(AutoRect.AutoSize(WeaponsLogoTexturePosition.WeaponLogoSpacing));
        Material shopMaterial = UIResourceMgr.GetInstance().GetMaterial("ShopUI");

        Material avatarMaterial = UIResourceMgr.GetInstance().GetMaterial("Avatar");
        weaponScroller.SetCenterFrameTexture(avatarMaterial, AvatarTexturePosition.Frame);


        weaponScroller.AddOverlay(shopMaterial, ShopTexturePosition.LockedLogo);
        weaponScroller.AddOverlay(shopMaterial, ShopTexturePosition.BuyLogo);


        UIImage uiImage = new UIImage();
        TexturePosInfo info = WeaponsLogoTexturePosition.GetWeaponTextureRect(weaponList.Count);
        uiImage.SetTexture(info.m_Material, info.m_TexRect, AutoRect.AutoSize(info.m_TexRect));
        uiImage.Rect = info.m_TexRect;
        weaponScroller.Add(uiImage);


        for (int i = 0; i < weaponList.Count; i++)
        {

            uiImage = new UIImage();
            info = WeaponsLogoTexturePosition.GetWeaponTextureRect(i);
            uiImage.SetTexture(info.m_Material, info.m_TexRect, AutoRect.AutoSize(info.m_TexRect));
            uiImage.Rect = info.m_TexRect;
            weaponScroller.Add(uiImage);

        }


        this.Add(weaponScroller);
        weaponScroller.EnableScroll();
        this.Add(cashPanel);
        for (int i = 0; i < weaponList.Count; i++)
        {
            if (weaponList[i].Exist == WeaponExistState.Locked)
            {
                weaponScroller.SetOverlay(i+1, 0);
            }
            else if (weaponList[i].Exist == WeaponExistState.Unlocked)
            {
                weaponScroller.SetOverlay(i + 1, 1);
            }
        }

        Material avatarLogoMaterial = UIResourceMgr.GetInstance().GetMaterial("Avatar");
        weaponScroller.SetMaskImage(avatarLogoMaterial, AvatarTexturePosition.Mask);

        weaponScroller.Show();


        iapDialog = new IAPDialog(UIDialog.DialogMode.YES_OR_NO);

        iapDialog.SetDialogEventHandler(this);

        this.Add(iapDialog);


        cashPanel.Show();

        SetUIHandler(this);

        uiInited = true;
        Hide();
    }



    public override void Hide()
    {
        base.Hide();
        iapDialog.Hide();
    }

    public override void Show()
    {
        cashPanel.SetCostPanelPosition(new Rect(650, 110, 314, 60));
        base.Show();
        UpdateWeaponInfo();
    }




    public void SelectPanel(int index)
    {
        for (int i = 0; i < 4; i++)
        {
            upgradePanels[i].Select(false);

        }
        upgradePanels[index].Select(true);

    }

    public void UpdateSelectionButtonsState()
    {
        SelectPanel(upgradeSelection - 1);

    }

    public void InitSelection()
    {
        if (selectedWeapon != null)
        {
            if (selectedWeapon.Exist == WeaponExistState.Owned)
            {
                upgradeSelection = 1;
                UpdateSelectionButtonsState();
            }
        }
    }




    public void UpdateWeaponInfo()
    {

        if (selectedWeapon != null)
        {

            for (int i = 0; i < 4; i++)
            {
                upgradePanels[i].VisibleAll();
            }
            upgradePanels[0].SetButtonText("DAMAGE");

            int maxLevel = (int)selectedWeapon.WConf.damageConf.maxLevel;
            int level = selectedWeapon.DamageLevel;
            upgradePanels[0].UpdateInfo(selectedWeapon.Damage, selectedWeapon.GetNextLevelDamage(), maxLevel, level, 0);

            maxLevel = (int)selectedWeapon.WConf.attackRateConf.maxLevel;
            level = selectedWeapon.FrequencyLevel;
            upgradePanels[1].UpdateInfo(selectedWeapon.AttackFrequency, selectedWeapon.GetNextLevelFrequency(), maxLevel, level, 1);


            maxLevel = (int)selectedWeapon.WConf.accuracyConf.maxLevel;
            level = selectedWeapon.AccuracyLevel;
            upgradePanels[2].UpdateInfo(selectedWeapon.Accuracy, selectedWeapon.GetNextLevelAccuracy(), maxLevel, level, 2);

            upgradePanels[3].UpdateInfo("x"+selectedWeapon.BulletCount.ToString(), "+" + selectedWeapon.WConf.bullet, (int)selectedWeapon.GetWeaponType());




            if (selectedWeapon.Exist == WeaponExistState.Owned)
            {
                for (int i = 0; i < 4; i++)
                {
                    upgradePanels[i].EnableAll();
                }
                upgradeButton.SetText(" UPGRADE");


                switch (upgradeSelection)
                {
                    case 1:
                        maxLevel = (int)selectedWeapon.WConf.damageConf.maxLevel;
                        level = selectedWeapon.DamageLevel;
                        cashPanel.SetCost(selectedWeapon.GetDamageUpgradePrice());

                        break;
                    case 2:
                        maxLevel = (int)selectedWeapon.WConf.attackRateConf.maxLevel;
                        level = selectedWeapon.FrequencyLevel;
                        cashPanel.SetCost(selectedWeapon.GetFrequencyUpgradePrice());

                        break;
                    case 3:
                        maxLevel = (int)selectedWeapon.WConf.accuracyConf.maxLevel;
                        level = selectedWeapon.AccuracyLevel;
                        cashPanel.SetCost(selectedWeapon.GetAccuracyUpgradePrice());
                        break;
                    case 4:
                        cashPanel.SetCost(selectedWeapon.WConf.bulletPrice);
                        upgradeButton.SetText(" BUY");
                        break;
                }

                if (maxLevel == level && upgradeSelection != 4)
                {
                    cashPanel.DisableCost();
                }


            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    upgradePanels[i].DisableUpgrade();
                    upgradePanels[i].Select(false);
                }
                upgradeButton.SetText(" BUY");

                cashPanel.SetCost(selectedWeapon.WConf.price);
            }

            if (selectedWeapon.GetWeaponType() == WeaponType.Saw)
            {
                upgradePanels[3].HideAll();
                upgradePanels[3].DisableUpgrade();
            }

        }
        else
        {
            for (int i = 1; i < 4; i++)
            {
                upgradePanels[i].HideAll();
            }
            upgradePanels[0].VisibleAll();
            upgradePanels[0].EnableAll();
            upgradePanels[0].SetButtonText("ARMOR");
            upgradePanels[0].HideArrow();
            upgradePanels[0].Select(true);
            GameConfig gConf = GameApp.GetInstance().GetGameConfig();
            upgradePanels[0].UpdateInfo("", "", gConf.playerConf.maxArmorLevel, gameState.ArmorLevel);

            cashPanel.SetCost(gameState.GetArmorPrice());

            if (gConf.playerConf.maxArmorLevel == gameState.ArmorLevel)
            {
                cashPanel.DisableCost();
            }


        }

        cashPanel.SetCash(gameState.GetCash());

    }

    public void HandleEvent(UIControl control, int command, float wparam, float lparam)
    {
        if (control == weaponScroller && command == (int)UIImageScroller.Command.ScrollSelect)
        {

            

            currentWeaponIndex = (int)wparam -1;
            upgradeSelection = 1;
            if (currentWeaponIndex >=0)
            {
                //Debug.Log("selection weapon:" + weaponList[currentWeaponIndex].Name);
                selectedWeapon = weaponList[currentWeaponIndex];

                UpdateWeaponInfo();
                InitSelection();
            }
            else
            {
                selectedWeapon = null;
                UpdateWeaponInfo();
                InitSelection();
            }


            //weaponList[weaponID].IsSelectedForBattle = true;

        }
        else if (control == upgradePanels[0])
        {
            AudioPlayer.PlayAudio(ArenaMenuUI.GetInstance().GetComponent<AudioSource>());
            upgradeSelection = 1;
            UpdateSelectionButtonsState();
            UpdateWeaponInfo();
            /*
            if (selectedWeapon != null)
            {
                cashPanel.SetCost(selectedWeapon.GetDamageUpgradePrice());
            }
            else
            {
                GameConfig gConf = GameApp.GetInstance().GetGameConfig();
                cashPanel.SetCost(gConf.playerConf.upgradeArmorPrice);
            }
            */


        }
        else if (control == upgradePanels[1])
        {
            AudioPlayer.PlayAudio(ArenaMenuUI.GetInstance().GetComponent<AudioSource>());
            upgradeSelection = 2;
            UpdateSelectionButtonsState();
            UpdateWeaponInfo();
            //cashPanel.SetCost(selectedWeapon.GetFrequencyUpgradePrice());
        }
        else if (control == upgradePanels[2])
        {
            AudioPlayer.PlayAudio(ArenaMenuUI.GetInstance().GetComponent<AudioSource>());
            upgradeSelection = 3;
            UpdateSelectionButtonsState();
            UpdateWeaponInfo();
            //cashPanel.SetCost(selectedWeapon.GetAccuracyUpgradePrice());
        }
        else if (control == upgradePanels[3])
        {
            AudioPlayer.PlayAudio(ArenaMenuUI.GetInstance().GetComponent<AudioSource>());
            upgradeSelection = 4;
            UpdateSelectionButtonsState();
            UpdateWeaponInfo();
            //cashPanel.SetCost(selectedWeapon.WConf.bulletPrice);
        }
        else if (control == upgradeButton)
        {


            if (selectedWeapon != null)
            {
                if (selectedWeapon.Exist == WeaponExistState.Owned)
                {
                    switch (upgradeSelection)
                    {
                        case 1:

                            if (!selectedWeapon.IsMaxLevelDamage())
                            {
                                if (gameState.UpgradeWeapon(selectedWeapon, selectedWeapon.Damage * selectedWeapon.WConf.damageConf.upFactor, 0f, 0f, selectedWeapon.GetDamageUpgradePrice()))
                                {
                                    Debug.Log("upgrade");
                                    ArenaMenuUI.GetInstance().GetAudioPlayer().PlayAudio("Upgrade");
                                }
                                else
                                {
                                    iapDialog.SetText(IAPDialog.NOT_ENOUGH_CASH);
                                    iapDialog.Show();
                                }
                            }


                            break;
                        case 2:
                            if (!selectedWeapon.IsMaxLevelCD())
                            {
                                if (gameState.UpgradeWeapon(selectedWeapon, 0f, selectedWeapon.AttackFrequency * selectedWeapon.WConf.attackRateConf.upFactor, 0f, selectedWeapon.GetFrequencyUpgradePrice()))
                                {
                                    ArenaMenuUI.GetInstance().GetAudioPlayer().PlayAudio("Upgrade");
                                }
                                else
                                {
                                    iapDialog.SetText(IAPDialog.NOT_ENOUGH_CASH);
                                    iapDialog.Show();
                                }
                            }
                            break;
                        case 3:
                            if (!selectedWeapon.IsMaxLevelAccuracy())
                            {
                                if (gameState.UpgradeWeapon(selectedWeapon, 0f, 0f, selectedWeapon.Accuracy * selectedWeapon.WConf.accuracyConf.upFactor, selectedWeapon.GetAccuracyUpgradePrice()))
                                {
                                    ArenaMenuUI.GetInstance().GetAudioPlayer().PlayAudio("Upgrade");
                                }
                                else
                                {
                                    iapDialog.SetText(IAPDialog.NOT_ENOUGH_CASH);
                                    iapDialog.Show();
                                }
                            }
                            break;
                        case 4:

                            if (selectedWeapon.BulletCount < 9999)
                            {
                                if (gameState.BuyBullets(selectedWeapon, selectedWeapon.WConf.bullet, selectedWeapon.WConf.bulletPrice))
                                {
                                    ArenaMenuUI.GetInstance().GetAudioPlayer().PlayAudio("Upgrade");
                                }
                                else
                                {
                                    iapDialog.SetText(IAPDialog.NOT_ENOUGH_CASH);
                                    iapDialog.Show();
                                }
                            }
                            break;
                    }
                }
                else
                {

                    WeaponBuyStatus wBuyStatus = gameState.BuyWeapon(selectedWeapon, selectedWeapon.WConf.price);
                    if (wBuyStatus == WeaponBuyStatus.Succeed)
                    {
                        ArenaMenuUI.GetInstance().GetAudioPlayer().PlayAudio("Upgrade");
                    }
                    else if (wBuyStatus == WeaponBuyStatus.NotEnoughCash)
                    {
                        iapDialog.SetText(IAPDialog.NOT_ENOUGH_CASH);
                        iapDialog.Show();
                    }
                    else if (wBuyStatus == WeaponBuyStatus.Locked)
                    {
                        iapDialog.SetText(IAPDialog.NOT_AVAILABLE);
                        iapDialog.Show();
                    }

                    if (selectedWeapon.Exist == WeaponExistState.Owned)
                    {
                        upgradePanels[0].Select(true);
                        int index = gameState.GetWeaponIndex(selectedWeapon);
                        if (weaponScroller != null)
                        {
                            weaponScroller.SetOverlay(index+1, -1);
                        }
                    }

                }

            }
            else
            {
                GameConfig gConf = GameApp.GetInstance().GetGameConfig();
                if (gameState.ArmorLevel != gConf.playerConf.maxArmorLevel)
                {

                    if (gameState.UpgradeArmor(gameState.GetArmorPrice()))
                    {
                        ArenaMenuUI.GetInstance().GetAudioPlayer().PlayAudio("Upgrade");
                    }
                    else
                    {
                        iapDialog.SetText(IAPDialog.NOT_ENOUGH_CASH);
                        iapDialog.Show();
                    }
                }
            }
            UpdateWeaponInfo();
        }
        else if (control == returnButton)
        {
            AudioPlayer.PlayAudio(ArenaMenuUI.GetInstance().GetComponent<AudioSource>());
            GameApp.GetInstance().Save();
            Hide();
            GameObject.Find("ArenaMenuUI").GetComponent<ArenaMenuUI>().GetPanel(MenuName.ARENA).Show();
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
