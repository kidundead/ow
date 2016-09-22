using UnityEngine;
using System.Collections;


public class TimerInfo
{
    public float lastDoTime;
    public float interval;
}

public class TimerManager
{
    protected static TimerManager instance;
    public TimerInfo[] timerInfos = new TimerInfo[100];

    public static TimerManager GetInstance()
    {
        if (instance == null)
        {
            instance = new TimerManager();
        }
        return instance;
    }


    public void SetTimer(int index, float interval, bool doAtStart)
    {
        TimerInfo info = new TimerInfo();
        if (doAtStart)
        {
            info.lastDoTime = -9999;
        }
        else
        {
            info.lastDoTime = Time.time;
        }
        info.interval = interval;
        timerInfos[index] = info;

    }

    public void Do(int index)
    {
        timerInfos[index].lastDoTime = Time.time;
    }

    public bool Ready(int index)
    {
        if (Time.time - timerInfos[index].lastDoTime > timerInfos[index].interval)
        {
            return true;
        }
        else
        {
            return false;
        }
    }




}
