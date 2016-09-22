using UnityEngine;
using System.Collections;
using Zombie3D;

public class AudioInfo
{
    public AudioSource audio;
    public float lastPlayingTime;
}

public class AudioPlayer
{

    protected Hashtable audioTable = new Hashtable();
    protected float lastPlayingTime;
    public void AddAudio(Transform folderTrans, string name)
    {

        if (folderTrans != null)
        {
            Transform audioTrans = folderTrans.Find(name);
            if (audioTrans != null)
            {
                if (!audioTable.Contains(name))
                {
                    AudioInfo info = new AudioInfo();
                    info.audio = audioTrans.GetComponent<AudioSource>();
                    audioTable.Add(name, info);
                }
            }

        }


    }

    /*
    public bool IsPlaying(string name)
    {
        AudioInfo info = audioTable[name] as AudioInfo;
        if (info != null)
        {
            AudioSource audio = info.audio;
            if (audio != null)
            {
                if (!audio.isPlaying)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        return false;
    }
    */


    public void PlayAudio(string name)
    {
        if (GameApp.GetInstance().GetGameState().MusicOn)
        {

            AudioInfo info = audioTable[name] as AudioInfo;
            if (info != null)
            {
                AudioSource audio = info.audio;

                if (audio != null)
                {

                    if (Time.time - info.lastPlayingTime > audio.clip.length)
                    {
                        audio.Play();
                        info.lastPlayingTime = Time.time;
                    }
                }
            }
        }
    }

    public static void PlayAudio(AudioSource audio)
    {
        if (GameApp.GetInstance().GetGameState().MusicOn)
        {
            audio.Play();
        }
    }



}
