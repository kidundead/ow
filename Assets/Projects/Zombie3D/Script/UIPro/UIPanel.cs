// Copyright (c) 2011, Triniti Interactive Company, China.
// All rights reserved.
//
// Created by Cheng Haitao at 4/28/2011 4:09:41 PM.
// E-Mail: chenghaitao@trinitigame.com.cn


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIPanel : UIControl, UIContainer
{

    protected ArrayList m_Controls = new ArrayList();

    //! UI事件处理器
    private UIHandler m_UIHandler;

    public UIPanel()
    {
        Visible = false;
        Enable = false;
    }

    public override void Draw()
    {
        base.Draw();
        if (Visible)
        {
            foreach (UIControl control in m_Controls)
            {
                if (control.Visible)
                    control.Draw();
            }
        }

    }

    public virtual void UpdateLogic()
    {

    }


    public virtual void Hide()
    {

        Visible = false;
        Enable = false;
        /*
        foreach (UIControl control in m_Controls)
        {

            UIPanel panel = control as UIPanel;
            if (panel != null)
            {
                panel.Hide();
            }
        }
         */
    }

    public virtual void Show()
    {
        Visible = true;
        Enable = true;
        /*
        foreach (UIControl control in m_Controls)
        {
            control.Visible = true;
            control.Enable = true;
            UIPanel panel = control as UIPanel;
            if (panel != null)
            {
                panel.Show();
            }
        }
         */
    }
    //! 设置事件处理器
    public void SetUIHandler(UIHandler ui_handler)
    {
        m_UIHandler = ui_handler;
    }


    public void DrawSprite(UISprite sprite)
    {
        m_Parent.DrawSprite(sprite);
    }

    public override bool HandleInput(UITouchInner touch)
    {
        for (int i = m_Controls.Count - 1; i >= 0; --i)
        {
            UIControl control = (UIControl)m_Controls[i];
            if (control.Enable)
            {
                bool handle = control.HandleInput(touch);
                if (handle)
                {
                    return true;
                }
            }
        }
        return false;
    }


    //! 发送子控件事件
    public void SendEvent(UIControl control, int command, float wparam, float lparam)
    {
        if (m_UIHandler != null)
        {
            m_UIHandler.HandleEvent(control, command, wparam, lparam);
        }
        else
        {
            m_Parent.SendEvent(this, command, wparam, lparam);
        }
    }


    public virtual void Add(UIControl control)
    {
        m_Controls.Add(control);
        control.SetParent(this);
    }


}