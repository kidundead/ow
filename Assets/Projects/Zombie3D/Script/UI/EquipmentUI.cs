using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zombie3D;




class EquipmentUIPosition
{

    public Rect Background = new Rect(0, 0, 960, 640);
    public Rect TitleImage = new Rect(90, 560, 273, 90);
    public Rect UpButton = new Rect(510, 550, 201, 94);
    public Rect DownButton = new Rect(710, 550, 201, 94);

    public Rect ScrollArea = new Rect(421, 450, 500, 180);

    public Rect ShopButton = new Rect(700, 10, 267, 61);


    public Rect ShopButtonText = new Rect(700 + 80, 10 - 10, 267, 61);


    public Rect ReturnButton = new Rect(24, 6, 130, 70);


    public Rect WeaponInfoText = new Rect(500, 90, 380, 60);

    public Rect GetMoreMoneyButton = new Rect(42, 558, 392, 82);

    public Rect WeaponInfoPanel = new Rect(450, 56, 490, 176);
}


public class WeaponInfoPanel : UIPanel
{
    protected UIImage background;
    
    protected UIText infoText;
    protected UIText bulletText;
    protected UIImage bulletLogo;
    public WeaponInfoPanel()
    {
        EquipmentUIPosition uiPos = new EquipmentUIPosition();
        Material dialogMaterial = UIResourceMgr.GetInstance().GetMaterial("Dialog");
        background = new UIImage();
        background.SetTexture(dialogMaterial, DialogTexturePosition.TextBox, AutoRect.AutoSize(DialogTexturePosition.TextBox));
        background.Rect =AutoRect.AutoPos( uiPos.WeaponInfoPanel);
        background.Enable = false;
        Add(background);


        Material buttonsMaterial = UIResourceMgr.GetInstance().GetMaterial("Buttons");

        bulletLogo = new UIImage();
        Rect brect = ButtonsTexturePosition.GetBulletsLogoRect(1);
        bulletLogo.SetTexture(buttonsMaterial, brect, AutoRect.AutoSize(brect));
        bulletLogo.Rect = AutoRect.AutoPos(new Rect(uiPos.WeaponInfoPanel.x + 250, uiPos.WeaponInfoPanel.y+48,44, 52));
        bulletLogo.Visible = false;
        bulletLogo.Enable = false;
        Add(bulletLogo);


        infoText = new UIText();
        infoText.Set(ConstData.FONT_NAME3, "", ColorName.fontColor_darkorange);
        
        infoText.Rect = AutoRect.AutoPos(new Rect(uiPos.WeaponInfoPanel.x + 50, uiPos.WeaponInfoPanel.y, uiPos.WeaponInfoPanel.width, uiPos.WeaponInfoPanel.height-40));
        
        
       
        Add(infoText);

        bulletText = new UIText();
        bulletText.Set(ConstData.FONT_NAME3, "", ColorName.fontColor_darkorange);
        bulletText.AlignStyle = UIText.enAlignStyle.left;
        bulletText.Rect = AutoRect.AutoPos(new Rect(uiPos.WeaponInfoPanel.x + 300, uiPos.WeaponInfoPanel.y - 62, 144, 152));

        Add(bulletText);
    }

    public void SetText(string text)
    {

        infoText.SetText(text);
    }

    public void SetBulletText(string text)
    {
        bulletText.SetText(text);
    }

    public void UpdateBulletLogo(int wTypeIndex)
    {
        if ((WeaponType)wTypeIndex == WeaponType.Saw)
        {
            bulletLogo.Visible = false;
        }
        else
        {
            bulletLogo.Visible = true;
            Material buttonsMaterial = UIResourceMgr.GetInstance().GetMaterial("Buttons");

            Rect brect = ButtonsTexturePosition.GetBulletsLogoRect(wTypeIndex);
            bulletLogo.SetTexture(buttonsMaterial, brect, AutoRect.AutoSize(brect));
        }

    }

}


public class EquipmentUI : UIPanel, UIHandler
{
    protected Rect[] buttonRect;

    public UIManager m_UIManager = null;
    public string m_ui_material_path;
    protected Material arenaMenuMaterial;
    protected UIImage background;
    protected UIImage titleImage;

    protected UIImage selectionImage;

    protected UIClickButton returnButton;
    protected UIClickButton getMoreMoneyButton;
    protected UIImage[] panels = new UIImage[6];
    protected UIText[] numbers = new UIText[6];
    protected CashPanel cashPanel;
    protected UIText weaponInfoText;
    protected WeaponInfoPanel weaponInfoPanel;

    private EquipmentUIPosition uiPos;

    protected bool uiInited = false;


    protected GameState gameState;
    protected Weapon selectedWeapon;


    protected int currentSelectionWeaponIndex = 0;


    protected int SELECTION_NUM = Constant.SELECTION_NUM;

    protected Rect[] selectionRect = new Rect[Constant.SELECTION_NUM];

    protected int[] rectToWeaponMap;

    protected UIDragGrid battleWeaponGrid;

    protected int currentSelect = -1;

    protected List<Weapon> weaponList;
    protected Touch lastTouch;

    protected UIImageScroller weaponScroller;
    protected Avatar3DFrame avatarFrame;

    Material weaponUI;

    void Init()
    {

        Cursor.visible = true;

        selectionRect[0] = AutoRect.AutoPos(new Rect(28, 88, 112, 112));

        selectionRect[1] = AutoRect.AutoPos(new Rect(28 + 150, 88, 112, 112));

        selectionRect[2] = AutoRect.AutoPos(new Rect(28 + 150 * 2, 88, 112, 112));


        gameState = GameApp.GetInstance().GetGameState();

        weaponUI = UIResourceMgr.GetInstance().GetMaterial("Weapons");
        Material weapon3Material = UIResourceMgr.GetInstance().GetMaterial("Weapons3");
        Rect gridbackRect = WeaponsLogoTexturePosition.GetWeaponIconTextureRect(13);
        battleWeaponGrid = new UIDragGrid(0);

        rectToWeaponMap = GameApp.GetInstance().GetGameState().GetRectToWeaponMap();

        for (int i = 0; i < SELECTION_NUM; i++)
        {
            battleWeaponGrid.AddGrid(selectionRect[i], weapon3Material, gridbackRect);
        }


        GameApp.GetInstance().Init();
        weaponList = GameApp.GetInstance().GetGameState().GetWeapons();


        PutBattleWeapons();


        weaponScroller = new UIImageScroller(AutoRect.AutoPos(new Rect(400, 0, 550, 640)), AutoRect.AutoPos(new Rect(440, 180, 500, 369)), 1, AutoRect.AutoSize(WeaponsLogoTexturePosition.WeaponLogoSize), ScrollerDir.Vertical, true);
        weaponScroller.SetImageSpacing(AutoRect.AutoSize(WeaponsLogoTexturePosition.WeaponLogoSpacing));
        weaponScroller.EnableLongPress();


        for (int i = 0; i < weaponList.Count; i++)
        {

            if (weaponList[i].Exist == WeaponExistState.Owned)
            {
                UIImage uiImage = new UIImage();

                TexturePosInfo info = WeaponsLogoTexturePosition.GetWeaponTextureRect(i);


                uiImage.SetTexture(info.m_Material, info.m_TexRect, AutoRect.AutoSize(info.m_TexRect));
                weaponScroller.Add(uiImage);
            }

        }

        this.Add(weaponScroller);
        weaponScroller.EnableScroll();
        Material shopMaterial = UIResourceMgr.GetInstance().GetMaterial("ShopUI");

        weaponScroller.AddOverlay(shopMaterial, ShopTexturePosition.LockedLogo);
        Material avatarLogoMaterial = UIResourceMgr.GetInstance().GetMaterial("Avatar");
        weaponScroller.SetMaskImage(avatarLogoMaterial, AvatarTexturePosition.Mask);

        Material avatarMaterial = UIResourceMgr.GetInstance().GetMaterial("Avatar");
        weaponScroller.SetCenterFrameTexture(avatarMaterial, AvatarTexturePosition.Frame);



        weaponScroller.Show();
        battleWeaponGrid.Show();
        this.Add(battleWeaponGrid);





        getMoreMoneyButton = new UITextButton();
        getMoreMoneyButton.SetTexture(UIButtonBase.State.Normal, arenaMenuMaterial,

ArenaMenuTexturePosition.GetMoneyButtonNormal, AutoRect.AutoSize(ArenaMenuTexturePosition.GetMoneyButtonSmallSize));
        getMoreMoneyButton.SetTexture(UIButtonBase.State.Pressed, arenaMenuMaterial,

ArenaMenuTexturePosition.GetMoneyButtonPressed, AutoRect.AutoSize(ArenaMenuTexturePosition.GetMoneyButtonSmallSize));
        getMoreMoneyButton.Rect = AutoRect.AutoPos(uiPos.GetMoreMoneyButton);

        this.Add(getMoreMoneyButton);


        cashPanel = new CashPanel();
        cashPanel.Show();
        this.Add(cashPanel);

        weaponInfoPanel = new WeaponInfoPanel();
        this.Add(weaponInfoPanel);


        if (AutoRect.GetPlatform() == Platform.IPad)
        {
            avatarFrame = new Avatar3DFrame(AutoRect.AutoPos(new Rect(0, 200, 500, 600)), new Vector3(-1.499798f * 0.9f, -0.6672753f * 0.9f, 4.420711f), new Vector3(1.3f, 1.3f, 1.3f) * 0.85f);
        }
        else
        {
            avatarFrame = new Avatar3DFrame(AutoRect.AutoPos(new Rect(0, 200, 500, 600)), new Vector3(-1.499798f, -0.6672753f, 4.420711f), new Vector3(1.3f, 1.3f, 1.3f));
        }

       
        
        this.Add(avatarFrame);


    }



    // Use this for initialization
    public EquipmentUI()
    {

        uiPos = new EquipmentUIPosition();
        //texPos = new EquipmentTexturePosition();

       
        arenaMenuMaterial = UIResourceMgr.GetInstance().GetMaterial("ArenaMenu");
        background = new UIImage();
        background.SetTexture(arenaMenuMaterial, ArenaMenuTexturePosition.Background);
        background.Rect = AutoRect.AutoPos(uiPos.Background);

        /*
        titleImage = new UIImage();
        titleImage.SetTexture(weaponUpgradeMaterial, WeaponUpgradeTexturePosition.TitleImage);
        titleImage.Rect = uiPos.TitleImage;
        */

        returnButton = new UIClickButton();
        returnButton.SetTexture(UIButtonBase.State.Normal, arenaMenuMaterial,

ArenaMenuTexturePosition.ReturnButtonNormal, AutoRect.AutoSize(ArenaMenuTexturePosition.ReturnButtonNormal));
        returnButton.SetTexture(UIButtonBase.State.Pressed, arenaMenuMaterial,

ArenaMenuTexturePosition.ReturnButtonPressed, AutoRect.AutoSize(ArenaMenuTexturePosition.ReturnButtonPressed));
        returnButton.Rect = AutoRect.AutoPos(uiPos.ReturnButton);





        selectionImage = new UIImage();


        /*
        weaponInfoText = new UIText();
        weaponInfoText.Set(ConstData.FONT_NAME2, "", ColorName.fontColor_darkorange);
        weaponInfoText.Rect = uiPos.WeaponInfoText;
        */



        this.Add(background);
        //this.Add(titleImage);

        this.Add(returnButton);
        //this.Add(weaponInfoText);


        uiInited = true;

        Init();
        selectionImage.Enable = false;
        this.Add(selectionImage);
        //weaponInfoText.Set(ConstData.FONT_NAME2, weaponList[0].Info, ColorName.fontColor_darkorange);
        UpdateWeaponInfo();
        
        SetUIHandler(this);

        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        

        if (!uiInited)
        {
            return;
        }
        foreach (UITouchInner touch in iPhoneInputMgr.MockTouches())
        {
            if (m_UIManager.HandleInput(touch))
            {
                continue;
            }
        }

    }

    public override void UpdateLogic()
    {
        if (avatarFrame != null)
        {
            avatarFrame.UpdateAnimation();
        }
    }

    protected void UpdateWeaponInfo()
    {
        Weapon weapon = weaponList[currentSelectionWeaponIndex];
        string damage =   "DAMAGE       " + Math.SignificantFigures(weapon.Damage, 4);
        string firerate = "FIRE RATE      " + Math.SignificantFigures(weapon.AttackFrequency, 4)+"s";
        string accuracy = "ACCURACY     " + Math.SignificantFigures(weapon.Accuracy, 4) + "%";
        string ammo = "x" + weapon.BulletCount;
        if (weapon.GetWeaponType() == WeaponType.Saw)
        {
            ammo = "";
        }

        weaponInfoPanel.UpdateBulletLogo((int)weapon.GetWeaponType());
        weaponInfoPanel.SetText("" + weapon.Name + "\n"+ damage + "\n" + firerate + "\n" + accuracy);
        weaponInfoPanel.SetBulletText(ammo);
    }


    public void HandleEvent(UIControl control, int command, float wparam, float lparam)
    {

        if (control == returnButton)
        {
            AudioPlayer.PlayAudio(ArenaMenuUI.GetInstance().GetComponent<AudioSource>());
            GameApp.GetInstance().Save();
            Hide();
            GameObject.Find("ArenaMenuUI").GetComponent<ArenaMenuUI>().GetPanel(MenuName.ARENA).Show();
        }
        else if (control == weaponScroller)
        {

            int index = (int)wparam;
            //scroller at
            if (command == (int)UIImageScroller.Command.ScrollSelect)
            {
                currentSelectionWeaponIndex = gameState.GetWeaponByOwnedIndex(index);
                UpdateWeaponInfo();
            }
            // long press
            else if (command == (int)UIImageScroller.Command.PressSelect)
            {
                if (gameState != null)
                {

                    Material weaponIconMaterial = UIResourceMgr.GetInstance().GetMaterial("Weapons3");
                    Rect rect = WeaponsLogoTexturePosition.GetWeaponIconTextureRect(currentSelectionWeaponIndex);
                    selectionImage.SetTexture(weaponIconMaterial,rect, AutoRect.AutoSize(rect));
                    selectionImage.Rect = weaponScroller.GetCenterRect();

                }
            }
            //long end
            else if (command == (int)UIImageScroller.Command.PressEnd)
            {
                selectionImage.Rect = new Rect(-1000, -1000, 200, 200);
                for (int i = 0; i < SELECTION_NUM; i++)
                {
                    if (selectionRect[i].Contains(new Vector2(wparam, lparam)))
                    {
                        SelectWeapon(currentSelectionWeaponIndex, i);
                        break;
                    }
                }

            }
            //drag move
            else if (command == (int)UIImageScroller.Command.DragMove)
            {

                selectionImage.Rect = new Rect(wparam - 438 * 1.2f / 2, lparam - 192 * 1.2f / 2, 438 * 1.2f, 192 * 1.2f);

            }

        }
        else if (control == battleWeaponGrid)
        {

            if (command == (int)UIDragGrid.Command.DragOutSide)
            {

                int i = (int)wparam;
                if (InBattleWeaponCount() > 1)
                {

                    int weaponID = rectToWeaponMap[i];
                    if (weaponID != -1)
                    {
                        weaponList[weaponID].IsSelectedForBattle = false;

                        rectToWeaponMap[i] = -1;
                        battleWeaponGrid.HideGridTexture(i);
                        avatarFrame.ChangeAvatar(GameApp.GetInstance().GetGameState().Avatar);
                    }
                }
                else
                {
                    battleWeaponGrid.SetGridTexturePosition(i, i);
                }

            }
            else if (command == (int)UIDragGrid.Command.DragExchange)
            {
                int i = (int)wparam;
                int j = (int)lparam;

                int temp = rectToWeaponMap[i];
                rectToWeaponMap[i] = rectToWeaponMap[j];
                rectToWeaponMap[j] = temp;

                if (rectToWeaponMap[i] != -1)
                {
                    Material weaponIconMaterial = UIResourceMgr.GetInstance().GetMaterial("Weapons3");
                    Rect rect = WeaponsLogoTexturePosition.GetWeaponIconTextureRect(rectToWeaponMap[i]);
                    battleWeaponGrid.SetGridTexture(i, weaponIconMaterial, rect);


                    battleWeaponGrid.SetGridTexturePosition(i, i);


                }
                else
                {
                    battleWeaponGrid.HideGridTexture(i);
                }

                if (rectToWeaponMap[j] != -1)
                {
                    Material weaponIconMaterial = UIResourceMgr.GetInstance().GetMaterial("Weapons3");
                    Rect rect = WeaponsLogoTexturePosition.GetWeaponIconTextureRect(rectToWeaponMap[j]);
                    battleWeaponGrid.SetGridTexture(j, weaponIconMaterial, rect);


                    battleWeaponGrid.SetGridTexturePosition(j, j);
                }
                else
                {
                    battleWeaponGrid.HideGridTexture(j);
                }
                avatarFrame.ChangeAvatar(GameApp.GetInstance().GetGameState().Avatar);

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


    void PutBattleWeapons()
    {

        for (int i = 0; i < SELECTION_NUM; i++)
        {
            int weaponID = rectToWeaponMap[i];
            if (weaponID != -1)
            {
                Material weaponIconMaterial = UIResourceMgr.GetInstance().GetMaterial("Weapons3");
                Rect rect = WeaponsLogoTexturePosition.GetWeaponIconTextureRect(weaponID);
                battleWeaponGrid.SetGridTexture(i, weaponIconMaterial, rect);
            }

        }

    }


    int InBattleWeaponCount()
    {
        int sum = 0;
        for (int i = 0; i < weaponList.Count; i++)
        {
            if (weaponList[i].IsSelectedForBattle)
            {
                sum++;
            }

        }
        return sum;
    }


    void SelectWeapon(int weaponID, int selectRectIndex)
    {
        bool alreadySelected = false;
        for (int j = 0; j < SELECTION_NUM; j++)
        {
            if (rectToWeaponMap[j] != -1)
            {
                if (rectToWeaponMap[j] == weaponID)
                {
                    alreadySelected = true;
                }

            }
        }

        if (!alreadySelected)
        {

            int oldWeaponID = rectToWeaponMap[selectRectIndex];
            if (oldWeaponID != -1)
            {

                weaponList[oldWeaponID].IsSelectedForBattle = false;
            }
            weaponList[weaponID].IsSelectedForBattle = true;
            Material weaponIconMaterial = UIResourceMgr.GetInstance().GetMaterial("Weapons3");
            Rect rect = WeaponsLogoTexturePosition.GetWeaponIconTextureRect(weaponID);
            battleWeaponGrid.SetGridTexture(selectRectIndex, weaponIconMaterial, rect);
            battleWeaponGrid.SetGridTexturePosition(selectRectIndex, selectRectIndex);
            rectToWeaponMap[selectRectIndex] = weaponID;

            avatarFrame.ChangeAvatar(GameApp.GetInstance().GetGameState().Avatar);
        }

    }


    public override void Show()
    {
        currentSelectionWeaponIndex = 0;
        weaponScroller.Clear();
        weaponScroller.SetImageSpacing(AutoRect.AutoSize(WeaponsLogoTexturePosition.WeaponLogoSpacing));

        for (int i = 0; i < weaponList.Count; i++)
        {

            if (weaponList[i].Exist == WeaponExistState.Owned)
            {

                UIImage uiImage = new UIImage();
                TexturePosInfo info = WeaponsLogoTexturePosition.GetWeaponTextureRect(i);
                uiImage.SetTexture(info.m_Material, info.m_TexRect);
                weaponScroller.Add(uiImage);
            }

        }
        weaponScroller.EnableScroll();
        Material shopMaterial = UIResourceMgr.GetInstance().GetMaterial("ShopUI");

        weaponScroller.AddOverlay(shopMaterial, new Rect(720, 610, 160, 75));
        Material avatarLogoMaterial = UIResourceMgr.GetInstance().GetMaterial("Avatar");
        weaponScroller.SetMaskImage(avatarLogoMaterial, AvatarTexturePosition.Mask);

        weaponScroller.Show();

        base.Show();
        avatarFrame.ChangeAvatar(GameApp.GetInstance().GetGameState().Avatar);
        avatarFrame.Show();


        cashPanel.SetCash(gameState.GetCash());
        UpdateWeaponInfo();
        weaponInfoPanel.Show();
    }


    public override void Hide()
    {
        avatarFrame.Hide();
        weaponInfoPanel.Hide();
        base.Hide();
    }

}
