using UnityEngine;
using System.Collections;
using Zombie3D;
public class PlayerSpawnScript : MonoBehaviour
{

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.3f);
    }

}

