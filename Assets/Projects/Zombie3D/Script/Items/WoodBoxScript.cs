using UnityEngine;
using System.Collections;
using Zombie3D;
public enum ItemType
{
    Hp,
    Power,
    Gold,
    AssaultGun,
    ShotGun,
    RocketLauncer,
    LaserGun,
    Sniper,
    MachineGun,
    Saw,
    Random,
    RandomBullets

}


public class WoodBoxScript : MonoBehaviour
{

    public float hp = 10;
    protected ResourceConfigScript rConf;
    protected Transform boxTransform;
    protected float startTime;
    // Use this for initialization
    void Start()
    {
        boxTransform = gameObject.transform;
        GetComponent<Rigidbody>().useGravity = false;
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - startTime > 20)
        {
            GetComponent<Rigidbody>().useGravity = true;
        }

        if (transform.position.y < Constant.FLOORHEIGHT + 30)
        {
            GetComponent<Renderer>().enabled = true;
        }
        else
        {
            GetComponent<Renderer>().enabled = false;
        }
        //transform.Translate(Vector3.down*0.1f*Time.deltaTime);
    }

    public void OnHit(float damage)
    {
        rConf = GameApp.GetInstance().GetResourceConfig();
        hp -= damage;
        if (hp <= 0)
        {
            Destroy(gameObject);
            Object.Instantiate(rConf.woodExplode, transform.position, Quaternion.identity);
            SendMessage("OnLoot");

        }

    }

}
