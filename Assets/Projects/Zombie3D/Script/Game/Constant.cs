using UnityEngine;
using System;
using System.Collections.Generic;
namespace Zombie3D
{

    public class Constant
    {
        //the zoom speed from normal camera to shooting camera
        public const float CAMERA_ZOOM_SPEED = 10.0f;
        //the deflection speed when shooting with armed heavy weapons(eg, machine gun)
        public const float DEFLECTION_SPEED = 2.0f;

        public const float FLOORHEIGHT = 10000.1f;

        public const float POWERBUFFTIME = 30.0f;

        public const float ITEM_HIGHPOINT =  1.2f;
        public const float ITEM_LOWPOINT = 1f;

        public const float PLAYING_WALKINGSPEED_DISCOUNT_WHEN_SHOOTING = 0.8f;

        public const float ANIMATION_ENDS_PERCENT = 1.0f;

        public const float SPARK_MIN_DISTANCE = 2.0f;

        public const float MAX_WAVE_TIME = 1800.0f;



        public const float DOCTOR_HP_RECOVERY = 1.0f;
        public const float MARINE_POWER_UP = 1.2f;
        public const float SWAT_HP = 2.0f;
        public const float NERD_MORE_LOOT_RATE = 1.3f;
        public const float COWBOY_SPEED_UP = 1.3f;
        public const float RICHMAN_MONEY_MORE = 1.2f;
        public const float ENEGY_ARMOR__DAMAGE_BOOST = 2.0f;
        public const float ENEGY_ARMOR_HP_BOOST = 3.0f;
        public const float ENEGY_ARMOR_SPEED_UP = 1.3f;

        public const float POWER_BUFF = 2.0f;

        public const int WOODBOX_LOOT = 300;

        public const int MAX_CASH = 99999999;

        public const int SELECTION_NUM = 3;

        public const float R = 0.5f;

    }

    public class AvatarInfo
    {

        public static string[] AVATAR_NAME = {
                                                  "JOE BLO",
                                                  "PLUMBER",
                                                  "NERD",
                                                  "DOCTOR",
                                                  "COWBOY",
                                                  "SWAT",
                                                  "MARINE",
                                                  "B.E.A.F."
                                              };

        public static string[] AVATAR_INFO = {
                                                 "JUST A REGULAR GUY, NOTHING SPECIAL ABOUT HIM.",
                                                 "THIS STOUT LITTLE FELLA HAS THE STRENGTH OF A BEAR. EVEN THE HEAVIEST WEAPONS ARE A PIECE OF CAKE FOR HIM.", 
                                             "YES, HIS GLASSES ARE THICK, BUT NOTHING GETS PAST THIS NERD. HIS POWERS OF OBSERVATION ARE ABSOLUTELY FIRST-RATE.", 
                                             "WITH EXTENSIVE MEDICAL TRAINING, THIS GUY CERTAINLY KNOWS HOW TO TAKE CARE OF HIMSELF. HIS HEALTH REGENERATES AUTOMATICALLY.", 
                                             "QUICK ON HIS FEET WITH FANTASTIC AGILITY, THIS GUY CAN GET AROUND.", 
                                             "USED TO TAKING A BEATING, THIS EXPERIENCED ENFORCER HAS TONS AND TONS OF HP.",
                                             "THIS MILITARY TRAINED OFFICER DEFINITELY KNOWS HOW TO USE A WEAPON. HIS ACCURACY AND DAMAGE ARE UNMATCHED.",         
                                             "BIO-ENHANCED ADVANCED FIGHTER. AN INCREDIBLY POWERFUL MYSTERIOUS WARRIOR FROM THE FUTURE, HIS ABILITIES IN EVERY AREA CAN ONLY BE DESCRIBED AS SUPERHUMAN."};



        public static string[] TIPS_INO ={
                                             "Shotgun deals great damage to enemies at close range, whereas a terrible accuracy at long range.",
                                             "Aim first, and then shoot. This would save you a lot of ammo.",
                                            
"The heavier the weapon you use the slower you run.",


"Killing any enemy has a chance to drop an item.",


"Boomers are very dangerous. They, however, make special noises.",


"It is very important to observe enemies more and to keep moving.",


"Nurses may attack you from long distance; remember to keep a distance from them.",


"Be careful that grenade has a wide explosion scope!",


"Be careful with that speedy guy, sometimes he will jump on you.",


"Power-Up considerably boosts your damage in a short time.",


"Red Cross can fully recover your character health instantly.",


"Get a variety of items by shattering wooden cases on the battleground.",


"It is crucial for combat to wisely use money to reconstruct weapons.",



"Giants charge in a straight line. Keep moving to evade!",


"AK-47 is an excellent weapon.",


"Slow firing weapons can be very useful and practical after reconstruction.",



"PGM is very powerful. All you need to do is wait until it automatically locks down an enemy and shoot.",


"RPG-7 is not only powerful but also deals area damage.",


"Every character has its unique abilities.",


"Doctor automatically restores your health.",


"Cowboy is good at running at an impressive high speed.",


"Worker has amazing strength. Weight of weapons has no bearing on him.",


"B.E.A.F is a very powerful warrior whose abilities are way better than others.",


"It is easier for Nerd to get all sorts of items.",


"Marine is a powerful solider, capable of dealing great damage to enemies.",


"Swat wears armor with extraordinary stamina.",


"Check ammo amount before battle.",


"Weapons with high rate of fire consume ammo very quickly.",


"Store is where you can buy various weapons and ammo.",


"Weapons will be gradually unlocked as more levels are unlocked.",


"Your running speed decreases while shooting.",


"Watch your back and pay more attention to your surroundings.",


"Once an item appears it will never disappear. Pick it up at the right time.",


"Remember to equip equipment to put them in use after purchase.",


"Great weapons need constant enhancing to see their values.",


"Electric saw won't consume any ammo. Use it for your pleasure.",
        

"Laser is tremendously powerful and able to penetrate enemies.",

"Generally speaking, the more powerful the weapon the more expensive it is.",


"When you feel the pinch, you'd better focus on 1 weapon."

                                         };

    }


    //PhysicsLayer constants according to Unity Physics Layer
    public class PhysicsLayer
    {
        public const int Default = 0;
        public const int PLAYER = 8;
        public const int ENEMY = 9;
        public const int WALL = 11;
        public const int SCENE_OBJECT = 13;
        public const int FLOOR = 15;
        public const int TRANSPARENT_WALL = 16;
        public const int SKY = 17;
        public const int DEADBODY = 18;
        public const int WOODBOX = 19;
        public const int ITEMS = 20;
        public const int PLAYER_WEAPON = 21;
        public const int TANK_WALL = 22;
        public const int WOODBOX_AIM = 23;
    }


    //Special Bone name(eg, hand bone path for weapons)
    public class BoneName
    {
        public const string RIGHT_HAND_PATH = "Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Neck/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand";
		public const string WEAPON_PATH = "Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Neck/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand/Weapon_Dummy";  //! 武器挂载在这个节点
        public const string ENEMY_AIMED_PATH = "Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Neck/Bip01 Head";
        public const string ENEMY_HAND = "Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Neck/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand";
        public const string ENEMY_LEFTHAND = "Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Neck/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm/Bip01 L Hand";
        public const string ENEMY_BODY = "Bip01";

    }



    public class ConstData
    {
        public const string ENEMY_NAME = "E_";
        //public const string FONT_NAME = "Arial12_Bold";
        public const string FONT_NAME0 = "font0";
        public const string FONT_NAME1 = "font1";
        public const string FONT_NAME2 = "font2";
        public const string FONT_NAME3 = "font3";
    }

    public class ColorName
    {
        public static Color fontColor_darkred = new Color(145.0f / 255.0f, 12.0f / 255.0f, 11.0f / 255.0f, 1f);
        public static Color fontColor_red = new Color(252.0f / 255.0f, 52.0f / 255.0f, 3.0f / 255.0f, 1f);
        public static Color fontColor_yellow = new Color(255.0f / 255.0f, 219.0f / 255.0f, 34.0f / 255.0f, 1f);
        public static Color fontColor_orange = new Color(202.0f / 255.0f, 135.0f / 255.0f, 21.0f / 255.0f, 1f);
        public static Color fontColor_darkorange = new Color(154.0f / 255.0f, 107.0f / 255.0f, 26.0f / 255.0f, 1f);
    }

    public class TimerName
    {
        public const int ZOMBIE_AUDIO = 0;
        public const int NURSE_AUDIO = 1;
        public const int AUTOLOCK = 2;
        public const int GUNFIRE_ANIMATION = 3;
    }

    public class AudioName
    {
        public const string ATTACK = "Attack";
        public const string SHOUT = "Shout";
        public const string DEAD = "Dead";
        public const string WALK = "Walk";
        public const string SPECIAL = "Special";
        public const string GETITEM = "GetItem";
        public const string SWITCH = "Switch";
    }

    public class GunName
    {
        public const string M4 = "M4";
        public const string AK47 = "AK-47";
        public const string MP5 = "MP5";
        public const string AUG = "AUG";
        public const string P90 = "P90";

        public const string WINCHESTER1200 = "Winchester 1200";
        public const string REMINGTON870 = "Remington 870";
        public const string XM1014 = "XM 1014";

        public const string GATLIN = "Gatling";
        public const string LASERGUN = "Laser";

        public const string RPG = "RPG-7";
        public const string SNIPER = "PGM";
        public const string SAW = "Chainsaw";

    }

    public class SceneName
    {
        public const string START_MENU = "StartMenuUI";
        public const string ARENA_MENU = "ArenaMenuUI";
        public const string MAP = "MapUI";
        public const string SCENE_ARENA = "Zombie3D_Arena";
        public const string SCENE_VILLAGE = "Zombie3D_Village";
        public const string SCENE_PARKING = "Zombie3D_ParkingLot";
        public const string SCENE_HOSPITAL = "Zombie3D_Hospital";
        public const string SCENE_TUTORIAL = "Zombie3D_Tutorial";
    }

    //Constants for animation name
    public class AnimationName
    {

        public const string PLAYER_IDLE = "Idle01";
        public const string PLAYER_RUN = "Run01";
        public const string PLAYER_SHOT = "Shoot01";
        public const string PLAYER_SHOT02 = "Shoot02";
        public const string PLAYER_RUNFIRE = "RunShoot01";
        public const string PLAYER_RUNFIRE02 = "RunShoot02";
        public const string PLAYER_RUN02 = "Run02";
        public const string PLAYER_GOTHIT = "Damage01";
        public const string PLAYER_DEATH = "Death0";
        public const string PLAYER_STANDBY = "Standby03";



        public const string ENEMY_IDLE = "Idle01";
        public const string ENEMY_RUN = "Run";
        public const string ENEMY_RUN01 = "Forward01";
        public const string ENEMY_RUN02 = "Forward02";
        public const string ENEMY_RUN03 = "Forward03";
        public const string ENEMY_PATROL = "Run02";
        public const string ENEMY_DEATH1 = "Death01";
        public const string ENEMY_DEATH2 = "Death02";

        public const string ENEMY_ATTACK = "Attack01";
        public const string ENEMY_GOTHIT = "Damage";
        public const string ENEMY_RUSHINGSTART = "Rush01";
        public const string ENEMY_RUSHING = "Rush02";
        public const string ENEMY_RUSHINGEND = "Rush03";

        public const string ENEMY_JUMPSTART = "JumpStart01";
        public const string ENEMY_JUMPING = "JumpIdle01";
        public const string ENEMY_JUMPGEND = "JumpEnd01";


        public const string SHOTGUN_RELOAD = "Reload";

    }


    public class TagName
    {
        public const string GRAVE = "Grave";
        public const string WAYPOINT = "WayPoint";
    }
}