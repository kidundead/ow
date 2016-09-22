//! @file UIControl.cs


using UnityEngine;
using System.Collections;


//! @class UISprite
//! @brief UI精灵
public class UISprite : Sprite
{
	//! 是否剪裁
	protected bool m_Clip;

	//! 剪裁区域
	protected Rect m_ClipRect;

	//! 构造
	public UISprite()
	{
		base.Layer = 0;
	}

	//! 层(UI精灵的层始终为0)
	public new int Layer
	{
		get { return base.Layer; }
	}

	//! 尺寸
	public new Vector2 Size
	{
		get { return m_Size; }
		set
		{
			m_Size = value;
			m_UpdateVertices = true;
			if (m_Clip)
			{
				m_UpdateUV = true;
			}
		}
	}

	//! 位置
	public new Vector2 Position
	{
		get { return m_Position; }
		set
		{
			m_Position = value;
			m_UpdateVertices = true;
			if (m_Clip)
			{
				m_UpdateUV = true;
			}
		}
	}

	//! 设置剪裁
	public void SetClip(Rect clip_rect)
	{
		m_Clip = true;
		m_ClipRect = clip_rect;

		m_UpdateVertices = true;
		m_UpdateUV = true;
	}

	//! 取消剪裁
	public void ClearClip()
	{
		m_Clip = false;

		m_UpdateVertices = true;
		m_UpdateUV = true;
	}


	//! 更新顶点坐标
	protected override void UpdateVertices()
	{
		if (m_Clip)
		{
			UpdateClipVertices();
		}
		else
		{
			base.UpdateVertices();
		}
	}

	//! 更新UV坐标
	protected override void UpdateUV()
	{
		if (m_Clip)
		{
			UpdateClipUV();
		}
		else
		{
			base.UpdateUV();
		}
	}

	//! 更新剪裁顶点坐标
	protected void UpdateClipVertices()
	{
		Rect rect = new Rect((int)(m_Position.x - m_Size.x / 2), (int)(m_Position.y - m_Size.y / 2), m_Size.x, m_Size.y);

		if ((m_ClipRect.xMin > rect.xMax) || (m_ClipRect.xMax < rect.xMin) ||
			(m_ClipRect.yMin > rect.yMax) || (m_ClipRect.yMax < rect.yMin))
		{
			m_Vertices[0] = m_Vertices[1] = m_Vertices[2] = m_Vertices[3] = new Vector3(-1, -1, 0);
			m_UpdateVertices = false;
			return;
		}

		if (m_ClipRect.xMin > rect.xMin)
		{
			rect.xMin = m_ClipRect.xMin;
		}
		if (m_ClipRect.xMax < rect.xMax)
		{
			rect.xMax = m_ClipRect.xMax;
		}
		if (m_ClipRect.yMin > rect.yMin)
		{
			rect.yMin = m_ClipRect.yMin;
		}
		if (m_ClipRect.yMax < rect.yMax)
		{
			rect.yMax = m_ClipRect.yMax;
		}

		// 无旋转
		if (m_Rotation == 0)
		{
			m_Vertices[0] = new Vector3(rect.xMin, rect.yMax, 0);
			m_Vertices[1] = new Vector3(rect.xMax, rect.yMax, 0);
			m_Vertices[2] = new Vector3(rect.xMax, rect.yMin, 0);
			m_Vertices[3] = new Vector3(rect.xMin, rect.yMin, 0);
		}
		// 有旋转
		else
		{
			float sin = Mathf.Sin(m_Rotation);
			float cos = Mathf.Cos(m_Rotation);

			float x, y;

			x = m_Position.x - rect.xMin;
			y = rect.yMax - m_Position.y;
			m_Vertices[0] = new Vector3(m_Position.x + (-x * cos - y * sin), m_Position.y + (-x * sin + y * cos), 0);

			x = rect.xMax - m_Position.x;
			y = rect.yMax - m_Position.y;
			m_Vertices[1] = new Vector3(m_Position.x + (x * cos - y * sin), m_Position.y + (x * sin + y * cos), 0);

			x = rect.xMax - m_Position.x;
			y = m_Position.y - rect.yMin;
			m_Vertices[2] = new Vector3(m_Position.x + (x * cos + y * sin), m_Position.y + (x * sin - y * cos), 0);

			x = m_Position.x - rect.xMin;
			y = m_Position.y - rect.yMin;
			m_Vertices[3] = new Vector3(m_Position.x + (-x * cos + y * sin), m_Position.y + (-x * sin -y * cos), 0);
		}

		m_UpdateVertices = false;
	}

	//! 更新剪裁UV坐标
	protected void UpdateClipUV()
	{
		Rect rect = new Rect((int)(m_Position.x - m_Size.x / 2), (int)(m_Position.y - m_Size.y / 2), m_Size.x, m_Size.y);

		if ((m_ClipRect.xMin > rect.xMax) || (m_ClipRect.xMax < rect.xMin) ||
			(m_ClipRect.yMin > rect.yMax) || (m_ClipRect.yMax < rect.yMin))
		{
			m_UV[0] = m_UV[1] = m_UV[2] = m_UV[3] = new Vector2(0, 0);
			m_UpdateVertices = false;
			return;
		}

		Rect real_texture_rect = m_TextureRect;

		float d = m_ClipRect.xMin - rect.xMin;
		if (d > 0)
		{
			real_texture_rect.xMin += d;
		}
		d = rect.xMax - m_ClipRect.xMax;
		if (d > 0)
		{
			real_texture_rect.xMax -= d;
		}
		d = m_ClipRect.yMin - rect.yMin;
		if (d > 0)
		{
			real_texture_rect.yMax -= d;
		}
		d = rect.yMax - m_ClipRect.yMax;
		if (d > 0)
		{
			real_texture_rect.yMin += d;
		}

		float factor_width = 1.0f / m_Material.mainTexture.width;
		float factor_height = 1.0f / m_Material.mainTexture.height;

		float u_min = real_texture_rect.xMin * factor_width;
		float u_max = real_texture_rect.xMax * factor_width;
		float v_min = 1.0f - real_texture_rect.yMax * factor_height;
		float v_max = 1.0f - real_texture_rect.yMin * factor_height;

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


//! @class UIControl
//! @brief UI控件
public class UIControl
{
	//! 父容器
	protected UIContainer m_Parent;

	//! 控件id
	protected int m_Id;

	//! 控件区域
	protected Rect m_Rect;

	//! 是否可见
	protected bool m_Visible;

	//! 是否禁用
	protected bool m_Enable;

	//! 是否剪裁
	protected bool m_Clip;

	//! 剪裁区域
	protected Rect m_ClipRect;

	//! 构造
	public UIControl()
	{
		m_Parent = null;
		m_Id = 0;
		m_Rect = new Rect(0, 0, 0, 0);
		m_Visible = true;
		m_Enable = true;
	}

	//! 设置父容器
	public void SetParent(UIContainer parent)
	{
		m_Parent = parent;
	}

	//! 控件id
	public int Id
	{
		get { return m_Id; }
		set { m_Id = value; }
	}

	//! 控件区域
	public Rect Rect
	{
		get { return m_Rect; }
		set {
            m_Rect = value; }
	}

	//! 是否可见
	public bool Visible
	{
		get { return m_Visible; }
		set { m_Visible = value; }
	}

	//! 是否禁用
	public bool Enable
	{
		get { return m_Enable; }
		set { m_Enable = value; }
	}

	//! 设置剪裁
	public void SetClip(Rect clip_rect)
	{
		m_Clip = true;
		m_ClipRect = clip_rect;
	}

	//! 取消剪裁
	public void ClearClip()
	{
		m_Clip = false;
	}

	//! 点是否在控件区域内
	public virtual bool PtInRect(Vector2 pt)
	{
		bool in_rect = (pt.x >= m_Rect.xMin && pt.x < m_Rect.xMax && pt.y >= m_Rect.yMin && pt.y < m_Rect.yMax);
		if (in_rect)
		{
			if (m_Clip)
			{
				return (pt.x >= m_ClipRect.xMin && pt.x < m_ClipRect.xMax && pt.y >= m_ClipRect.yMin && pt.y < m_ClipRect.yMax);
			}
			else
			{
				return true;
			}
		}
		else
		{
			return false;
		}
	}

	//! 绘制
	public virtual void Draw()
	{
	}

	//! 更新
	public virtual void Update()
	{
	}

	//! 处理输入
	public virtual bool HandleInput(UITouchInner touch)
	{
		return false;
	}
}

