using UnityEngine;
using System.Collections;

public class Sound : MonoBehaviour
{

    public AudioSource[] audioSources;
    public static bool playingSaw;

    void Awake()
    {
        if (Settings.sound == null)
        {
            DontDestroyOnLoad(gameObject);
            Settings.sound = this;
        }
        else if (Settings.sound != this)
            Destroy(gameObject);
    }

    void Start()
    {
        playingSaw = false;
    }

    void Update()
    {

    }

    public static void PlayAxeChop()
    {
        if (Settings.sound != null)
            Settings.sound.audioSources[0].Play();
    }

    public static void PlaySaw()
    {
        if (Settings.sound != null)
        {
            Settings.sound.audioSources[1].UnPause();
            Settings.sound.audioSources[1].Play();
        }
        playingSaw = true;
    }

    public void Pause()
    {
        foreach (AudioSource s in audioSources)
            if (s != null)
                s.Pause();
        playingSaw = false;
    }

    public void Unpause()
    {
        foreach (AudioSource s in audioSources)
            if (s != null && !s.loop)
                s.UnPause();
    }
}
