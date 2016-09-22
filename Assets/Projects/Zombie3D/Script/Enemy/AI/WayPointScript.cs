using UnityEngine;
using System.Collections;

public class WayPointScript : MonoBehaviour {


    
    public WayPointScript[] nodes;
    public WayPointScript parent;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	

	}

    void OnDrawGizmos()
    {
        if (transform.position.y < 10000+ 5)
        {
            Gizmos.color = Color.white;
        }
        else
        {
            Gizmos.color = Color.magenta;
        }
        Gizmos.DrawSphere(transform.position, 1.0f);

        foreach (WayPointScript w in nodes)
        {
            Gizmos.DrawLine(transform.position, w.transform.position);
        }
    }
}
