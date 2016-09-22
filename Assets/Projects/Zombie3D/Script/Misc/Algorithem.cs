using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IComparable
{

    bool GreaterThan(IComparable c);

}

public class Algorithem<T>
{

    public static void RandomSort(T[] array)
    {
        for (int j = 0; j < array.Length; j++)
        {
            int pos = Random.Range(j, array.Length);
            T temp = array[pos];
            array[pos] = array[j];
            array[j] = temp;
        }
        
    }


    


}


public class Math
{

    public static float SignificantFigures(float f, int digitalNum)
    {
        string s = f.ToString("0.0000000000");
        int i = Mathf.Max(s.IndexOf("."), digitalNum);
        s = s.Remove(i);
        f = float.Parse(s);
        return f;
    }

    public static bool RandomRate(float rate)
    {
        int rnd = Random.Range(0,100);
        if(rnd < rate)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public static Rect AddRect(Rect rect1, Rect rect2)
    {
        return new Rect(rect1.x + rect2.x, rect1.y + rect2.y, rect1.width + rect2.width, rect1.height + rect2.height);
    }

}