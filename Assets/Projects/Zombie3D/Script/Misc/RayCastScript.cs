using UnityEngine;
using System.Collections;
using Zombie3D;
public class RayCastScript : MonoBehaviour
{

    public float life;

    // Use this for initialization
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        GameObject beginObj = GameObject.Find("Begin");
        GameObject endObj = GameObject.Find("End");
        Ray ray = new Ray(beginObj.transform.position, beginObj.transform.position - beginObj.transform.position);
        RaycastHit rayhit;
        if (Physics.Raycast(ray, out rayhit, 100, 1 << PhysicsLayer.WALL))
        {
            Debug.Log(beginObj.transform.position+","+endObj.transform.position);
        }




    }


    void OnDrawGizmos()
    {
        GameObject beginObj = GameObject.Find("Begin");
        GameObject endObj = GameObject.Find("End");

        Ray ray = new Ray(beginObj.transform.position, beginObj.transform.position -beginObj.transform.position);
        RaycastHit rayhit;
        if (Physics.Raycast(ray, out rayhit, 100, 1 << PhysicsLayer.WALL))
        {
            Gizmos.DrawLine(beginObj.transform.position, endObj.transform.position);
        }
        
    }
}
