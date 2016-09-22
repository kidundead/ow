using UnityEngine;
using System.Collections;
using Zombie3D;

public class ClipNearScript : MonoBehaviour {

    Transform selfTrans;
    Transform cameraTrans;
    bool init = false;
	// Use this for initialization

	IEnumerator Start () {
        yield return 0;
        selfTrans = transform;
        cameraTrans = GameApp.GetInstance().GetGameScene().GetCamera().transform;
        init = true;
	}
	
	// Update is called once per frame
	void Update () {

        if (!init)
        {
            return;
        }

        if ((selfTrans.position - cameraTrans.position).sqrMagnitude < 5.0f * 5.0f)
        {
            GetComponent<Renderer>().enabled = false;
        }
        else
        {
            GetComponent<Renderer>().enabled = true;
        }

	}
}
