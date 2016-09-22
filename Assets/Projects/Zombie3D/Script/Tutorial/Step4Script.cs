using UnityEngine;
using System.Collections;
using Zombie3D;


public class Step4Script : MonoBehaviour, ITutorialStep, ITutorialUIEvent
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
        player.InputController.EnableShootingInput = false;

        guis.EnableTutorialOKButton(false);

    }

    public void UpdateTutorialStep(float deltaTime, Player player)
    {
        //guis.SetTutorialText("TAP AND SLIDE ANYWHERE ON THE SCREEN TO ADJUST YOUR LINE OF SIGHT.");

        Transform boxTrans = GameObject.Find("WoodBox").transform;

        Vector3 relativePos = player.GetTransform().InverseTransformPoint(boxTrans.position);

        float tan55 = Mathf.Tan(Mathf.Deg2Rad * 55.0f);
        if (relativePos.z > 0)
        {

            if (Mathf.Abs(relativePos.z / relativePos.x) > tan55)
            {
                ts.GoToNextStep();
            }
        }

    }

    public void EndStep(Player player)
    {
        player.InputController.CameraRotation = new Vector2(0, 0);
    }

    public void SetGameGUI(ITutorialGameUI guis)
    {
        this.guis = guis;
    }

    public void OK(Player player)
    {
 


    }

}
