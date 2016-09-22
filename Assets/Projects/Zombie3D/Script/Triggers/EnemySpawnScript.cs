using UnityEngine;
using System.Collections;
using Zombie3D;

public enum EnemyType
{
    E_ZOMBIE = 0,
    E_NURSE = 1,
    E_TANK = 2,
    E_HUNTER = 3,
    E_BOOMER = 4,
    E_SWAT = 5
}
public class EnemySpawnScript : MonoBehaviour
{

    public EnemyType enemyType;
    public int onlySpawnFromRound = 1;
    public int onlySpawnEvery = 1;
    public bool isKilled = false;

    protected float lastSpawnTime = 0;
    protected TriggerScript triggerBelongsto;
    protected static GameObject enemyFolder;
    
    protected bool disable = false;


    // Use this for initialization
    IEnumerator Start()
    {

        yield return 0;

        int infectionRate = GameApp.GetInstance().GetGameScene().GetInfectionRate();
        int infectionLevel = infectionRate / 25;
        int rnd = Random.Range(0, 100);
        if (rnd > 60 + infectionLevel * 15)
        {
            disable = true;
        }
        else
        {
            GameApp.GetInstance().GetGameScene().ModifyEnemyNum(1);
        }

    }



    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.3f);
    }


    public void Spawn(int spawnNum)
    {
        if (GameApp.GetInstance().GetGameScene() == null || disable)
        {
            return;
        }

        //Debug.Log("spawn "+enemyType);
        GameObject enemyPrefab = GameApp.GetInstance().GetResourceConfig().enemy[(int)enemyType];

        for (int i = 0; i < spawnNum; i++)
        {

            GameObject currentEnemy = (GameObject)Instantiate(enemyPrefab, transform.position, Quaternion.Euler(0, 0, 0));

            int enemyID = GameApp.GetInstance().GetGameScene().GetNextEnemyID();
            currentEnemy.name = ConstData.ENEMY_NAME + enemyID.ToString();

            Enemy enemy = null;

            switch ((int)enemyType)
            {
                case 0: enemy = new Zombie();
                    break;
                case 1: enemy = new Nurse();
                    break;
                case 2: enemy = new Tank();
                    break;
            }
            enemy.Init(currentEnemy);
            enemy.EnemyType = enemyType;
            enemy.Spawn = this;
            enemy.Name = currentEnemy.name;
            GameApp.GetInstance().GetGameScene().GetEnemies().Add(currentEnemy.name, enemy);

            // currentEnemy.transform.parent = enemyFolder.transform;
        }
        lastSpawnTime = Time.time;
    }

    void OnResetSpawnTrigger()
    {
        if (triggerBelongsto != null)
        {
            triggerBelongsto.EnemyKilled();
        }
        isKilled = true;
    }


    public TriggerScript TriggerBelongsto
    {
        set
        {
            triggerBelongsto = value;
        }
    }


}
