using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zombie3D;
class LootManagerScript : MonoBehaviour
{

    public float dropRate = 1.0f;

    public ItemType[] itemTables = new ItemType[10];
    public float[] rateTables = new float[10];

    void Start()
    {
    }

    void Update()
    {

    }

    WeaponConfig BonusEquipment(int wave)
    {

        WeaponConfig wConf = null;
        GameScene gameScene = GameApp.GetInstance().GetGameScene();

        if (gameScene.BonusWeapon == null)
        {
            GameConfig gConfig = GameApp.GetInstance().GetGameConfig();
            List<WeaponConfig> wConfList = gConfig.GetPossibleLootWeapons(wave);
            int count = wConfList.Count;
            float[] bonusEquipRateTable = new float[count];
            for (int i = 0; i < count; i++)
            {
                float increaseRate = wConfList[i].lootConf.increaseRate * (wave - wConfList[i].lootConf.fromWave);

                bonusEquipRateTable[i] = wConfList[i].lootConf.rate + increaseRate;

                if (GameApp.GetInstance().GetGameState().Avatar == AvatarType.Nerd)
                {
                    bonusEquipRateTable[i] *= Constant.NERD_MORE_LOOT_RATE;
                }
            }


            float rnd = Random.value;
            float totalRnd = 0;
            for (int i = 0; i < bonusEquipRateTable.Length; i++)
            {
                if (bonusEquipRateTable[i] > 0 && rnd <= totalRnd + bonusEquipRateTable[i])
                {
                    wConf = wConfList[i];

                    break;

                }
                totalRnd += bonusEquipRateTable[i];
            }

        }

        return wConf;

    }


    void SpawnItem(ItemType itemType)
    {
        /*
        GameObject itemObj = Object.Instantiate(GameApp.GetInstance().GetGameConfig().items[(int)itemID], transform.position, Quaternion.identity) as GameObject;
        Item item = new Item();
        item.ItemObject = itemObj;
        item.ID = itemID;
        item.Init();
        GameApp.GetInstance().GetItems().Add(item);
          */

        ResourceConfigScript rConf = GameApp.GetInstance().GetResourceConfig();

        GameObject item = null;

        Ray ray = new Ray(transform.position + Vector3.up * 1.0f, Vector3.down);
        RaycastHit hit;
        float floorY = Constant.FLOORHEIGHT;
        if (Physics.Raycast(ray, out hit, 100, 1 << PhysicsLayer.FLOOR))
        {
            floorY = hit.point.y;
        }

        
        switch (itemType)
        {
            case ItemType.Hp:
                item = Object.Instantiate(rConf.itemHP, new Vector3(transform.position.x, floorY+Constant.ITEM_LOWPOINT, transform.position.z), Quaternion.identity) as GameObject;
                item.GetComponent<ItemScript>().itemType = itemType;
                break;
            case ItemType.Power:
                item = Object.Instantiate(rConf.itemPower, new Vector3(transform.position.x, floorY+Constant.ITEM_LOWPOINT, transform.position.z), Quaternion.identity) as GameObject;
                item.GetComponent<ItemScript>().itemType = itemType;
                break;
            case ItemType.Gold:
                item = Object.Instantiate(rConf.itemGold, new Vector3(transform.position.x, floorY + Constant.ITEM_LOWPOINT, transform.position.z), Quaternion.identity) as GameObject;
                item.GetComponent<ItemScript>().itemType = itemType;
                break;
            case ItemType.RandomBullets:
                WeaponType weaponType = GameApp.GetInstance().GetGameState().RandomBattleWeapons();

                switch (weaponType)
                {
                    case WeaponType.AssaultRifle:
                        item = Object.Instantiate(rConf.itemAssaultGun, new Vector3(transform.position.x, floorY + Constant.ITEM_LOWPOINT, transform.position.z), Quaternion.identity) as GameObject;
                        ItemScript its = item.AddComponent<ItemScript>();
                        item.AddComponent<SphereCollider>().isTrigger = true;
                        item.layer = PhysicsLayer.ITEMS;
                        item.GetComponent<ItemScript>().itemType = ItemType.AssaultGun;
                        break;
                    case WeaponType.ShotGun:
                        item = Object.Instantiate(rConf.itemShotGun, new Vector3(transform.position.x, floorY + Constant.ITEM_LOWPOINT, transform.position.z), Quaternion.identity) as GameObject;
                        item.AddComponent<ItemScript>();
                        item.AddComponent<SphereCollider>().isTrigger = true;
                        item.layer = PhysicsLayer.ITEMS;
                        item.GetComponent<ItemScript>().itemType = ItemType.ShotGun;
                        break;
                    case WeaponType.RocketLauncher:
                        item = Object.Instantiate(rConf.itemRocketLauncer, new Vector3(transform.position.x, floorY + Constant.ITEM_LOWPOINT, transform.position.z), Quaternion.identity) as GameObject;
                        item.AddComponent<ItemScript>();
                        item.AddComponent<SphereCollider>().isTrigger = true;
                        item.layer = PhysicsLayer.ITEMS;
                        item.GetComponent<ItemScript>().itemType = ItemType.RocketLauncer;
                        break;
                    case WeaponType.LaserGun:
                        item = Object.Instantiate(rConf.itemLaser, new Vector3(transform.position.x, floorY + Constant.ITEM_LOWPOINT, transform.position.z), Quaternion.identity) as GameObject;
                        item.AddComponent<ItemScript>();
                        item.AddComponent<SphereCollider>().isTrigger = true;

                        item.layer = PhysicsLayer.ITEMS;
                        item.GetComponent<ItemScript>().itemType = ItemType.LaserGun;
                        break;
                    case WeaponType.Saw:
                        item = Object.Instantiate(rConf.saw, new Vector3(transform.position.x, floorY + Constant.ITEM_LOWPOINT, transform.position.z), Quaternion.identity) as GameObject;
                        item.AddComponent<ItemScript>();
                        item.AddComponent<SphereCollider>().isTrigger = true;
                        item.layer = PhysicsLayer.ITEMS;
                        item.GetComponent<ItemScript>().itemType = ItemType.Saw;
                        break;
                    case WeaponType.Sniper:
                        item = Object.Instantiate(rConf.itemMissle, new Vector3(transform.position.x, floorY + Constant.ITEM_LOWPOINT, transform.position.z), Quaternion.identity) as GameObject;
                        item.AddComponent<ItemScript>();
                        item.AddComponent<SphereCollider>().isTrigger = true;
                        item.layer = PhysicsLayer.ITEMS;
                        item.GetComponent<ItemScript>().itemType = ItemType.Sniper;
                        break;
                    case WeaponType.MachineGun:
                        item = Object.Instantiate(rConf.itemGatlin, new Vector3(transform.position.x, floorY + Constant.ITEM_LOWPOINT, transform.position.z), Quaternion.identity) as GameObject;
                        item.AddComponent<ItemScript>();
                        item.AddComponent<SphereCollider>().isTrigger = true;
                        item.layer = PhysicsLayer.ITEMS;
                        item.GetComponent<ItemScript>().itemType = ItemType.MachineGun;
                        break;
                }
                break;
        }
        /*
        GameObject halo = Object.Instantiate(rConf.halo, item.transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
        halo.AddComponent<LookAtCameraScript>();
        halo.transform.localScale = Vector3.one * 1.2f;
        halo.transform.parent = item.transform;
        */



    }

    public void OnLoot()
    {
        float rnd = Random.value;
        float drop = dropRate;
        if (GameApp.GetInstance().GetGameState().Avatar == AvatarType.Nerd)
        {
            drop *= Constant.NERD_MORE_LOOT_RATE;
        }
        //Debug.Log("OnLoot.." + rnd);
        if (rnd < drop)
        {
            rnd = Random.value;
            //Debug.Log("OnLoot..DropRate" + rnd);
            float totalRnd = 0;
            for (int i = 0; i < itemTables.Length; i++)
            {
                if (rateTables[i] > 0 && rnd <= totalRnd + rateTables[i])
                {
                    //Debug.Log("Spawn " + itemTables[i]);
                    SpawnItem(itemTables[i]);
                    return;

                }
                totalRnd += rateTables[i];
            }



        }

        /*
        ArenaTriggerFromConfigScript stfcs = GameApp.GetInstance().GetGameScene().ArenaTrigger;
        if (stfcs != null)
        {
            int wave = stfcs.WaveNum;
            WeaponConfig wConf = BonusEquipment(wave);
            List<Weapon> weaponList = GameApp.GetInstance().GetGameState().GetWeapons();
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
                            Debug.Log("Bonus Weapon:" + wConf.name);
                            //GameApp.GetInstance().GetGameScene().BonusWeapon = WeaponFactory.GetInstance().CreateWeapon(wConf.wType);
                            GameApp.GetInstance().GetGameScene().BonusWeapon = w;
                            //GameApp.GetInstance().GetGameScene().BonusWeapon.Name = wConf.name;
                            //GameApp.GetInstance().GetGameScene().BonusWeapon.WConf = wConf;
                            //GameApp.GetInstance().GetGameScene().BonusWeapon.LoadConfig();
                            //GameApp.GetInstance().GetGameState().GetWeapons().Add(GameApp.GetInstance().GetGameScene().BonusWeapon);
                            GameApp.GetInstance().DebugInfo = "YOU JUST GOT THE " + wConf.name + "!";
                            
                                

                        }
                        return;
                    }
                }

            }
        }
         */

    }
}
