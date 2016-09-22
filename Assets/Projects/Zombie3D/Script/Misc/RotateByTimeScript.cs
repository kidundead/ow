using UnityEngine;
using System.Collections;
using Zombie3D;
public class RotateByTimeScript : MonoBehaviour
{

    public Vector3 rotateSpeed = new Vector3(0, 45, 0);
    protected float deltaTime = 0.0f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        deltaTime += Time.deltaTime;
        if (deltaTime < 0.03f)
        {
            return;
        }

        transform.Rotate(rotateSpeed * deltaTime,Space.Self);
        deltaTime = 0.0f;
    }

}

