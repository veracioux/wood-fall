
//#define WOODFALL_DEBUG

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/*using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using UnityEngine.SceneManagement;*/

public class Init : MonoBehaviour
{
    public static Init init;
    public static bool isShowingBanner, isRecordingReady = false, uploading = false;
    static float time = -190;
    //public static bool loggedInGooglePlay = false;

    void Awake()
    {
        if (init == null)
        {
            DontDestroyOnLoad(gameObject);
            init = this;
        }
        else if (init != this)
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
    }

    void OnGUI()
    {
        //GUI.Label(new Rect(20, 20, 300, 100), triedGPLogin.ToString() + "  " + PlayerData.hasLoaded.ToString() + "  " + PlayerData.disableAds.ToString());
    }

    void Start()
    {
        Settings._Load();
        Settings.Refresh();
        Statistics.Load();
        /*PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();
        PlayGamesPlatform.InitializeInstance(config);
        if (Settings.googlePlayLogin)
            PlayGamesPlatform.Instance.Authenticate(GPLogin, false);*/
        Tutorial.data = PlayerPrefs.GetString("tut", "1111111");
    }

    void Update()
    {
    }

    /*public static void GPLogin(bool success)
    {
        loggedInGooglePlay = success;
        if (success)
        {
            Statistics.GPLoad();
        }
    }*/

}
