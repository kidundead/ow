using System;
using System.Collections.Generic;
using UnityEngine;
using Zombie3D;


class ReviewDialogUI : MonoBehaviour, UIHandler, UIDialogEventHandler
{
    public UIManager m_UIManager = null;
    protected ReviewDialog reviewDialog;

    void Start()
    {

        m_UIManager = gameObject.AddComponent<UIManager>() as UIManager;
        m_UIManager.SetParameter(9, 3, false);
        m_UIManager.SetUIHandler(this);

        m_UIManager.CLEAR = false;

        reviewDialog = new ReviewDialog();

        reviewDialog.SetDialogEventHandler(this);


        m_UIManager.Add(reviewDialog);

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

    public static ReviewDialogUI GetInstance()
    {
        return GameObject.Find("ReviewDialogUI").GetComponent<ReviewDialogUI>();
    }

    public bool IsVisible()
    {
        return reviewDialog.Visible;
    }
    public void ShowDialog()
    {
        reviewDialog.Show();
    }

    public void HideDialog()
    {
        reviewDialog.Hide();
    }

    public void HandleEvent(UIControl control, int command, float wparam, float lparam)
    {


    }

    public void Yes()
    {
        reviewDialog.Hide();
        GameApp.GetInstance().GetGameState().AddScore(1000);
        GameApp.GetInstance().Save();
        Application.OpenURL("http://www.trinitigame.com/callofminizombies/review/");
    }


    public void No()
    {
        reviewDialog.Hide();
    }
}

