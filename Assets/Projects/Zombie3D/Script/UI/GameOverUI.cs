using UnityEngine;
using System.Collections;
using Zombie3D;

class GameOverUIPosition
{
    public Rect DialogImage = new Rect(175, 180, 610, 376);

    public Rect GameOverLabel = new Rect(300, 524, 360, 76);

    public Rect RetryButton = new Rect(540, 60, 356, 116);
    public Rect QuitButton = new Rect(60, 60, 356, 116);

    public Rect ReturnText = new Rect(400, 0, 202, 87);


   
    public Rect FirstLineText = new Rect(0, 270, 960, 187);
    //public Rect SurviveTimeText = new Rect(210, 220, 402, 187);
    //public Rect ScoreText = new Rect(210, 170, 402, 187);
    //public Rect NewEquipmentText = new Rect(220, 160, 402, 187);

    public Rect GameOver = new Rect(0, 200, 960, 280);

    public Rect CashText = new Rect(400, 550, 202, 87);

    public Rect Mask = new Rect(0, 0, 1024, 768);
}

public class GameOverUI : UIPanel, UIHandler
{
    protected Rect[] buttonRect;
    
    protected Material gameuiMaterial;

    protected UIImage dialogImage;
    protected UITextImage gameoverLabel;
    protected UITextButton retryButton;
    protected UITextButton quitButton;

    protected UIText newEquipmentText;
    protected UIText returnText;

    protected UIText surviveTimeText;
    protected UIText firstLineText;
    protected UIText scoreText;
    protected UIText cashText;


    protected UIImage gameover;
    protected UIImage mask;

    private GameOverUIPosition uiPos;
    private GameOverTexturePosition texPos;

    protected float screenRatioX;
    protected float screenRatioY;

    protected bool uiInited = false;


    protected GameState gameState;
    protected Weapon selectedWeapon;
    protected float startTime;

    // Use this for initialization
    public GameOverUI()
    {



        uiPos = new GameOverUIPosition();
        texPos = new GameOverTexturePosition();

        gameState = GameApp.GetInstance().GetGameState();
        selectedWeapon = gameState.GetWeapons()[0];

        gameuiMaterial = UIResourceMgr.GetInstance().GetMaterial("GameUI");
        Material buttonsMaterial = UIResourceMgr.GetInstance().GetMaterial("Buttons");
        retryButton = new UITextButton();

        retryButton.Rect = AutoRect.AutoPos(uiPos.RetryButton);
        retryButton.SetTexture(UIButtonBase.State.Normal, buttonsMaterial, ButtonsTexturePosition.ButtonNormal, AutoRect.AutoSize( ButtonsTexturePosition.ButtonNormal));
        retryButton.SetTexture(UIButtonBase.State.Pressed, buttonsMaterial, ButtonsTexturePosition.ButtonPressed, AutoRect.AutoSize(ButtonsTexturePosition.ButtonPressed));
        retryButton.SetText(ConstData.FONT_NAME1, "RETRY", ColorName.fontColor_orange);

        quitButton = new UITextButton();

        quitButton.Rect = AutoRect.AutoPos(uiPos.QuitButton);
        quitButton.SetTexture(UIButtonBase.State.Normal, buttonsMaterial, ButtonsTexturePosition.ButtonNormal, AutoRect.AutoSize(ButtonsTexturePosition.ButtonNormal));
        quitButton.SetTexture(UIButtonBase.State.Pressed, buttonsMaterial, ButtonsTexturePosition.ButtonPressed, AutoRect.AutoSize(ButtonsTexturePosition.ButtonPressed));
        quitButton.SetText(ConstData.FONT_NAME1, "QUIT", ColorName.fontColor_orange);


        dialogImage = new UIImage();
        dialogImage.SetTexture(gameuiMaterial,

GameUITexturePosition.Dialog, AutoRect.AutoSize(GameUITexturePosition.Dialog));

        dialogImage.Rect =AutoRect.AutoPos( uiPos.DialogImage);

        mask = new UIImage();
        mask.SetTexture(gameuiMaterial, GameUITexturePosition.Mask, AutoRect.AutoSize(uiPos.Mask));
        mask.Rect = AutoRect.AutoValuePos(uiPos.Mask);

        gameoverLabel = new UITextImage();
        gameoverLabel.SetTexture(buttonsMaterial,

ButtonsTexturePosition.Label, ButtonsTexturePosition.LargeLabelSize*0.1f);


        gameoverLabel.Rect = AutoRect.AutoPos(uiPos.GameOverLabel);
        //gameoverLabel.SetText(ConstData.FONT_NAME1, " GAME OVER", ColorName.fontColor_orange);



        cashText = new UIText();
        cashText.Set(ConstData.FONT_NAME1, "Cash", ColorName.fontColor_orange);
        cashText.Rect = AutoRect.AutoPos(uiPos.CashText);


        /*
        surviveTimeText = new UIText();
        surviveTimeText.Set(ConstData.FONT_NAME2, "TIME ", ColorName.fontColor_darkorange);
        surviveTimeText.Rect = uiPos.SurviveTimeText;
        */

        firstLineText = new UIText();
        firstLineText.Set(ConstData.FONT_NAME1, "GAME OVER", ColorName.fontColor_darkorange);
        firstLineText.AlignStyle = UIText.enAlignStyle.center;
        firstLineText.Rect = AutoRect.AutoPos(uiPos.FirstLineText);

        /*
        scoreText = new UIText();
        scoreText.Set(ConstData.FONT_NAME2, "KILLS   "+GameApp.GetInstance().GetGameScene().Killed, ColorName.fontColor_darkorange);
        scoreText.Rect = uiPos.ScoreText;
        */


        gameover = new UIImage();
        gameover.SetTexture(gameuiMaterial, GameUITexturePosition.GameOver, AutoRect.AutoSize(GameUITexturePosition.GameOver)*0.1f);
        gameover.Rect = AutoRect.AutoPos(uiPos.GameOver);
        gameover.Visible = false;
        gameover.Enable = false;
        retryButton.Visible = false;
        quitButton.Visible = false;
        //this.Add(dialogImage);
        //this.Add(gameoverLabel);
        this.Add(mask);
        //this.Add(firstLineText);
        //this.Add(scoreText);
        this.Add(retryButton);
        this.Add(quitButton);
        this.Add(gameover);
        
        this.SetUIHandler(this);

        uiInited = true;
        GameScene gameScene = GameApp.GetInstance().GetGameScene();

        if (gameScene.GetQuest() != null)
        {
            surviveTimeText.SetText("SurviveTime " + gameScene.GetQuest().GetQuestInfo());
            firstLineText.SetText("Kills " + gameScene.Killed);
        }

    }


    public override void Show()
    {
        //firstLineText.SetText("GAME OVER");
        startTime = Time.time;
        gameover.Visible = true;
        base.Show();
    }

    public void DisplayBattleEndUI()
    {
        //m_UIManager.active = true;
        
    }

    public override void UpdateLogic()
    {

        if (gameover.Visible)
        {
            Material buttonsMaterial = UIResourceMgr.GetInstance().GetMaterial("Buttons");
      
            float size =(0.1f+(Time.time - startTime)*0.9f);
            if (Time.time - startTime > 1.2f)
            {
                size = (0.1f + (1.2f) * 0.9f) - (Time.time - startTime - 1.2f) * 0.2f;
                if (size <= 1)
                {
                    size = 1;
                    retryButton.Visible = true;
                    quitButton.Visible = true;
                }
            }

            gameover.SetTexture(gameuiMaterial, GameUITexturePosition.GameOver, AutoRect.AutoSize(GameUITexturePosition.GameOver)*size);
        }


    }


    public void HandleEvent(UIControl control, int command, float wparam, float lparam)
    {

        if (control == retryButton)
        {
            GameApp.GetInstance().Save();
            Application.LoadLevel(Application.loadedLevelName);
        }
        else if (control == quitButton)
        {
            UIResourceMgr.GetInstance().UnloadAllUIMaterials();
            Application.LoadLevel(SceneName.MAP);
            GameApp.GetInstance().Save();
        }
    }

}
