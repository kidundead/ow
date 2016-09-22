//! @file Utils.cs


using UnityEngine;
using System;
using System.IO;
using System.Globalization;
using System.Collections;
using System.Runtime.InteropServices;


//! @class Utils
public class Utils
{
	private static string m_SavePath;

	static Utils()
	{
		string path = Application.dataPath;

	#if UNITY_IPHONE
		if (!Application.isEditor)
		{
			path = path.Substring(0, path.LastIndexOf('/'));
			path = path.Substring(0, path.LastIndexOf('/'));
		}
		path += "/Documents";
	#else // Win32
		path += "/../Documents";
	#endif

		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}

		m_SavePath = path;
	}

    public static bool CreateDocumentSubDir(string dirname)
    {
        string path = m_SavePath + "/" + dirname;
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            return true;
        }
        return false;
    }

    public static void DeleteDocumentDir(string dirname)
    {
        string path = m_SavePath + "/" + dirname;
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }
    }

	public static string SavePath()
	{
		return m_SavePath;
	}

    public static string GetTextAsset(string txt_name)
    {
        TextAsset conf = Resources.Load(txt_name) as TextAsset;
        if (null != conf)
        {
            return conf.text;
        }
        return "";
    }

    public static void FileSaveString(string name, string content)
    {
        string filename = Utils.SavePath() + "/" + name;

        try
        {
            FileStream file = new FileStream(filename, FileMode.Create);
            StreamWriter sw = new StreamWriter(file);

            sw.Write(content);

            sw.Close();
            file.Close();
        }
        catch
        {
            Debug.Log("Save" + filename + " error");
        }
    }

    public static void FileGetString(string name, ref string content)
    {
        string filename = Utils.SavePath() + "/" + name;
        if (!File.Exists(filename))
        {
            return;
        }

        try
        {
            FileStream file = new FileStream(filename, FileMode.Open);
            StreamReader sr = new StreamReader(file);

            content = sr.ReadToEnd();

            sr.Close();
            file.Close();
        }
        catch
        {
            Debug.Log("Load" + filename + " error");
        }
    }

    public static bool IsChineseLetter(string input)
    {
        for (int i = 0; i < input.Length; i++)
        {
            int lint = System.Convert.ToInt32(System.Convert.ToChar(input.Substring(i, 1)));
            if (lint >= 128)
            {
                return true;
            }
        }
        return false;
    }

	public static string PhotoKey()
	{
		DateTime dt = DateTime.Now;
		return dt.ToString("yyyyMMddHHmmssfff", DateTimeFormatInfo.InvariantInfo) + UnityEngine.Random.Range(1, 10000);
	}

	public static void AvataTakeTempPhoto()
	{
		string filename = "TempPhoto.png";
		Application.CaptureScreenshot(filename);
	}

	public static void AvataTakePhoto(string photo_key)
	{
	#if UNITY_IPHONE
		AvataTakePhotoiPhone(photo_key);
	#else // Win32
		AvataTakePhotoWin32(photo_key);
	#endif
	}

#if UNITY_IPHONE
	[DllImport("__Internal")]
	protected static extern void AvataTakePhotoPlugin(string save_path, string photo_key);

	public static void AvataTakePhotoiPhone(string photo_key)
	{
		AvataTakePhotoPlugin(SavePath(), photo_key);
	}

    [DllImport("__Internal")]
	protected static extern void SendMail(string address, string subject, string content);
    public static void ToSendMail(string address, string subject, string content)
    {
        SendMail(address, subject, content);
    }

#else // Win32

    public static void ToSendMail(string address, string subject, string content)
    {
    }

    public static void AvataTakePhotoWin32(string photo_key)
	{
		try
		{
			// ����
			Texture2D texture1 = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
			texture1.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
			texture1.Apply();

			// ȡ����
			byte [] bytes1 = texture1.EncodeToPNG();

			// ����
			string filename1 = Utils.SavePath() + "/Avatar/" + photo_key + "_photo.png";
			FileStream file1 = new FileStream(filename1, FileMode.Create, FileAccess.Write);
			BinaryWriter bw1 = new BinaryWriter(file1);
			bw1.Write(bytes1);
			bw1.Close();
			file1.Close();

			/*
			// ��������ͼ
			Texture2D texture2 = new Texture2D(78, 115, TextureFormat.ARGB32, false);
			texture2.SetPixels(texture1.GetPixels(200, 110, 78, 115));
			texture2.Apply();

			// ȡ����
			byte[] bytes2 = texture2.EncodeToPNG();

			// ��������ͼ
			string filename2 = Utils.SavePath() + "/" + photo_key + "_thumbnail.png";
			FileStream file2 = new FileStream(filename2, FileMode.Create, FileAccess.Write);
			BinaryWriter bw2 = new BinaryWriter(file2);
			bw2.Write(bytes2);
			bw2.Close();
			file2.Close();
			*/

			// ɾ��ͼƬ����
			UnityEngine.Object.Destroy(texture1);
			//UnityEngine.Object.Destroy(texture2);
		}
		catch
		{
			Debug.Log("AvataTakePhotoWin32 error");
		}
	}
#endif

#if UNITY_IPHONE
    [DllImport("__Internal")]
    protected static extern int MessgeBox1(string title, string message, string button);

    [DllImport("__Internal")]
    protected static extern int MessgeBox2(string title, string message, string button1, string button2);
#endif

    public static int ShowMessageBox1(string title, string message, string button)
    {
#if UNITY_IPHONE
        return MessgeBox1(title, message, button);
#else
        return 0;
#endif
    }

    public static int ShowMessageBox2(string title, string message, string button1, string button2)
    {
#if UNITY_IPHONE
        return MessgeBox2(title, message, button1, button2);
#else
        return 0;
#endif
    }

}

