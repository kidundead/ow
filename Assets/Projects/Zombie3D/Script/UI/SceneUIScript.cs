using UnityEngine;
using System.Collections;
using Zombie3D;
public class ButtonNames
{
    public const int WEAPON_SWITCH = 0;
    public const int BOMB = 1;
    public const int CONTINUE = 2;
    public const int START_OVER = 3;

}


public class SceneUIScript : MonoBehaviour
{

    //public GUISkin gs;

    protected bool increase = true;

    protected float frames;
    protected float updateInterval = 2.0f;
    protected float timeLeft;
    protected string fpsStr;
    protected float accum;
    protected int count = 0;
    protected Rect[] buttonRect;



    // Use this for initialization
    void Start()
    {

        timeLeft = updateInterval;

        buttonRect = new Rect[4];
        buttonRect[ButtonNames.WEAPON_SWITCH] = new Rect(0.8f * Screen.width, 0.05f * Screen.height, 0.16f * Screen.width, 0.07f * Screen.height);
        buttonRect[ButtonNames.BOMB] = new Rect(0.4f * Screen.width, 0.75f * Screen.height, 0.24f * Screen.width, 0.08f * Screen.height);
        buttonRect[ButtonNames.CONTINUE] = new Rect(0.4f * Screen.width, 0.75f * Screen.height, 0.14f * Screen.width, 0.14f * Screen.height);
        buttonRect[ButtonNames.START_OVER] = new Rect(0.4f * Screen.width, 0.75f * Screen.height, 0.14f * Screen.width, 0.14f * Screen.height);

    }

    // Update is called once per frame
    void Update()
    {

        timeLeft -= Time.deltaTime;

        accum += Time.timeScale / Time.deltaTime;
        frames++;

        if (timeLeft <= 0)
        {
            fpsStr = "FPS:" + (accum / frames).ToString();
            frames = 0;
            accum = 0;
            timeLeft = updateInterval;
        }

    }

   

}
