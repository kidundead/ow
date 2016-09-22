using UnityEngine;
using System.Collections;
using Zombie3D;


public class Step6Script : MonoBehaviour, ITutorialStep, ITutorialUIEvent
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
        player.InputController.EnableMoveInput = true;
        player.InputController.EnableTurningAround = true;
        player.InputController.EnableShootingInput = true;
        guis.EnableTutorialOKButton(false);
    }

    public void UpdateTutorialStep(float deltaTime, Player player)
    {
        //guis.SetTutorialText("USE THE RIGHT JOYSTICK TO AIM AT AND SHOOT THE BOX IN FRONT OF YOU.");

        GameObject box = GameObject.Find("WoodBox");

        if (box == null)
        {
            GameObject.Find("Arrow").SetActiveRecursively(false);
            ts.GoToNextStep();
        }

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
 


    }

}
