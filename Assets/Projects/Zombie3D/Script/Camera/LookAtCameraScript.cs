using UnityEngine;
using System.Collections;
using Zombie3D;

public class LookAtCameraScript : MonoBehaviour
{
    
    protected Transform cameraTransform;
    protected float lastUpdateTime;

    // Use this for initialization
    void Start()
    {
        if (Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Time.time - lastUpdateTime < 0.02f)
        {
            return;
        }
        lastUpdateTime = Time.time;

        transform.LookAt(cameraTransform);       //! 面向camera

    }
}
