using UnityEngine;
using System.Collections;
using Zombie3D;
public class OneShotParticleScript : MonoBehaviour
{

    // Use this for initialization
    IEnumerator Start()
    {
        yield return new WaitForSeconds(GetComponent<ParticleEmitter>().minEnergy / 2);
        GetComponent<ParticleEmitter>().emit = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
