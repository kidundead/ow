using UnityEngine;
using System.Collections;

public class GraveScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, 1.0f);
    }
}
