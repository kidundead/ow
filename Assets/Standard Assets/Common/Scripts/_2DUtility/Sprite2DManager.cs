//! @file Sprite2DManager.cs


using UnityEngine;
using System.Collections;


//! @class Sprite2DManager
//! @brief 2D精灵管理器
public class Sprite2DManager : MonoBehaviour
{
	//! 使用的层(外部指定)
	public int LAYER = 0;

	//! 最大精灵层数(外部指定)
	public int MAX_SPRITE_LAYER = 16;

	//! SpriteMesh
	private SpriteMesh m_SpriteMesh = null;

	//! SpriteCamera
	private SpriteCamera m_SpriteCamera = null;

	//! 构造
	public Sprite2DManager()
	{
	}

	//! 析构
	~Sprite2DManager()
	{
	}

	public void Awake()
	{
		Initialize();
		InitializeSpriteMesh();
		InitializeSpriteCamera();
	}

	public void Start()
	{
	}

	//! 添加2D精灵
	public void Add(Sprite2D sprite)
	{
		m_SpriteMesh.Add(sprite);
	}

	//! 移除2D精灵
	public void Remove(Sprite2D sprite)
	{
		m_SpriteMesh.Remove(sprite);
	}

	//! 移除全部2D精灵
	public void RemoveAll()
	{
		m_SpriteMesh.RemoveAll();
	}

	//! 取SpriteMesh
	public SpriteMesh GetSpriteMesh()
	{
		return m_SpriteMesh;
	}

	//! 取SpriteCamera
	public SpriteCamera GetSpriteCamera()
	{
		return m_SpriteCamera;
	}

	//! 设置可视范围
	public void SetViewport(Rect range)
	{
		m_SpriteCamera.SetViewport(range);
	}

	//! 设置可视范围
	public void SetViewport(Vector2 position, Vector2 size)
	{
		Rect range = new Rect(position.x - size.x / 2, position.y - size.y / 2, size.x, size.y);
		m_SpriteCamera.SetViewport(range);
	}

	//! 屏幕点转化为世界点
	public Vector2 ScreenToWorld(Vector2 point)
	{
		return m_SpriteCamera.ScreenToWorld(point);
	}

	private void Initialize()
	{
		transform.position = Vector3.zero;
		transform.rotation = Quaternion.identity;
		transform.localScale = Vector3.one;
	}

	private void InitializeSpriteMesh()
	{
		GameObject obj = new GameObject("SpriteMesh");
		obj.transform.parent = gameObject.transform;

		m_SpriteMesh = (SpriteMesh)obj.AddComponent(typeof(SpriteMesh));
		m_SpriteMesh.Initialize(LAYER, MAX_SPRITE_LAYER);
	}

	private void InitializeSpriteCamera()
	{
		GameObject obj = new GameObject("SpriteCamera");
		obj.transform.parent = gameObject.transform;

		m_SpriteCamera = (SpriteCamera)obj.AddComponent(typeof(SpriteCamera));
		m_SpriteCamera.Initialize(LAYER);
		m_SpriteCamera.SetClear(true);
		m_SpriteCamera.SetDepth(0);
	}
}

