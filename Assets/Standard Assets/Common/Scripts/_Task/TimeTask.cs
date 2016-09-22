using UnityEngine;
using System.Collections;

//定时任务
public class TimeTask : Task 
{
    private float m_fDuringTime;
    private float m_fCallBackTime;
    private int m_iMaxCallTimes;

    public void Awake() 
    {
        base.Awake();
        m_fDuringTime = 0;
        m_fCallBackTime = 0;
        m_iMaxCallTimes = 0;
	}
	
    public void Update() 
    {
        if (enTaskState.kDoing == State)
        {
            m_fDuringTime += Time.deltaTime;
            if (m_fDuringTime >= m_fCallBackTime)
            {
                if (m_iMaxCallTimes <= 0 || CallTimes < m_iMaxCallTimes)
                {
                    ExecuteCallback(false);

                    m_fDuringTime = 0;
                    State = enTaskState.kDoing;
                }
                else
                {
                    ExecuteCallback(true);
                }
            }
        }
	}

    public float CallbackTime
    {
        set { m_fCallBackTime = value; }
    }

    public int MaxCallTimes
    {
        set { m_iMaxCallTimes = value; }
    }
}