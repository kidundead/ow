using UnityEngine;
using System.Collections;

public class MoveAroundScript : MonoBehaviour {

    public Transform pointA;
    public Transform pointB;
    public Transform target;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if ((transform.position - pointA.position).sqrMagnitude < 1.0)
        {
            target = pointB;
        }
        else if ((transform.position - pointB.position).sqrMagnitude < 1.0)
        {
            target = pointA;
        }


        Vector3 dir = (target.position - transform.position).normalized;
        transform.Translate(dir*Time.deltaTime *10.0f) ;
        
    
	}
}
