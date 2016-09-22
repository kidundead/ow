using UnityEngine;
using System.Collections;
using Zombie3D;
//Game Config Class for prefab config
[AddComponentMenu("TPS/PrefabObjectManager")]
public class ResourceConfigScript : MonoBehaviour
{

    public GameObject human;
    public GameObject plumbers;
    public GameObject marine;
    public GameObject nerd;
    public GameObject doctor;
    public GameObject cowboy;
    public GameObject swat;
    public GameObject enegyArmor;

    
    




    public GameObject mapData;
    public Texture reticle;

    public GameObject hitBlood;
    public GameObject deadBlood;
    public GameObject deadFoorblood;
    public GameObject deadFoorblood2;

    public GameObject hitparticles;
    public GameObject projectile;
    public GameObject rocketExlposion;
    public GameObject boomerExplosion;
    public GameObject shotgunfire;
    public GameObject rpgFloor;
    public GameObject laser;
    public GameObject laserHit;
    public GameObject fireline;
    public GameObject bullets;
    public GameObject shotgunBullet;
    public GameObject nurseSaliva;
    public GameObject nurseSalivaProjectile;
    public GameObject salivaExplosion;
    public GameObject gunfire;
    public GameObject woodExplode;
    public GameObject copBomb;
    public GameObject halo;
    public GameObject graveRock;

    public GameObject m4;
    public GameObject winchester1200;
    public GameObject ak47;
    public GameObject mp5;
    public GameObject aug;
    public GameObject p90;
    public GameObject gatlin;
    public GameObject remington870;
    public GameObject xm1014;
    public GameObject sniper;
    public GameObject lasergun;
    public GameObject saw;
    public GameObject rpgGun;

    /*
    public GameObject m4_model;
    public GameObject winchester1200_model;
    public GameObject ak47_model;
    public GameObject mp5_model;
    public GameObject aug_model;
    public GameObject p90_model;
    public GameObject gatlin_model;
    public GameObject remington870_model;
    public GameObject xm1014_model;
    public GameObject sniper_model;
    public GameObject lasergun_model;
    public GameObject saw_model;
    public GameObject rpgGun_model;

    
    public GameObject m4_model_mask;
    public GameObject winchester1200_model_mask;
    public GameObject ak47_model_mask;
    public GameObject mp5_model_mask;
    public GameObject aug_model_mask;
    public GameObject p90_model_mask;
    public GameObject gatlin_model_mask;
    public GameObject remington870_model_mask;
    public GameObject xm1014_model_mask;
    public GameObject sniper_model_mask;
    public GameObject lasergun_model_mask;
    public GameObject saw_model_mask;
    public GameObject rpgGun_model_mask;
    */

    public GameObject itemHP;
    public GameObject itemPower;
    public GameObject itemGold;
    public GameObject itemAssaultGun;
    public GameObject itemShotGun;
    public GameObject itemRocketLauncer;
    public GameObject itemGatlin;
    public GameObject itemLaser;
    public GameObject itemMissle;

    public GameObject powerLogo;

    public GameObject woodBoxes;

    public TextAsset configXml;

    public TextAsset shareHtm;

    public AudioClip menuAudio;



    public GameObject[] levels = new GameObject[10];

    public GameObject[] enemy = new GameObject[10];
    public GameObject[] enemy_elite = new GameObject[10];
    public GameObject[] deadbody = new GameObject[10];

    public GameObject[] deadhead = new GameObject[10];

    public GameObject[] items = new GameObject[10];





    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
