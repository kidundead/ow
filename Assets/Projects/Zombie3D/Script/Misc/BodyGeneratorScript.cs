using UnityEngine;
using System.Collections;
using Zombie3D;

public class BodyGeneratorScript : MonoBehaviour
{

    public Timer timer = new Timer();
    public ResourceConfigScript rConfig;
    // Use this for initialization
    void Start()
    {

        rConfig = GameObject.Find("ResourceConfig").GetComponent<ResourceConfigScript>();
        
        timer.SetTimer(4.0f, false);

    }

    public void PlayDead()
    {
        PlayBloodEffect();
        PlayBodyExlodeEffect();


        GameObject enemyDeadHeadPrefab = rConfig.deadhead[0];
        GameObject deadhead = (GameObject)GameObject.Instantiate(enemyDeadHeadPrefab, transform.position + new Vector3(0, 2, 0), transform.rotation);

        deadhead.GetComponent<Rigidbody>().AddForce(Random.Range(-5, 5), Random.Range(-5, 0), Random.Range(-5, 5), ForceMode.Impulse);
    }

    public void PlayBodyExlodeEffect()
    {
        GameObject enemyDeadBodyPrefab = rConfig.deadbody[0];
        GameObject.Instantiate(enemyDeadBodyPrefab, transform.position + new Vector3(0, 0.2f, 0), Quaternion.identity);

    }

    public void PlayBloodEffect()
    {
        GameObject enemyDeadBloodPrefab = rConfig.deadBlood;
        GameObject enemyDeadFloorBloodPrefab;
        int r = Random.Range(0, 100);
        float fby = Constant.FLOORHEIGHT + 0.02f;
        if (r > 50)
        {
            enemyDeadFloorBloodPrefab = rConfig.deadFoorblood;
        }
        else
        {
            enemyDeadFloorBloodPrefab = rConfig.deadFoorblood2;
            fby = Constant.FLOORHEIGHT + 0.01f;
        }

        GameObject.Instantiate(enemyDeadBloodPrefab, transform.position + new Vector3(0, 0.5f, 0), Quaternion.Euler(0, 0, 0));
        GameObject.Instantiate(enemyDeadFloorBloodPrefab, new Vector3(transform.position.x, fby, transform.position.z), Quaternion.Euler(270, 0, 0));
    }


    // Update is called once per frame
    void Update()
    {
        if (timer.Ready())
        {
            PlayDead();
            timer.Do();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(transform.position, 1.0f);

    }
}
