using UnityEngine;
using System.Collections;
using Zombie3D;

public class ScreenDirection : MonoBehaviour
{

    protected float startTime;
    // Use this for initialization
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ResetDirection()
    {
        /*
        if (Input.deviceOrientation == DeviceOrientation.Portrait)
        {
            iPhoneSettings.screenOrientation = iPhoneScreenOrientation.Portrait;
        }
        */
#if UNITY_IPHONE

        if (GameApp.GetInstance().PreviousOrientation != Input.deviceOrientation)
        {
            FlurryTAd.RotateTad();
            GameApp.GetInstance().PreviousOrientation = Input.deviceOrientation;

            if (Input.deviceOrientation == DeviceOrientation.LandscapeRight)
            {
                iPhoneSettings.screenOrientation = iPhoneScreenOrientation.LandscapeRight;
            }
            else if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft)
            {
                iPhoneSettings.screenOrientation = iPhoneScreenOrientation.LandscapeLeft;
            }
        }


#endif
        /*
        if (Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown)
        {
            iPhoneSettings.screenOrientation = iPhoneScreenOrientation.PortraitUpsideDown;
        }
        */
    }

    void LateUpdate()
    {
        //////// screen orientation 
        /*
    	if ((iPhoneInput.orientation == iPhoneOrientation.LandscapeLeft) && (iPhoneSettings.screenOrientation != iPhoneScreenOrientation.LandscapeLeft))
		{
        iPhoneSettings.screenOrientation = iPhoneScreenOrientation.LandscapeLeft; 
    	}             
        if ((iPhoneInput.orientation == iPhoneOrientation.LandscapeRight) && (iPhoneSettings.screenOrientation != iPhoneScreenOrientation.LandscapeRight))
		{
         iPhoneSettings.screenOrientation = iPhoneScreenOrientation.LandscapeRight; 
    	}
        */
        //if (Time.time - startTime > 1.0f)
        {
            ResetDirection();
        }

    }
}

