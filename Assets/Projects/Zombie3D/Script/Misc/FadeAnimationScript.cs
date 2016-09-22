using UnityEngine;
using System.Collections;
using Zombie3D;
public class FadeAnimationScript : MonoBehaviour
{
    public Color startColor = Color.black;            //! 开始颜色：黑色
    public Color endColor = new Color(0,0,0,0);       //! 结束颜色：白色
    
    public float animationSpeed = 0.5f;               //! 播放速度


    public bool enableAlphaAnimation = false;


    public string colorPropertyName = "_TintColor";


    protected float deltaTime = 0;

    // Use this for initialization
    void Start()
    {
        GetComponent<Renderer>().material.SetColor(colorPropertyName, startColor);    //! 设置 开始 颜色 
        
    }

	//! 重新设置 淡入淡出
    public void StartFade(Color startColor, Color endColor)
    {
        this.startColor = startColor;
        this.endColor = endColor;
        GetComponent<Renderer>().material.SetColor(colorPropertyName, startColor);
        enableAlphaAnimation = true;
        
    }

	//! 重新设置 淡入淡出 + delta time
    public void StartFade(Color startColor, Color endColor, float time)
    {
        StartFade(startColor, endColor);
        if (time != 0)
        {
            animationSpeed = 1.0f / time;
        }

    }

    public bool FadeOutComplete()
    {
        Color color = GetComponent<Renderer>().material.GetColor(colorPropertyName);
        return (color.a == 0);
    }

    public bool FadeInComplete()
    {
        Color color = GetComponent<Renderer>().material.GetColor(colorPropertyName);
        return (color.a == 1);
    }

    public static FadeAnimationScript GetInstance()
    {
        return GameObject.Find("CameraFade").GetComponent<FadeAnimationScript>();
    }

    public void FadeInBlack()
    {
        StartFade(new Color(0,0,0,0), Color.black);
    }

    public void FadeInBlack(float time)
    {
        StartFade(new Color(0, 0, 0, 0), Color.black, time);
    }

    public void FadeOutBlack()
    {
        StartFade(Color.black,new Color(0, 0, 0, 0));
    }


    // Update is called once per frame
    void Update()
    {

        deltaTime += Time.deltaTime;     //! 累加持续时间 


        if (deltaTime < 0.02f)
        {
            return;
        }

        


        if (enableAlphaAnimation)
        {
            float startAlpha = startColor.a;
            float endAlpha = endColor.a;


            float sign = Mathf.Sign((endAlpha - startAlpha));

            Color color = GetComponent<Renderer>().material.GetColor(colorPropertyName);

            color.a += sign * animationSpeed * deltaTime;
            if (Mathf.Sign( endAlpha - color.a) != sign)
            {
                color.a = endAlpha;
                enableAlphaAnimation = false;
            }

            GetComponent<Renderer>().material.SetColor(colorPropertyName, color);     //! 设置alpha值
        }

        deltaTime = 0.0f;
    }
}
