using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    [System.Serializable]
    public class Sound
    {
        public string name;

        public bool loop;

        public AudioClip clip;

        [Range(0f, 1f)]
        public float volume = 0.5f;
        [Range(.1f, 3f)]
        public float pitch = 1f;

        [HideInInspector]
        public AudioSource source;
    }

    public Sound[] sounds;
    public static AudioManager instance;
    public bool muted;

    void Awake()
    {
        muted = false;
        //Make sure there is only 1 AudioManager in the Scene
        if (instance == null)
        {
            instance = this;
        }

        else
        {
            Destroy(gameObject);
        }

        //Don't destroy AudioManager so music doesn't get interrupted
        DontDestroyOnLoad(gameObject);

        foreach(Sound s in sounds)
        {
            //Add AudioSource for every sound in the array
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.loop = s.loop;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }

        Play("BGM");
    }

    public void Play(string name)
    {
        bool found = false;
        //Look for desired sound in array
        for (int i =0; i < sounds.Length; i++ )
        {
            if (sounds[i].name == name)
            {
                //Play desired sound if not muted
                if (!muted)
                {
                    sounds[i].source.Play();
                }
                else
                {
                    Debug.Log("Audio is muted");
                }
                found = true;
            }
        }

        if (!found)
        {
            //Tells if desired sound could not be found
            Debug.LogWarning("Sound: " + name + " could not be found (Play)");
        }
    }

    public void Stop(string name)
    {
        bool found = false;
        //Look for desired sound in array
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == name)
            {
                //Play desired sound if not muted
                if (!muted)
                {
                    sounds[i].source.Stop();
                }
                else
                {
                    Debug.Log("Audio is muted");
                }
                found = true;
            }
        }

        if (!found)
        {
            Debug.LogWarning("Sound: " + name + " could not be found (Stop)");
        }
    }

    public void Mute()
    {
        if (!muted)
        {
            muted = true;
        }

        else
        {
            muted = false;
        }
    }
}
