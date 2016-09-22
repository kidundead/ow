// Copyright (c) 2011, Triniti Interactive Company, China.
// All rights reserved.
//
// Created by Cheng Haitao at 4/28/2011 4:12:24 PM.
// E-Mail: chenghaitao@trinitigame.com.cn


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ScrollerDir
{
    Horizontal,
    Vertical
}

public class TexturePosInfo
{

    public Material m_Material;
    public Rect m_TexRect;
    public Vector2 m_Size;

    public TexturePosInfo(Material material, Rect texRect)
    {
        m_Material = material;
        m_TexRect = texRect;
    }
    public TexturePosInfo(Material material, Rect texRect, Vector2 size)
    {
        m_Material = material;
        m_TexRect = texRect;
        m_Size = size;
    }

    public TexturePosInfo()
    {
    }
}

public class UIImageScroller : UIPanel, UIHandler
{

    //! 命令
    public enum Command
    {
        ScrollSelect,
        PressSelect,
        PressEnd,
        DragMove
    }


    protected UIMove m_UIMove = new UIMove();
    protected UIImage m_CenterFrameImage = new UIImage();
    protected List<UIImage> m_overlayImageList = new List<UIImage>();
    protected List<TexturePosInfo> m_overlayInfoList = new List<TexturePosInfo>();
    protected List<UIImage> m_MaskList = new List<UIImage>();

    //protected UIImage m_MaskImage = new UIImage();

    protected int m_CountPerRow = 1;
    protected int m_IconWidth;
    protected int m_IconHeight;
    protected float m_ScrollerPos;
    protected bool m_Scalable;
    protected ScrollerDir m_Dir = ScrollerDir.Horizontal;
    protected Vector2 m_Velocity;
    protected Vector2 m_LastMove;
    protected int m_SelectionIndex = 0;
    protected float m_CenterPos;
    protected bool m_fingerOn = false;
    protected Rect m_ClipRect;
    protected Vector2 m_Spacing;
    protected int m_LongPressSelectionIndex = -1;
    protected float m_lastBeginPressTime = -1;
    protected bool m_EnableLongPress = false;

    protected Vector2 m_BeginPos;
    protected bool m_bMoveNotDrag = false;

    public UIImageScroller(Rect eventRect, Rect clipRect, int countPerRow, Vector2 iconSize, ScrollerDir dir, bool scalable)
    {
        m_UIMove.Rect = eventRect;
        m_CountPerRow = countPerRow;
        m_IconWidth = (int)iconSize.x;
        m_IconHeight = (int)iconSize.y;
        m_Spacing = iconSize;
        m_Dir = dir;
        m_Scalable = scalable;
        SetScrollerClip(clipRect);


        if (m_Dir == ScrollerDir.Vertical)
        {
            m_CenterPos = (m_ClipRect.y + 0.5f * m_ClipRect.height) - 0.5f * m_IconHeight;
        }
        else if (m_Dir == ScrollerDir.Horizontal)
        {
            m_CenterPos = (m_ClipRect.x + 0.5f * m_ClipRect.width) - 0.5f * m_IconWidth;
        }

        m_CenterFrameImage = new UIImage();
        m_CenterFrameImage.SetParent(this);
        //m_MaskImage.SetParent(this);
        this.SetUIHandler(this);

    }

    public void SetCenterFrameTexture(Material material, Rect rect)
    {
        m_CenterFrameImage.SetTexture(material, rect, AutoRect.AutoSize(rect));
        if (m_Dir == ScrollerDir.Vertical)
        {
            float x = (m_ClipRect.x + 0.5f * m_ClipRect.width) - 0.5f * rect.width;
            float y = (m_ClipRect.y + 0.5f * m_ClipRect.height) - 0.5f * rect.height;
            m_CenterFrameImage.Rect = new Rect(x, y, rect.width, rect.height);
        }

    }

    public void EnableLongPress()
    {
        m_EnableLongPress = true;
    }

    public void SetImageSpacing(Vector2 spacing)
    {
        m_Spacing = spacing;
    }

    protected void SetScrollerClip(Rect rect)
    {
        m_ClipRect = rect;

    }

    public void Reset()
    {
        SetSelection(0);
    }

    public void SetSelection(int index)
    {
        m_SelectionIndex = index;
        if (m_Dir == ScrollerDir.Horizontal)
        {
            m_ScrollerPos = m_CenterPos - m_SelectionIndex * m_Spacing.x;
        }
        else if (m_Dir == ScrollerDir.Vertical)
        {
            m_ScrollerPos = m_CenterPos + m_SelectionIndex * m_Spacing.y;
        }

    }

    public int GetSelection()
    {
        return m_SelectionIndex;
    }

    public void Clear()
    {
        m_Controls.Clear();
        m_overlayImageList.Clear();
        m_overlayInfoList.Clear();

    }

    public Rect GetCenterRect()
    {
        return new Rect((int)(m_ClipRect.x + 0.5f * (m_ClipRect.width - m_IconWidth)), m_CenterPos, m_IconWidth, m_IconHeight);
    }

    public override void Add(UIControl control)
    {
        base.Add(control);
        UIImage overlayImage = new UIImage();
        m_overlayImageList.Add(overlayImage);
        overlayImage.Visible = false;
        overlayImage.SetParent(this);

        UIImage maskImage = new UIImage();

        m_MaskList.Add(maskImage);
        maskImage.SetParent(this);

    }

    public void EnableScroll()
    {
        this.Add(m_UIMove);

        Reset();
    }

    public void SetOverlay(int iconID, int overlayID)
    {

        int index = iconID;

        if (overlayID != -1)
        {
            TexturePosInfo info = m_overlayInfoList[overlayID];

            m_overlayImageList[index].SetTexture(info.m_Material, info.m_TexRect);
            m_overlayImageList[index].Visible = true;
        }
        else
        {
            m_overlayImageList[index].Visible = false;
        }
    }

    public void AddOverlay(Material material, Rect texRect)
    {
        m_overlayInfoList.Add(new TexturePosInfo(material, texRect));
    }

    public void SetMaskImage(Material material, Rect texRect)
    {
        for (int i = 0; i < m_MaskList.Count; i++)
        {
            UIImage image = m_MaskList[i];
            image.SetTexture(material, texRect, AutoRect.AutoSize(texRect));
        }

    }

    public void HandleEvent(UIControl control, int command, float wparam, float lparam)
    {

        if (control == m_UIMove)
        {
            if (command == (int)(UIMove.Command.Move))
            {
                if (m_LongPressSelectionIndex == -1)
                {


                    m_lastBeginPressTime = -1;
                    m_LongPressSelectionIndex = -1;

                    if (m_Dir == ScrollerDir.Horizontal)
                    {
                        m_ScrollerPos += wparam;
                    }
                    else if (m_Dir == ScrollerDir.Vertical)
                    {
                        m_ScrollerPos += lparam;

                    }
                    m_LastMove = new Vector2((wparam), lparam);
                    m_fingerOn = true;

                }
                else
                {



                }

            }
            else if (command == (int)(UIMove.Command.MovePos))
            {
                if (m_LongPressSelectionIndex != -1)
                {

                    m_Parent.SendEvent(this, (int)Command.DragMove, wparam, lparam);
                }
                else
                {
                    if (m_EnableLongPress)
                    {
                        float x = (int)(m_ClipRect.x + 0.5f * (m_ClipRect.width - m_IconWidth));
                        float y = m_CenterPos;
                        Rect centerRect = new Rect(x, y, m_IconWidth, m_IconHeight);
                        Vector2 diffPos = m_BeginPos - new Vector2(wparam, lparam);
                        if (diffPos.sqrMagnitude > AutoRect.AutoValue(10) * AutoRect.AutoValue(10))
                        {
                            if (Mathf.Abs(diffPos.x) > Mathf.Abs(diffPos.y))
                            {
                                m_LongPressSelectionIndex = m_SelectionIndex;
                                m_Parent.SendEvent(this, (int)Command.PressSelect, m_SelectionIndex, 0);
                            }
                            else
                            {
                                m_bMoveNotDrag = true;
                            }
                        }

                    }

                }
            }
            else if (command == (int)(UIMove.Command.End))
            {
                float x = Mathf.Clamp(m_LastMove.x, -30, 30);
                float y = Mathf.Clamp(m_LastMove.y, -30, 30);
                m_Velocity = new Vector2(x, y);
                m_fingerOn = false;
                m_LastMove = Vector2.zero;

                if (m_EnableLongPress)
                {
                    m_lastBeginPressTime = -1;
                    m_LongPressSelectionIndex = -1;
                    m_Parent.SendEvent(this, (int)Command.PressEnd, wparam, lparam);
                }

            }
            else if (command == (int)(UIMove.Command.Begin))
            {
                if (m_EnableLongPress)
                {
                    m_BeginPos = new Vector2(wparam, lparam);
                    float x = (int)(m_ClipRect.x + 0.5f * (m_ClipRect.width - m_IconWidth));
                    float y = m_CenterPos;
                    Rect centerRect = new Rect(x, y, m_IconWidth, m_IconHeight);
                    if (centerRect.Contains(new Vector2(wparam, lparam)))
                    {

                        if (m_lastBeginPressTime == -1)
                        {
                            m_lastBeginPressTime = Time.time;
                        }
                    }
                }
            }
        }
    }

    public override void Draw()
    {
        UpdateImage();
        //base.Draw();

        if (m_fingerOn)
        {
            m_CenterFrameImage.Draw();
        }

        for (int i = 0; i < m_SelectionIndex; i++)
        {
            UIImage image = m_Controls[i] as UIImage;
            image.Draw();
            if (m_overlayImageList[i].Visible)
            {
                m_overlayImageList[i].Draw();
            }

            m_MaskList[i].Draw();

        }

        for (int i = m_Controls.Count - 2; i > m_SelectionIndex; i--)
        {
            UIImage image = m_Controls[i] as UIImage;
            image.Draw();
            if (m_overlayImageList[i].Visible)
            {
                m_overlayImageList[i].Draw();
            }
            m_MaskList[i].Draw();

        }
        if (!m_fingerOn)
        {
            m_CenterFrameImage.Draw();
        }
        UIImage selectedImage = m_Controls[m_SelectionIndex] as UIImage;
        selectedImage.Draw();
        if (m_overlayImageList[m_SelectionIndex].Visible)
        {
            m_overlayImageList[m_SelectionIndex].Draw();
        }


    }

    public void UpdateImage()
    {

        if (m_EnableLongPress)
        {

            if (m_LongPressSelectionIndex == -1 && m_lastBeginPressTime != -1 && Time.time - m_lastBeginPressTime > 1f)
            {
                m_LongPressSelectionIndex = m_SelectionIndex;
                m_Parent.SendEvent(this, (int)Command.PressSelect, m_SelectionIndex, 0);

            }
        }

        
        // Update Speed: Slow Down and Set min speed

        int minSpeed = 5;
        if (Mathf.Abs(m_Velocity.x) >= minSpeed)
        {
            m_Velocity.x -= Time.deltaTime * 60 * Mathf.Sign(m_Velocity.x);
        }
        else
        {
            if (!m_fingerOn)
            {
                m_Velocity.x = Mathf.Sign(m_Velocity.x) * minSpeed;
            }
        }
        if (Mathf.Abs(m_Velocity.y) >= minSpeed)
        {
            m_Velocity.y -= Time.deltaTime * 60 * Mathf.Sign(m_Velocity.y);
        }
        else
        {
            if (!m_fingerOn && m_Velocity.y != 0)
            {
                m_Velocity.y = Mathf.Sign(m_Velocity.y) * minSpeed;
            }

        }


        //Update Scroller Position and Stop at Center Pos
        if (m_Dir == ScrollerDir.Horizontal)
        {
            m_ScrollerPos += (int)(m_Velocity.x * Time.deltaTime * 100);

            //Clamp Scroller Pos
            m_ScrollerPos = Mathf.Clamp(m_ScrollerPos, m_CenterPos - (m_Controls.Count - 2) * m_Spacing.x, m_CenterPos);

            //Stop and Send Event
            for (int i = 0; i < m_Controls.Count - 1; i++)
            {
                float x = m_ScrollerPos + i * m_Spacing.x;
                float centerOfClip = (m_ClipRect.width * 0.5f + m_ClipRect.x);
                float centerOfIcon = x + 0.5f * m_IconWidth;
                if (Mathf.Abs(centerOfIcon - centerOfClip) < m_Spacing.x / 2)
                {
                    int selection = i;
                    m_Velocity.x = 0;
                    if (selection != m_SelectionIndex)
                    {
                        m_Parent.SendEvent(this, (int)Command.ScrollSelect, selection, 0);
                        m_SelectionIndex = selection;
                    }
                }
            }

            if (!m_fingerOn && Mathf.Abs(m_Velocity.x) <= minSpeed)
            {
                float dest = m_CenterPos - m_SelectionIndex * m_Spacing.x;
                if (Mathf.Abs(m_ScrollerPos - dest) > 20)
                {
                    m_ScrollerPos += Mathf.Sign(dest - m_ScrollerPos) * 100 * Time.deltaTime * 10;
                }
                else
                {
                    m_ScrollerPos = dest;
                }
            }

        }
        else if (m_Dir == ScrollerDir.Vertical)
        {
            //Update Scroller Position
            m_ScrollerPos += (m_Velocity.y * Time.deltaTime * 100);


            //Clamp Scroller Pos
            m_ScrollerPos = Mathf.Clamp(m_ScrollerPos, m_CenterPos, m_CenterPos + (m_Controls.Count - 2) * m_Spacing.y);

            //Stop and Send Event
            for (int i = 0; i < m_Controls.Count - 1; i++)
            {
                float y = m_ScrollerPos - i * m_Spacing.y;
                float centerOfClip = (m_ClipRect.height * 0.5f + m_ClipRect.y);
                float centerOfIcon = y + 0.5f * m_IconHeight;

                if (Mathf.Abs(centerOfIcon - centerOfClip) < m_Spacing.y / 2)
                {

                    int selection = i;

                    m_Velocity.y = 0;
                    if (selection != m_SelectionIndex)
                    {
                        m_Parent.SendEvent(this, (int)Command.ScrollSelect, selection, 0);
                        m_SelectionIndex = selection;
                    }
                }
            }

            if (!m_fingerOn && Mathf.Abs(m_Velocity.y) <= minSpeed)
            {
                float dest = m_CenterPos + m_SelectionIndex * m_Spacing.y;
                if (Mathf.Abs(m_ScrollerPos - dest) > 20)
                {
                    m_ScrollerPos += Mathf.Sign(dest - m_ScrollerPos) * 100 * Time.deltaTime * 10;
                }
                else
                {
                    m_ScrollerPos = dest;
                }
            }

        }

        //Update Image Position and Scale
        for (int i = 0; i < m_Controls.Count - 1; i++)
        {
            float x = 0;
            float y = 0;
            if (m_Dir == ScrollerDir.Horizontal)
            {
                x = m_Spacing.x * (i / m_CountPerRow) + m_ScrollerPos;
                y = m_IconHeight * (i % m_CountPerRow) + (int)(m_ClipRect.y + 0.5f * (m_ClipRect.height - m_IconHeight));
                UIImage control = m_Controls[i] as UIImage;

                control.Rect = new Rect(x, y, m_IconWidth, m_IconHeight);
                m_overlayImageList[i].Rect = new Rect(x, y, m_IconWidth, m_IconHeight);
                m_MaskList[i].Rect = control.Rect;

                if (m_SelectionIndex == i)
                {

                    control.SetTextureSize(new Vector2(m_IconWidth * 1f, m_IconHeight * 1f));
                    if (m_overlayInfoList[0] != null)
                    {
                        m_overlayImageList[i].SetTextureSize(new Vector2(m_overlayInfoList[0].m_TexRect.width * 1f, m_overlayInfoList[0].m_TexRect.height * 1f));
                    }
                }
                else
                {
                    control.SetTextureSize(new Vector2(m_IconWidth * 0.7f, m_IconHeight * 0.7f));
                    if (m_overlayInfoList[0] != null)
                    {
                        m_overlayImageList[i].SetTextureSize(new Vector2(m_overlayInfoList[0].m_TexRect.width * 0.7f, m_overlayInfoList[0].m_TexRect.height * 0.7f));
                    }
                }

                control.SetClip(m_ClipRect);
                m_overlayImageList[i].SetClip(m_ClipRect);
            }
            else if (m_Dir == ScrollerDir.Vertical)
            {
                int index = i;
                x = m_IconWidth * (index % m_CountPerRow) + (int)(m_ClipRect.x + 0.5f * (m_ClipRect.width - m_IconWidth));
                y = m_ScrollerPos - m_Spacing.y * (index / m_CountPerRow);
                y = Mathf.Clamp(y, m_CenterPos - m_Spacing.y * 1.5f, m_CenterPos + m_Spacing.y * 1.5f);

                UIImage control = m_Controls[i] as UIImage;

                control.Rect = new Rect(x, y, m_IconWidth, m_IconHeight);

                m_overlayImageList[index].Rect = new Rect(x, y, m_IconWidth, m_IconHeight);

                m_MaskList[index].Rect = new Rect(x, y, m_IconWidth, m_IconHeight);

                /*
                if (m_SelectionIndex == i)
                {

                    control.SetTextureSize(new Vector2(m_IconWidth * 1f, m_IconHeight * 1f));
                    if (m_overlayImageList[index] != null)
                    {
                        m_overlayImageList[index].SetTextureSize(new Vector2(m_overlayInfoList[0].m_TexRect.width * 1f, m_overlayInfoList[0].m_TexRect.height * 1f));
                    }
                }
                else
                {
                */
                if (m_Scalable)
                {
                    float centerY = control.Rect.y + control.Rect.height * 0.5f;
                    float centerPanelY = m_ClipRect.y + m_ClipRect.height * 0.5f;
                    float dis = Mathf.Abs(centerPanelY - centerY);
                    float scaleFactor = (1 - dis / m_ClipRect.height);
                    control.SetTextureSize(new Vector2(m_IconWidth * scaleFactor, m_IconHeight * scaleFactor));
                    m_MaskList[index].SetTextureSize(new Vector2(m_IconWidth * scaleFactor, m_IconHeight * scaleFactor));
                    if (m_overlayInfoList[0] != null)
                    {
                        m_overlayImageList[index].SetTextureSize(AutoRect.AutoSize(new Vector2(m_overlayInfoList[0].m_TexRect.width * scaleFactor, m_overlayInfoList[0].m_TexRect.height * scaleFactor)));
                    }
                }


                /*
                control.SetTextureSize(new Vector2(m_IconWidth * 0.7f, m_IconHeight * 0.7f));
                if (m_overlayInfoList[0] != null)
                {
                    m_overlayImageList[index].SetTextureSize(new Vector2(m_overlayInfoList[0].m_TexRect.width * 0.7f, m_overlayInfoList[0].m_TexRect.height * 0.7f));
                }
                */
                //}

                control.SetClip(m_ClipRect);
                m_overlayImageList[index].SetClip(m_ClipRect);
                m_MaskList[index].SetClip(m_ClipRect);


            }

        }
    }


}