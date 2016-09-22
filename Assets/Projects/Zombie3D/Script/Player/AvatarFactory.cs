using UnityEngine;
using System.Collections;

namespace Zombie3D
{

    public class AvatarFactory
    {
        protected static AvatarFactory instance;
        public static AvatarFactory GetInstance()
        {
            if (instance == null)
            {
                instance = new AvatarFactory();
            }
            return instance;
        }


        public GameObject CreateAvatar(AvatarType aType)
        {
            GameObject avatarObj = null;
            switch (aType)
            {
                case AvatarType.Human:
                    avatarObj = (GameObject)GameObject.Instantiate(GameApp.GetInstance().GetResourceConfig().human, Vector3.zero, Quaternion.identity);          
                    break;
                case AvatarType.Plumber:
                    avatarObj = (GameObject)GameObject.Instantiate(GameApp.GetInstance().GetResourceConfig().plumbers, Vector3.zero, Quaternion.identity);                         
                    break;
                case AvatarType.Marine:
                    avatarObj = (GameObject)GameObject.Instantiate(GameApp.GetInstance().GetResourceConfig().marine, Vector3.zero, Quaternion.identity);          
                    break;
                case AvatarType.EnegyArmor:
                    avatarObj = (GameObject)GameObject.Instantiate(GameApp.GetInstance().GetResourceConfig().enegyArmor, Vector3.zero, Quaternion.identity);                         
                    break;
                case AvatarType.Nerd:
                    avatarObj = (GameObject)GameObject.Instantiate(GameApp.GetInstance().GetResourceConfig().nerd, Vector3.zero, Quaternion.identity);                           
                    break;
                case AvatarType.Doctor:
                    avatarObj = (GameObject)GameObject.Instantiate(GameApp.GetInstance().GetResourceConfig().doctor, Vector3.zero, Quaternion.identity);          
                    break;
                case AvatarType.Cowboy:
                    avatarObj = (GameObject)GameObject.Instantiate(GameApp.GetInstance().GetResourceConfig().cowboy, Vector3.zero, Quaternion.identity);                           
                    break;
                case AvatarType.Swat:
                    avatarObj = (GameObject)GameObject.Instantiate(GameApp.GetInstance().GetResourceConfig().swat, Vector3.zero, Quaternion.identity);          
                    break;

            }
            return avatarObj;
        }



    }
}