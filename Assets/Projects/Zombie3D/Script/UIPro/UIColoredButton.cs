// Copyright (c) 2011, Triniti Interactive Company, China.
// All rights reserved.
//
// Created by Cheng Haitao at 4/28/2011 4:11:08 PM.
// E-Mail: chenghaitao@trinitigame.com.cn


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class UIColoredButton : UIClickButton
{

    protected Color animatedColor = Color.red;
    protected float alpha = 0.0f;
    protected bool bIncreasing = true;

    public void SetAnimatedColor(Color color)
    {
        animatedColor = color;
    }

    public override void Draw()
    {
        /*
        if (bIncreasing)
        {
            alpha += Time.deltaTime;
            if (alpha >= 1)
            {
                alpha = 1.0f;
                bIncreasing = false;
               
            }

        }
        else
        {
            alpha -= Time.deltaTime;
            if (alpha <= 0)
            {
                alpha = 0.0f;
                bIncreasing = true;
            }
        }
        */

        alpha = Mathf.PingPong(Time.time*4, 1);
        
        SetColor(UIButtonBase.State.Normal, new Color(animatedColor.r, animatedColor.g, animatedColor.b, alpha));
        SetColor(UIButtonBase.State.Pressed, new Color(animatedColor.r, animatedColor.g, animatedColor.b, 1));
       
        base.Draw();

    }
    
}
