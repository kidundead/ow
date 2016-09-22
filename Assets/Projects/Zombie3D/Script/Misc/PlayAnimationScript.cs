using UnityEngine;
using System.Collections;
using Zombie3D;


public class PlayAnimationScript : MonoBehaviour
{
    public string animationName;
    protected float lastUpdateTime;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastUpdateTime < 0.02f)
        {
            return;
        }
        lastUpdateTime = Time.time;

        //animation.Play(animationName);

    }

}

