using UnityEngine;
using System.Collections;

public class DelayCameraDisplayScript : MonoBehaviour
{

    public float delayTime = 0.5f;
    protected float startTime;

    // Use this for initialization
    void Start()
    {

        GetComponent<Camera>().enabled = false;
        startTime = Time.time;

    }

    // Update is called once per frame
    void Update()
    {

        if (Time.time - startTime > delayTime)
        {
            GetComponent<Camera>().enabled = true;
        }


    }
}
