using UnityEngine;
using System.Collections;
using Zombie3D;

public class CopBombScript : MonoBehaviour {

    protected float startTime;
    public float explodeTime = 4.0f;
    public float radius = 5.0f;
    public float damage = 20.0f;
    //public float flySpeed;
    public Vector3 speed;
    
	// Use this for initialization
	void Start () {
        startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {

        //transform.Translate(speed * Time.deltaTime, Space.World);
        //proTransform.Translate(flySpeed * dir * deltaTime, Space.World);
        if (Time.time - startTime > explodeTime)
        {
             ResourceConfigScript rc = GameApp.GetInstance().GetResourceConfig();
             Player player  = GameApp.GetInstance().GetGameScene().GetPlayer();

             float distance = Mathf.Sqrt((transform.position - player.GetTransform().position).sqrMagnitude);

             if (distance < radius)
             {
                 Ray ray = new Ray(transform.position, player.GetTransform().position - (transform.position));
                 RaycastHit rayhit;
                 if (Physics.Raycast(ray, out rayhit, distance, 1 << PhysicsLayer.WALL | 1 << PhysicsLayer.TRANSPARENT_WALL | 1 << PhysicsLayer.PLAYER))
                 {
                     Debug.Log(rayhit.collider.gameObject.name);
                     if (rayhit.collider.gameObject.name == "Player")
                     {
                         player.OnHit(damage);

                     }
                 }
                 else
                 {
                     player.OnHit(damage);
                 }
                
             }

            GameObject.Instantiate(rc.rocketExlposion, transform.position, Quaternion.identity);

            GameObject.Destroy(gameObject);
        }

	}
}
