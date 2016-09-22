using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zombie3D;



public enum SpawnFromType
{
    Grave,
    Door
}

public class SpawnConfig
{
    public List<Wave> Waves { get; set; }
}

public class Wave
{
    public List<Round> Rounds { get; set; }
    public float intermission { get; set; }
}

public class Round
{
    public List<EnemyInfo> EnemyInfos { get; set; }
    public float intermission { get; set; }
}

public class EnemyInfo
{
    public EnemyType EType { get; set; }
    public int Count { get; set; }
    public SpawnFromType From { get; set; }
}

public class ArenaTriggerFromConfigScript : MonoBehaviour
{

    protected Player player;

    protected int waveNum;

    protected GameParametersXML paramXML;

    protected GameScene gameScene;
    protected GameObject[] doors;

    protected float lastUpdateTime = -1000;

    protected int currentSpawnIndex = 0;

    protected float spawnSpeed;

    protected float timeBetweenWaves;

    protected float waveStartTime;
    protected float waveEndTime;

    protected int currentDoorIndex = 0;
    protected int doorCount = 0;
    protected bool levelClear = false;

    private SpawnConfig spawnConfigInfo;
    //public GUISkin guiSkin;


    // Use this for initialization
    IEnumerator Start()
    {

        yield return 0;

        spawnSpeed = 2 - waveNum * 0.1f;
        timeBetweenWaves = 1.0f;
        gameScene = GameApp.GetInstance().GetGameScene();

        GameApp.GetInstance().GetGameScene().ArenaTrigger = this;
        doors = GameObject.FindGameObjectsWithTag("Door");
        doorCount = doors.Length;

        waveStartTime = Time.time;
        player = GameApp.GetInstance().GetGameScene().GetPlayer();

        paramXML = new GameParametersXML();


        
        if (Application.isEditor || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            spawnConfigInfo = paramXML.Load(null, GameApp.GetInstance().GetGameState().LevelNum);
        }
        else
        {

            spawnConfigInfo = paramXML.Load("/", GameApp.GetInstance().GetGameState().LevelNum);
        }

        int limit = GameApp.GetInstance().GetGameConfig().globalConf.enemyLimit;

        waveNum = 0;
        int startWaveNum = GameApp.GetInstance().GetGameState().LevelNum;
        int roundNum = 0;

        gameScene.ArenaTrigger = this;
        Debug.Log("wave num:" + spawnConfigInfo.Waves.Count);
        foreach (Wave wave in spawnConfigInfo.Waves)
        {

            int enemyLeft = GameApp.GetInstance().GetGameScene().GetEnemies().Count;
            while (enemyLeft > 0 && Time.time - waveStartTime < Constant.MAX_WAVE_TIME)
            {
                yield return 0;
                enemyLeft = GameApp.GetInstance().GetGameScene().GetEnemies().Count;
            }

            waveNum++;

            waveStartTime = Time.time;
            gameScene.CalculateDifficultyFactor(GameApp.GetInstance().GetGameState().LevelNum);
            Debug.Log("Wave " + waveNum);
            GameApp.GetInstance().GetGameScene().BonusWeapon = null;
            GameApp.GetInstance().DebugInfo = "";
            //GameApp.GetInstance().GetGameScene().RefreshWoodBoxes();
            yield return new WaitForSeconds(wave.intermission);
            foreach (Round round in wave.Rounds)
            {
                roundNum++;
                Debug.Log("Round " + roundNum);
                yield return new WaitForSeconds(round.intermission);
                foreach (EnemyInfo enemyInfo in round.EnemyInfos)
                {
                    EnemyType enemyType = enemyInfo.EType;
                    //Debug.Log("Spawn " + enemyInfo.Count + " " + enemyType + " from " + enemyInfo.From);

                    int spawnNum = enemyInfo.Count;
                    SpawnFromType from = enemyInfo.From;

                    GameObject enemyPrefab = null;// GameApp.GetInstance().GetResourceConfig().enemy[(int)enemyType];
                    Transform grave = null;
                    if (from == SpawnFromType.Grave)
                    {
                        grave = CalculateGravePosition(player.GetTransform());
                    }

                    for (int i = 0; i < spawnNum; i++)
                    {

                        bool bElite = false;
                        bElite = EliteSpawn(enemyType, spawnNum, i);

                        if (bElite)
                        {
                            enemyPrefab = GameApp.GetInstance().GetResourceConfig().enemy_elite[(int)enemyType];
                        }
                        else
                        {
                            enemyPrefab = GameApp.GetInstance().GetResourceConfig().enemy[(int)enemyType];
                        }
                        enemyLeft = GameApp.GetInstance().GetGameScene().GetEnemies().Count;
                        while (enemyLeft >= limit)
                        {
                            yield return 0;
                            enemyLeft = GameApp.GetInstance().GetGameScene().GetEnemies().Count;

                        }


                        Vector3 spawnPosition = Vector3.zero;
                        if (from == SpawnFromType.Door)
                        {
                            spawnPosition = doors[currentDoorIndex].transform.position;
                            currentDoorIndex++;
                            if (currentDoorIndex == doorCount)
                            {
                                currentDoorIndex = 0;
                            }
                        }
                        else if (from == SpawnFromType.Grave)
                        {

                            float rndX = Random.Range(-grave.localScale.x / 2, grave.localScale.x / 2);
                            float rndZ = Random.Range(-grave.localScale.z / 2, grave.localScale.z / 2);
                            spawnPosition = grave.position + new Vector3(rndX, 0f, rndZ);

                            GameObject.Instantiate(GameApp.GetInstance().GetResourceConfig().graveRock, spawnPosition + Vector3.down * 0.3f, Quaternion.identity);
                        }

                        GameObject currentEnemy = null;
                        if (bElite)
                        {
                            currentEnemy = (GameObject)Instantiate(enemyPrefab, spawnPosition, Quaternion.Euler(0, 0, 0));
                       
                        }
                        else
                        {
                            currentEnemy = gameScene.GetEnemyPool(enemyType).CreateObject(spawnPosition, Quaternion.Euler(0, 0, 0));
                            currentEnemy.layer = PhysicsLayer.ENEMY;
                        
                        }
                        
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
                                currentEnemy.transform.Translate(Vector3.up * 2);
                                break;
                            case 3:
                                enemy = new Hunter();
                                break;
                            case 4:
                                enemy = new Boomer();
                                break;
                            case 5:
                                enemy = new Swat();
                                break;
                        }
                        enemy.IsElite = bElite;
                        enemy.Init(currentEnemy);
                        enemy.EnemyType = enemyType;
                        //enemy.Spawn = this;
                        enemy.Name = currentEnemy.name;

                        if (from == SpawnFromType.Grave)
                        {
                            enemy.SetInGrave(true);
                        }
                        GameApp.GetInstance().GetGameScene().GetEnemies().Add(currentEnemy.name, enemy);
                        yield return new WaitForSeconds(0.3f);
                    }
                }

            }


        }


        int enemyCount = GameApp.GetInstance().GetGameScene().GetEnemies().Count;
        while (enemyCount > 0)
        {

            enemyCount = GameApp.GetInstance().GetGameScene().GetEnemies().Count;
            yield return 0;


        }

        //Scene Ends





        GameApp.GetInstance().GetGameScene().PlayingState = PlayingState.GameWin;
        List<Weapon> weaponList = GameApp.GetInstance().GetGameState().GetWeapons();
        GameConfig gConfig = GameApp.GetInstance().GetGameConfig();
        WeaponConfig wConf = gConfig.GetUnLockWeapon(GameApp.GetInstance().GetGameState().LevelNum);
        if (wConf != null)
        {
            foreach (Weapon w in weaponList)
            {
                //No duplicated weapons
                if (w.Name == wConf.name)
                {
                    if (w.Exist == WeaponExistState.Locked)
                    {
                        w.Exist = WeaponExistState.Unlocked;
                        GameApp.GetInstance().GetGameScene().BonusWeapon = w;
                        break;
                    }
                }
            }
        }
        else
        {
            GameApp.GetInstance().GetGameScene().BonusWeapon = null;
        }



        GameApp.GetInstance().GetGameState().Achievement.CheckAchievemnet_Survivior(GameApp.GetInstance().GetGameState().LevelNum);
        GameApp.GetInstance().GetGameState().Achievement.CheckAchievemnet_LastSurvivior(GameApp.GetInstance().GetGameState().LevelNum);

        GameApp.GetInstance().GetGameState().Achievement.SubmitScore(GameApp.GetInstance().GetGameState().LevelNum);

        GameApp.GetInstance().GetGameState().LevelNum++;

        /*
        AvatarType aType = (AvatarType)Mathf.Clamp((GameApp.GetInstance().GetGameState().LevelNum - 1)/5, 0, GameApp.GetInstance().GetGameState().GetAvatarNum() - 1);

        if (GameApp.GetInstance().GetGameState().GetAvatarData(aType) == AvatarState.NotAvaliable)
        {
            GameApp.GetInstance().GetGameState().EnableAvatar(aType);
        }
        */


        //GameApp.GetInstance().GetGameState().AddScore(1000);
        
        GameApp.GetInstance().Save();

        yield return new WaitForSeconds(4.0f);
        if (gameScene.BonusWeapon != null)
        {
            GameUIScript.GetGameUIScript().ClearLevelInfo();
            GameUIScript.GetGameUIScript().GetPanel(GameUIName.NEW_ITEM).Show();
            yield return new WaitForSeconds(4.0f);
        }

        FadeAnimationScript.GetInstance().StartFade(new Color(0, 0, 0, 0.0f), new Color(0, 0, 0, 1), 1.0f);
        yield return new WaitForSeconds(1.5f);
        UIResourceMgr.GetInstance().UnloadAllUIMaterials();
        Application.LoadLevel(SceneName.MAP);


    }

    public bool EliteSpawn(EnemyType eType, int spawnNum, int index)
    {
        bool bElite = false;
        switch (eType)
        {
            case EnemyType.E_ZOMBIE:
                if (spawnNum < 5)
                {
                    bElite = Math.RandomRate(5);
                }
                else
                {
                    if (index % 5 == 4)
                    {
                        bElite = true;
                    }
                    else
                    {
                        bElite = false;
                    }
                }
                break;
            case EnemyType.E_NURSE:
                if (spawnNum < 4)
                {
                    bElite = Math.RandomRate(10);
                }
                else
                {
                    if (index % 4 == 3)
                    {
                        bElite = true;
                    }
                    else
                    {
                        bElite = false;
                    }

                }

                break;
          
            case EnemyType.E_BOOMER:
                bElite = Math.RandomRate(10);
                break;
            case EnemyType.E_SWAT:
                bElite = Math.RandomRate(15);
                break;
            case EnemyType.E_HUNTER:
                if (GameApp.GetInstance().GetGameState().LevelNum > 15)
                bElite = Math.RandomRate(30);
                break;
            case EnemyType.E_TANK:
                if(GameApp.GetInstance().GetGameState().LevelNum>20)
                bElite = Math.RandomRate(50);
                break;

        }

        return bElite;
    }


    public Transform CalculateGravePosition(Transform playerTrans)
    {


        Transform gravePos = playerTrans;
        GameObject[] graves = GameObject.FindGameObjectsWithTag(TagName.GRAVE);
        GameObject nearestGrave = null;
        float minDisSqr = 99999.0f;
        foreach (GameObject g in graves)
        {

            float disSqr = (playerTrans.position - g.transform.position).sqrMagnitude;
            if (disSqr < minDisSqr)
            {
                nearestGrave = g;
                minDisSqr = disSqr;
            }
        }

        return nearestGrave.transform;




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


        /*
        if (currentSpawnIndex == spawns.Length)
        {
            //Debug.Log(GameApp.GetInstance().GetGameScene().GetEnemies().Count);
            int enemyLeft = GameApp.GetInstance().GetGameScene().GetEnemies().Count ;


            if (enemyLeft == 0 || (Time.time - waveStartTime > 2*60 && enemyLeft < 5))
            {
                
                waveNum++;
                currentSpawnIndex = 0;
                float difficultyFactor = ((float)(waveNum-1)) / 10.0f;
                GameApp.GetInstance().GetGameScene().UpdateDifficultyLevel(difficultyFactor);
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
        */


    }



    void OnDrawGizmos()
    {
        if (gameScene != null)
        {
            Vector3[] path = gameScene.GetPath();
            if (path != null && path.Length > 0)
            {
                Vector3 lastV = path[path.Length - 1];
                foreach (Vector3 v in path)
                {
                    Gizmos.color = Color.white;
                    Gizmos.DrawSphere(v, 0.1f);
                    Gizmos.DrawLine(lastV, v);
                    lastV = v;
                }
            }
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
