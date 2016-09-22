using UnityEngine;
using System.Collections;
using Zombie3D;


public class FatZombieControllerHitScript : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.gameObject.layer == PhysicsLayer.SCENE_OBJECT)
        {

            Rigidbody body = hit.collider.attachedRigidbody;
            if (body == null)
            {
                return;
            }
            float pushPower = 20.0f;
            Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
            body.velocity = pushDir * pushPower;

        }


    }
}

