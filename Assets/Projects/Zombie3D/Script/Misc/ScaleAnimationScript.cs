using UnityEngine;
using System.Collections;
using Zombie3D;
public class ScaleAnimationScript : MonoBehaviour
{
    public float destScaleMax = 0.4f;
    public float destScaleMin = 0.2f;
    protected float destScale;
    public float scaleSpeed = 1.0f;

   
    // Use this for initialization
    void Start()
    {
        destScale = Random.Range(destScaleMin, destScaleMax);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localScale.x < destScale)
        {
            transform.localScale += Vector3.one * Time.deltaTime * scaleSpeed;
        }
        
    }
}
