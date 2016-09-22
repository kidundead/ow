using UnityEngine;
using System.Collections;
using Zombie3D;


public class Step1Script : MonoBehaviour, ITutorialStep, ITutorialUIEvent
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
        

        GameObject halo = GameObject.Instantiate(GameApp.GetInstance().GetResourceConfig().halo, player.GetTransform().TransformPoint(Vector3.forward * 15.0f), Quaternion.Euler(90, 0, 0)) as GameObject;
        halo.name = "Halo";

        GameDialog dialog = GameUIScript.GetGameUIScript().GetDialog();
        dialog.SetText("\nUSE THE LEFT JOYSTICK TO WALK AROUND. GO AHEAD AND WALK INTO THE CIRCLE OF PULSATING LIGHT.");
        dialog.Show();

        //dialogText.Rect = uiPos.DesignerText;

    }

    public void UpdateTutorialStep(float deltaTime, Player player)
    {
        guis.EnableTutorialOKButton(true);

        //guis.SetTutorialText("USE THE LEFT JOYSTICK TO WALK AROUND.");


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
