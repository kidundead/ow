using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zombie3D;



public class IAPLockPanel : UIPanel
{
    protected UIImage maskImage;
    protected UIBlock block;
    protected UIImage spinner;

    public IAPLockPanel()
    {
        maskImage = new UIImage();
        Material material = UIResourceMgr.GetInstance().GetMaterial("Avatar");
        maskImage.SetTexture(material, AvatarTexturePosition.Mask, AutoRect.AutoSize(ArenaMenuTexturePosition.Background));
        maskImage.Rect = AutoRect.AutoPos(new Rect(0, 0, 960, 640));

        block = new UIBlock();
        block.Rect = AutoRect.AutoPos(new Rect(0, 0, 960, 640));

        spinner = new UIImage();
        Material arenaMaterial = UIResourceMgr.GetInstance().GetMaterial("ArenaMenu");
        spinner.SetTexture(arenaMaterial, ArenaMenuTexturePosition.Spinner, AutoRect.AutoSize(ArenaMenuTexturePosition.Spinner));
        spinner.Rect = AutoRect.AutoPos(new Rect(0, 0, 960, 640));

        Add(maskImage);
        Add(spinner);
        Add(block);

    }

    public void UpdateSpinner()
    {
        spinner.SetRotation(Mathf.Deg2Rad * (int)(Time.time / 0.05f) * (-1) * 30f);
    }

}


class ShopUIPosition
{
    public Rect Background = new Rect(0, 0, 960, 640);
    public Rect Dialog = new Rect(40, 60, 887, 535);
    public Rect TitleImage = new Rect(90, 560, 267, 71);
    public Rect ReturnButton = new Rect(24, 6, 130, 70);
    public Rect CategoryText = new Rect(130, 510, 802, 87);

    public Rect LeftButton = new Rect(0, 260, 152, 212);
    public Rect RightButton = new Rect(806, 264, 152, 212);

}


public class ShopUI : UIPanel, UIHandler
{
    protected Rect[] buttonRect;

    public UIManager m_UIManager = null;
    public string m_ui_material_path;  //最后带反斜杠
    protected Material arenaMaterial;
    protected Material dialogMaterial;
    protected Material shop2Material;
    protected const int BUTTON_NUM = 3;
    protected UIClickButton[] itemButton = new UIClickButton[BUTTON_NUM];
    protected UIText[] itemText = new UIText[BUTTON_NUM];
    protected UIImage[] soldoutLogo = new UIImage[BUTTON_NUM];
    protected UIImage background;
    protected UIImage dialogImage;
    protected UIImage titleImage;
    protected UIClickButton returnButton;


    protected UIClickButton leftArrowButton;
    protected UIClickButton rightArrowButton;
    protected IAPLockPanel iapLockPanel;
    protected Shop shop;
    protected List<IAPItem>[] itemList;
    protected int[] currentScroll = new int[(int)IAPName.None];

    protected UIPanel fromPanel;


    protected UIText categoryText;
    protected CashPanel cashPanel;

    private ShopUIPosition uiPos;
    //private ShopTexturePosition texPos;

    protected float screenRatioX;
    protected float screenRatioY;

    protected IAPName iapProcessing = IAPName.None;
    protected int page = 0;

    // Use this for initialization
    public ShopUI()
    {
        shop = new Shop();
        shop.CreateIAPShopData();
        itemList = shop.GetIAPList();
        for (int i = 0; i < (int)IAPType.Count; i++)
        {
            currentScroll[i] = 0;
        }

        uiPos = new ShopUIPosition();
        //texPos = new ShopTexturePosition();


        arenaMaterial = UIResourceMgr.GetInstance().GetMaterial("ArenaMenu");
        dialogMaterial = UIResourceMgr.GetInstance().GetMaterial("ShopUI");
        shop2Material = UIResourceMgr.GetInstance().GetMaterial("ShopUI2");
        background = new UIImage();
        background.SetTexture(arenaMaterial,

ArenaMenuTexturePosition.Background, AutoRect.AutoSize(ArenaMenuTexturePosition.Background));
        background.Rect = AutoRect.AutoPos(uiPos.Background);

        dialogImage = new UIImage();
        dialogImage.SetTexture(dialogMaterial,

ShopTexturePosition.Dialog, AutoRect.AutoSize(ShopTexturePosition.Dialog));
        dialogImage.Rect = AutoRect.AutoPos(uiPos.Dialog);

        titleImage = new UIImage();
        titleImage.SetTexture(arenaMaterial, ArenaMenuTexturePosition.ShopTitleImage, AutoRect.AutoSize(ArenaMenuTexturePosition.ShopTitleImage));
        titleImage.Rect = AutoRect.AutoPos(uiPos.TitleImage);


        cashPanel = new CashPanel();




        for (int i = 0; i < BUTTON_NUM; i++)
        {

            itemButton[i] = new UIClickButton();
            /*
            itemButton[i].SetTexture(UIButtonBase.State.Normal, dialogMaterial,

itemList[i][currentScroll[i]].textureRect, AutoRect.AutoSize(itemList[i][currentScroll[i]].textureRect));
            itemButton[i].SetTexture(UIButtonBase.State.Pressed, dialogMaterial,

itemList[i][currentScroll[i]].textureRect, AutoRect.AutoSize(itemList[i][currentScroll[i]].textureRect));
            */
            itemButton[i].Rect = AutoRect.AutoPos(new Rect(102 + 251 * i, 640 - 448, 252, 354));



            soldoutLogo[i] = new UIImage();
            soldoutLogo[i].SetTexture(dialogMaterial, ShopTexturePosition.SoldOutLogo);
            soldoutLogo[i].Rect = new Rect(110 + 270 * i, 640 - 350, 160, 82);
            soldoutLogo[i].Enable = false;
            soldoutLogo[i].Visible = false;
            itemText[i] = new UIText();
            itemText[i].Set(ConstData.FONT_NAME2, itemList[0][currentScroll[i]].Desc, ColorName.fontColor_orange);
            itemText[i].AlignStyle = UIText.enAlignStyle.center;
            itemText[i].Rect = AutoRect.AutoPos(new Rect(104 + 252 * i, 114, 240, 90));

            /*
            if (itemList[i][currentScroll[i]].Name == IAPName.AutoMissle && GameApp.GetInstance().GetGameState().IsWeaponOwned(WeaponType.Sniper))
            {
                soldoutLogo[i].Visible = true;

            }
            if (itemList[i][currentScroll[i]].Name == IAPName.EnegyArmor && GameApp.GetInstance().GetGameState().GetAvatarData(AvatarType.EnegyArmor) == AvatarState.Avaliable)
            {
                soldoutLogo[i].Visible = true;

            }
            */
        }



        returnButton = new UIClickButton();
        returnButton.SetTexture(UIButtonBase.State.Normal, arenaMaterial,

ArenaMenuTexturePosition.ReturnButtonNormal, AutoRect.AutoSize(ArenaMenuTexturePosition.ReturnButtonNormal));
        returnButton.SetTexture(UIButtonBase.State.Pressed, arenaMaterial,

ArenaMenuTexturePosition.ReturnButtonPressed, AutoRect.AutoSize(ArenaMenuTexturePosition.ReturnButtonPressed));
        returnButton.Rect = AutoRect.AutoPos(uiPos.ReturnButton);


        categoryText = new UIText();
        categoryText.Set(ConstData.FONT_NAME1, "CASH BAG    WEAPON     AVATAR", ColorName.fontColor_orange);
        categoryText.Rect = uiPos.CategoryText;




        iapLockPanel = new IAPLockPanel();


        this.Add(background);
        this.Add(dialogImage);
        //this.Add(titleImage);

        for (int i = 0; i < BUTTON_NUM; i++)
        {
            this.Add(itemButton[i]);
            this.Add(itemText[i]);
            this.Add(soldoutLogo[i]);

        }

        this.Add(cashPanel);
        this.Add(returnButton);

        leftArrowButton = new UIClickButton();
        leftArrowButton.SetTexture(UIButtonBase.State.Normal, dialogMaterial,

ShopTexturePosition.ArrowNormal, AutoRect.AutoSize(ShopTexturePosition.ArrowNormal));
        leftArrowButton.SetTexture(UIButtonBase.State.Pressed, dialogMaterial,

ShopTexturePosition.ArrowPressed, AutoRect.AutoSize(ShopTexturePosition.ArrowPressed));
        leftArrowButton.Rect = AutoRect.AutoPos(uiPos.LeftButton);

        rightArrowButton = new UIClickButton();
        rightArrowButton.SetTexture(UIButtonBase.State.Normal, dialogMaterial,

ShopTexturePosition.RightArrowNormal, AutoRect.AutoSize(ShopTexturePosition.RightArrowNormal));
        rightArrowButton.SetTexture(UIButtonBase.State.Pressed, dialogMaterial,

ShopTexturePosition.RightArrowPressed, AutoRect.AutoSize(ShopTexturePosition.RightArrowPressed));
        rightArrowButton.Rect = AutoRect.AutoPos(uiPos.RightButton);
        
        this.Add(leftArrowButton);

        this.Add(rightArrowButton);
        this.Add(iapLockPanel);


        this.SetUIHandler(this);

        UpdateItemsUI();
        Hide();


    }


    public void SetFromPanel(UIPanel panel)
    {
        fromPanel = panel;
    }

    public void GetPurchaseStatus()
    {
        if (iapProcessing != IAPName.None)
        {
            int statusCode = IAP.purchaseStatus(null);
            iapLockPanel.UpdateSpinner();
            if (statusCode == 0)
            {

            }
            else if (statusCode == 1)
            {
                Debug.Log("statusCode:" + statusCode);
                GameApp.GetInstance().GetGameState().DeliverIAPItem(iapProcessing);
                cashPanel.SetCash(GameApp.GetInstance().GetGameState().GetCash());
                iapLockPanel.Hide();
                iapProcessing = IAPName.None;
            }
            else
            {
                Debug.Log("statusCode:" + statusCode);
                iapLockPanel.Hide();
                iapProcessing = IAPName.None;

            }
        }
    }

    public override void Show()
    {
        cashPanel.SetCash(GameApp.GetInstance().GetGameState().GetCash());
        cashPanel.Show();


        base.Show();
    }

    public void HandleEvent(UIControl control, int command, float wparam, float lparam)
    {
        for (int i = 0; i < BUTTON_NUM; i++)
        {
            if (control == itemButton[i] && !soldoutLogo[i].Visible)
            {
                AudioPlayer.PlayAudio(ArenaMenuUI.GetInstance().GetComponent<AudioSource>());
                IAP.NowPurchaseProduct(itemList[0][i + page * 3].ID, "1");
                iapProcessing = (IAPName)i + page * 3;
                Debug.Log("IAP ID:" + itemList[0][i + page * 3].ID);
                iapLockPanel.Show();

            }
        }

        if (control == returnButton)
        {
            AudioPlayer.PlayAudio(ArenaMenuUI.GetInstance().GetComponent<AudioSource>());
            Hide();
            if (fromPanel != null)
            {
                fromPanel.Show();
            }
            else
            {
                ArenaMenuUI ui = GameObject.Find("ArenaMenuUI").GetComponent<ArenaMenuUI>();
                ui.GetPanel(MenuName.ARENA).Show();
            }
        }
        else if (control == leftArrowButton)
        {
            if (page > 0)
            {
                page--;
                UpdateItemsUI();
            }
        }
        else if (control == rightArrowButton)
        {
            if (page < 1)
            {
                page++;
                UpdateItemsUI();
            }
        }

    }

    protected void UpdateItemsUI()
    {
        if (page == 0)
        {
            for (int i = 0; i < 3; i++)
            {
                itemButton[i].SetTexture(UIButtonBase.State.Normal, shop2Material,

    ShopTexturePosition.GetIAPLogoRect(i), AutoRect.AutoSize(ShopTexturePosition.GetIAPLogoRect(i)));
                itemButton[i].SetTexture(UIButtonBase.State.Pressed, shop2Material,

    ShopTexturePosition.GetIAPLogoRect(i), AutoRect.AutoSize(ShopTexturePosition.GetIAPLogoRect(i)));

                itemText[i].SetText(itemList[0][i].Desc);

            }

            itemButton[2].Visible = true;
            itemButton[2].Enable = true;
            itemText[2].Visible = true;
            itemText[2].Enable = true;
        }
        else if (page == 1)
        {
            for (int i = 0; i < 2; i++)
            {
                itemButton[i].SetTexture(UIButtonBase.State.Normal, shop2Material,

    ShopTexturePosition.GetIAPLogoRect(i + 3), AutoRect.AutoSize(ShopTexturePosition.GetIAPLogoRect(i + 3)));
                itemButton[i].SetTexture(UIButtonBase.State.Pressed, shop2Material,

    ShopTexturePosition.GetIAPLogoRect(i + 3), AutoRect.AutoSize(ShopTexturePosition.GetIAPLogoRect(i + 3)));

                itemText[i].SetText(itemList[0][i + 3].Desc);
            }
            itemButton[2].Visible = false;
            itemButton[2].Enable = false;
            itemText[2].Visible = false;
            itemText[2].Enable = false;
        }
    }
}
