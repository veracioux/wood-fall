using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Music : MonoBehaviour
{

    public AudioClip[] soundtrack;
    public float[] volumes;
    public int current = 0;
    bool isPaused = false;
    public AudioSource audioSource;

    void Awake()
    {
        if (Settings.music == null)
        {
            DontDestroyOnLoad(gameObject);
            Settings.music = this;
        }
        else if (Settings.music != this)
            Destroy(gameObject);
    }
    void Start()
    {
        for (int i = 0; i < System.DateTime.Now.Millisecond % soundtrack.Length; i++)
            Next();
    }

    void Update()
    {
        if (!audioSource.isPlaying && !isPaused)
        {
            Next();
        }
    }

    public void Next()
    {
        current++;
        current %= soundtrack.Length;
        audioSource.clip = soundtrack[current];
        audioSource.volume = Settings.musicVolume * volumes[current];
        audioSource.Play();
    }

    public void Prev()
    {
        current += soundtrack.Length - 1;
        current %= soundtrack.Length;
        audioSource.clip = soundtrack[current];
        audioSource.volume = Settings.musicVolume * volumes[current];
        audioSource.Play();
    }

    public void Pause()
    {
        isPaused = true;
        audioSource.Pause();
    }
    public void Unpause()
    {
        isPaused = false;
        audioSource.UnPause();
    }
}
