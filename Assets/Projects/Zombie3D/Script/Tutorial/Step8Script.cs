using UnityEngine;
using System.Collections;
using Zombie3D;


public class Step8Script : MonoBehaviour, ITutorialStep, ITutorialUIEvent
{
    protected ITutorialGameUI guis;
    protected TutorialScript ts;
    protected Enemy enemy;
    
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartStep(TutorialScript ts, Player player)
    {
        this.ts = ts;
        player.InputController.EnableMoveInput = true;
        player.InputController.EnableTurningAround = true;
        player.InputController.EnableShootingInput = true;
        guis.EnableTutorialOKButton(false);
        GameObject currentEnemy = (GameObject)Instantiate(GameApp.GetInstance().GetResourceConfig().enemy[0], new Vector3(26.7f, 10000f, 0.17f), Quaternion.Euler(0, 0, 0));

        int enemyID = GameApp.GetInstance().GetGameScene().GetNextEnemyID();
        currentEnemy.name = ConstData.ENEMY_NAME + enemyID.ToString();

        Enemy enemy = new Zombie();
             
        enemy.Init(currentEnemy);
        enemy.EnemyType = EnemyType.E_ZOMBIE;
        enemy.Name = currentEnemy.name;

        GameApp.GetInstance().GetGameScene().GetEnemies().Add(currentEnemy.name, enemy);



    }

    public void UpdateTutorialStep(float deltaTime, Player player)
    {
        //guis.SetTutorialText("ALRIGHT, NOW THAT YOU'VE GOT DOWN THE BASICS, TRY SHOOTING THE ZOMBIE IN FRONT OF YOU.");

        if (GameApp.GetInstance().GetGameScene().GetEnemies().Count == 0)
        {
            ts.GoToNextStep();
        }

    }

    public void EndStep(Player player)
    {
    }

    public void SetGameGUI(ITutorialGameUI guis)
    {
        this.guis = guis;
    }

    public void OK(Player player)
    {
 


    }

}
