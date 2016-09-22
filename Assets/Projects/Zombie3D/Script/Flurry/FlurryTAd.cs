// Copyright (c) 2011, Triniti Interactive Company, China.
// All rights reserved.
//
// Created by Cheng Haitao at 5/16/2011 3:09:04 PM.
// E-Mail: chenghaitao@trinitigame.com.cn


using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;



class FlurryTAd
{
    [DllImport("__Internal")]
    protected static extern void ShowTadView(bool bVisible);
    public static void ShowTAd(bool bVisible)
    {
#if UNITY_IPHONE
        ShowTadView(bVisible);
#endif
    }

    [DllImport("__Internal")]
    protected static extern void RotateTadView();
    public static void RotateTad()
    {
#if UNITY_IPHONE
        RotateTadView();
#endif
    }


    /*
    [DllImport("__Internal")]
    protected static extern void Initialize(string version, string key);
    public static void Init(string version, string key)
    {
#if UNITY_IPHONE
        Initialize(version, key);
#endif
    }
    */
    
}

