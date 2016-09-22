using UnityEngine;
using System.Collections;
using System;
using System.IO;

//! 屏幕适配 缩放
public static class ResolutionConstant
{
    public static float R = 1f;
}

public class UIText : UIControlVisible
{
    public enum enAlignStyle
    {
        left,
        center,
        right,
    }

    private string m_Text = null;
    private Font m_Font = null;
    private float m_LineSpacing = 1.0f;
    private float m_CharacterSpacing = 1.0f;
    private Color m_Color = Color.black;
    private bool m_bIsAutoLine = true;
    private enAlignStyle m_AlignStyle = enAlignStyle.left;

    public UIText()
    {
    }

    ~UIText()
    {
    }

    public Font Font
    {
        set { m_Font = value; }
        get { return m_Font; }
    }

    public new Rect Rect
    {
        get { return base.Rect; }
        set
        {
            /*
            value.x = value.x * ResolutionConstant.R;
            value.y = value.y * ResolutionConstant.R;
            value.width = value.width * ResolutionConstant.R;
            value.height = value.height * ResolutionConstant.R;*/
            m_Rect = value;
            //base.Rect = value;
            UpdateText();
            SetClip(value);
        }
    }

    public void Set(string font, string text, Color color)
    {
        m_Font = mgrFont.Instance().getFont(font);
        m_Color = color;
        m_Text = text;
        UpdateText();
    }

    public void SetColor(Color clr)
    {
        m_Color = clr;
        UpdateText();
    }

    public void SetFont(string name)
    {
        m_Font = mgrFont.Instance().getFont(name);
        UpdateText();
    }

    public void SetText(string text)
    {
        m_Text = text;
        UpdateText();
    }

    public string GetText()
    {
        return m_Text;
    }

    public float CharacterSpacing
    {
        set { m_CharacterSpacing = value; }
        get { return m_CharacterSpacing ; }
    }

    public float LineSpacing
    {
        set { m_LineSpacing = value; }
        get { return m_LineSpacing ; }
    }

    public enAlignStyle AlignStyle
    {
        set { m_AlignStyle = value; }
        get { return m_AlignStyle; }
    }

    public bool AutoLine
    {
        set { m_bIsAutoLine = value; }
        get { return m_bIsAutoLine; }
    }

    public override void Draw()
    {
        if (m_Sprite == null)
        {
            //Debug.Log("m_Sprite is null!!!!!!!!!" + UnityEngine.Random.Range(0, 1000));
            return;
        }

        for (int i = 0; i < m_Sprite.Length; i++)
        {
            m_Parent.DrawSprite(m_Sprite[i]);
        }
    }

    private void UpdateText()
    {
        m_Sprite = null;
        m_LineSpacing *= ResolutionConstant.R;
        
        //
        if (m_Font == null)
        {
            //Debug.Log("UpdateText m_font==null!!!!!!!!!" + UnityEngine.Random.Range(0, 1000));
            return;
        }
        if ((m_Text == null) || (m_Text.Length <= 0))
        {
            //Debug.Log("UpdateText m_text==null!!!!!!!!!" + UnityEngine.Random.Range(0, 1000));
            return;
        }

        ArrayList sprites = new ArrayList();
        ArrayList finalLines = new ArrayList(); // 最终分析后的所有行;

        string[] initLines = m_Text.Split('\n');

        if (m_bIsAutoLine)
        {
            for (int j = 0; j < initLines.Length; ++j)
            {
                ArrayList subLines = new ArrayList();
                string[] words = initLines[j].Split(' ');

                string oneLine = "";
                float oneLineTextWidth = 0;
                for (int m = 0; m < words.Length; ++m)
                {
                    float word_width = m_Font.GetTextWidth(words[m], CharacterSpacing);

                    //Debug.Log(words[m] + " : " + word_width + "   111111111111-------" + UnityEngine.Random.Range(0, 1099));
                    //Debug.Log("line_width : " + oneLineTextWidth + "   22222222222-------" + UnityEngine.Random.Range(0, 1099));

                    if (oneLineTextWidth + word_width <= Rect.width * 0.95f)
                    {
                        oneLine += words[m];
                        oneLineTextWidth += word_width;
                    }
                    else
                    {
                        oneLine.Trim();
                        if ("" != oneLine)
                        {
                            //Debug.Log("line_text = " + oneLine + "   33333333333333333333-------" + UnityEngine.Random.Range(0, 1099));
                            subLines.Add(oneLine);
                        }
                        oneLine = words[m];
                        oneLineTextWidth = word_width;
                    }

                    oneLine += " ";
                    oneLineTextWidth += CharacterSpacing;
                    oneLineTextWidth += m_Font.GetTextWidth(" ");
                }

                oneLine.Trim();
                if ("" != oneLine)
                {
                    //Debug.Log(oneLine + " : 2222222222222-------" + UnityEngine.Random.Range(0, 1099));
                    subLines.Add(oneLine);
                }

                for (int k = 0; k < subLines.Count; ++k)
                {
                    finalLines.Add(subLines[k]);
                }
            }
        }
        else
        {
            for (int j = 0; j < initLines.Length; ++j)
            {
                finalLines.Add(initLines[j]);
            }
        }

        float line_height = m_Font.CellHeight + LineSpacing;
        int x_count = (int)(m_Font.TextureWidth / m_Font.CellWidth);

        //Debug.Log("text = " + m_Text + " : " + UnityEngine.Random.Range(0, 1000));

        for (int j = 0; j < finalLines.Count; ++j)
        {
            float width = 0;

            for (int i = 0; i < ((string)finalLines[j]).Length; ++i)
            {
                char ch = ((string)finalLines[j])[i];
                float chWidth = (float)m_Font.getCharWidth(ch);

                int id = ch - ' ';//0x20;
                
                int x_index = id % x_count;
                int y_index = id / x_count;
                float cx = x_index * m_Font.CellWidth;
                float cy = y_index * m_Font.CellHeight;

                //Debug.Log("char= " + ch + " id=" + id + " x= " + cx + " y= " + cy + " : " + UnityEngine.Random.Range(0, 1000));

                UISprite sprite = new UISprite();
                sprite.Position = new Vector2(m_Rect.x + width + m_Font.CellWidth / 2 * ResolutionConstant.R, m_Rect.y + m_Rect.height - (j + 1) * line_height * ResolutionConstant.R + ResolutionConstant.R * m_Font.CellHeight / 2);
                sprite.Size = new Vector2(m_Font.CellWidth * ResolutionConstant.R, m_Font.CellHeight * ResolutionConstant.R);
                sprite.Material = m_Font.getTexture();
                sprite.TextureRect = new Rect(cx, cy, m_Font.CellWidth, m_Font.CellHeight);
                
                sprite.Color = m_Color;

                if (m_Clip)
                {
                    sprite.SetClip(m_ClipRect);
                }

                sprites.Add(sprite);

                width += chWidth  + CharacterSpacing;  //加上字符间隔的宽度
            }
        }

        //Debug.Log(AlignStyle + " : " + UnityEngine.Random.Range(0, 1099));

        if (enAlignStyle.center == AlignStyle)
        {
            int index = 0;
            for (int j = 0; j < finalLines.Count; ++j)
            {
                string linetext = (string)finalLines[j];
                float textWidth = (float)m_Font.GetTextWidth(linetext, CharacterSpacing);
                float offset = (Rect.width - textWidth) / 2;
                float offsetY = (Rect.height - m_Font.CellHeight * ResolutionConstant.R - Rect.height*0.1f) / 2;
                for (int i = 0; i < linetext.Length; ++i)
                {
                    ((UISprite)sprites[i + index]).Position = new Vector2(((UISprite)sprites[i + index]).Position.x + offset, ((UISprite)sprites[i + index]).Position.y - offsetY);
                }

                index += linetext.Length;
            }
        }
        else if (enAlignStyle.right == AlignStyle)
        {
            int index = 0;
            for (int j = 0; j < finalLines.Count; ++j)
            {
                string linetext = (string)finalLines[j];
                float textWidth = (float)m_Font.GetTextWidth(linetext, CharacterSpacing);
                float offset = Rect.width - textWidth;

                for (int i = 0; i < linetext.Length; ++i)
                {
                    ((UISprite)sprites[i + index]).Position = new Vector2(((UISprite)sprites[i + index]).Position.x + offset, ((UISprite)sprites[i + index]).Position.y);
                }

                index += linetext.Length;
            }
        }

        // copy sprites to m_Sprite;
        m_Sprite = new UISprite[sprites.Count];
        for (int i = 0; i < sprites.Count; i++)
        {
            m_Sprite[i] = (UISprite)sprites[i];
        }
    }
}

public class Font
{
	public Material _Material = null;
	int _texWidth = 0;
	int _texHeight = 0;
	int _cellWidth = 0;
	int _cellHeight = 0;
    int _cellxOffset = 0;
    int _cellyOffset = 0;

	public ArrayList _widths = new ArrayList();

	public Material getTexture()
    {
	   	return _Material;
    }

	public float getCharWidth(char c)
	{
		int idx = c - ' ';

        return ((float)_widths[idx] + _cellxOffset * 2) * ResolutionConstant.R;
	}

	public float GetTextWidth(string text)
	{
		float width = 0;
		for (int i = 0; i < text.Length; ++i)
		{
			char ch = text[i];
			width += getCharWidth(ch);
		}

		return width;
	}

	public float GetTextWidth(string text, float CharacterSpacing)
	{
		float width = 0;
		for (int i = 0; i < text.Length; ++i)
		{
			char ch = text[i];
			width += getCharWidth(ch);
			if (i < text.Length - 1)
				width += CharacterSpacing;
		}

		return width;
	}

	public int TextureWidth
	{
		set { _texWidth = value; }
		get { return _texWidth; }
	}

	public int TextureHeight
	{
		set { _texHeight = value; }
		get { return _texHeight; }
	}

	public int CellWidth
	{
		set { _cellWidth = value; }
		get { return _cellWidth; }
	}

	public int CellHeight
	{
		set { _cellHeight = value; }
		get { return _cellHeight; }
	}

    public int OffsetX
    {
        set { _cellxOffset = value; }
        get { return _cellxOffset; }
    }

    public int OffsetY
    {
        set { _cellyOffset = value; }
        get { return _cellyOffset; }
    }
}

public class mgrFont
{
	static protected mgrFont _mgrFontInstance = null;

	protected Hashtable _fonts = null;

	static public mgrFont Instance()
	{
		if(_mgrFontInstance == null)
		{
			_mgrFontInstance = new mgrFont();
		}

		return _mgrFontInstance;
	}

	public Font getFont(string fontName)
	{
        if (null==_fonts)
        {
            _fonts = new Hashtable();
        }
        
        if (_fonts.Contains(fontName))
        {
            return (Font)_fonts[fontName];
        }

        Font ft = new Font();
        ft._Material = Resources.Load(fontName + "_M") as Material;
        if (null == ft._Material)
        {
            Debug.Log("Cannot find text_matertial : " + fontName);
            return null;
        }

        TextAsset textFile = Resources.Load(fontName + "_cfg") as TextAsset;
        if (null != textFile && null != textFile.text)
        {
            string[] lines = textFile.text.Split(new char[] { '\n' });

            // texture info; 4 int data;
            string[] larray = lines[0].Split(new char[] { ' ' });
            ft.TextureWidth = int.Parse(larray[0]);
            ft.TextureHeight = int.Parse(larray[1]);
            ft.CellWidth = int.Parse(larray[2]);
            ft.CellHeight = int.Parse(larray[3]);
            ft.OffsetX = int.Parse(larray[4]);
            ft.OffsetY = int.Parse(larray[5]);

            // char width datas;
            string[] larray1 = lines[1].Split(new char[] { ' ' });
            for (int i = 0; i < larray1.Length; i++)
            {
                ft._widths.Add(float.Parse(larray1[i]));
            }
        }
        else
        {
            Debug.Log("Cannot find font text file : " + fontName);
            return null;
        }

        _fonts[fontName] = ft;
		return ft;
	}
};




	
