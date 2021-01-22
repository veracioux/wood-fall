using UnityEngine;
using UnityEngine.UI;
using System.Collections;
/*using GooglePlayGames;
using UnityEngine.SocialPlatforms;*/

public class Settings : MonoBehaviour
{


    public static float musicVolume = 1, soundVolume = 1;
    public static bool touchInput = true, recordReplays = false/*, googlePlayLogin = true*/;
    public static int difficulty = 0, nDifficulties = 5;
    public static Music music;
    public static Sound sound;
    public Slider _music, _sound;
    public Text input;
    //public GameObject gpOn, gpOff;
    public bool usesSliders;
    bool inited = false;

    void Start()
    {
        StartCoroutine(load());
        if (usesSliders)
        {
            _music.value = musicVolume;
            _sound.value = soundVolume;
            if (input != null)
                input.text = touchInput ? "Touch" : "Rotation";
        }
        /*if (!PlayGamesPlatform.Instance.IsAuthenticated())
        {
            Init.loggedInGooglePlay = googlePlayLogin = false;
            PlayerPrefs.SetInt("play", 0);
        }*/
        /*if (gpOn != null)
            gpOn.SetActive(googlePlayLogin);
        if (gpOff != null)
            gpOff.SetActive(!googlePlayLogin);*/
    }

    IEnumerator load()
    {
        yield return new WaitUntil(() => music != null);
        yield return new WaitUntil(() => sound != null);
        music.audioSource.volume = musicVolume * music.volumes[music.current];
        foreach (AudioSource s in sound.audioSources)
            if (s != null)
                s.volume = soundVolume;
    }

    IEnumerator set()
    {
        yield return new WaitUntil(() => _music != null);
        yield return new WaitUntil(() => _sound != null);
        yield return new WaitUntil(() => input != null);
        _music.value = musicVolume;
        _sound.value = soundVolume;
        input.text = touchInput ? "Touch" : "Rotation";
    }

    public static void Refresh()
    {
        if (music != null)
            music.audioSource.volume = musicVolume * music.volumes[music.current];
        if (sound != null)
            foreach (AudioSource s in sound.audioSources)
                if (s != null)
                    s.volume = soundVolume;
        Save();
    }

    public void Reload()
    {
        if (inited)
        {
            musicVolume = _music.value;
            soundVolume = _sound.value;
            Refresh();
        }
        inited = true;
    }

    public static void Save()
    {
        PlayerPrefs.SetFloat("music", musicVolume);
        PlayerPrefs.SetFloat("sound", soundVolume);
        PlayerPrefs.SetInt("touchInput", touchInput ? 1 : 0);
        PlayerPrefs.SetInt("record", recordReplays ? 1 : 0);
        PlayerPrefs.SetInt("difficulty", difficulty);
        PlayerPrefs.SetInt("theme", LevelGenerator.themeID);
        PlayerPrefs.SetInt("seed", LevelGenerator.seed);
    }

    public void Load()
    {
        _Load();
    }

    public static void _Load()
    {
        musicVolume = PlayerPrefs.GetFloat("music", 0.5f);
        soundVolume = PlayerPrefs.GetFloat("sound", 0.5f);
        touchInput = PlayerPrefs.GetInt("touchInput", 1) == 1 ? true : false;
        recordReplays = PlayerPrefs.GetInt("record", 0) == 1 ? true : false;
        difficulty = PlayerPrefs.GetInt("difficulty", 0);
        LevelGenerator.themeID = PlayerPrefs.GetInt("theme", 0);
        LevelGenerator.seed = PlayerPrefs.GetInt("seed", 0);
        //googlePlayLogin = PlayerPrefs.GetInt("play", 1) == 1 ? true : false;
    }

    /*public void GooglePlayLogin()
    {
        PlayGamesPlatform.Instance.Authenticate(Init.GPLogin, false);
        gpOff.SetActive(false);
        gpOn.SetActive(true);
        googlePlayLogin = true;
        PlayerPrefs.SetInt("play", 1);
    }

    IEnumerator GPLogin()
    {
        yield return new WaitUntil(() => Init.loggedInGooglePlay);
    }

    public void GooglePlayLogout()
    {
        PlayGamesPlatform.Instance.SignOut();
        gpOn.SetActive(false);
        gpOff.SetActive(true);
        PlayerPrefs.SetInt("play", 0);
        googlePlayLogin = false;
        Init.loggedInGooglePlay = false;
    }*/

    public void ToggleInputMethod()
    {
        touchInput = !touchInput;
        input.text = touchInput ? "Touch" : "Rotation";
        PlayerPrefs.SetInt("touchInput", touchInput ? 1 : 0);
    }

    public static string GetDifficultyName(int difficulty)
    {
        string diff = "Difficulty";
        switch (difficulty)
        {
            case 0:
                diff = "Easy";
                break;
            case 1:
                diff = "Normal";
                break;
            case 2:
                diff = "Hard";
                break;
            case 3:
                diff = "Insane";
                break;
            case 4:
                diff = "Impossible";
                break;
        }
        return diff;
    }

    void OnGUI()
    {
        //GUI.Label(new Rect(40, 90, 100, 100), (_master != null).ToString());
    }
}
