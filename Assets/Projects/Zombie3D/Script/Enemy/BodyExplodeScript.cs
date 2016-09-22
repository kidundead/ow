using UnityEngine;
using System.Collections;

public class BodyExplodeScript : MonoBehaviour {

    protected Vector3[] dir = new Vector3[7];
    protected Transform[] trans;
	// Use this for initialization
	void Start () {

        trans = GetComponentsInChildren<Transform>();
        foreach (Transform t in trans)
        {
            Debug.Log(t.name);
        }

        for (int i = 0; i < 7; i++)
        {
            transform.rotation = Quaternion.AngleAxis(360f/7, Vector3.up) * transform.rotation;
            dir[i] = transform.forward;
            
        }
	}
    
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < 7; i++)
        {
            trans[i].Translate(dir[i]*Time.deltaTime, Space.World);
        }

	}
}
