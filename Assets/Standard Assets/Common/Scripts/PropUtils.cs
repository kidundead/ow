using UnityEngine;
using System.Collections;

public class PropUtils
{
    protected Hashtable m_Props = new Hashtable();

    public void Clear()
    {
        m_Props.Clear();
    }

    public void RemoveProp(string key)
    {
        m_Props.Remove(key);
    }

    public void RemoveProps(string[] keys)
    {
        if (null == keys) return;
        for (int i = 0; i < keys.Length; i++)
        {
            m_Props.Remove(keys[i]);
        }
    }

    public void SetProp(string key, object value)
    {
        if (m_Props.ContainsKey(key))
            m_Props.Remove(key);
        m_Props[key] = value;
    }

    public object GetObject(string key)
    {
        if (!m_Props.ContainsKey(key))
            return null;
        return m_Props[key];
    }

    public bool GetBool(string key)
    {
        if (!m_Props.ContainsKey(key) || m_Props[key] == null)
            return false;
        object value = m_Props[key];
        if (value is string)
            return bool.Parse(value.ToString());
        return (bool)m_Props[key];
    }

    public float GetFloat(string key, float ret)
    {
        if (!m_Props.ContainsKey(key) || m_Props[key] == null)
            return ret;
        object value = m_Props[key];
        if (value is string)
            return float.Parse(value.ToString());
        return (float)value;
    }

    public int GetInt(string key, int ret)
    {
        if (!m_Props.ContainsKey(key) || m_Props[key] == null)
            return ret;
        object value = m_Props[key];
        if (value is string)
            return int.Parse(value.ToString());
        return (int)value;
    }

    public long GetLong(string key, long ret)
    {
        if (!m_Props.ContainsKey(key) || m_Props[key] == null)
            return ret;
        object value = m_Props[key];
        if (value is string)
            return long.Parse(value.ToString());
        return (long)value;
    }

    public string GetString(string key)
    {
        if (!m_Props.ContainsKey(key) || m_Props[key] == null)
            return "";
        return m_Props[key].ToString();
    }

    public Vector2 GetV2(string key)
    {
        if (!m_Props.ContainsKey(key) || m_Props[key] == null)
            return Vector2.zero;
        return (Vector2)m_Props[key];
    }

    public Vector3 GetV3(string key)
    {
        if (!m_Props.ContainsKey(key) || m_Props[key] == null)
            return Vector3.zero;
        return (Vector3)m_Props[key];
    }

    public GameObject GetGameObject(string key)
    {
        if (!m_Props.ContainsKey(key) || m_Props[key] == null)
            return null;
        return (GameObject)m_Props[key];
    }
}