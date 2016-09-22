using UnityEngine;
using System.Collections;
using Zombie3D;
public class RemoveTimerScript : MonoBehaviour
{

    public float life;
    protected float createdTime;

    // Use this for initialization
    void Start()
    {

        createdTime = Time.time;

    }

    // Update is called once per frame
    void Update()
    {


        if (Time.time - createdTime > life)
        {
            GameObject.Destroy(gameObject);

        }

    }
}
