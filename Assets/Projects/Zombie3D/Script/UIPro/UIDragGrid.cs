// Copyright (c) 2011, Triniti Interactive Company, China.
// All rights reserved.
//
// Created by Cheng Haitao at 4/28/2011 4:12:59 PM.
// E-Mail: chenghaitao@trinitigame.com.cn


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIDragGrid : UIPanel, UIHandler
{

    //! 命令
    public enum Command
    {
        DragBegin,
        DragMove,
        DragEnd,
        DragExchange,
        DragOutSide
    }

    public class UIDragIcon
    {
        public UIMove m_UIMove;
        public UIImage m_Image;
        public UIImage m_Background;
    }

    protected List<UIDragIcon> m_dragIcons = new List<UIDragIcon>();

    protected int m_GridCount = 0;

    protected Rect m_IconRect;

    public UIDragGrid(int gridCount)
    {
        m_GridCount = gridCount;

        SetUIHandler(this);

    }

    public void AddGrid(Rect gridRect, Material backMaterial, Rect backTexPos)
    {
        UIDragIcon icon = new UIDragIcon();
        icon.m_Background = new UIImage();
        icon.m_Background.Rect = gridRect;
        icon.m_Background.SetTexture(backMaterial, backTexPos, AutoRect.AutoSize(backTexPos));
        icon.m_Background.SetParent(this);
        icon.m_Background.Enable = false;

        icon.m_Image = new UIImage();
        icon.m_Image.Rect = gridRect;
        icon.m_Image.SetParent(this);
        icon.m_Image.Enable = false;



        icon.m_UIMove = new UIMove();
        icon.m_UIMove.Rect = gridRect;
        icon.m_UIMove.Enable = true;
        this.Add(icon.m_UIMove);
        m_IconRect = gridRect;
        m_dragIcons.Add(icon);


    }

    public void SetGridTexturePosition(int fromID, int toID)
    {
        m_dragIcons[fromID].m_Image.Rect = m_dragIcons[toID].m_UIMove.Rect;
    }

    public void HideGridTexture(int gridID)
    {
        m_dragIcons[gridID].m_Image.Visible = false;
    }


    public void SetGridTexture(int gridID, Material material, Rect textRect)
    {
        m_dragIcons[gridID].m_Image.Visible = true;
        m_dragIcons[gridID].m_Image.SetTexture(material, textRect, AutoRect.AutoSize(textRect));
    }


    public override void Draw()
    {
        base.Draw();
        if (Visible)
        {
            foreach (UIDragIcon icon in m_dragIcons)
            {
                if (icon.m_Background.Visible)
                {
                    icon.m_Background.Draw();
                }
            }


            foreach (UIDragIcon icon in m_dragIcons)
            {
                if (icon.m_Image.Visible)
                {
                    icon.m_Image.Draw();
                }
            }
        }

    }


    public void HandleEvent(UIControl control, int command, float wparam, float lparam)
    {

        for (int i = 0; i < m_dragIcons.Count; i++)
        {
            UIMove uiMove = m_dragIcons[i].m_UIMove;
            UIImage uiImage = m_dragIcons[i].m_Image;

            if (control == uiMove)
            {
                if (command == (int)(UIMove.Command.MovePos))
                {
                    uiImage.Rect = new Rect(wparam - 0.5f * m_IconRect.width, lparam - 0.5f * m_IconRect.height, m_IconRect.width, m_IconRect.height);
                    //m_Parent.SendEvent(this, (int)Command.DragMove, wparam, lparam);
                }
                else if (command == (int)(UIMove.Command.End))
                {
                    uiImage.Rect = uiMove.Rect;
                    int inGrid = -1;
                    for (int j = 0; j < m_dragIcons.Count; j++)
                    {
                        if (m_dragIcons[j].m_UIMove.Rect.Contains(new Vector2(wparam, lparam)))
                        {
                            inGrid = j;
                        }
                    }

                    if (inGrid == -1)
                    {

                        uiImage.Rect = new Rect(-1000, -1000, 200, 200);
                        m_Parent.SendEvent(this, (int)Command.DragOutSide, i, 0);
                    }
                    else
                    {
                        uiImage.Rect = m_dragIcons[inGrid].m_UIMove.Rect;
                        m_Parent.SendEvent(this, (int)Command.DragExchange, i, inGrid);
                    }

                }
                else if (command == (int)(UIMove.Command.Begin))
                {
                    //m_Parent.SendEvent(this, (int)Command.DragBegin, wparam, lparam);
                }
            }

        }


    }



}