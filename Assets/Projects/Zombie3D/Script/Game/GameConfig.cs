using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using Zombie3D;

public class GlobalConfig
{
    public int startMoney;     //! 开始 金 钱
    public int enemyLimit;     //! 敌人上限
    public float resolution;   //! 
}

public class PlayerConfig
{
    public float hp;               //! hp
    public float walkSpeed;        //! 移动速度
    public int upgradeArmorPrice;  //! 升级护甲价格
    public float upPriceFactor;    //! 升级价格因子
    public int maxArmorLevel;      //! 最大护甲等级
}

//! 伤害值， 攻击速度 移动速度 hp 冲击间隔 冲击伤害
public class MonsterConfig
{
    public float damage;
    public float attackRate;
    public float walkSpeed;
    public float hp;

    public float rushInterval;
    public float rushDamage;
    public float rushSpeed;
    public float rushAttackDamage;

    public int score;
    public int lootCash;
}

public class LootConfig
{

    public int giveAtWave;
    public int fromWave;
    public int toWave;
    public float rate;
    public float increaseRate;
}

//! 人物 的价格
public class AvatarConfig
{
    public int price;
}

//! 升级参数 
public class UpgradeConfig
{
    public float baseData;
    public float upFactor;
    public float basePrice;
    public float upPriceFactor;
    public float maxLevel;

}

//! 武器参数配置
public class WeaponConfig
{
    public string name;
    public WeaponType wType;
    public float moveSpeedDrag;
    public float range;
    public int price;
    public int bulletPrice;
    public int initBullet;
    public int bullet;
    public float flySpeed;
    public WeaponExistState startEquip;
    public UpgradeConfig damageConf;
    public UpgradeConfig attackRateConf;
    public UpgradeConfig accuracyConf;
    public LootConfig lootConf;
}

public class EquipConfig
{
    public float effectValue;
    public LootConfig lootConf;
}
	
public class GameConfig
{
    public GlobalConfig globalConf;
    public PlayerConfig playerConf;
    public ArrayList avatarConfTable = new ArrayList();
    public Hashtable monsterConfTable = new Hashtable();
    public ArrayList weaponConfTable = new ArrayList();
    public Hashtable equipConfTable = new Hashtable();


    public MonsterConfig GetMonsterConfig(string name)
    {
        return monsterConfTable[name] as MonsterConfig;
    }

    public WeaponConfig GetWeaponConfig(string name)
    {
        foreach (WeaponConfig wconf in weaponConfTable)
        {
            if (wconf.name == name)
            {
                return wconf;
            }
        }
        return null;
    }

    public AvatarConfig GetAvatarConfig(int index)
    {
        return avatarConfTable[index - 1] as AvatarConfig;
    }

    public List<WeaponConfig> GetPossibleLootWeapons(int wave)
    {
        List<WeaponConfig> wConfList = new List<WeaponConfig>();
        foreach (WeaponConfig wConf in weaponConfTable)
        {
            LootConfig lConf = wConf.lootConf;
            if (wave >= lConf.fromWave && wave <= lConf.toWave)
            {
                wConfList.Add(wConf);
            }
        }

        return wConfList;

    }

    public WeaponConfig GetUnLockWeapon(int wave)
    {
      
        foreach (WeaponConfig wConf in weaponConfTable)
        {
            LootConfig lConf = wConf.lootConf;
            if (wave == lConf.giveAtWave)
            {
                return wConf;
            }
        }

        return null;

    }



    public List<WeaponConfig> GetWeapons()
    {
        List<WeaponConfig> wConfList = new List<WeaponConfig>();
        foreach (WeaponConfig wConf in weaponConfTable)
        {

            wConfList.Add(wConf);

        }
        return wConfList;
    }

    public void LoadFromXML(string path)
    {
        globalConf = new GlobalConfig();   //! 全局配置 
        playerConf = new PlayerConfig();   //! 玩家配置

        XmlReader reader = null;
        StringReader s = null;
        Stream stream = null;
        if (path != null)
        {
            path = Application.dataPath + path;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            stream = File.Open(path + "config.xml", FileMode.Open);
            reader = XmlReader.Create(stream);

        }
        else
        {
            TextAsset XMLFile = GameApp.GetInstance().GetResourceConfig().configXml;
            s = new StringReader(XMLFile.text);

            reader = XmlReader.Create(s);

        }

        WeaponConfig weaponConf = null;
        AvatarConfig avatarConf = null;
        while (reader.Read())
        {

            switch (reader.NodeType)
            {
                case XmlNodeType.Element:

                    if (reader.Name == "Global")
                    {
                        LoadGlobalConf(reader);
                    }
                    else if (reader.Name == "Player")
                    {
                        LoadPlayerConf(reader);
                    }
                    else if (reader.Name == "Avatar")
                    {
                        avatarConf = new AvatarConfig();
                        LoadAvatarConf(reader, avatarConf);
                        avatarConfTable.Add(avatarConf);
                    }
                    else if (reader.Name == "Monster")
                    {
                        LoadMonstersConf(reader);
                    }
                    else if (reader.Name == "Weapon")
                    {
                        weaponConf = new WeaponConfig();
                        LoadWeaponConf(reader, weaponConf);
                        weaponConfTable.Add(weaponConf);
                    }
                    else if (reader.Name == "Damage")
                    {
                        LoadUpgradeConf(reader, weaponConf, "Damage");
                    }
                    else if (reader.Name == "Frequency")
                    {
                        LoadUpgradeConf(reader, weaponConf, "Frequency");
                    }
                    else if (reader.Name == "Accuracy")
                    {
                        LoadUpgradeConf(reader, weaponConf, "Accuracy");
                    }
                    else if (reader.Name == "Loot")
                    {
                        LoadLootWeapon(reader, weaponConf);
                    }

                    break;
                case XmlNodeType.EndElement:
                    break;
            }
        }



        if (reader != null)
            reader.Close();
        if (s != null)
        {
            s.Close();
        }

        if (stream != null)
        {
            stream.Close();
        }

    }

    void LoadLootWeapon(XmlReader reader, WeaponConfig weaponConf)
    {

        LootConfig lootConf = new LootConfig();
        if (reader.HasAttributes)
        {
            while (reader.MoveToNextAttribute())
            {
                if (reader.Name == "giveAtWave")
                {
                    lootConf.giveAtWave = int.Parse(reader.Value);
                }
                else if (reader.Name == "fromWave")
                {
                    lootConf.fromWave = int.Parse(reader.Value);
                }
                else if (reader.Name == "toWave")
                {
                    lootConf.toWave = int.Parse(reader.Value);
                }
                else if (reader.Name == "lootRate")
                {
                    lootConf.rate = float.Parse(reader.Value);
                }
                else if (reader.Name == "increaseRate")
                {
                    lootConf.increaseRate = float.Parse(reader.Value);
                }
            }
        }
        weaponConf.lootConf = lootConf;
    }


    void LoadAvatarConf(XmlReader reader, AvatarConfig avatarConf)
    {
        if (reader.HasAttributes)
        {
            while (reader.MoveToNextAttribute())
            {
                if (reader.Name == "price")
                {
                    avatarConf.price = int.Parse(reader.Value);
                }
            }
        }

    }


    void LoadGlobalConf(XmlReader reader)
    {
        if (reader.HasAttributes)
        {
            while (reader.MoveToNextAttribute())
            {
                if (reader.Name == "startMoney")
                {
                    globalConf.startMoney = int.Parse(reader.Value);
                }
                else if (reader.Name == "enemyLimit")
                {
                    globalConf.enemyLimit = int.Parse(reader.Value);
                }
                else if (reader.Name == "resolution")
                {
                    globalConf.resolution = float.Parse(reader.Value);

                    //globalConf.resolution;
                }

            }
        }

    }


    void LoadPlayerConf(XmlReader reader)
    {

        if (reader.HasAttributes)
        {
            while (reader.MoveToNextAttribute())
            {
                if (reader.Name == "hp")
                {
                    playerConf.hp = float.Parse(reader.Value);
                }
                else if (reader.Name == "walkSpeed")
                {
                    playerConf.walkSpeed = float.Parse(reader.Value);
                }
                else if (reader.Name == "armorPrice")
                {
                    playerConf.upgradeArmorPrice = int.Parse(reader.Value);
                }
                else if (reader.Name == "upPriceFactor")
                {
                    playerConf.upPriceFactor = float.Parse(reader.Value);
                }
                else if (reader.Name == "maxArmorLevel")
                {
                    playerConf.maxArmorLevel = int.Parse(reader.Value);
                }
            }
        }

    }

    void LoadMonstersConf(XmlReader reader)
    {

        MonsterConfig mConf = new MonsterConfig();
        string name = "";

        if (reader.HasAttributes)
        {
            while (reader.MoveToNextAttribute())
            {
                if (reader.Name == "name")
                {
                    name = reader.Value;
                }
                else if (reader.Name == "damage")
                {
                    mConf.damage = float.Parse(reader.Value);
                }
                else if (reader.Name == "attackRate")
                {
                    mConf.attackRate = float.Parse(reader.Value);
                }
                else if (reader.Name == "walkSpeed")
                {
                    mConf.walkSpeed = float.Parse(reader.Value);
                }
                else if (reader.Name == "hp")
                {
                    mConf.hp = float.Parse(reader.Value);
                }
                else if (reader.Name == "rushSpeed")
                {
                    mConf.rushSpeed = float.Parse(reader.Value);
                }
                else if (reader.Name == "rushDamage")
                {
                    mConf.rushDamage = float.Parse(reader.Value);
                }
                else if (reader.Name == "rushAttack")
                {
                    mConf.rushAttackDamage = float.Parse(reader.Value);
                }
                else if (reader.Name == "rushRate")
                {
                    mConf.rushInterval = float.Parse(reader.Value);
                }
                else if (reader.Name == "score")
                {
                    mConf.score = int.Parse(reader.Value);
                }
                else if (reader.Name == "lootCash")
                {
                    mConf.lootCash = int.Parse(reader.Value);
                }
            }
        }

        monsterConfTable.Add(name, mConf);
    }

    void LoadWeaponConf(XmlReader reader, WeaponConfig weaponConf)
    {
        if (reader.HasAttributes)
        {
            while (reader.MoveToNextAttribute())
            {
                if (reader.Name == "name")
                {
                    weaponConf.name = reader.Value;
                }
                else if (reader.Name == "type")
                {
                    string stype = reader.Value;
                    switch (stype)
                    {
                        case "Rifle":
                            weaponConf.wType = WeaponType.AssaultRifle;
                            break;
                        case "ShotGun":
                            weaponConf.wType = WeaponType.ShotGun;
                            break;
                        case "RPG":
                            weaponConf.wType = WeaponType.RocketLauncher;
                            break;
                        case "MachineGun":
                            weaponConf.wType = WeaponType.MachineGun;
                            break;
                        case "Laser":
                            weaponConf.wType = WeaponType.LaserGun;
                            break;
                        case "Sniper":
                            weaponConf.wType = WeaponType.Sniper;
                            break;
                        case "Saw":
                            weaponConf.wType = WeaponType.Saw;
                            break;
                    }
                }
                else if (reader.Name == "moveSpeedDrag")
                {
                    weaponConf.moveSpeedDrag = float.Parse(reader.Value);
                }
                else if (reader.Name == "range")
                {
                    weaponConf.range = float.Parse(reader.Value);
                }
                else if (reader.Name == "price")
                {
                    weaponConf.price = int.Parse(reader.Value);
                }
                else if (reader.Name == "bulletPrice")
                {
                    weaponConf.bulletPrice = int.Parse(reader.Value);
                }
                else if (reader.Name == "initBullet")
                {
                    weaponConf.initBullet = int.Parse(reader.Value);
                }
                else if (reader.Name == "bullet")
                {
                    weaponConf.bullet = int.Parse(reader.Value);
                }
                else if (reader.Name == "flySpeed")
                {
                    weaponConf.flySpeed = float.Parse(reader.Value);
                }
                else if (reader.Name == "startEquip")
                {
                    weaponConf.startEquip = (WeaponExistState)int.Parse(reader.Value);
                }
            }
        }

    }

    void LoadUpgradeConf(XmlReader reader, WeaponConfig weaponConf, string uType)
    {
        UpgradeConfig uConf = new UpgradeConfig();

        if (reader.HasAttributes)
        {
            while (reader.MoveToNextAttribute())
            {
                if (reader.Name == "base")
                {
                    uConf.baseData = float.Parse(reader.Value);
                }
                else if (reader.Name == "upFactor")
                {
                    uConf.upFactor = float.Parse(reader.Value);
                }
                else if (reader.Name == "basePrice")
                {
                    uConf.basePrice = float.Parse(reader.Value);
                }
                else if (reader.Name == "upPriceFactor")
                {
                    uConf.upPriceFactor = float.Parse(reader.Value);
                }
                else if (reader.Name == "maxLevel")
                {
                    uConf.maxLevel = int.Parse(reader.Value);
                }

            }
        }

        switch (uType)
        {
            case "Damage":
                weaponConf.damageConf = uConf;
                break;
            case "Frequency":
                weaponConf.attackRateConf = uConf;
                break;
            case "Accuracy":
                weaponConf.accuracyConf = uConf;
                break;
        }



    }


}
