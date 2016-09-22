using UnityEngine;
using System.Collections;
using Zombie3D;


public class Step9Script : MonoBehaviour, ITutorialStep, ITutorialUIEvent
{
    protected ITutorialGameUI guis;
    protected TutorialScript ts;
    protected Enemy enemy;
    
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartStep(TutorialScript ts, Player player)
    {
        this.ts = ts;
        player.InputController.EnableMoveInput = false;
        player.InputController.EnableTurningAround = false;
        player.InputController.EnableShootingInput = false;
        player.InputController.CameraRotation = Vector2.zero;
        guis.EnableTutorialOKButton(true);
        GameDialog dialog = GameUIScript.GetGameUIScript().GetDialog();
        dialog.SetText("\nCONGRATS! YOU'VE COMPLETED THE CALL OF MINI BOOTCAMP! READY TO GO INTO BATTLE?");
        dialog.Show();
    }

    public void UpdateTutorialStep(float deltaTime, Player player)
    {
        //guis.SetTutorialText("CONGRATS! YOU'VE COMPLETED THE CALL OF MINI BOOTCAMP! READY TO GO INTO BATTLE?");

      
    }

    public void EndStep(Player player)
    {
    }

    public void SetGameGUI(ITutorialGameUI guis)
    {
        this.guis = guis;
    }

    public void OK(Player player)
    {
        GameApp.GetInstance().GetGameState().FirstTimeGame = false;
        GameApp.GetInstance().Save();
        Application.LoadLevel(SceneName.SCENE_ARENA);

    }

}
