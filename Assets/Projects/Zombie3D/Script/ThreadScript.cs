using UnityEngine;
using System.Collections;
using System.Threading;


public class ThreadScript : MonoBehaviour
{


    protected static Vector3 p = Vector3.zero;
    protected static float lastTime = 0;
    protected static object o = new object();
    // Use this for initialization
    void Start()
    {

        Thread t = new Thread(DoWork);
        t.Start();

    }

    public static void DoWork()
    {
        while (true)
        {
            Thread.Sleep(3000);

            System.Random r = new System.Random();
            lock (o)
            {
                p.x = (float)r.NextDouble() * 10;

                p.y = (float)r.NextDouble() * 10;
                Thread.Sleep(100);
                p.z = (float)r.NextDouble() * 10;
            }
            Debug.Log(p);

        }
    }

    // Update is called once per frame
    void Update()
    {
        lock (o)
        {
            p.x = 800.0f;
            p.y = 800.0f;
            p.z = 800.0f;
        }
    }
}
