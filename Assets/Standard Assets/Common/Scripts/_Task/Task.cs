using UnityEngine;
using System.Collections;

public delegate void CallbackFunc(object param, object attach, bool bFinish);

public enum enTaskState
{
    kReady,
    kDoing,
    kFinish,
}

public class Task : MonoBehaviour
{
    private enTaskState m_State = enTaskState.kReady;
    private object m_AttachParam;
    private object m_CallbackParam;
    private CallbackFunc m_Callback;
    private int m_iCallTimes;
    private bool m_Global;

    public void Awake () 
    {
        AttachParam = null;
        CallbackParam = null;
        Callback = null;
        m_iCallTimes = 0;
        State = enTaskState.kDoing;
        m_Global = false;
    }

    public object AttachParam
    {
        set { m_AttachParam = value; }
    }

    public object CallbackParam
    {
        set { m_CallbackParam = value; }
    }

    public CallbackFunc Callback
    {
        set { m_Callback = value; }
    }

    public enTaskState State
    {
        set { m_State = value; }
        get { return m_State;  }
    }

    public int CallTimes
    {
        get { return m_iCallTimes; }
    }

    public bool Global
    {
        set { m_Global = value; }
        get { return m_Global; }
    }

    protected void ExecuteCallback(bool bFinish)
    {
        if (null != m_Callback)
        {
            m_Callback(m_CallbackParam, m_AttachParam, bFinish);
            ++m_iCallTimes;
        }
        State = enTaskState.kFinish;
    }
}