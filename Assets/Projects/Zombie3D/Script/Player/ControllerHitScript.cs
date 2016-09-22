using UnityEngine;
using System.Collections;
using Zombie3D;
public class ControllerHitScript : MonoBehaviour
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
        string name = hit.collider.name;
        if (name.StartsWith("E_"))
        {
            Enemy enemy = GameApp.GetInstance().GetGameScene().GetEnemyByID(name);
            if (enemy.EnemyType == EnemyType.E_ZOMBIE || enemy.EnemyType == EnemyType.E_NURSE)
            {
                Rigidbody body = hit.collider.attachedRigidbody;
                if (body == null || body.isKinematic)
                {
                    return;
                }
                float pushPower = 2.0f;
                Vector3 pushDir = new Vector3(hit.moveDirection.z, hit.moveDirection.y, hit.moveDirection.x);
                body.velocity = pushDir * pushPower;
            }


        }

    }
}
