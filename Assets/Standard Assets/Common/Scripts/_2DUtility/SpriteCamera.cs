//! @file SpriteCamera.cs


using UnityEngine;
using System.Collections;


//! @class SpriteCamera
//! @brief 精灵摄像机
public class SpriteCamera : MonoBehaviour
{
	//! 使用的层
	private int m_Layer = 0;

	//! 摄像机位置
	private Transform m_Transform = null;

	//! 摄像机
	private Camera m_Camera = null;

	//! 摄像机范围
	private Rect m_Range = new Rect(-1.0f, -1.0f, 1.0f, 1.0f);

	//! 初始化
	public void Initialize(int layer)
	{
		//
		transform.position = Vector3.zero;
		transform.rotation = Quaternion.identity;
		transform.localScale = Vector3.one;

		//
		m_Layer = layer;

		//
		m_Transform = gameObject.transform;
		m_Camera = (Camera)gameObject.AddComponent(typeof(Camera));

		//
		m_Transform.position = Vector3.zero;
		m_Transform.LookAt(Vector3.zero, Vector3.up);

		//
		m_Camera.clearFlags = CameraClearFlags.Nothing;
		m_Camera.backgroundColor = Color.black;
		m_Camera.nearClipPlane = -1.0f;
		m_Camera.farClipPlane = 1.0f;
		m_Camera.orthographic = true;
		m_Camera.aspect = 1.0f;
		m_Camera.orthographicSize = 1.0f;
		m_Camera.depth = 0;
		m_Camera.cullingMask = (1 << m_Layer);
	}

	//! 是否清除
	public void SetClear(bool clear)
	{
		if (clear)
		{
			m_Camera.clearFlags = CameraClearFlags.SolidColor;
		}
		else
		{
			m_Camera.clearFlags = CameraClearFlags.Nothing;
		}
	}

	//! 设置绘制深度
	public void SetDepth(float depth)
	{
		m_Camera.depth = depth;
	}

	//! 设置可视范围
	public void SetViewport(Rect range)
	{
	#if UNITY_IPHONE
		m_Transform.position = new Vector3((range.xMin + range.xMax) / 2, (range.yMin + range.yMax) / 2, 0);
	#else // Win32
		m_Transform.position = new Vector3((range.xMin + range.xMax + 1) / 2, (range.yMin + range.yMax - 1) / 2, 0);
	#endif

		m_Camera.aspect = range.width / range.height;
		m_Camera.orthographicSize = range.height / 2;


		m_Range = range;
	}

	//! 屏幕点转化为世界点
	public Vector2 ScreenToWorld(Vector2 point)
	{
		return new Vector2(m_Range.x + point.x / Screen.width * m_Range.width, m_Range.y + point.y / Screen.height * m_Range.height);
	}
}

