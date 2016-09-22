using UnityEngine;
using System.Collections;
using Zombie3D;


public class Step2Script : MonoBehaviour, ITutorialStep, ITutorialUIEvent
{
    protected ITutorialGameUI guis;
    protected TutorialScript ts;

    protected GameObject halo;
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
        halo = GameObject.Find("Halo");
        guis.EnableTutorialOKButton(false);
    }

    public void UpdateTutorialStep(float deltaTime, Player player)
    {
        //guis.SetTutorialText("GO AHEAD AND WALK INTO THE CIRCLE OF PULSATING LIGHT.");

        if((halo.transform.position - player.GetTransform().position).sqrMagnitude < 1.0f*1.0f)
        {
            GameObject.Destroy(halo);
            //GameObject.Find("Arrow").SetActiveRecursively(false);
            GameObject arrowObj = GameObject.Find("Arrow");
            arrowObj.transform.position = GameObject.Find("WoodBox").transform.TransformPoint(Vector3.forward * 2f);
            ItemScript its = arrowObj.GetComponent<ItemScript>();
            its.HighPos = 2.2f;
            its.LowPos = 2.0f;
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
