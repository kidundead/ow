using UnityEngine;
using System.Collections;

namespace Zombie3D
{

    public class WeaponFactory
    {
        protected static WeaponFactory instance;
        public static WeaponFactory GetInstance()
        {
            if (instance == null)
            {
                instance = new WeaponFactory();
            }
            return instance;
        }


        public Weapon CreateWeapon(WeaponType wType)
        {
            Weapon weapon = null;
            switch (wType)
            {
                case WeaponType.AssaultRifle:
                    weapon = new AssaultRifle();
                    break;
                case WeaponType.ShotGun:
                    weapon = new ShotGun();
                    break;
                case WeaponType.RocketLauncher:
                    weapon = new RocketLauncher();
                    break;
                case WeaponType.MachineGun:
                    weapon = new MachineGun();
                    break;
                case WeaponType.LaserGun:
                    weapon = new LaserGun();
                    break;
                case WeaponType.Sniper:
                    weapon = new Sniper();
                    break;
                case WeaponType.Saw:
                    weapon = new Saw();
                    break;

            }
            //Debug.Log("Create Weapon"+weapon.GetWeaponType());
            return weapon;
        }


        public GameObject CreateWeaponModel(string weaponName, Vector3 pos, Quaternion rotation)
        {
            GameObject model = null;
            ResourceConfigScript rConf = GameApp.GetInstance().GetResourceConfig();
            switch (weaponName)
            {
                case GunName.M4:
                    model = (GameObject)GameObject.Instantiate(rConf.m4, pos, rotation);
                    break;
                case GunName.MP5:
                    model = (GameObject)GameObject.Instantiate(rConf.mp5, pos, rotation);
                    break;
                case GunName.AK47:
                    model = (GameObject)GameObject.Instantiate(rConf.ak47, pos, rotation);
                    break;
                case GunName.P90:
                    model = (GameObject)GameObject.Instantiate(rConf.p90, pos, rotation);
                    break;
                case GunName.AUG:
                    model = (GameObject)GameObject.Instantiate(rConf.aug, pos, rotation);
                    break;

                case GunName.WINCHESTER1200:
                    model = (GameObject)GameObject.Instantiate(rConf.winchester1200, pos, rotation);
                    break;
                case GunName.REMINGTON870:
                    model = (GameObject)GameObject.Instantiate(rConf.remington870, pos, rotation);
                    break;
                case GunName.XM1014:
                    model = (GameObject)GameObject.Instantiate(rConf.xm1014, pos, rotation);
                    break;
                case GunName.RPG:
                    model = (GameObject)GameObject.Instantiate(rConf.rpgGun, pos, rotation);
                    break;
                case GunName.LASERGUN:
                    model = (GameObject)GameObject.Instantiate(rConf.lasergun, pos, rotation);
                    break;

                case GunName.GATLIN:
                    model = (GameObject)GameObject.Instantiate(rConf.gatlin, pos, rotation);
                    break;

                case GunName.SNIPER:
                    model = (GameObject)GameObject.Instantiate(rConf.sniper, pos, rotation);
                    break;
                case GunName.SAW:
                    model = (GameObject)GameObject.Instantiate(rConf.saw, pos, rotation);
                    break;
                default:
                    model = (GameObject)GameObject.Instantiate(rConf.m4, pos, rotation);
                    break;
            }
            return model;
        }

        /*
        public GameObject CreateWeapon2DModel(string weaponName, Vector3 pos, Quaternion rotation)
        {
            GameObject model = null;
            ResourceConfigScript rConf = GameApp.GetInstance().GetResourceConfig();
            switch (weaponName)
            {
                case GunName.M4:
                    model = (GameObject)GameObject.Instantiate(rConf.m4_model, pos, rotation);
                    break;
                case GunName.MP5:
                    model = (GameObject)GameObject.Instantiate(rConf.mp5_model, pos, rotation);
                    break;
                case GunName.AK47:
                    model = (GameObject)GameObject.Instantiate(rConf.ak47_model, pos, rotation);
                    break;
                case GunName.P90:
                    model = (GameObject)GameObject.Instantiate(rConf.p90_model, pos, rotation);
                    break;
                case GunName.AUG:
                    model = (GameObject)GameObject.Instantiate(rConf.aug_model, pos, rotation);
                    break;

                case GunName.WINCHESTER1200:
                    model = (GameObject)GameObject.Instantiate(rConf.winchester1200_model, pos, rotation);
                    break;
                case GunName.REMINGTON870:
                    model = (GameObject)GameObject.Instantiate(rConf.remington870_model, pos, rotation);
                    break;
                case GunName.XM1014:
                    model = (GameObject)GameObject.Instantiate(rConf.xm1014_model, pos, rotation);
                    break;
                case GunName.RPG:
                    model = (GameObject)GameObject.Instantiate(rConf.rpgGun_model, pos, rotation);
                    break;
                case GunName.LASERGUN:
                    model = (GameObject)GameObject.Instantiate(rConf.lasergun_model, pos, rotation);
                    break;

                case GunName.GATLIN:
                    model = (GameObject)GameObject.Instantiate(rConf.gatlin_model, pos, rotation);
                    break;
               
                case GunName.SNIPER:
                    model = (GameObject)GameObject.Instantiate(rConf.sniper_model, pos, rotation);
                    break;
                case GunName.SAW:
                    model = (GameObject)GameObject.Instantiate(rConf.saw_model, pos, rotation);
                    break;
                default:
                    model = (GameObject)GameObject.Instantiate(rConf.m4_model, pos, rotation);
                    break;
            }
            return model;
        }
        */
        /*
        public GameObject CreateWeapon2DModelMask(string weaponName, Vector3 pos, Quaternion rotation)
        {
            GameObject model = null;
            
            ResourceConfigScript rConf = GameApp.GetInstance().GetResourceConfig();
            switch (weaponName)
            {
                case GunName.M4:
                    model = (GameObject)GameObject.Instantiate(rConf.m4_model_mask, pos, rotation);
                    break;
                case GunName.MP5:
                    model = (GameObject)GameObject.Instantiate(rConf.mp5_model_mask, pos, rotation);
                    break;
                case GunName.AK47:
                    model = (GameObject)GameObject.Instantiate(rConf.ak47_model_mask, pos, rotation);
                    break;
                case GunName.P90:
                    model = (GameObject)GameObject.Instantiate(rConf.p90_model_mask, pos, rotation);
                    break;
                case GunName.AUG:
                    model = (GameObject)GameObject.Instantiate(rConf.aug_model_mask, pos, rotation);
                    break;

                case GunName.WINCHESTER1200:
                    model = (GameObject)GameObject.Instantiate(rConf.winchester1200_model_mask, pos, rotation);
                    break;
                case GunName.REMINGTON870:
                    model = (GameObject)GameObject.Instantiate(rConf.remington870_model_mask, pos, rotation);
                    break;
                case GunName.XM1014:
                    model = (GameObject)GameObject.Instantiate(rConf.xm1014_model_mask, pos, rotation);
                    break;
                case GunName.RPG:
                    model = (GameObject)GameObject.Instantiate(rConf.rpgGun_model_mask, pos, rotation);
                    break;
                case GunName.LASERGUN:
                    model = (GameObject)GameObject.Instantiate(rConf.lasergun_model_mask, pos, rotation);
                    break;

                case GunName.GATLIN:
                    model = (GameObject)GameObject.Instantiate(rConf.gatlin_model_mask, pos, rotation);
                    break;

                case GunName.SNIPER:
                    model = (GameObject)GameObject.Instantiate(rConf.sniper_model_mask, pos, rotation);
                    break;
                case GunName.SAW:
                    model = (GameObject)GameObject.Instantiate(rConf.saw_model_mask, pos, rotation);
                    break;
                default:
                    model = (GameObject)GameObject.Instantiate(rConf.m4_model_mask, pos, rotation);
                    break;
            }
            return model;
        }
        */

    }
}