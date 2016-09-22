using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;


namespace Zombie3D
{

    public enum AvatarState
    {
        Avaliable = 0,
        NotAvaliable,
        ToBuy

    }

    public enum WeaponBuyStatus
    {
        Locked,
        NotEnoughCash,
        Succeed
    }

    public class GameState
    {

        //protected int theDays;
        protected int cash;
        protected int score;
        public int LevelNum { get; set; }
        public AchievementState Achievement { get; set; }
        public float MenuMusicTime { get; set; }

        protected int[] infectionRate;
        protected List<Weapon> weaponList;
        protected AvatarState[] avatarData;
        protected GameConfig gConfig;
        protected bool inited;
        protected bool weaponsInited;
        public bool FirstTimeGame { get; set; }
        public AvatarType Avatar { get; set; }
        public bool MusicOn { get; set; }
        public int ArmorLevel { get; set; }
        protected int[] rectToWeaponMap = new int[Constant.SELECTION_NUM];
        public bool AlreadyCountered { get; set; }
        public bool AlreadyPopReview { get; set; }

        protected bool fromShopMenu = false;

        public bool FromShopMenu
        {
            get
            {
                return fromShopMenu;
            }
            set
            {
                fromShopMenu = value;
            }
        }

        public void AddScore(int scoreAdd)
        {
            score += scoreAdd;
        }

        public int GetScore()
        {
            return score;
        }

        public AvatarState GetAvatarData(AvatarType aType)
        {
            return avatarData[(int)aType];
        }

        public int GetAvatarNum()
        {
            return avatarData.Length;
        }

        public int[] GetRectToWeaponMap()
        {
            return rectToWeaponMap;
        }

        public bool GotAllWeapons()
        {
            for (int i = 0; i < weaponList.Count; i++)
            {
                if (weaponList[i].Exist != WeaponExistState.Owned)
                {
                    return false;
                }
            }
            return true;

        }
        public void EnableAvatar(AvatarType aType)
        {
            avatarData[(int)aType] = AvatarState.Avaliable;
            Achievement.GotNewAvatar();
        }

        public void LoadData(BinaryReader br)
        {
            Debug.Log("Load");

            if (!inited)
            {
                Init();
            }

            weaponsInited = true;

            cash = br.ReadInt32();

            score = br.ReadInt32();

            LevelNum = br.ReadInt32();

            weaponList = new List<Weapon>();

            int count = br.ReadInt32();


            for (int i = 0; i < count; i++)
            {
                int weaponType = br.ReadInt32();
                Weapon weapon = WeaponFactory.GetInstance().CreateWeapon((WeaponType)weaponType);

                weapon.Name = br.ReadString();
                weapon.WConf = gConfig.GetWeaponConfig(weapon.Name);
                weapon.LoadConfig();



                weapon.BulletCount = br.ReadInt32();
                weapon.Damage = (float)br.ReadDouble();
                weapon.AttackFrequency = (float)br.ReadDouble();
                weapon.Accuracy = (float)br.ReadDouble();
                weapon.DamageLevel = br.ReadInt32();
                weapon.FrequencyLevel = br.ReadInt32();
                weapon.AccuracyLevel = br.ReadInt32();
                weapon.IsSelectedForBattle = br.ReadBoolean();
                weapon.Exist = (WeaponExistState)br.ReadInt32();



                weaponList.Add(weapon);
            }


            for (int i = 0; i < rectToWeaponMap.Length; i++)
            {
                rectToWeaponMap[i] = br.ReadInt32();
            }

            for (int i = 0; i < avatarData.Length; i++)
            {
                avatarData[i] = (AvatarState)br.ReadInt32();

            }
            Avatar = (AvatarType)br.ReadInt32();
            ArmorLevel = br.ReadInt32();
            FirstTimeGame = br.ReadBoolean();
            Achievement.Load(br);
        
        }

        public int GetWeaponIndex(Weapon w)
        {
            for (int i = 0; i < weaponList.Count; i++)
            {
                if (weaponList[i] == w)
                {
                    return i;
                }
            }
            return 0;
        }

        public void SaveData(BinaryWriter bw)
        {
            Debug.Log("Save");
            bw.Write(cash);
            bw.Write(score);
            bw.Write(LevelNum);
            bw.Write(weaponList.Count);


            for (int i = 0; i < weaponList.Count; i++)
            {
                int type = (int)(weaponList[i].GetWeaponType());
                bw.Write(type);
                bw.Write(weaponList[i].Name);
                bw.Write(weaponList[i].BulletCount);
                bw.Write((double)weaponList[i].Damage);
                bw.Write((double)weaponList[i].AttackFrequency);
                bw.Write((double)weaponList[i].Accuracy);
                bw.Write(weaponList[i].DamageLevel);
                bw.Write(weaponList[i].FrequencyLevel);
                bw.Write(weaponList[i].AccuracyLevel);
                bw.Write(weaponList[i].IsSelectedForBattle);
                bw.Write((int)weaponList[i].Exist);
            }

            for (int i = 0; i < rectToWeaponMap.Length; i++)
            {
                bw.Write(rectToWeaponMap[i]);
            }


            for (int i = 0; i < avatarData.Length; i++)
            {
                bw.Write((int)avatarData[i]);
            }
            bw.Write((int)Avatar);
            bw.Write(ArmorLevel);
            bw.Write(FirstTimeGame);

            Achievement.Save(bw);


        }


        public GameState()
        {
            Debug.Log("create game state");
            inited = false;
            weaponList = new List<Weapon>();
            Achievement = new AchievementState();
            AlreadyCountered = false;
            AlreadyPopReview = false;
        }

        public void ClearState()
        {
            inited = false;
            weaponsInited = false;
            Init();

        }
        public void Init()
        {

            if (!inited)
            {
                Debug.Log("game state init");
                gConfig = GameApp.GetInstance().GetGameConfig();   //! Config

                cash = gConfig.globalConf.startMoney;              //! 玩家金钱

                MusicOn = true;
                infectionRate = new int[4];

                for (int i = 0; i < 4; i++)
                {
                    infectionRate[i] = 100;
                }

                infectionRate[2] = 100;

                Avatar = AvatarType.Human;

                avatarData = new AvatarState[(int)AvatarType.AvatarCount];   //! 所有人物的状态：可用 & 不可用 & 需购买

                for (int i = 0; i < avatarData.Length; i++)
                {
                    avatarData[i] = AvatarState.ToBuy;
                }

                avatarData[0] = AvatarState.Avaliable;     //! 第一个人物可用

                FirstTimeGame = true;

                /*
                for (int i = 0; i < avatarData.Length; i++)
                {

                    avatarData[i] = AvatarState.Avaliable;
                }

                avatarData[6] = AvatarState.ToBuy;
                 
                avatarData[7] = AvatarState.ToBuy;
                */
                ArmorLevel = 0;

                LevelNum = 1;

                inited = true;
            }

        }

        public void InitWeapons()
        {
            if (!weaponsInited)
            {
                Debug.Log("Init Weapons");
                weaponList.Clear();

                for (int i = 0; i < rectToWeaponMap.Length; i++)
                {
                    rectToWeaponMap[i] = -1;
                }

				//! 武器列表
                List<WeaponConfig> wConfList = gConfig.GetWeapons();

                foreach (WeaponConfig wConf in wConfList)
                {
                    string weaponName = wConf.name;
                    Weapon weapon = WeaponFactory.GetInstance().CreateWeapon(wConf.wType);
                    if (wConf.name == GunName.MP5)
                    {
                        weapon.IsSelectedForBattle = true;
                        rectToWeaponMap[0] = GetWeaponIndex(weapon);
                    }

                    if (wConf.name == GunName.SAW)
                    {
                        weapon.IsSelectedForBattle = true;
                        rectToWeaponMap[1] = 8;
                    }

                    weapon.Exist = wConf.startEquip;

                    weapon.Name = weaponName;
                    weapon.WConf = wConf;
                    weapon.LoadConfig();
                    weaponList.Add(weapon);
                }

                weaponsInited = true;
            }
        }

        public int GetArmorPrice()
        {
            float price = gConfig.playerConf.upgradeArmorPrice;
            for (int i = 0; i < ArmorLevel; i++)
            {
                price += price * gConfig.playerConf.upPriceFactor;
            }
            return (int)price;
        }

        public void DeliverIAPItem(IAPName iapName)
        {
            switch (iapName)
            {
                case IAPName.Cash50W:
                    AddCash(500000);
                    break;
                case IAPName.Cash120W:
                    AddCash(1200000);
                    break;

                case IAPName.Cash270W:
                    AddCash(2700000);
                    break;
                case IAPName.Cash750W:
                    AddCash(7500000);
                    break;
                case IAPName.Cash1650W:
                    AddCash(16500000);
                    break;
            }

            GameApp.GetInstance().Save();

        }

        public bool IsWeaponOwned(WeaponType wType)
        {
            for (int i = 0; i < weaponList.Count; i++)
            {
                if (weaponList[i].GetWeaponType() == wType && weaponList[i].Exist == WeaponExistState.Owned)
                {
                    return true;
                }
            }
            return false;

        }

        public List<Weapon> GetBattleWeapons()
        {
            List<Weapon> battleWeapons = new List<Weapon>();

            for (int i = 0; i < rectToWeaponMap.Length; i++)
            {
                if (rectToWeaponMap[i] != -1)
                {
                    battleWeapons.Add(weaponList[rectToWeaponMap[i]]);
                }
            }


            return battleWeapons;
        }

        public int GetWeaponByOwnedIndex(int index)
        {
            int ownedIndex = 0;
            for (int i = 0; i < weaponList.Count; i++)
            {
                Weapon w = weaponList[i];

                if (w.Exist == WeaponExistState.Owned)
                {
                    if (ownedIndex == index)
                    {
                        return i;
                    }

                    ownedIndex++;
                }
            }
            return -1;
        }


        /*
        public int TheDays
        {
            get
            {
                return theDays;
            }
        }



        public void OneDayPassed()
        {
            theDays++;
        }

        public void ModifyInfectionRate(int sceneIndex, int rate)
        {
            infectionRate[sceneIndex] += rate;
            infectionRate[sceneIndex] = Mathf.Clamp(infectionRate[sceneIndex], 0, 100);
        }

        public void IncreaseInfectionRateExcept(int sceneIndex, int rate)
        {
            for (int i = 0; i < 4; i++)
            {
                if (i != sceneIndex)
                {
                    infectionRate[i] += rate;
                    infectionRate[i] = Mathf.Clamp(infectionRate[i], 0, 100);
                }

            }
        }

        public void IncreaseInfectionRateAll(int rate)
        {
            for (int i = 0; i < 4; i++)
            {
                infectionRate[i] += rate;
                infectionRate[i] = Mathf.Clamp(infectionRate[i], 0, 100);
            }
        }

        public int GetInfectionRate(int sceneIndex)
        {
            return infectionRate[sceneIndex];
        }
        */


        public void AddCash(int cashGot)
        {
            cash += cashGot;
            cash = Mathf.Clamp(cash, 0, Constant.MAX_CASH);
            Achievement.CheckAchievemnet_RichMan(cash);
        }

        public void LoseCash(int cashSpend)
        {
            cash -= cashSpend;
        }

        public int GetCash()
        {
            return cash;
        }

        public List<Weapon> GetWeapons()
        {
            return weaponList;
        }
        /*
        public bool BuyShotGun(int price)
        {
            if (cash >= price)
            {
                weaponList[1] = new ShotGun();
                LoseCash(price);
                return true;
            }
            return false;

        }

        public bool BuyRPG(int price)
        {
            if (cash >= price)
            {
                weaponList[2] = new RocketLauncher();
                LoseCash(price);
                return true;
            }
            return false;
        }
        */
        public WeaponBuyStatus BuyWeapon(Weapon w, int price)
        {
            int index = GetWeaponIndex(w);

            if (weaponList[index].Exist == WeaponExistState.Unlocked)
            {
                if (cash >= price)
                {
                    weaponList[index].Exist = WeaponExistState.Owned;
                    LoseCash(price);
                    Achievement.GotNewWeapon();
                    return WeaponBuyStatus.Succeed;
                }
                else
                {
                    Debug.Log("Not Enough Cash!");
                    return WeaponBuyStatus.NotEnoughCash;
                }
            }
            else
            {
                Debug.Log("Weapon is Not Available!");
                return WeaponBuyStatus.Locked;
            }

        }

        public bool BuyAvatar(AvatarType aType, int price)
        {
            if (avatarData[(int)aType] == AvatarState.ToBuy)
            {
                if (cash >= price)
                {
                    avatarData[(int)aType] = AvatarState.Avaliable;
                    LoseCash(price);
                    Achievement.GotNewAvatar();
                    return true;
                }

            }

            return false;

        }


        public WeaponType RandomWeaponAlreadyHave()
        {
            int sum = weaponList.Count;
            int rnd = Random.Range(0, sum);


            if (sum != 0)
            {
                return weaponList[rnd].GetWeaponType();

            }
            else
            {
                return WeaponType.AssaultRifle;
            }

        }

        public WeaponType RandomBattleWeapons()
        {

            List<Weapon> battleWeapons = new List<Weapon>();

            foreach (Weapon w in weaponList)
            {
                if (w.IsSelectedForBattle && w.GetWeaponType() != WeaponType.Saw)
                {
                    battleWeapons.Add(w);
                }
            }


            int sum = battleWeapons.Count;
            int rnd = Random.Range(0, sum);


            if (sum != 0)
            {
                return battleWeapons[rnd].GetWeaponType();

            }
            else
            {
                return WeaponType.AssaultRifle;
            }

        }






        public bool BuyBullets(Weapon w, int bulletsNum, int price)
        {
            if (this.cash >= price)
            {
                w.AddBullets(bulletsNum);

                LoseCash(price);
                return true;
            }
            return false;
        }



        public bool UpgradeWeapon(Weapon w, float power, float frequency, float accuracy, int price)
        {
            if (this.cash >= price)
            {
                w.Upgrade(power, frequency, accuracy);
                LoseCash(price);
                return true;
            }
            return false;

        }

        public bool UpgradeArmor(int price)
        {
            if (this.cash >= price)
            {
                if (ArmorLevel < gConfig.playerConf.maxArmorLevel)
                {
                    ArmorLevel++;
                    LoseCash(price);
                    return true;
                }
            }

            return false;

        }

    }

}