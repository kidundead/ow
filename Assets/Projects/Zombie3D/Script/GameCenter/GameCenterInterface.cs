
using UnityEngine;
using System;
using System.Runtime.InteropServices;
using System.Collections;
using System.Text;


public class GameCenterInterface
{
#if UNITY_IPHONE

[DllImport("__Internal")]
protected static extern void GMInitialize();

[DllImport("__Internal")]
protected static extern void GMUninitialize();

[DllImport("__Internal")]
protected static extern bool GMIsSupported();

[DllImport("__Internal")]
protected static extern bool GMIsLogin();

[DllImport("__Internal")]
protected static extern bool GMLogin();

[DllImport("__Internal")]
protected static extern int GMLoginStatus();

[DllImport("__Internal")]
protected static extern bool GMGetAccount([Out] [MarshalAs(UnmanagedType.LPStr, SizeConst=1024)] StringBuilder account);

[DllImport("__Internal")]
protected static extern bool GMGetName([Out] [MarshalAs(UnmanagedType.LPStr, SizeConst=1024)] StringBuilder name);

[DllImport("__Internal")]
protected static extern bool GMSubmitScore(string category, int score);

[DllImport("__Internal")]
protected static extern int GMSubmitScoreStatus(string category, int score);

[DllImport("__Internal")]
protected static extern bool GMSubmitAchievement(string category, int percent);

[DllImport("__Internal")]
protected static extern int GMSubmitAchievementStatus(string category, int percent);

[DllImport("__Internal")]
protected static extern bool GMOpenLeaderboard();

[DllImport("__Internal")]
protected static extern bool GMOpenLeaderboardForCategory(string category);

[DllImport("__Internal")]
protected static extern bool GMOpenAchievement();
	
#endif
    public static void Initialize()
    {
#if UNITY_IPHONE
        if (!Application.isEditor)
        {
            GMInitialize();
        }
#endif
    }

    public static void Uninitialize()
    {
#if UNITY_IPHONE
        if (!Application.isEditor)
        {
            GMUninitialize();
        }
#endif
    }

    public static bool IsSupported()
    {
        bool bRet = true;
#if UNITY_IPHONE
        if (!Application.isEditor)
        {
             bRet = GMIsSupported();
        }
#endif
        return bRet;
    }

    public static bool IsLogin()
    {
        bool bRet = false;
#if UNITY_IPHONE
        if (!Application.isEditor)
        {
            bRet = GMIsLogin();
        }
#endif
        return bRet;
    }

    public static bool Login()
    {
        bool bRet = false;
#if UNITY_IPHONE
        if (!Application.isEditor)
        {
            bRet = GMLogin();
        }
        
#endif
        return bRet;
    }

    public static int LoginStatus()
    {
        int iRet = 0;
#if UNITY_IPHONE
        if (!Application.isEditor)
        {
            iRet = GMLoginStatus();
        }
        
#endif
        return iRet;
    }

    public static string GetAccount()
    {
        string strRet="";
#if UNITY_IPHONE
        if (!Application.isEditor)
        {
            StringBuilder account = new StringBuilder(1024);
            bool bRet = GMGetAccount(account);
            if (bRet)
            {
                strRet = account.ToString();
            }
        }
        
#endif
        return strRet;
    }

    public static string GetName()
    {
        string strRet = "";
#if UNITY_IPHONE
        if (!Application.isEditor)
        {
            StringBuilder name = new StringBuilder(1024);
            bool bRet = GMGetName(name);
            if (bRet)
            {
            strRet = name.ToString();
            }
        }
#endif
        return strRet;
    }

    public static bool SubmitScore(string category, int score)
    {
        bool bRet = false;
#if UNITY_IPHONE
        if (!Application.isEditor)
        {
            bRet = GMSubmitScore(category, score);
        }
        
#endif
        return bRet;
    }

    public static int SubmitScoreStatus(string category, int score)
    {
        int iRet = 0;
#if UNITY_IPHONE
        if (!Application.isEditor)
        {
            iRet = GMSubmitScoreStatus(category, score);
        }
        
#endif
        return iRet;
    }

    public static bool SubmitAchievement(string category, int percent)
    {
        bool bRet = false;
#if UNITY_IPHONE
        if (!Application.isEditor)
        {
            bRet = GMSubmitAchievement(category, percent);
        }
        
#endif
        return bRet;
    }

    public static int SubmitAchievementStatus(string category, int percent)
    {
        int iRet = 0;
#if UNITY_IPHONE
        if (!Application.isEditor)
        {
            iRet = GMSubmitAchievementStatus(category, percent);
        }
        
#endif
        return iRet;
    }

    public static bool OpenLeaderboard()
    {
        bool bRet = false;
#if UNITY_IPHONE
        if (!Application.isEditor)
        {
            bRet = GMOpenLeaderboard();
        }
        
#endif
        return bRet;
    }

    public static bool OpenLeaderboard(string category)
    {
        bool bRet = false;
#if UNITY_IPHONE
        if (!Application.isEditor)
        {
             bRet = GMOpenLeaderboardForCategory(category);
        }
       
#endif
        return bRet;
    }

    public static bool OpenAchievement()
    {
        bool bRet = false;
#if UNITY_IPHONE
        if (!Application.isEditor)
        {
            bRet = GMOpenAchievement();
        }
        
#endif
        return bRet;
    }
}