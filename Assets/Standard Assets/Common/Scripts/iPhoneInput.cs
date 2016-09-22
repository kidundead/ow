//! @file Input.cs


using UnityEngine;

public struct UITouchInner
{
	public int fingerId;
	public Vector2 position;
	public Vector2 deltaPosition;
	public float deltaTime;
	public int tapCount;
	public TouchPhase phase;
}

 public class iPhoneInputMgr
 {
 	private static UITouchInner[] touches = new UITouchInner[0];
 	
#if UNITY_IPHONE
    private static UITouchInner[] touches0 = new UITouchInner[0];
    private static UITouchInner[] touches1 = new UITouchInner[1];
    private static UITouchInner[] touches2 = new UITouchInner[2];
#else 
    private static bool buttonDown = false;
 	private static Vector2 lastPosition = new Vector2(0, 0);
 	private static float lastTime = 0;
    private static int lastFrameCounter = -1;
#endif

    public static UITouchInner[] MockTouches()
    {
#if UNITY_IPHONE
        if (0==Input.touches.Length)
        {
            return touches0;
        }
        else if (1==Input.touches.Length)
        {
            int i=0;
            foreach (Touch touch in Input.touches)
            {
                touches1[i].deltaPosition = touch.deltaPosition;
                touches1[i].deltaTime = touch.deltaTime;
                touches1[i].fingerId = touch.fingerId;
                touches1[i].phase = touch.phase;
                touches1[i].position = touch.position;
                touches1[i].tapCount = touch.tapCount;
                ++i;
            }
            return touches1;
        }
        else if (2==Input.touches.Length)
        {
            int i=0;
            foreach (Touch touch in Input.touches)
            {
                touches2[i].deltaPosition = touch.deltaPosition;
                touches2[i].deltaTime = touch.deltaTime;
                touches2[i].fingerId = touch.fingerId;
                touches2[i].phase = touch.phase;
                touches2[i].position = touch.position;
                touches2[i].tapCount = touch.tapCount;
                ++i;
            }
            return touches2;
        }
        else
        {
            touches = new UITouchInner[Input.touches.Length];
            int i=0;
            foreach (Touch touch in Input.touches)
            {
                touches[i].deltaPosition = touch.deltaPosition;
                touches[i].deltaTime = touch.deltaTime;
                touches[i].fingerId = touch.fingerId;
                touches[i].phase = touch.phase;
                touches[i].position = touch.position;
                touches[i].tapCount = touch.tapCount;
                ++i;
            }
            return touches;
        }

#else
        return DoMockTouches();
#endif
    }

#if !UNITY_IPHONE
    public static UITouchInner[] DoMockTouches()
    {
        if (Time.frameCount == lastFrameCounter)
        {
            return touches;
        }

        lastFrameCounter = Time.frameCount;
        
        if (Input.GetMouseButton(0))
		{
			if (buttonDown)
			{
				if ((Input.mousePosition.x != lastPosition.x) || (Input.mousePosition.y != lastPosition.y))
				{
					touches = new UITouchInner[1];
					touches[0].fingerId = 0;
					touches[0].position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
					touches[0].deltaPosition = touches[0].position - lastPosition;
					touches[0].deltaTime = Time.time - lastTime;
					touches[0].tapCount = 0;
					touches[0].phase = TouchPhase.Moved;
	
					lastPosition = touches[0].position;
					lastTime = Time.time;
				}
				else
				{
					touches = new UITouchInner[0];
				}
			}
			else
			{
				touches = new UITouchInner[1];
				touches[0].fingerId = 0;
				touches[0].position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
				touches[0].deltaPosition = new Vector2(0, 0);
				touches[0].deltaTime = 0;
				touches[0].tapCount = 0;
				touches[0].phase = TouchPhase.Began;

				buttonDown = true;
				lastPosition = touches[0].position;
				lastTime = Time.time;
			}
		}
		else
		{
			if (buttonDown)
			{
				touches = new UITouchInner[1];
				touches[0].fingerId = 0;
				touches[0].position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
				touches[0].deltaPosition = new Vector2(0, 0);
				touches[0].deltaTime = 0;
				touches[0].tapCount = 0;
				touches[0].phase = TouchPhase.Ended;

				buttonDown = false;
			}
			else
			{
				touches = new UITouchInner[0];
			}
        }

        return touches;
	}
#endif

 }



