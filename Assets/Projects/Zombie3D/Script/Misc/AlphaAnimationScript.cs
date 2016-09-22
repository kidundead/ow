using UnityEngine;
using System.Collections;
using Zombie3D;
public class AlphaAnimationScript : MonoBehaviour
{

    public float maxAlpha = 1;
    public float minAlpha = 0;
    public float animationSpeed = 5.5f;

    public float maxBright = 1;
    public float minBright = 0;

    public bool enableAlphaAnimation = false;
    public bool enableBrightAnimation = false;

    public string colorPropertyName = "_TintColor";

    protected float alpha;
    protected float startTime;
    protected bool increasing = true;

    public Color startColor = Color.yellow;
    protected float lastUpdateTime;
    protected float deltaTime = 0;
    //protected Color color;


    // Use this for initialization
    void Start()
    {
 
        startTime = Time.time;
    }




    // Update is called once per frame
    void Update()
    {

        deltaTime += Time.deltaTime;


        if (deltaTime < 0.02f)
        {
            return;
        }
        Color color = Color.white;
        if (enableAlphaAnimation || enableBrightAnimation)
        {
            color = GetComponent<Renderer>().material.GetColor(colorPropertyName);
        }

        if (enableAlphaAnimation)
        {
            if (increasing)
            {
                color.a += animationSpeed * deltaTime;
                color.a = Mathf.Clamp(color.a, minAlpha, maxAlpha);
                if (color.a == maxAlpha)
                {
                    increasing = false;
                }
            }
            else
            {
                color.a -= animationSpeed * deltaTime;
                color.a = Mathf.Clamp(color.a, minAlpha, maxAlpha);
                if (color.a == minAlpha)
                {
                    increasing = true;
                }
            }
        }

        if (enableBrightAnimation)
        {
            if (increasing)
            {
                color.r += animationSpeed * deltaTime;
                color.g += animationSpeed * deltaTime;
                color.b += animationSpeed * deltaTime;

                //color.r = Mathf.Clamp(color.r, minBright, maxBright);
                // color.g = Mathf.Clamp(color.g, minBright, maxBright);
                //color.b = Mathf.Clamp(color.b, minBright, maxBright);
                if (color.r >= maxBright || color.g >= maxBright || color.b >= maxBright)
                {
                    increasing = false;
                }
            }
            else
            {
                color.r -= animationSpeed * deltaTime;
                color.g -= animationSpeed * deltaTime;
                color.b -= animationSpeed * deltaTime;

                //color.r = Mathf.Clamp(color.r, minBright, maxBright);
                //color.g = Mathf.Clamp(color.g, minBright, maxBright);
                //color.b = Mathf.Clamp(color.b, minBright, maxBright);

                if (color.r <= minBright || color.g <= minBright || color.b <= minBright)
                {
                    increasing = true;
                }
            }
        }



        GetComponent<Renderer>().material.SetColor(colorPropertyName, color);
        deltaTime = 0.0f;
    }
}
