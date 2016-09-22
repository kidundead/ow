//! @file Sprite.cs


using UnityEngine;


//! @class Sprite
//! @brief 精灵
public class Sprite
{
	//! 尺寸
	protected Vector2 m_Size;

	//! 位置
	protected Vector2 m_Position;

	//! 旋转
	protected float m_Rotation;

	//! 层
	protected int m_Layer;

	//! 材质
	protected Material m_Material;

	//! 贴图区域
	protected Rect m_TextureRect;

	//! 水平翻转
	protected bool m_FlipX;

	//! 垂直翻转
	protected bool m_FlipY;

	//! 颜色
	protected Color m_Color;

	//! 顶点坐标
	protected Vector3 [] m_Vertices = new Vector3[4];

	//! 是否需要更新顶点坐标
	protected bool m_UpdateVertices;

	//! UV坐标
	protected Vector3 [] m_UV = new Vector3[4];

	//! 是否需要更新UV坐标
	protected bool m_UpdateUV;

	//! 顶点索引
	static protected int [] m_Triangles = new int[6];


	//! 最小层号
	public const int MinLayer = 0;

	//! 最大层号
	public const int MaxLayer = 15;


	//! 静态构造
	static Sprite()
	{
		// 0 ------ 1
		// |        |
		// |        |
		// |        |
		// 3 ------ 2

		m_Triangles[0] = 0;
		m_Triangles[1] = 3;
		m_Triangles[2] = 1;

		m_Triangles[3] = 3;
		m_Triangles[4] = 2;
		m_Triangles[5] = 1;
	}

	//! 构造
	public Sprite()
	{
		m_Position = Vector2.zero;
		m_Size = Vector2.zero;
		m_Rotation = 0;
		m_Layer = 0;
		m_Material = null;
		m_TextureRect = new Rect(0, 0, 0, 0);
		m_FlipX = false;
		m_FlipY = false;
		m_Color = Color.white;

		m_Vertices[0] = Vector3.zero;
		m_Vertices[1] = Vector3.zero;
		m_Vertices[2] = Vector3.zero;
		m_Vertices[3] = Vector3.zero;
		m_UpdateVertices = true;

		m_UV[0] = Vector3.zero;
		m_UV[1] = Vector3.zero;
		m_UV[2] = Vector3.zero;
		m_UV[3] = Vector3.zero;
		m_UpdateUV = true;
	}

	//! 析构
	~Sprite()
	{
	}

	//! 尺寸
	public Vector2 Size
	{
		get { return m_Size; }
		set { m_Size = value; m_UpdateVertices = true; }
	}

	//! 位置
	public Vector2 Position
	{
		get { return m_Position; }
		set { m_Position = value; m_UpdateVertices = true; }
	}

	//! 旋转
	public float Rotation
	{
		get { return m_Rotation; }
		set { m_Rotation = value; m_UpdateVertices = true; }
	}

	//! 层 [0 - 15]共16层, 绘制顺序为从小到大
	public int Layer
	{
		get { return m_Layer; }
		set
		{
			if ((value >= MinLayer) && (value <= MaxLayer))
			{
				m_Layer = value;
				m_UpdateVertices = true;
			}
		}
	}

	//! 材质
	public Material Material
	{
		get { return m_Material; }
		set { m_Material = value; m_UpdateUV = true; }
	}

	//! 贴图区域
	public Rect TextureRect
	{
		get { return m_TextureRect; }
		set { m_TextureRect = value; m_UpdateUV = true; }
	}

	//! 水平翻转
	public bool FlipX
	{
		get { return m_FlipX; }
		set { m_FlipX = value; m_UpdateUV = true; }
	}

	//! 垂直翻转
	public bool FlipY
	{
		get { return m_FlipY; }
		set { m_FlipY = value; m_UpdateUV = true; }
	}

	//! 颜色
	public Color Color
	{
		get { return m_Color; }
		set { m_Color = value; }
	}

	//! 顶点坐标
	public Vector3 [] Vertices
	{
		get
		{
			if (m_UpdateVertices)
			{
				UpdateVertices();
			}

			return m_Vertices;
		}
	}

	//! UV坐标
	public Vector3 [] UV
	{
		get
		{
			if (m_UpdateUV)
			{
				UpdateUV();
			}

			return m_UV;
		}
	}

	//! 顶点索引
	static public int [] Triangles
	{
		get
		{
			return m_Triangles;
		}
	}

	//! 更新顶点坐标
	protected virtual void UpdateVertices()
	{
		// 无旋转
		if (m_Rotation == 0)
		{
			Rect rect = new Rect((int)(m_Position.x - m_Size.x / 2), (int)(m_Position.y - m_Size.y / 2), m_Size.x, m_Size.y);

			m_Vertices[0] = new Vector3(rect.xMin, rect.yMax, 0);
			m_Vertices[1] = new Vector3(rect.xMax, rect.yMax, 0);
			m_Vertices[2] = new Vector3(rect.xMax, rect.yMin, 0);
			m_Vertices[3] = new Vector3(rect.xMin, rect.yMin, 0);
		}
		// 有旋转
		else
		{
			float hx = m_Size.x / 2;
			float hy = m_Size.y / 2;

			float sin = Mathf.Sin(m_Rotation);
			float cos = Mathf.Cos(m_Rotation);

			m_Vertices[0] = new Vector3(m_Position.x + (-hx * cos - hy * sin), m_Position.y + (-hx * sin + hy * cos), 0);
			m_Vertices[1] = new Vector3(m_Position.x + (hx * cos - hy * sin), m_Position.y + (hx * sin + hy * cos), 0);
			m_Vertices[2] = new Vector3(m_Position.x + (hx * cos + hy * sin), m_Position.y + (hx * sin - hy * cos), 0);
			m_Vertices[3] = new Vector3(m_Position.x + (-hx * cos + hy * sin), m_Position.y + (-hx * sin -hy * cos), 0);
		}

		m_UpdateVertices = false;
	}

	//! 更新UV坐标
	protected virtual void UpdateUV()
	{
		float factor_width = 1.0f / m_Material.mainTexture.width;
		float factor_height = 1.0f / m_Material.mainTexture.height;

		float u_min = m_TextureRect.xMin * factor_width;
		float u_max = m_TextureRect.xMax * factor_width;
		float v_min = 1.0f - m_TextureRect.yMax * factor_height;
		float v_max = 1.0f - m_TextureRect.yMin * factor_height;

		if ((m_FlipX == false) && (m_FlipY == false))
		{
			m_UV[0] = new Vector2(u_min, v_max);
			m_UV[1] = new Vector2(u_max, v_max);
			m_UV[2] = new Vector2(u_max, v_min);
			m_UV[3] = new Vector2(u_min, v_min);
		}
		else if ((m_FlipX == true) && (m_FlipY == false))
		{
			m_UV[0] = new Vector2(u_max, v_max);
			m_UV[1] = new Vector2(u_min, v_max);
			m_UV[2] = new Vector2(u_min, v_min);
			m_UV[3] = new Vector2(u_max, v_min);
		}
		else if ((m_FlipX == false) && (m_FlipY == true))
		{
			m_UV[0] = new Vector2(u_min, v_min);
			m_UV[1] = new Vector2(u_max, v_min);
			m_UV[2] = new Vector2(u_max, v_max);
			m_UV[3] = new Vector2(u_min, v_max);
		}
		else // ((m_FlipX == true) && (m_FlipY == true))
		{
			m_UV[0] = new Vector2(u_max, v_min);
			m_UV[1] = new Vector2(u_min, v_min);
			m_UV[2] = new Vector2(u_min, v_max);
			m_UV[3] = new Vector2(u_max, v_max);
		}

		m_UpdateUV = false;
	}
}

