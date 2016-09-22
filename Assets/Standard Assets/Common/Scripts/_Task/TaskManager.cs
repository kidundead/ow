using UnityEngine;
using System.Collections;

public class TaskMangager
{
    private static TaskMangager s_intance = null;

    public static TaskMangager Instance()
    {
        if (null == s_intance)
        {
            s_intance = new TaskMangager();
        }
        return s_intance;
    }

    private PropUtils m_TaskMap = new PropUtils();

    public bool AddTimeTask(string taskname, float calltime, int maxCallNum,
                            CallbackFunc func, object param, object attach, bool bGlobal)
    {
        if (null != m_TaskMap.GetObject(taskname))
        {
            return false;
        }

        GameObject obj = new GameObject(taskname);
        m_TaskMap.SetProp(taskname, obj);
        TimeTask task = obj.AddComponent<TimeTask>() as TimeTask;
        task.Callback = func;
        task.CallbackParam = param;
        task.Global = bGlobal;
        task.CallbackTime = calltime;
        task.MaxCallTimes = maxCallNum;
        if (bGlobal)
        {
            GameObject.DontDestroyOnLoad(obj);
        }
        return true;
    }

    public Task GetTask(string taskname)
    {
        GameObject obj = m_TaskMap.GetGameObject(taskname);
        if (null != obj)
        {
            return obj.GetComponent("Task") as Task;
        }
        return null;
    }

    public void RemoveTask(string taskname)
    {
        GameObject obj = m_TaskMap.GetGameObject(taskname);
        if (null != obj)
        {
            m_TaskMap.RemoveProp(taskname);
            Task task = obj.GetComponent("Task") as Task;
            if (null != task)
            {
                if (task.Global)
                {
                    GameObject.Destroy(obj);
                }
            }
        }
        else
        {
            Debug.Log("dont fint the Task = " + taskname + " : " + UnityEngine.Random.Range(0, 1000));
        }
    }
        
}