using UnityEngine;
using System.Collections;
using Zombie3D;


public class Step7Script : MonoBehaviour, ITutorialStep, ITutorialUIEvent
{
    protected ITutorialGameUI guis;
    protected TutorialScript ts;

    
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
        dialog.SetText("\nALRIGHT, NOW THAT YOU'VE GOT DOWN THE BASICS, TRY SHOOTING THE ZOMBIE IN FRONT OF YOU.");
        dialog.Show();
    }

    public void UpdateTutorialStep(float deltaTime, Player player)
    {
        //guis.SetTutorialText("ALRIGHT, NOW THAT YOU'VE GOT DOWN THE BASICS, TRY SHOOTING THE ZOMBIE IN FRONT OF YOU.");

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

        ts.GoToNextStep();

    }

}
