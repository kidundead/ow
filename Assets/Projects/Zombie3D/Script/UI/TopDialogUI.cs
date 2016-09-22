using System;
using System.Collections.Generic;
using UnityEngine;
using Zombie3D;


class TopDialogUI : MonoBehaviour, UIHandler, UIDialogEventHandler
{
    public UIManager m_UIManager = null;
    protected IAPDialog iapDialog;
    void Start()
    {

        m_UIManager = gameObject.AddComponent<UIManager>() as UIManager;
        m_UIManager.SetParameter(9, 3, false);
        m_UIManager.SetUIHandler(this);

        m_UIManager.CLEAR = false;

        iapDialog = new IAPDialog(UIDialog.DialogMode.YES_OR_NO);

        iapDialog.SetDialogEventHandler(this);
        iapDialog.SetText(IAPDialog.NOT_ENOUGH_CASH);


        m_UIManager.Add(iapDialog);

        m_UIManager.SetUIHandler(this);


        //m_UIManager.Add(loadingPanel);



    }

    // Update is called once per frame
    void Update()
    {

        foreach (UITouchInner touch in iPhoneInputMgr.MockTouches())
        {
            if (m_UIManager != null)
            {
                if (m_UIManager.HandleInput(touch))
                {
                    continue;
                }
            }
        }

    }

    public static TopDialogUI GetInstance()
    {
        return GameObject.Find("TopDialogUI").GetComponent<TopDialogUI>();
    }

    public void ShowDialog()
    {
        iapDialog.Show();
    }

    public void HideDialog()
    {
        iapDialog.Hide();
    }

    public void HandleEvent(UIControl control, int command, float wparam, float lparam)
    {


    }

    public void Yes()
    {
        Debug.Log("yes");
        //Hide();
        AvatarUI avatarUIPanel = ArenaMenuUI.GetInstance().GetPanel(MenuName.AVATAR) as AvatarUI;
        avatarUIPanel.Hide();
        iapDialog.Hide();
        ShopUI shopUI = ArenaMenuUI.GetInstance().GetPanel(MenuName.SHOP) as ShopUI;
        shopUI.SetFromPanel(avatarUIPanel);
        shopUI.Show();
    }


    public void No()
    {
        iapDialog.Hide();
    }
}

