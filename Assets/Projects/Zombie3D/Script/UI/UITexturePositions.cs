// Copyright (c) 2011, Triniti Interactive Company, China.
// All rights reserved.
//
// Created by Cheng Haitao at 5/3/2011 1:25:03 PM.
// E-Mail: chenghaitao@trinitigame.com.cn


using UnityEngine;
using System.Collections;
using Zombie3D;

public enum Platform
{
    iPhone3GS,
    iPhone4,
    IPad
}

public class AutoRect
{
    public static Rect AutoPos(Rect rect)
    {
        ResetResolution();

		float width = Screen.width;
		float height = Screen.height;

        if (width != 960 && width != 640 && width != 480 && width != 320)
        {
            Vector2 centerPos = new Vector2(480, 320);
            //! Vector2 newCenterPos = new Vector2(512, 384);         //! 1024 * 768 
			Vector2 newCenterPos = new Vector2(width, height)/2;
            float x = newCenterPos.x + (rect.x - centerPos.x) * 1;
            float y = newCenterPos.y + (rect.y - centerPos.y) * 1;
            return new Rect(x, y, rect.width * ResolutionConstant.R, rect.height * ResolutionConstant.R);
        }
        else
        {
            return new Rect(rect.x * ResolutionConstant.R, rect.y * ResolutionConstant.R, rect.width * ResolutionConstant.R, rect.height * ResolutionConstant.R);
        }
    }

    public static Vector2 AutoPos(Vector2 v)
    {
		float width = Screen.width;
		float height = Screen.height;

        if (Screen.width != 960 && Screen.width != 640 && Screen.width != 480 && Screen.width != 320)
        {
            Vector2 centerPos = new Vector2(480, 320);
			//! Vector2 newCenterPos = new Vector2(512, 384);         //! 1024 * 768 
			Vector2 newCenterPos = new Vector2(width, height)/2;
            float x = newCenterPos.x + (v.x - centerPos.x) * 1;
            float y = newCenterPos.y + (v.y - centerPos.y) * 1;
            return new Vector2(x, y);

        }
        else
        {
            ResetResolution();
            return new Vector2(v.x * ResolutionConstant.R, v.y * ResolutionConstant.R);
        }
    }

    public static Rect AutoValuePos(Rect rect)
    {

        ResetResolution();

        return new Rect(rect.x * ResolutionConstant.R, rect.y * ResolutionConstant.R, rect.width * ResolutionConstant.R, rect.height * ResolutionConstant.R);

    }
    public static Vector2 AutoValuePos(Vector2 v)
    {

        ResetResolution();
        return new Vector2(v.x * ResolutionConstant.R, v.y * ResolutionConstant.R);

    }

    public static Vector2 AutoSize(Rect rect)
    {
        ResetResolution();
        if (ResolutionConstant.R == 1)
        {
            return new Vector2(rect.width * ResolutionConstant.R, rect.height * ResolutionConstant.R);
        }
        else
        {
            return new Vector2(rect.width, rect.height);
        }
    }

    public static Vector2 AutoSize(Vector2 v)
    {
        ResetResolution();
        if (ResolutionConstant.R == 1)
        {
            return new Vector2(v.x * ResolutionConstant.R, v.y * ResolutionConstant.R);
        }
        else
        {
            return v;
        }
    }

    public static Rect AutoTex(Rect rect)
    {
        ResetResolution();
        return new Rect(rect.x * ResolutionConstant.R, rect.y * ResolutionConstant.R, rect.width * ResolutionConstant.R, rect.height * ResolutionConstant.R);
    }

    public static Vector2 AutoTex(Vector2 v)
    {
        ResetResolution();
        return new Vector2(v.x * ResolutionConstant.R, v.y * ResolutionConstant.R);
    }

    public static float AutoX(float x)
    {
        ResetResolution();

        if (Screen.width != 960 && Screen.width != 640 && Screen.width != 480 && Screen.width != 320)
        {
            float centerX = 480;
            float newCenterX = 512;
            float newX = newCenterX + (x - centerX) * 1;
            return newX;

        }
        else
        {
            return x * ResolutionConstant.R;
        }

    }

    public static float AutoY(float y)
    {
        ResetResolution();
        if (Screen.width != 960 && Screen.width != 640 && Screen.width != 480 && Screen.width != 320)
        {
            float centerY = 320;
            float newCenterY = 384;
            float newy = newCenterY + (y - centerY) * 1;
            return newy;
        }
        else
        {
            return y * ResolutionConstant.R;
        }

    }

    public static float AutoValue(float x)
    {

        return x * ResolutionConstant.R;

    }

    public static void ResetResolution()
    {

		float width  = Screen.width;
		float height = Screen.height;
        if (width == 960 || width == 640)
        {
            ResolutionConstant.R = 1.0f;
        }
        else if (width == 480 || width == 320)
        {
            ResolutionConstant.R = 0.5f;
        }
        else
        {
            ResolutionConstant.R = 1.0f;
        }
    }

    public static Platform GetPlatform()
    {
        if (Screen.width == 960 || Screen.width == 640)
        {
            return Platform.iPhone4;
        }
        else if (Screen.width == 480 || Screen.width == 320)
        {
            return Platform.iPhone3GS;
        }
        else
        {
            return Platform.IPad;
        }
    }


}


class StartMenuTexturePosition
{
    public static Rect Background = AutoRect.AutoTex(new Rect(0, 0, 960, 640));

    public static Rect AcheivementButtonNormal = AutoRect.AutoTex(new Rect(0, 640, 104, 60));
    public static Rect AcheivementButtonPressed = AutoRect.AutoTex(new Rect(104, 640, 104, 60));

    public static Rect LeaderBoardsButtonNormal = AutoRect.AutoTex(new Rect(208, 640, 104, 60));
    public static Rect LeaderBoardsButtonPressed = AutoRect.AutoTex(new Rect(312, 640, 104, 60));

}


class ArenaMenuTexturePosition
{
    public static Rect Background = AutoRect.AutoTex(new Rect(0, 0, 960, 640));
    public static Rect ButtonNormal = AutoRect.AutoTex(new Rect(0, 723, 366, 120));
    public static Rect ButtonPressed = AutoRect.AutoTex(new Rect(366, 723, 366, 120));
    public static Rect OptionsButton = AutoRect.AutoTex(new Rect(260, 640, 130, 74));
    public static Rect OptionsButtonPressed = AutoRect.AutoTex(new Rect(390, 640, 130, 74));
    public static Rect ReturnButtonNormal = AutoRect.AutoTex(new Rect(0, 640, 130, 70));
    public static Rect ReturnButtonPressed = AutoRect.AutoTex(new Rect(130, 640, 130, 70));
    public static Rect TextBackround = AutoRect.AutoTex(new Rect(627, 853, 396, 131));
    public static Rect Panel = AutoRect.AutoTex(new Rect(520, 640, 264, 64));
    public static Rect CashPanel = AutoRect.AutoTex(new Rect(0, 716, 314, 60));
    public static Rect ShopTitleImage = AutoRect.AutoTex(new Rect(0, 914, 267, 71));
    public static Rect UpgradeButtonNormal = AutoRect.AutoTex(new Rect(532, 704, 424, 108));
    public static Rect UpgradeButtonPressed = AutoRect.AutoTex(new Rect(532, 812, 424, 108));
    public static Rect GetMoneyButtonNormal = AutoRect.AutoTex(new Rect(0, 776, 532, 112));
    public static Rect GetMoneyButtonPressed = AutoRect.AutoTex(new Rect(0, 888, 532, 112));


    public static Rect StarEmptySelected = AutoRect.AutoTex(new Rect(532, 920, 24, 22));
    public static Rect StarFullSelected = AutoRect.AutoTex(new Rect(532, 942, 24, 22));
    public static Rect StarEmpty = AutoRect.AutoTex(new Rect(532, 964, 24, 22));
    public static Rect StarFull = AutoRect.AutoTex(new Rect(532, 986, 24, 22));

    public static Rect Arrow = AutoRect.AutoTex(new Rect(960, 0, 36, 26));
    public static Rect ArrowSelected = AutoRect.AutoTex(new Rect(960, 26, 36, 26));
    public static Rect CloseButtonNormal = AutoRect.AutoTex(new Rect(556, 920, 88, 80));
    public static Rect CloseButtonPressed = AutoRect.AutoTex(new Rect(644, 920, 88, 80));
    public static Rect Spinner = AutoRect.AutoTex(new Rect(960, 52, 52, 52));


    public static Vector2 GetMoneyButtonSmallSize = AutoRect.AutoTex(new Vector2(400, 90));

}

class MapUITexturePosition
{
    public static Rect Background = AutoRect.AutoTex(new Rect(0, 0, 960, 640));
    public static Rect FactoryImg = AutoRect.AutoTex(new Rect(0, 640, 266, 202));
    public static Rect HospitalImg = AutoRect.AutoTex(new Rect(266, 640, 164, 244));
    public static Rect ParkingImg = AutoRect.AutoTex(new Rect(430, 640, 230, 184));
    public static Rect ShopImg = AutoRect.AutoTex(new Rect(430, 824, 204, 192));
    public static Rect Village = AutoRect.AutoTex(new Rect(660, 640, 186, 260));
    //public static Rect Village = AutoRect.AutoTex(new Rect(430, 824, 204, 192));


    public static Rect ZombieAnimation1 = AutoRect.AutoTex(new Rect(864, 640, 66, 70));
    public static Rect ZombieAnimation2 = AutoRect.AutoTex(new Rect(930, 640, 66, 70));
    public static Rect ZombieAnimation3 = AutoRect.AutoTex(new Rect(864, 710, 66, 70));
}

class ButtonsTexturePosition
{

    public static Rect ButtonNormal = AutoRect.AutoTex(new Rect(0, 0, 356, 116));
    public static Rect ButtonPressed = AutoRect.AutoTex(new Rect(0, 116, 356, 116));

    public static Rect SoundButtonNormal = AutoRect.AutoTex(new Rect(356, 0, 148, 80));
    public static Rect SoundButtonPressed = AutoRect.AutoTex(new Rect(356, 0 + 80, 148, 80));

    public static Rect Label = AutoRect.AutoTex(new Rect(0, 232, 260, 76));

    public static Vector2 MiddleSizeButton = AutoRect.AutoTex(new Vector2(356, 90));
    public static Vector2 SmallSizeButton = AutoRect.AutoTex(new Vector2(250, 80));
    public static Vector2 TinySizeButton = AutoRect.AutoTex(new Vector2(226, 80));

    public static Vector2 LargeLabelSize = AutoRect.AutoTex(new Vector2(360, 76));

    public static Rect GetBulletsLogoRect(int index)
    {
        if (index == (int)WeaponType.Saw)
        {
            return new Rect(0, 0, 0, 0);
        }
        else
        {
            return AutoRect.AutoTex(new Rect(44 * (index - 1), 308, 44, 52));
        }
    }
    public static Rect Day = AutoRect.AutoTex(new Rect(264, 232, 210, 108));

}

class AvatarTexturePosition
{
    public static Rect[] AvatarLogo = new Rect[(int)AvatarType.AvatarCount];
    public static Rect Frame = AutoRect.AutoTex(new Rect(0, 800, 490, 222));
    public static Vector2 AvatarLogoSize = AutoRect.AutoTex(new Vector2(448, 200));
    public static Vector2 AvatarLogoSpacing = AutoRect.AutoTex(new Vector2(0, 82));
    public static Rect Mask = AutoRect.AutoTex(new Rect(896, 0, 27, 22));

    public static void InitLogosTexturePos()
    {
        for (int i = 0; i < AvatarTexturePosition.AvatarLogo.Length; i++)
        {
            int x = i % 2;
            int y = i / 2;
            AvatarTexturePosition.AvatarLogo[i] = AutoRect.AutoTex(new Rect(x * 448, y * 200, 448, 200));
        }
    }

}



class WeaponUpgradeTexturePosition
{
    public static Rect ButtonNormal = AutoRect.AutoTex(new Rect(0, 719 - 640, 416, 91));
    public static Rect ButtonPress = AutoRect.AutoTex(new Rect(0, 810 - 640, 416, 91));

    public static Rect CashPanel = AutoRect.AutoTex(new Rect(0, 640 - 640, 310, 79));

    public static Rect ArrowImage = AutoRect.AutoTex(new Rect(960, 0, 43, 19));

    public static Rect UpgradeButtonNormal = AutoRect.AutoTex(new Rect(0, 901 - 640, 235, 77));
    public static Rect UpgradeButtonPressed = AutoRect.AutoTex(new Rect(235, 901 - 640, 235, 77));


}

class DialogTexturePosition
{
    public static Rect Dialog = AutoRect.AutoTex(new Rect(0, 0, 574, 306));
    public static Rect TextBox = AutoRect.AutoTex(new Rect(0, 306, 490, 176));

}

class WeaponsLogoTexturePosition
{

    public static Vector2 WeaponLogoSize = AutoRect.AutoTex(new Vector2(438, 192));
    public static Vector2 WeaponLogoSpacing = AutoRect.AutoTex(new Vector2(0, 86));

    public static TexturePosInfo GetWeaponTextureRect(int index)
    {
        TexturePosInfo info = new TexturePosInfo();
        if (index < 10)
        {
            info.m_Material = UIResourceMgr.GetInstance().GetMaterial("Weapons");

            int x = 438 * (index % 2);
            int y = 192 * (index / 2);
            info.m_TexRect = AutoRect.AutoTex(new Rect(x, y, 438, 192));
        }
        else
        {
            info.m_Material = UIResourceMgr.GetInstance().GetMaterial("Weapons2");
            index -= 10;
            int x = 438 * (index % 2);
            int y = 192 * (index / 2);
            info.m_TexRect = AutoRect.AutoTex(new Rect(x, y, 438, 192));
        }

        return info;
    }

    public static Rect GetWeaponIconTextureRect(int index)
    {
        int x = 112 * (index % 4);
        int y = 112 * (index / 4);
        return AutoRect.AutoTex(new Rect(x, y, 112, 112));

    }

}



class ShopTexturePosition
{
    public static Rect Dialog = AutoRect.AutoTex(new Rect(0, 0, 887, 535));

    public static Rect SoldOutLogo = AutoRect.AutoTex(new Rect(720, 535, 160, 82));

    public static Rect LockedLogo = AutoRect.AutoTex(new Rect(438, 535, 438, 192));
    public static Rect BuyLogo = AutoRect.AutoTex(new Rect(0, 535, 438, 192));
    public static Rect SmallBuyLogo = AutoRect.AutoTex(new Rect(0, 727, 448, 200));
    public static Rect CashLogo = AutoRect.AutoTex(new Rect(0, 535, 240, 240));

    public static Rect DayLargePanel = AutoRect.AutoTex(new Rect(0, 928, 358, 76));
    public static Rect MapButtonNormal = AutoRect.AutoTex(new Rect(448, 790, 356, 112));
    public static Rect MapButtonPressed = AutoRect.AutoTex(new Rect(448, 902, 356, 112));
    public static Rect ArrowNormal = AutoRect.AutoTex(new Rect(878, 0, 76, 106));
    public static Rect ArrowPressed = AutoRect.AutoTex(new Rect(878, 106, 76, 106));
    public static Rect RightArrowNormal = AutoRect.AutoTex(new Rect(878, 212, 76, 106));
    public static Rect RightArrowPressed = AutoRect.AutoTex(new Rect(878, 318, 76, 106));



    public static Rect GetIAPLogoRect(int index)
    {
        int x = index % 4;
        int y = index / 4;
        return AutoRect.AutoTex(new Rect(x * 252, y * 354, 252, 354));
    }

}




class OptionsMenuTexturePosition
{

    public Rect Background = AutoRect.AutoTex(new Rect(0, 0, 960, 640));

    public Rect OptionsImage = AutoRect.AutoTex(new Rect(0, 843, 267, 71));

    public Rect ShareButtonNormal = AutoRect.AutoTex(new Rect(456, 0 + 512, 184, 81));
    public Rect ShareButtonPressed = AutoRect.AutoTex(new Rect(640, 0 + 512, 184, 81));
    public Rect ReturnButtonNormal = AutoRect.AutoTex(new Rect(0, 640, 228, 83));
    public Rect ReturnButtonPressed = AutoRect.AutoTex(new Rect(228, 640, 228, 83));



    public Rect ButtonNormal = AutoRect.AutoTex(new Rect(0, 83 + 512, 350, 101));
    public Rect ButtonPressed = AutoRect.AutoTex(new Rect(350, 83 + 512, 350, 101));
    public Rect MusicButtonNormal = AutoRect.AutoTex(new Rect(0, 184 + 512, 292, 97));
    public Rect MusicButtonPressed = AutoRect.AutoTex(new Rect(292, 184 + 512, 292, 97));
    public Rect MusicOnLogoButtonNormal = AutoRect.AutoTex(new Rect(0, 281 + 512, 189, 97));
    public Rect MusicOnLogoButtonPressed = AutoRect.AutoTex(new Rect(189, 281 + 512, 189, 97));
    public Rect MusicOffLogoButtonNormal = AutoRect.AutoTex(new Rect(378, 281 + 512, 189, 97));
    public Rect MusicOffLogoButtonPressed = AutoRect.AutoTex(new Rect(567, 281 + 512, 189, 97));

}

class PauseMenuTexturePosition
{
    public Rect Background = AutoRect.AutoTex(new Rect(512, 0, 512, 509));
    public Rect ButtonNormal = AutoRect.AutoTex(new Rect(0, 83 + 512, 350, 101));
    public Rect ButtonPressed = AutoRect.AutoTex(new Rect(350, 83 + 512, 350, 101));
    public Rect MusicButtonNormal = AutoRect.AutoTex(new Rect(0, 184 + 512, 292, 97));
    public Rect MusicButtonPressed = AutoRect.AutoTex(new Rect(292, 184 + 512, 292, 97));
    public Rect MusicOnLogoButtonNormal = AutoRect.AutoTex(new Rect(0, 281 + 512, 189, 97));
    public Rect MusicOnLogoButtonPressed = AutoRect.AutoTex(new Rect(189, 281 + 512, 189, 97));
    public Rect MusicOffLogoButtonNormal = AutoRect.AutoTex(new Rect(378, 281 + 512, 189, 97));
    public Rect MusicOffLogoButtonPressed = AutoRect.AutoTex(new Rect(567, 281 + 512, 189, 97));


}

class CreditsMenuTexturePosition
{
    public static Rect Background = AutoRect.AutoTex(new Rect(0, 0, 960, 640));
    public Rect TitleImage = AutoRect.AutoTex(new Rect(732, 711, 267, 71));
    public Rect Dialog = AutoRect.AutoTex(new Rect(0, 0, 736, 376));
    public Rect ReturnButtonNormal = AutoRect.AutoTex(new Rect(0, 640, 228, 83));
    public Rect ReturnButtonPressed = AutoRect.AutoTex(new Rect(228, 640, 228, 83));

    public Rect RightButtonNormal = AutoRect.AutoTex(new Rect(888, 0, 131, 80));
    public Rect RightButtonPressed = AutoRect.AutoTex(new Rect(888, 80, 131, 80));
}

class GameUITexturePosition
{
    public static Rect PlayerLogoBackground = AutoRect.AutoTex(new Rect(290, 0, 134, 88));
    public static Rect HPBackground = AutoRect.AutoTex(new Rect(572, 50, 288, 50));
    public static Rect HPImage = new Rect(572, 0, 288, 50);
    public static Rect WeaponLogoBackground = AutoRect.AutoTex(new Rect(424, 0, 148, 88));

    public static Rect WeaponLogoAssault = AutoRect.AutoTex(new Rect(206, 347, 206, 112));
    public static Rect WeaponLogoShotgun = AutoRect.AutoTex(new Rect(206, 347, 206, 112));
    public static Rect WeaponLogoRPG = AutoRect.AutoTex(new Rect(206, 347, 206, 112));
    public static Rect WeaponSwitchButtonLeft = AutoRect.AutoTex(new Rect(346, 65, 70, 63));
    public static Rect WeaponSwitchButtonRight = AutoRect.AutoTex(new Rect(346, 128, 64, 63));
    public static Rect WeaponSwitchButtonLeftPressed = AutoRect.AutoTex(new Rect(416, 65, 70, 63));
    public static Rect WeaponSwitchButtonRightPressed = AutoRect.AutoTex(new Rect(410, 128, 64, 63));
    public static Rect PauseButtonNormal = AutoRect.AutoTex(new Rect(808, 264, 78, 50));
    public static Rect PauseButtonPressed = AutoRect.AutoTex(new Rect(886, 264, 78, 50));
    public static Rect MoveJoystick = AutoRect.AutoTex(new Rect(0, 0, 198, 198));
    public static Rect MoveJoystickThumb = AutoRect.AutoTex(new Rect(198, 0, 92, 92));
    public static Rect ShootJoystick = AutoRect.AutoTex(new Rect(808, 314, 198, 198));
    public static Rect ShootJoystickThumb = AutoRect.AutoTex(new Rect(860, 0, 132, 124));
    public static Rect Reticle = AutoRect.AutoTex(new Rect(178, 300, 64, 40));
    public static Rect Dialog = AutoRect.AutoTex(new Rect(210, 100, 530, 400));
    public static Rect DialogSize = AutoRect.AutoTex(new Rect(210, 100, 588, 444));
    public static Rect DayClear = AutoRect.AutoTex(new Rect(464, 736, 502, 120));
    public static Rect GameOver = AutoRect.AutoTex(new Rect(464, 856, 502, 120));
    public static Rect Mask = AutoRect.AutoTex(new Rect(964, 292, 27, 22));
    public static Rect SemiMaskSize = AutoRect.AutoTex(new Rect(480, 0, 480, 640));
    public static Rect Switch = AutoRect.AutoTex(new Rect(59, 426, 34, 50));

    public static Rect GetAvatarLogoRect(int avatarLogoIndex)
    {
        return AutoRect.AutoTex(new Rect(avatarLogoIndex % 4 * 116, 336 + avatarLogoIndex / 4 * 81 + 512, 116, 81));
    }

    public static Rect GetWeaponLogoRect(int weaponLogoIndex)
    {
        if (weaponLogoIndex == 12)
        {
            return AutoRect.AutoTex(new Rect(808, 124, 194, 112));
        }
        else
        {
            return AutoRect.AutoTex(new Rect(weaponLogoIndex % 5 * 194, weaponLogoIndex / 5 * 112 + 512, 194, 112));
        }

    }

    public static Rect GetHPTextureRect(int width)
    {
        return AutoRect.AutoTex(new Rect(GameUITexturePosition.HPImage.xMin, GameUITexturePosition.HPImage.yMin, width, GameUITexturePosition.HPImage.height));

    }



    public static Rect GetNumberRect(int n)
    {


        if (n >= 0)
        {
            int row = n / 3;
            int col = n % 3;
            return AutoRect.AutoTex(new Rect(59 * col, 198 + 76 * row, 59, 76));
        }
        else
        {
            return AutoRect.AutoTex(new Rect(0, 0, 0, 0));
        }

    }




}


class GameOverTexturePosition
{
    public Rect Dialog = AutoRect.AutoTex(new Rect(0, 0, 736, 376));
    public Rect ButtonLeft = AutoRect.AutoTex(new Rect(743, 166, 213, 77));
    public Rect ButtonLeftPressed = AutoRect.AutoTex(new Rect(743, 243, 213, 77));
    public Rect ButtonRight = AutoRect.AutoTex(new Rect(743, 320, 184, 77));
    public Rect ButtonRightPressed = AutoRect.AutoTex(new Rect(743, 397, 184, 77));
}


