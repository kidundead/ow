using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zombie3D;

public class ArenaTriggerScript : MonoBehaviour
{

    protected Player player;

    protected int waveNum;

    public EnemySpawnScript[] spawns;

    protected GameScene gameScene;

    protected float lastUpdateTime = -1000;

    protected int currentSpawnIndex = 0;

    protected float spawnSpeed;

    protected float timeBetweenWaves;

    protected float waveStartTime;
    protected float waveEndTime;

    // Use this for initialization
    IEnumerator Start()
    {

        yield return 0;

        waveNum = 1;
        spawnSpeed = 2 - waveNum * 0.1f;
        timeBetweenWaves = 1.0f;
        gameScene = GameApp.GetInstance().GetGameScene();
        //GameApp.GetInstance().GetGameScene().ArenaTrigger = this;

        Algorithem<EnemySpawnScript>.RandomSort(spawns);
        GameApp.GetInstance().GetGameScene().RefreshWoodBoxes();
        waveStartTime = Time.time;

        
    }

    public int WaveNum
    {
        get
        {
            return waveNum;
        }
            
    }

    // Update is called once per frame
    void Update()
    {

        if (Time.time - lastUpdateTime < spawnSpeed)
        {
            return;
        }
        lastUpdateTime = Time.time;

        

 
        if (currentSpawnIndex == spawns.Length)
        {
            //Debug.Log(GameApp.GetInstance().GetGameScene().GetEnemies().Count);
            int enemyLeft = GameApp.GetInstance().GetGameScene().GetEnemies().Count ;

            /*     Next Wave Condition:
             *     1. All enemies killed in the wave.
             *     2. Only less than 5 enemies left and you played too long (more than 2 minutes)
             * 
             *
             */
            

            if (enemyLeft == 0 || (Time.time - waveStartTime > 2*60 && enemyLeft < 5))
            {
                
                waveNum++;
                currentSpawnIndex = 0;
                float difficultyFactor = ((float)(waveNum-1)) / 10.0f;
                //GameApp.GetInstance().GetGameScene().UpdateDifficultyLevel(difficultyFactor);
                waveEndTime = Time.time;

                Algorithem<EnemySpawnScript>.RandomSort(spawns);
                GameApp.GetInstance().GetGameScene().RefreshWoodBoxes();
                spawnSpeed = 2 - waveNum * 0.1f;
                if (spawnSpeed < 0.5f)
                {
                    spawnSpeed = 0.5f;
                }
                Debug.Log("Wave " + waveNum);
            }

        }
        else
        {
            if (Time.time - waveEndTime > timeBetweenWaves)
            {
                if (currentSpawnIndex == 0)
                {
                    waveStartTime = Time.time;
                }

                EnemySpawnScript es = spawns[currentSpawnIndex];
                if ((waveNum % es.onlySpawnEvery) == 0 && (waveNum >= es.onlySpawnFromRound))
                {
                    es.Spawn(1);
                }

                currentSpawnIndex++;

            }
            

        }

        
        
    }



    void OnDrawGizmos()
    {

    }
}
