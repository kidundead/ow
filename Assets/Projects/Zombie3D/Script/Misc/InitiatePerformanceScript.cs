using UnityEngine;
using System.Collections;
using Zombie3D;

public class InitiatePerformanceScript : MonoBehaviour {

    public ResourceConfigScript rConfig;
    protected Timer timer = new Timer();
	// Use this for initialization
	void Start () {
       rConfig = GameObject.Find("ResourceConfig").GetComponent<ResourceConfigScript>();
       timer.SetTimer(1f, false);
	}
	
	// Update is called once per frame
	void Update () {

        if (timer.Ready())
        {
            for (int i = 0; i < 10; i++)
            {
                GameObject enemyDeadBodyPrefab = rConfig.deadbody[0];
                GameObject enemyDeadFloorBloodPrefab = rConfig.deadFoorblood;
                GameObject obj = GameObject.Instantiate(enemyDeadBodyPrefab, transform.position + new Vector3(0, 0.2f, 0), Quaternion.identity) as GameObject;
                //GameObject obj = GameObject.Instantiate(enemyDeadFloorBloodPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.Euler(270, 0, 0)) as GameObject;
 
                GameObject.Destroy(obj);
            }
            timer.Do();
        }


	}
}
