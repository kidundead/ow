using UnityEngine;
using System.Collections;
using System.Xml;
using System;

public class UIHelper : MonoBehaviour 
{
    public string m_ui_cfgxml;
    public string m_ui_material_path;  //最后带反斜杠
    public string m_font_path; //最后带反斜杠
    public UIManager m_UIManagerRef = null;
    public Hashtable m_control_table = null;
    
    public Hashtable m_animations = null;

    // Use this for initialization
	public void Start () 
    {
        m_control_table = new Hashtable();
        m_animations = new Hashtable();

        XmlElement tempElem = null;
        string value = "";
        TextAsset xml = Resources.Load(m_ui_cfgxml) as TextAsset;
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xml.text);
        XmlNode root = xmlDoc.DocumentElement;
        foreach (XmlNode node1 in root.ChildNodes)
        {
            if ("UIElem"==node1.Name)
            {
                foreach (XmlNode xmlNode in node1.ChildNodes)
                {
                    tempElem = (XmlElement)xmlNode;
                    if ("UIButton" == xmlNode.Name)
                    {
                        UIButtonBase button = null;

                        value = tempElem.GetAttribute("rect").Trim();
                        string[] digital = value.Split(',');

                        value = tempElem.GetAttribute("type").Trim();
                        if ("click" == value)
                        {
                            button = new UIClickButton();
                            ((UIClickButton)button).Rect = new Rect(int.Parse(digital[0].Trim()), int.Parse(digital[1].Trim()), int.Parse(digital[2].Trim()), int.Parse(digital[3].Trim()));
                        }
                        else if ("push" == value)
                        {
                            button = new UIPushButton();
                            ((UIPushButton)button).Rect = new Rect(int.Parse(digital[0].Trim()), int.Parse(digital[1].Trim()), int.Parse(digital[2].Trim()), int.Parse(digital[3].Trim()));
                        }
                        else if ("select" == value)
                        {
                            button = new UISelectButton();
                            ((UISelectButton)button).Rect = new Rect(int.Parse(digital[0].Trim()), int.Parse(digital[1].Trim()), int.Parse(digital[2].Trim()), int.Parse(digital[3].Trim()));
                        }

                        if (null == button) continue;

                        value = tempElem.GetAttribute("id").Trim();
                        button.Id = int.Parse(value);

                        value = tempElem.GetAttribute("enable").Trim();
                        if (value.Length > 1) button.Enable = ("true" == value);

                        value = tempElem.GetAttribute("visible").Trim();
                        if (value.Length > 1) button.Visible = ("true" == value);

                        tempElem = (XmlElement)xmlNode.SelectSingleNode("Normal");
                        if (null != tempElem)
                        {
                            value = tempElem.GetAttribute("rect").Trim();
                            digital = value.Split(',');
                            value = tempElem.GetAttribute("material").Trim();
                            button.SetTexture(UIButtonBase.State.Normal, LoadUIMaterial(value), new Rect(int.Parse(digital[0].Trim()), int.Parse(digital[1].Trim()), int.Parse(digital[2].Trim()), int.Parse(digital[3].Trim())));
                        }

                        tempElem = (XmlElement)xmlNode.SelectSingleNode("Press");
                        if (null != tempElem)
                        {
                            value = tempElem.GetAttribute("rect").Trim();
                            digital = value.Split(',');
                            value = tempElem.GetAttribute("material").Trim();
                            button.SetTexture(UIButtonBase.State.Pressed, LoadUIMaterial(value), new Rect(int.Parse(digital[0].Trim()), int.Parse(digital[1].Trim()), int.Parse(digital[2].Trim()), int.Parse(digital[3].Trim())));
                        }

                        tempElem = (XmlElement)xmlNode.SelectSingleNode("Disable");
                        if (null != tempElem)
                        {
                            value = tempElem.GetAttribute("rect").Trim();
                            digital = value.Split(',');
                            value = tempElem.GetAttribute("material").Trim();
                            button.SetTexture(UIButtonBase.State.Disabled, LoadUIMaterial(value), new Rect(int.Parse(digital[0].Trim()), int.Parse(digital[1].Trim()), int.Parse(digital[2].Trim()), int.Parse(digital[3].Trim())));
                        }

                        tempElem = (XmlElement)xmlNode.SelectSingleNode("Hover");
                        if (null != tempElem)
                        {
                            value = tempElem.GetAttribute("rect").Trim();
                            digital = value.Split(',');
                            value = tempElem.GetAttribute("material").Trim();
                            button.SetHoverSprite(LoadUIMaterial(value), new Rect(int.Parse(digital[0].Trim()), int.Parse(digital[1].Trim()), int.Parse(digital[2].Trim()), int.Parse(digital[3].Trim())));
                        }

                        m_UIManagerRef.Add(button);
                        m_control_table.Add(button.Id, button);
                    }
                    else if ("UIImage" == xmlNode.Name)
                    {
                        UIImage image = new UIImage();
                        value = tempElem.GetAttribute("id").Trim();
                        image.Id = int.Parse(value);

                        value = tempElem.GetAttribute("rect").Trim();
                        string[] digital = value.Split(',');
                        image.Rect = new Rect(int.Parse(digital[0]), int.Parse(digital[1].Trim()), int.Parse(digital[2].Trim()), int.Parse(digital[3].Trim()));

                        value = tempElem.GetAttribute("enable").Trim();
                        if (value.Length > 1) image.Enable = ("true" == value);

                        value = tempElem.GetAttribute("visible").Trim();
                        if (value.Length > 1) image.Visible = ("true" == value);

                        tempElem = (XmlElement)xmlNode.SelectSingleNode("Texture");
                        if (null != tempElem)
                        {
                            value = tempElem.GetAttribute("rect").Trim();
                            digital = value.Split(',');
                            value = tempElem.GetAttribute("material").Trim();
                            image.SetTexture(LoadUIMaterial(value), new Rect(int.Parse(digital[0].Trim()), int.Parse(digital[1].Trim()), int.Parse(digital[2].Trim()), int.Parse(digital[3].Trim())));
                        }

                        m_UIManagerRef.Add(image);
                        m_control_table.Add(image.Id, image);
                    }
                    else if ("UIText" == xmlNode.Name)
                    {
                        UIText text = new UIText();
                        value = tempElem.GetAttribute("id").Trim();
                        text.Id = int.Parse(value);

                        value = tempElem.GetAttribute("rect").Trim();
                        string[] digital = value.Split(',');
                        text.Rect = new Rect(int.Parse(digital[0]), int.Parse(digital[1].Trim()), int.Parse(digital[2].Trim()), int.Parse(digital[3].Trim()));

                        value = tempElem.GetAttribute("chargap").Trim();
                        if (value.Length > 1) text.CharacterSpacing = int.Parse(value);

                        value = tempElem.GetAttribute("linegap").Trim();
                        if (value.Length > 1) text.LineSpacing = int.Parse(value);

                        value = tempElem.GetAttribute("autoline").Trim();
                        if (value.Length > 1) text.AutoLine = ("true" == value);

                        value = tempElem.GetAttribute("align").Trim();
                        if (value.Length > 1) text.AlignStyle = (UIText.enAlignStyle)Enum.Parse(typeof(UIText.enAlignStyle), value);

                        value = tempElem.GetAttribute("enable").Trim();
                        if (value.Length > 1) text.Enable = ("true" == value);

                        value = tempElem.GetAttribute("visible").Trim();
                        if (value.Length > 1) text.Visible = ("true" == value);

                        value = tempElem.GetAttribute("font").Trim();
                        text.SetFont(m_font_path + value);

                        value = tempElem.GetAttribute("color").Trim();
                        if (value.Length > 1)
                        {
                            digital = value.Split(',');
                            text.SetColor(new Color(int.Parse(digital[0].Trim()) / 255.0f, int.Parse(digital[1].Trim()) / 255.0f, int.Parse(digital[2].Trim()) / 255.0f, int.Parse(digital[3].Trim()) / 255.0f));
                        }
                        text.SetText(xmlNode.InnerText.Trim(new char[] { ' ', '\t', '\r', '\n' }));

                        m_UIManagerRef.Add(text);
                        m_control_table.Add(text.Id, text);
                    } //UIText

                }// for
                
            }
            else if ("UIAnimation" == node1.Name)
            {
                foreach (XmlNode xmlNode in node1.ChildNodes)
                {
                    tempElem = (XmlElement)xmlNode;
                    if ("Animation" != xmlNode.Name) continue;
                    
                    UIAnimations animation = new UIAnimations();

                    value = tempElem.GetAttribute("id").Trim();
                    animation.animation_id = int.Parse(value);
                    Debug.Log(value);

                    value = tempElem.GetAttribute("control_id").Trim();
                    Debug.Log(value);
                    string[] digital = value.Split(',');
                    for (int i = 0; i < digital.Length; ++i)
                    {
                        UIAnimations.ControlData data = new UIAnimations.ControlData();
                        data.control_id = int.Parse(digital[i].Trim());
                        animation.control_data.Add(data);
                    }

                    tempElem = (XmlElement)xmlNode.SelectSingleNode("Translate");
                    if (null != tempElem)
                    {
                        animation.translate_have = true;

                        value = tempElem.GetAttribute("time").Trim();
                        animation.translate_time = float.Parse(value);

                        value = tempElem.GetAttribute("offset").Trim();
                        if (value.Length>0)
                        {
                            digital = value.Split(',');
                            animation.translate_offset.x = int.Parse(digital[0].Trim());
                            animation.translate_offset.y = int.Parse(digital[1].Trim());
                        }

                        value = tempElem.GetAttribute("restore").Trim();
                        if (value.Length > 0) animation.translate_restore = ("true" == value);

                        value = tempElem.GetAttribute("loop").Trim();
                        if (value.Length > 0) animation.translate_loop = ("true" == value);

                        value = tempElem.GetAttribute("reverse").Trim();
                        if (value.Length > 0) animation.translate_reverse = ("true" == value);
                    }

                    tempElem = (XmlElement)xmlNode.SelectSingleNode("Rotate");
                    if (null != tempElem)
                    {
                        animation.rotate_have = true;

                        value = tempElem.GetAttribute("time").Trim();
                        animation.rotate_time = float.Parse(value);

                        value = tempElem.GetAttribute("angle").Trim();
                        animation.rotate_angle = Mathf.Deg2Rad*float.Parse(value);
                       
                        value = tempElem.GetAttribute("restore").Trim();
                        if (value.Length > 0) animation.rotate_restore = ("true" == value);

                        value = tempElem.GetAttribute("loop").Trim();
                        if (value.Length > 0) animation.rotate_loop = ("true" == value);

                        value = tempElem.GetAttribute("reverse").Trim();
                        if (value.Length > 0) animation.rotate_reverse = ("true" == value);
                    }

                    m_animations.Add(animation.animation_id, animation);
                
                } // for
            } //if ("UIAnimation" == node1.Name)
        } //for root
	}

    public void Update()
    {
        foreach (UIAnimations animation in m_animations.Values)
        {
            if (!animation.IsRuning()) continue;

            animation.Update(Time.deltaTime);

            bool have_translate = false;
            Vector2 translate_pos = new Vector2(0, 0);
            if (animation.IsTranslating())
            {
                translate_pos = animation.GetTranslate();
                have_translate = true;
            }

            bool have_rotate = false;
            float rotate_delta = 0;
            if (animation.IsRotating())
            {
                rotate_delta = animation.GetRotate();
                have_rotate = true;
            }

            for (int i = 0; i < animation.control_data.Count; ++i)
            {
                UIAnimations.ControlData data = (UIAnimations.ControlData)animation.control_data[i];

                int control_id = data.control_id;

                string type = m_control_table[control_id].GetType().ToString();
                if ("UIClickButton" == type)
                {
                    UIClickButton button = ((UIClickButton)m_control_table[control_id]);
                    if (have_translate)
                    {
                        button.Rect = new Rect(translate_pos.x + data.pos.x, translate_pos.y + data.pos.y, button.Rect.width, button.Rect.height);
                    }
                    
                    if (have_rotate)
                    {
                        button.SetRotate(rotate_delta);
                    }
                }
                else if ("UIPushButton" == type)
                {
                    UIPushButton button = ((UIPushButton)m_control_table[control_id]);
                    if (have_translate) button.Rect = new Rect(translate_pos.x, translate_pos.y, button.Rect.width, button.Rect.height);
                    if (have_rotate) button.SetRotate(rotate_delta);
                }
                else if ("UISelectButton" == type)
                {
                    UISelectButton button = ((UISelectButton)m_control_table[control_id]);
                    if (have_translate) button.Rect = new Rect(translate_pos.x, translate_pos.y, button.Rect.width, button.Rect.height);
                    if (have_rotate) button.SetRotate(rotate_delta);
                }
                else if ("UIImage" == type)
                {
                    UIImage image = ((UIImage)m_control_table[control_id]);
                    if (have_translate) image.Rect = new Rect(translate_pos.x, translate_pos.y, image.Rect.width, image.Rect.height);
                    if (have_rotate) image.SetRotation(rotate_delta);
                }
                else if ("UIText" == type)
                {
                    UIText text = ((UIText)m_control_table[control_id]);
                    if (have_translate) text.Rect = new Rect(translate_pos.x, translate_pos.y, text.Rect.width, text.Rect.height);
                }
            }
        }
    }

    public void StartAnimation(int index)
    {
        UIAnimations animation = (UIAnimations)m_animations[index];

        for (int i = 0; i < animation.control_data.Count; ++i)
        {
            UIAnimations.ControlData data = (UIAnimations.ControlData)animation.control_data[i];
            int control_id = data.control_id;

            string type = m_control_table[control_id].GetType().ToString();
            if ("UIClickButton" == type)
            {
                UIClickButton button = ((UIClickButton)m_control_table[control_id]);
                data.pos.x = button.Rect.x;
                data.pos.y = button.Rect.y;
                data.angle = button.GetRotate();
            }
            else if ("UIPushButton" == type)
            {
                UIPushButton button = ((UIPushButton)m_control_table[control_id]);
                data.pos.x = button.Rect.x;
                data.pos.y = button.Rect.y;
                data.angle = button.GetRotate();
            }
            else if ("UISelectButton" == type)
            {
                UISelectButton button = ((UISelectButton)m_control_table[control_id]);
                data.pos.x = button.Rect.x;
                data.pos.y = button.Rect.y;
                data.angle = button.GetRotate();
            }
            else if ("UIImage" == type)
            {
                UIImage image = ((UIImage)m_control_table[control_id]);
                data.pos.x = image.Rect.x;
                data.pos.y = image.Rect.y;
                data.angle = image.GetRotation();
            }
            else if ("UIText" == type)
            {
                UIText text = ((UIText)m_control_table[control_id]);
                data.pos.x = text.Rect.x;
                data.pos.y = text.Rect.y;
            }
        }

        animation.Reset();
        animation.Start();

        //动作开始前存储当前该动作控件的初始数据
    }

    public Material LoadUIMaterial(string name)
    {
        string path_material = m_ui_material_path + name + "_M";
        Material material = Resources.Load(path_material) as Material;
        if (material == null)
        {
            Debug.Log("load material error: " + path_material);
        }
        return material;
    }
}

class UIAnimations
{
    public class ControlData
    {
        public int control_id;

        public Vector2 pos;
        public float angle;

        public ControlData()
        {
            control_id = -1;

            pos = new Vector2(0, 0);
            angle = 0;
        }
    }

    public enum state
    {
        none,
        doing,
        wait_end,
        end,
    }

    public int animation_id;

    public ArrayList control_data;

    state translate_state;
    float translate_duringtime;
    Vector2 translate_current;
    bool translate_exchange;

    public bool translate_have;
    public Vector2 translate_start;
    public float translate_time;
    public Vector2 translate_offset;
    public bool translate_restore;
    public bool translate_loop;
    public bool translate_reverse;

//     public state scale_state;
//     public float scale_duringtime;
//     public float scale_deltafactor;
// 
//     public bool scale_have;
//     public float scale_time;
//     public float scale_factor;
//     public bool scale_restore;
//     public bool scale_loop;

    state rotate_state;
    float rotate_duringtime;
    float rotate_current;
    bool rotate_exchange;

    public bool rotate_have;
    public float rotate_start;
    public float rotate_time;
    public float rotate_angle;
    public bool rotate_restore;
    public bool rotate_loop;
    public bool rotate_reverse;

    public state alpha_state;
    public float alpha_duringtime;
    public float alpha_deltafactor;

    public bool alpha_have;
    public float alpha_time;
    public float alpha_factor;
    public bool alpha_restore;
    public bool alpha_loop;

    public UIAnimations()
    {
        animation_id = 0;

        control_data = new ArrayList();

        translate_have = false;
        translate_start = new Vector2(0, 0);
        translate_time = 0;
        translate_offset = new Vector2(0,0);
        translate_restore = false;
        translate_loop = false;
        translate_reverse = false;

//         scale_state = state.none;
//         scale_duringtime = 0;
//         scale_deltafactor = 1;
// 
//         scale_have = false;
//         scale_time = 0;
//         scale_factor = 1;
//         scale_restore = false;
//         scale_loop = false;

        rotate_have = false;
        rotate_start = 0;
        rotate_time = 0;
        rotate_angle = 0;
        rotate_restore = false;
        rotate_loop = false;
        rotate_reverse = false;

        alpha_have = false;
        alpha_time = 0;
        alpha_factor = 1;
        alpha_restore = false;
        alpha_loop = false;
    }

    public void Reset()
    {
        translate_state = state.none;
        translate_duringtime = 0;
        translate_current = new Vector2(0, 0);
        translate_exchange = false;

        rotate_state = state.none;
        rotate_duringtime = 0;
        rotate_current = 0;
        rotate_exchange = false;

        alpha_state = state.none;
        alpha_duringtime = 0;
        alpha_deltafactor = 1;
    }

    public bool IsRuning()
    {
        return ((state.none != translate_state) ||
                //(state.none != scale_state) ||
                (state.none != rotate_state) ||
                (state.none != alpha_state));
    }

    public void Start()
    {
        if (translate_have)
        {
            translate_state = state.doing;
        }

        if (rotate_have)
        {
            rotate_state = state.doing;
        }
    }

    public void Stop()
    {
        translate_state = state.none;
    }

    public bool IsTranslating()
    {
        return (state.none != translate_state);
    }

    public Vector2 GetTranslate() 
    {
        switch (translate_state)
        {
            case state.none:
                break;
            case state.doing:
                break;
            case state.wait_end:
                translate_state = state.end;
                break;
            case state.end:
                if (translate_loop)
                {
                    translate_state = state.doing;
                    if (translate_reverse)
                    {
                        translate_offset = -translate_offset;
                        translate_start = translate_current;
                    }
                }
                else if (translate_restore)
                {   
                    if (translate_reverse)
                    {
                        translate_offset = -translate_offset;
                        translate_start = translate_current;
                        if (!translate_exchange)
                        {
                            
                            translate_exchange = true;
                            translate_state = state.doing;
                        }
                        else
                        {
                            translate_state = state.none;
                        }
                    }
                    else
                    {
                        translate_state = state.none;
                        translate_current = translate_start;
                    }
                }
                else 
                {
                    translate_state = state.none;
                }
                translate_duringtime = 0;
                break;
        }
        return translate_current;
    }

    public bool IsRotating()
    {
        return (state.none != rotate_state);
    }

    public float GetRotate()
    {
        switch (rotate_state)
        {
            case state.none:
                break;
            case state.doing:
                break;
            case state.wait_end:
                rotate_state = state.end;
                break;
            case state.end:

                if (rotate_loop)
                {
                    rotate_state = state.doing;
                    if (rotate_reverse)
                    {
                        rotate_angle = -rotate_angle;
                        rotate_start = rotate_current;
                    }
                }
                else if (rotate_restore)
                {
                    if (rotate_reverse)
                    {
                        rotate_angle = -rotate_angle;
                        rotate_start = rotate_current;
                        if (!rotate_exchange)
                        {

                            rotate_exchange = true;
                            rotate_state = state.doing;
                        }
                        else
                        {
                            rotate_state = state.none;
                        }
                    }
                    else
                    {
                        rotate_state = state.none;
                        rotate_current = rotate_start;
                    }
                }
                else
                {
                    rotate_state = state.none;
                }
                rotate_duringtime = 0;
                break;
        }
        return rotate_current;
    }

    public void Update(float delta_time)
    {
        float translate_deltatime = delta_time;
        if (state.doing == translate_state)
        {
            translate_duringtime += translate_deltatime;
            if (translate_duringtime > translate_time)
            {
                translate_duringtime = translate_time;
                translate_state = state.wait_end;
            }
            translate_current = (translate_duringtime / translate_time) * translate_offset + translate_start;
        }

        float rotate_deltatime = delta_time;
        if (state.doing == rotate_state)
        {
            rotate_duringtime += rotate_deltatime;
            if (rotate_duringtime > rotate_time)
            {
                rotate_duringtime = rotate_time;
                rotate_state = state.wait_end;
            }
            rotate_current = (rotate_duringtime / rotate_time) * rotate_angle + rotate_start;
        }
    }
}
