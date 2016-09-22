using UnityEngine;
using System.Collections;
using Zombie3D;
public class ItemScript : MonoBehaviour
{

    public ItemType itemType;
    bool moveUp = false;
    public Vector3 rotationSpeed = new Vector3(0f,45f,0f);
    public bool enableUpandDown = true;
    protected float deltaTime = 0;
    public float moveSpeed = 0.2f;
    public float HighPos = 1.2f;
    public float LowPos = 1.0f;
    protected float floorY = Constant.FLOORHEIGHT;
    // Use this for initialization
    void Start()
    {
        Ray ray = new Ray(transform.position + Vector3.up * 1.0f, Vector3.down);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, 100, 1 << PhysicsLayer.FLOOR))
        {
            floorY = hit.point.y;
        }


    }

    // Update is called once per frame
    void Update()
    {
        deltaTime += Time.deltaTime;
        if (deltaTime < 0.03f)
        {
            return;
        }
        

        //item rotation
        transform.Rotate(rotationSpeed*deltaTime);


        if (enableUpandDown)
        {
            //item floating up and down animation
            if (!moveUp)
            {
                float nextY = Mathf.MoveTowards(transform.position.y, floorY + LowPos, moveSpeed * deltaTime);

                transform.position = new Vector3(transform.position.x, nextY, transform.position.z);

                if (nextY <= floorY + LowPos)
                {
                    moveUp = true;
                }

            }
            else
            {
                float nextY = Mathf.MoveTowards(transform.position.y, floorY + HighPos, moveSpeed * deltaTime);

                transform.position = new Vector3(transform.position.x, nextY, transform.position.z);

                if (nextY >= floorY + HighPos)
                {
                    moveUp = false;
                }

            }
        }
        deltaTime = 0.0f;
    }


    void OnTriggerEnter(Collider c)
    {
        if (c.GetComponent<Collider>().gameObject.layer == PhysicsLayer.PLAYER)
        {
            Player player = GameApp.GetInstance().GetGameScene().GetPlayer();
            player.OnPickUp(itemType);
            Destroy(gameObject);
        }

    }
}

