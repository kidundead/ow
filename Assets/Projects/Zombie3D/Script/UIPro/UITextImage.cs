// Copyright (c) 2011, Triniti Interactive Company, China.
// All rights reserved.
//
// Created by Cheng Haitao at 4/28/2011 4:11:08 PM.
// E-Mail: chenghaitao@trinitigame.com.cn


using UnityEngine;
using System.Collections;

public class UITextImage : UIImage, UIContainer
{
    protected UIText m_Text = new UIText();


    public void SetText(string font, string text, Color color)
    {
        m_Text.Set(font, text, color);
        m_Text.AlignStyle = UIText.enAlignStyle.center;
        m_Text.Rect = Rect;
        m_Text.SetParent(this);
    }

    public void SetTextAlign(UIText.enAlignStyle style)
    {
        m_Text.AlignStyle = style;
        m_Text.Rect = Rect;
    }

    public void SetText(string text)
    {
        m_Text.SetText(text);
    }


    public override void Draw()
    {
        base.Draw();
        m_Text.Draw();


    }

    public void DrawSprite(UISprite sprite)
    {
        m_Parent.DrawSprite(sprite);
    }

    //! 发送子控件事件
    public void SendEvent(UIControl control, int command, float wparam, float lparam)
    {
        //m_Parent.SendEvent(control, command, wparam, lparam);
    }


    public void Add(UIControl control)
    {
    }

}
