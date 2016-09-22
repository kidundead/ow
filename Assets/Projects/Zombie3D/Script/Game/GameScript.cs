using UnityEngine;
using System.Collections;
using Zombie3D;
//Game Script:  Entry Point of the application
public class GameScript : MonoBehaviour
{

    protected float lastUpdateTime;
    protected float deltaTime = 0;

    /*
    IEnumerator GameLoop()
    {

        while (true)
        {
            //game loop time control
            deltaTime = Time.time - lastUpdateTime;
            lastUpdateTime = Time.time;

            //execute game loop every frame
            GameApp.GetInstance().Loop(deltaTime);

            //sleep 20 miliseconds
            yield return new WaitForSeconds(0.02f);

        }
    }*/



    // Use this for initialization
    void Start()
    {

        Debug.Log("start");

        
        GameApp.GetInstance().Init();
        GameApp.GetInstance().CreateScene();
        lastUpdateTime = Time.time;                  
        //StartCoroutine(GameLoop());

    }

    // Update is called once per frame
    void Update()
    {
        //game loop time control
        deltaTime += Time.deltaTime;
        //if (deltaTime >= 0.01f)
        {
            //execute game loop
            GameApp.GetInstance().Loop(deltaTime);
            deltaTime = 0;
        }
    }
}
