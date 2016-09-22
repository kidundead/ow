//! @file UIControlImplEx.cs


using UnityEngine;
using System.Collections;


//! @class UIFontInfo字体
//! @brief 字体
public class UIFontInfo
{
	//! 材质
	protected Material m_Material;
	//! 配置
	protected TextAsset m_Conf;

	//! UV区域
	protected Rect [] m_UVRect = null;
	//! 高度
	protected float m_Height;
	//! 宽度
	protected float [] m_Width = null;

	public UIFontInfo(string name, Material material, TextAsset conf)
	{
		//
		m_Material = material;
		m_Conf = conf;

		//
		Configure configure = new Configure();
		configure.Load(conf.text);

		m_Height = int.Parse(configure.GetSingle(name, "height"));

		m_UVRect = new Rect[512];
		for (int i = 0; i < 512; i++)
		{
			m_UVRect[i] = new Rect(0, 0, 0, 0);
		}
		m_Width = new float [512];
		for (int i = 0; i < 512; i++)
		{
			m_Width[i] = 0;
		}

		int count = configure.CountArray2(name, "chars");
		for (int i = 0; i < count; i++)
		{
			int ch_value = int.Parse(configure.GetArray2(name, "chars", i, 0));

			Rect rect = new Rect(0, 0, 0, 0);
			rect.x = float.Parse(configure.GetArray2(name, "chars", i, 2));
			rect.y = float.Parse(configure.GetArray2(name, "chars", i, 3));
			rect.width = float.Parse(configure.GetArray2(name, "chars", i, 4));
			rect.height = float.Parse(configure.GetArray2(name, "chars", i, 5));

			m_Width[ch_value] = rect.width;
			m_UVRect[ch_value] = rect;
		}
	}

	public Material GetMaterial()
	{
		return m_Material;
	}

	public TextAsset GetConf()
	{
		return m_Conf;
	}

	public Rect GetUVRect(char ch)
	{
		int index = (int)ch;
		if (index >= m_UVRect.Length)
		{
			index = 0;
		}
		return m_UVRect[index];
	}

	public float GetHeight()
	{
		return m_Height;
	}

	public float GetWidth(char ch)
	{
		int index = (int)ch;
		if (index >= m_Width.Length)
		{
			index = 0;
		}
		return m_Width[index];
	}

	public int GetTextWidth(string text)
	{
		int width = 0;
		for (int i = 0; i < text.Length; ++i)
		{
			char ch = text[i];
			width += (int)GetWidth(ch);
		}

		return width;
	}
}


//! @class UILabel
//! @brief 文本控件
public class UILabel : UIControlVisible
{
	//! 字体
	UIFontInfo m_Font;

	//! 颜色
	protected Color m_Color;

	//! 字符间距
	protected float m_CharacterSpacing;
	//! 行距
	protected float m_LineSpacing;

	//! 文本
	protected string m_Text;

	//! 构造
	public UILabel()
	{
		m_Font = null;
		m_Color = Color.white;

		m_CharacterSpacing = 0;
		m_LineSpacing = 0;

		m_Text = null;
	}

	//! 设置字体
	public void SetFont(UIFontInfo font)
	{
		m_Font = font;
		UpdateText();
	}

	//! 设置颜色
	public void SetColor(Color color)
	{
		m_Color = color;

		if (m_Sprite == null)
		{
			return;
		}

		for (int i = 0; i < m_Sprite.Length; i++)
		{
			SetSpriteColor(i, color);
		}
	}

	//! 设置字符间距
	public void SetCharacterSpacing(float character_spacing)
	{
		m_CharacterSpacing = character_spacing;
		UpdateText();
	}

	//! 设置行距
	public void SetLineSpacing(float line_spacing)
	{
		m_LineSpacing = line_spacing;
		UpdateText();
	}

	//! 设置文本
	public void SetText(string text)
	{
		m_Text = text;
		UpdateText();
	}

	//! 控件区域
	public new Rect Rect
	{
		get { return base.Rect; }
		set
		{
			base.Rect = value;

			//
			UpdateText();
		}
	}

	//! 绘制
	public override void Draw()
	{
		if (m_Sprite == null)
		{
			return;
		}

		for (int i = 0; i < m_Sprite.Length; i++)
		{
			m_Parent.DrawSprite(m_Sprite[i]);
		}
	}

	//! 更新文本
	private void UpdateText()
	{
		// 删除以前字符
		m_Sprite = null;

		//
		if (m_Font == null)
		{
			return;
		}
		if ((m_Text == null) || (m_Text.Length <= 0))
		{
			return;
		}

		//
		ArrayList sprites = new ArrayList();

		//
		float width = 0;
		float height = 0;

		if ((height + m_Font.GetHeight()) > m_Rect.height)
		{
			return;
		}

		for (int i = 0; i < m_Text.Length; ++i)
		{
			char ch = m_Text[i];

			if ((ch == '\n') || (ch == '\r'))
			{
				width = 0;
				height += (m_Font.GetHeight() + m_LineSpacing);

				if ((height + m_Font.GetHeight()) > m_Rect.height)
				{
					// 超出显示高度
					break;
				}
			}
			else
			{
				float chWidth = m_Font.GetWidth(ch);

				if ((width + chWidth) > m_Rect.width)
				{
					break;
				}

				UISprite sprite = new UISprite();
				sprite.Position = new Vector2(m_Rect.x + width + chWidth / 2, m_Rect.y + height + m_Font.GetHeight() / 2);
				sprite.Size = new Vector2(chWidth, m_Font.GetHeight());
				sprite.Material = m_Font.GetMaterial();
				sprite.TextureRect = m_Font.GetUVRect(ch);
				sprite.Color = m_Color;

				if (m_Clip)
				{
					sprite.SetClip(m_ClipRect);
				}

				sprites.Add(sprite);

				width += (chWidth + m_CharacterSpacing);
			}
		}

		// copy sprites to m_Sprite;
		m_Sprite = new UISprite [sprites.Count];
		for (int i = 0; i < sprites.Count; i++)
		{
			m_Sprite[i] = (UISprite)sprites[i];
		}
	}

	// 计算文本宽度
	public int GetTextWidth(string text)
	{
		if (m_Font == null)
		{
			return 0;
		}

		return m_Font.GetTextWidth(text);
	}
}

