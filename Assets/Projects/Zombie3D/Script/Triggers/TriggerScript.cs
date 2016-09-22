using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zombie3D;

public class TriggerScript : MonoBehaviour
{

    protected Transform triggerTransform;
    protected Player player;
    protected bool triggered;
    protected bool alreadyMaxSpawned;
    protected bool hasSecondarySpawns;

    public Transform SecondPosition;
    public int minEnemy;
    public int maxSpawn;
    protected int currentEnemyNum = 0;
    protected int spawnedNum = 0;
    public float radius;
    public EnemySpawnScript[] spawns = new EnemySpawnScript[5];
    public EnemySpawnScript[] secondarySpawns = new EnemySpawnScript[5];
    protected GameScene gameScene;

    protected float lastUpdateTime = -1000;


    // Use this for initialization
    IEnumerator Start()
    {

        yield return 0;

        triggerTransform = gameObject.transform;
        triggered = false;
        foreach (EnemySpawnScript es in spawns)
        {
            if (es != null)
            {
                es.TriggerBelongsto = this;
            }
        }

        hasSecondarySpawns = false;
        foreach (EnemySpawnScript es in secondarySpawns)
        {
            if (es != null)
            {
                es.TriggerBelongsto = this;
                hasSecondarySpawns = true;
            }
        }
        alreadyMaxSpawned = false;
        GameApp.GetInstance().GetGameScene().AddTrigger(this);
        gameScene = GameApp.GetInstance().GetGameScene();

    }

    public bool AlreadyMaxSpawned
    {
        get
        {
            return alreadyMaxSpawned;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Time.time - lastUpdateTime < 1 || triggerTransform == null)
        {

            return;
        }

        lastUpdateTime = Time.time;

        if (!triggered)
        {
            player = gameScene.GetPlayer();
            bool secondTrigged = false;
            if (SecondPosition != null)
            {
                if ((player.GetTransform().position - SecondPosition.position).sqrMagnitude <= radius * radius)
                {
                    secondTrigged = true;
                }

            }

            if (((player.GetTransform().position - triggerTransform.position).sqrMagnitude <= radius * radius) || secondTrigged)
            {

                foreach (EnemySpawnScript es in spawns)
                {
                    if (es != null)
                    {
                        es.Spawn(1);
                        currentEnemyNum++;
                        spawnedNum++;
                    }
                }

                triggered = true;

            }

        }
        else
        {
            if (spawnedNum < maxSpawn && hasSecondarySpawns)
            {

                if (currentEnemyNum <= minEnemy)
                {
                    foreach (EnemySpawnScript es in secondarySpawns)
                    {
                        if (es != null)
                        {
                            es.Spawn(1);
                            currentEnemyNum++;
                            spawnedNum++;
                        }
                    }
                }



            }
            else
            {
                alreadyMaxSpawned = true;
            }

        }


    }

    public void EnemyKilled()
    {
        currentEnemyNum--;
    }



    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.3f);

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, this.radius);
        if (SecondPosition != null)
        {
            Gizmos.DrawWireSphere(SecondPosition.position, this.radius);
        }




        
        if (gameScene != null)
        {
            if (gameScene.GetEnemies() != null)
            {
                foreach (Enemy enemy in gameScene.GetEnemies().Values)
                {

                    if (enemy.LastTarget != Vector3.zero)
                    {
                        Gizmos.color = Color.blue;
                        Gizmos.DrawSphere(enemy.LastTarget, 0.3f);
                        //if (enemy.ray != null)
                        {
                            Gizmos.DrawLine(enemy.ray.origin, enemy.rayhit.point);
                            //Debug.Log(enemy.ray.origin + "," + enemy.rayhit.point);
                        }
                    }
                }
            }


        }

        

    }
}
