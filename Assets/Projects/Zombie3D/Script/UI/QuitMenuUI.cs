using UnityEngine;
using System.Collections;
using Zombie3D;

/*
class QuitMenuTexturePosition
{
    public Rect Background = new Rect(0, 0, 743, 400);
    
    public Rect LeftButtonNormal = new Rect(743, 0, 145, 83);
    public Rect LeftButtonPressed = new Rect(743, 83, 145, 83);

    public Rect RightButtonNormal = new Rect(888, 0, 131, 80);
    public Rect RightButtonPressed = new Rect(888, 80, 131, 80);
}

class QuitMenuUIPosition
{

    public Rect Background = new Rect(120, 150, 743, 400);


    public Rect LeftButton = new Rect(190, 140, 145, 83);
    public Rect RightButton = new Rect(670, 140, 131, 80);

    public Rect LeftText = new Rect(220 + 80, 400 - 30, 279, 91);
    public Rect RightText = new Rect(300 + 80, 350 - 30, 279, 91);
    public Rect FirstLineText = new Rect(200 + 80, 310 - 90, 529, 191);
    public Rect SecondLineText = new Rect(300 + 80, 250 - 90, 479, 91);
    public Rect ReturnText = new Rect(400, 0, 279, 91);
}


public class QuitMenuUI : UIPanel, UIHandler
{
    protected Rect[] buttonRect;

    public UIControl panel;
    public UIManager m_UIManager = null;
   
    public string m_ui_material_path;  

    protected Material quitMenuMaterial;

    protected UIImage background;
    protected UIClickButton rightYesButton;

    protected UIText leftButtonText;
    protected UIText rightButtonText;
    protected UIText firstLineText;
    protected UIText secondLineText;
    protected UIText returnText;



    protected UIClickButton nouseButton;
    protected UIClickButton leftNoButton;

    private QuitMenuUIPosition uiPos;
    private QuitMenuTexturePosition texPos;

    protected float screenRatioX;
    protected float screenRatioY;
    protected GameState gameState;
    protected GameObject pauseUI;
    // Use this for initialization

    public void EnableUI()
    {
        m_UIManager.enabled = true;
    }

    public Material GetQuitMenuMaterial()
    {
        return quitMenuMaterial;
    }


    public void Init()
    {
        uiPos = new QuitMenuUIPosition();
        texPos = new QuitMenuTexturePosition();
        gameState = GameApp.GetInstance().GetGameState();



        quitMenuMaterial = UIResourceMgr.GetInstance().GetMaterial("QuitMenu");

        background = new UIImage();
        background.SetTexture(quitMenuMaterial,

texPos.Background);
        background.Rect = uiPos.Background;


        rightYesButton = new UIClickButton();
        rightYesButton.SetTexture(UIButtonBase.State.Normal, quitMenuMaterial,

texPos.RightButtonNormal);
        rightYesButton.SetTexture(UIButtonBase.State.Pressed, quitMenuMaterial,

texPos.RightButtonPressed);


        rightYesButton.Rect = uiPos.RightButton;



        leftNoButton = new UIClickButton();
        leftNoButton.SetTexture(UIButtonBase.State.Normal, quitMenuMaterial,

texPos.LeftButtonNormal);
        leftNoButton.SetTexture(UIButtonBase.State.Pressed, quitMenuMaterial,

texPos.LeftButtonPressed);


        leftNoButton.Rect = uiPos.LeftButton;


        leftButtonText = new UIText();
        leftButtonText.Set(ConstData.FONT_NAME1, "NO", ColorName.fontColor_menu);
        leftButtonText.Rect = uiPos.LeftText;



        rightButtonText = new UIText();
        rightButtonText.Set(ConstData.FONT_NAME1, "YES", ColorName.fontColor_menu);
        rightButtonText.Rect = uiPos.RightText;

        firstLineText = new UIText();
        firstLineText.Set(ConstData.FONT_NAME1, "YOU WILL LEAVE THE GAME, ARE YOU SURE?", ColorName.fontColor_gray);
        firstLineText.Rect = uiPos.FirstLineText;


        secondLineText = new UIText();
        secondLineText.Set(ConstData.FONT_NAME1, "ARE YOU SURE?", ColorName.fontColor_gray);
        secondLineText.Rect = uiPos.SecondLineText;


        returnText = new UIText();
        returnText.Set(ConstData.FONT_NAME1, "Back", ColorName.fontColor_menu);
        returnText.Rect = uiPos.ReturnText;

        this.Add(background);


        this.Add(rightYesButton);

        this.Add(leftNoButton);

        //m_UIManager.Add(leftButtonText);
        //m_UIManager.Add(rightButtonText);
        this.Add(firstLineText);
        //m_UIManager.Add(secondLineText);

        //m_UIManager.Add(returnText);
        this.SetUIHandler(this);
        Hide();

        //gameObject.SetActiveRecursively(false);
    }

    public QuitMenuUI()
    {

      

    }
    

    public void HandleEvent(UIControl control, int command, float wparam, float lparam)
    {

    }

}
*/