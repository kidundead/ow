using UnityEngine;
using System.Collections;
using Zombie3D;
public class FireLineScript : MonoBehaviour
{

    public Vector3 beginPos;
    public Vector3 endPos;
    public float speed;

    protected float startTime;
    //protected float lastUpdateTime;
    protected float deltaTime = 0; 
    // Use this for initialization
    void Start()
    {
        startTime = Time.time;
        //lastUpdateTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        deltaTime += Time.deltaTime;

        if (deltaTime < 0.03f)
        {
            return;
        }

        
        //Debug.Log(deltaTime + "," +Time.deltaTime);
        transform.Translate(speed * (endPos - beginPos).normalized * deltaTime, Space.World);

        if ((transform.position - endPos).magnitude < 1)
        {
            gameObject.active = false;
            //Destroy(gameObject);
        }
        //lastUpdateTime = Time.time;
        deltaTime = 0;
    }
}
