//! @file AudioManager.cs


using UnityEngine;
using System.Collections;


//! @class AudioManager
public class AudioManager : MonoBehaviour
{
	public void Awake()
	{
		transform.position = Vector3.zero;
		transform.rotation = Quaternion.identity;
	}

    public AudioClip LoadMusic(string name)
    {
        return Resources.Load(name, typeof(AudioClip)) as AudioClip;
    }

    public AudioClip LoadSound(string name)
    {
        return Resources.Load(name, typeof(AudioClip)) as AudioClip;
    }

    public void PlayMusic(string name)
    {
        if (OptionsInterface.IsOpenMusic())
        {
            AudioClip clip = LoadMusic(name);

            GameObject obj = new GameObject("AudioMusic::" + clip.name);
            obj.transform.parent = transform;

            AudioSource source = obj.AddComponent(typeof(AudioSource)) as AudioSource;
            source.clip = clip;
            source.loop = true;
            source.playOnAwake = false;
            source.Play();
        }
    }

    public void PlaySound(string name)
    {
        if (OptionsInterface.IsOpenSound())
        {
            AudioClip clip = LoadSound(name);

            GameObject obj = new GameObject("AudioSound::" + clip.name);
            obj.transform.parent = transform;

            AudioSource source = obj.AddComponent(typeof(AudioSource)) as AudioSource;
            source.clip = clip;
            source.Play();
            source.playOnAwake = false;
            Destroy(obj, clip.length);
        }
    }

    public void StopMusic()
    {
        ArrayList al = new ArrayList();

        foreach (Transform child in transform)
        {
            if (child.name.StartsWith("AudioMusic::"))
            {
                al.Add(child.gameObject);
            }
        }

        int count = al.Count;
        for (int i = 0; i < count; ++i)
        {
            Destroy(((GameObject)al[i]));
        }
    }
}

