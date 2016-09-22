using UnityEngine;
using System;
using System.Collections;
using System.IO;

public class OptionsInterface 
{
    private static bool m_bOpenMusic;
    private static bool m_bOpenSound;
    private static bool m_bRevertYAris;
	private static string m_filename;

    static OptionsInterface()
    {
        m_bOpenMusic = true;
        m_bOpenSound = true;
        m_bRevertYAris = false;
		
		m_filename = "/Options.dat";
    }

    public static bool IsOpenMusic()
    {
        return m_bOpenMusic;
    }

    public static void SetMusic(bool bVal)
    {
        m_bOpenMusic = bVal;
        Save();
    }

    public static bool IsOpenSound()
    {
        return m_bOpenSound;
    }

    public static void SetSound(bool bVal)
    {
        m_bOpenSound = bVal;
        Save();
    }

    public static bool IsRevertYAris()
    {
        return m_bRevertYAris;
    }

    public static void SetRevertYAris(bool bVal)
    {
        m_bRevertYAris = bVal;
        Save();
    }

    public static void Load()
    {
        string content = "";
        Utils.FileGetString(m_filename, ref content);

        string[] lines = content.Split('\n');
        for (int i = 0; i < lines.Length; ++i)
        {
            string line = lines[i];

            int pos = line.IndexOf(':');
            if (pos < 0)
            {
                continue;
            }

            string key = line.Substring(0, pos);
            string value = line.Substring(pos + 1);

            if (key == "music")
            {
                int v = int.Parse(value);
                m_bOpenMusic = (v == 1);
            }
            else if (key == "sound")
            {
                int v = int.Parse(value);
                m_bOpenSound = (v == 1);
            }
            else if (key == "Yaris")
            {
                int v = int.Parse(value);
                m_bRevertYAris = (v == 1);
            }
        }
    }

    public static void Save()
    {
        string strContent = "";
        strContent += "music:" + (m_bOpenMusic ? 1 : 0) + "\n";
        strContent += "sound:" + (m_bOpenSound ? 1 : 0) + "\n";
        strContent += "Yaris:" + (m_bRevertYAris ? 1 : 0) + "\n";

        Utils.FileSaveString(m_filename, strContent);
    }
}
