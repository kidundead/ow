using UnityEngine;
using System.Collections;
using Zombie3D;


public class Step3Script : MonoBehaviour, ITutorialStep, ITutorialUIEvent
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
        dialog.SetText("\nTAP AND SLIDE ON THE SCREEN TO ADJUST YOUR LINE OF SIGHT, THEN AIM AT THE BOX IN FRONT OF YOU.");
        dialog.Show();
    }

    public void UpdateTutorialStep(float deltaTime, Player player)
    {
        //guis.SetTutorialText("TAP AND SLIDE ANYWHERE ON THE SCREEN TO ADJUST YOUR LINE OF \n SIGHT.");
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
