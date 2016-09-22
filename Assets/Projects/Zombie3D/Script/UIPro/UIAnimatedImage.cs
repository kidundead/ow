// Copyright (c) 2011, Triniti Interactive Company, China.
// All rights reserved.
//
// Created by Cheng Haitao at 4/28/2011 4:11:08 PM.
// E-Mail: chenghaitao@trinitigame.com.cn


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class UIAnimatedImage : UIImage
{

    protected List<TexturePosInfo> animationTexturesList = new List<TexturePosInfo>();
    protected int currentFrame = 0;
    protected int frameRate = 10;
    protected float lastFrameChangeTime = 0;
    public void AddAnimation(Material material, Rect texture_rect, Vector2 size)
    {

        animationTexturesList.Add(new TexturePosInfo(material, texture_rect, size));
        
    }

    public void SetAnimationFrameRate(int frameRate)
    {
        this.frameRate = frameRate;
    }

    public override void Draw()
    {
        Enable = false;
        TexturePosInfo info = animationTexturesList[currentFrame];
        SetTexture(info.m_Material, info.m_TexRect, info.m_Size);
        

        base.Draw();


        if (Time.time - lastFrameChangeTime > (1.0f / frameRate))
        {
            currentFrame++;
            if (currentFrame == animationTexturesList.Count)
            {
                currentFrame = 0;
            }
            lastFrameChangeTime = Time.time;

        }
    }
    
}
