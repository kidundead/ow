using UnityEngine;
using System.Collections;



public class Timer
{

    protected TimerInfo info = new TimerInfo();
    protected bool start = false;
    public string Name { get; set; }

    public void SetTimer(float interval, bool doAtStart)
    {
       
        if (doAtStart)
        {
            info.lastDoTime = -9999;
        }
        else
        {
            info.lastDoTime = Time.time;
        }
        info.interval = interval;
        start = true;
    }

    public void Do()
    {
        info.lastDoTime = Time.time;
    }

    public bool Ready()
    {
        if (start && Time.time - info.lastDoTime > info.interval)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


}
