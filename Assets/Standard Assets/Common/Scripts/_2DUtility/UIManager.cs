//! @file UIManager.cs


using UnityEngine;
using System.Collections;


//! @class UIHandler
//! @brief UI事件处理器
public interface UIHandler
{
	void HandleEvent(UIControl control, int command, float wparam, float lparam);
}


//! @class UIManager
//! @brief UI控件管理器
public class UIManager : MonoBehaviour, UIContainer
{
	//! 使用的层(外部指定)
	public int LAYER = 0;

	//! 深度(外部指定)
	public int DEPTH = 0;

	//! 清屏(外部指定)
	public bool CLEAR = false;

	//! UIMesh
	private UIMesh m_UIMesh;

	//! SpriteCamera
	private SpriteCamera m_SpriteCamera;

	//! UI事件处理器
	private UIHandler m_UIHandler;

	//! 控件数组
	private ArrayList m_Controls;

	//! 构造
	public UIManager()
	{
		m_UIMesh = null;
		m_SpriteCamera = null;
		m_UIHandler = null;
		m_Controls = new ArrayList();
	}

	//! 设置事件处理器
	public void SetUIHandler(UIHandler ui_handler)
	{
		m_UIHandler = ui_handler;
	}

	//! 添加子控件
	public void Add(UIControl control)
	{
		m_Controls.Add(control);
		control.SetParent(this);
	}

	//! 移除子控件
	public void Remove(UIControl control)
	{
		m_Controls.Remove(control);
	}

	//! 移除全部子控件
	public void RemoveAll()
	{
		m_Controls.Clear();
	}

// 	//! 处理输入事件
// 	public bool HandleInput(Touch touch)
// 	{
//         UITouchInner touch_inner = new UITouchInner();
//         touch_inner.deltaPosition = touch.deltaPosition;
//         touch_inner.deltaTime = touch.deltaTime;
//         touch_inner.fingerId = touch.fingerId;
//         touch_inner.phase = touch.phase;
//         touch_inner.position = touch.position;
//         touch_inner.tapCount = touch.tapCount;
//         return HandleInputInner(touch_inner);
// 	}

    public bool HandleInput(UITouchInner touch)
    {
        for (int i = m_Controls.Count - 1; i >= 0; --i)
        {
            UIControl control = (UIControl)m_Controls[i];
            if (control.Enable)
            {
                bool handle = control.HandleInput(touch);
                if (handle)
                {
                    return true;
                }
            }
        }
        return false;
    }

	public void Awake()
	{
	}

	public void SetParameter(int layer, int depth, bool clear)
	{
		LAYER = layer;
		DEPTH = depth;
		CLEAR = clear;
	}

	public void Start()
	{
		Initialize();
		InitializeSpriteMesh();
		InitializeSpriteCamera();
	}

	public void LateUpdate()
	{
		// 处理绘制
		m_UIMesh.RemoveAll();
		for (int i = 0; i < m_Controls.Count; ++i)
		{
			UIControl control = (UIControl)m_Controls[i];
			control.Update();
			if (control.Visible)
			{
				control.Draw();
			}
		}

		m_UIMesh.DoLateUpdate();
	}

	//! 绘制子控件精灵
	public void DrawSprite(UISprite sprite)
	{
		m_UIMesh.Add(sprite);
	}

	//! 发送子控件事件
	public void SendEvent(UIControl control, int command, float wparam, float lparam)
	{
		if (m_UIHandler != null)
		{
			m_UIHandler.HandleEvent(control, command, wparam, lparam);
		}
	}

	private void Initialize()
	{
		transform.position = Vector3.zero;
		transform.rotation = Quaternion.identity;
		transform.localScale = Vector3.one;
	}

	private void InitializeSpriteMesh()
	{
		GameObject obj = new GameObject("UIMesh");
		obj.transform.parent = gameObject.transform;

		m_UIMesh = (UIMesh)obj.AddComponent(typeof(UIMesh));
		m_UIMesh.Initialize(LAYER);
	}

	private void InitializeSpriteCamera()
	{
		GameObject obj = new GameObject("SpriteCamera");
		obj.transform.parent = gameObject.transform;

		m_SpriteCamera = (SpriteCamera)obj.AddComponent(typeof(SpriteCamera));
		m_SpriteCamera.Initialize(LAYER);
		m_SpriteCamera.SetClear(CLEAR);
		m_SpriteCamera.SetDepth(DEPTH);
		m_SpriteCamera.SetViewport(new Rect(0, 0, Screen.width, Screen.height));
	}
}

