using UnityEngine;
using System.Collections;

public class LaserTrembleScript : MonoBehaviour {

    public float minScaleX = 0.01f;
    public float maxScaleX = 0.02f;
    public float scaleSpeed = 0.1f;
    protected bool increasing;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (increasing)
        {
            if (transform.localScale.x < maxScaleX)
            {
                transform.localScale += Vector3.right * Time.deltaTime * scaleSpeed;
            }
            else
            {
                increasing = false;
            }
        }
        else
        {
            if (transform.localScale.x > minScaleX)
            {
                transform.localScale -= Vector3.right * Time.deltaTime * scaleSpeed;
            }
            else
            {
                increasing = true;
            }

        }       

	}
}
