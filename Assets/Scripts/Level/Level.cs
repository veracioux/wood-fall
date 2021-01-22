using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
//using GooglePlayGames;

public class Level : MonoBehaviour
{

    public static float defTimeScale = 1, time = 0;
    public static bool isPaused, hasStarted, ignoreTouches, canRestart;
    public static float gameTime = 0;
    public GameObject overlay, menu, startOverlay, gameOver, shareReplay, replay, mainMenu, _continue, revive, canvas, arrow, noAdsAvailable;
    public static Player player;
    public static Level level;
    public static Tail tail;
    public static float health = 1;
    public Image healthBar;
    public Powerup[] powerups;
    public bool powerup1, powerup2, usedRevive, tutorialEnabled;
    public RectTransform powerupSlot1, powerupSlot2;
    public static Transform reviver;
    Coroutine unpauseTimer;

    public bool isTouching, reviving = false, reviveApproved = false;

    public Text distanceCounter, starCounter, gameOverDistance, bestScore, newBestScore, gameOverStars, bestStars, newBestStars, metadata;
    private Vector3 offset;
    public static float gravity = 9.81f, test, fpsTime = 0.9f, fps = 0f, rfps = 0f, damageModifier = 1, gravityModifier = 1, tailGravityModifier = 1;
    public float distance = 0;
    public int stars = 0;
    public Image tutorialPlayer;
    public Animator blackScreen;
    bool pressedContinue = false;


    void Start()
    {
        DisableAnalytics.disableAnalytics();
        SetTimeScale(1f);
        Time.timeScale = 0;
        level = this;
        health = 1;
        time = gameTime;
        ignoreTouches = isPaused = hasStarted = false;
        canRestart = true;
        Physics2D.IgnoreLayerCollision(12, 0, false);
        Physics2D.IgnoreLayerCollision(8, 9, false);
        Physics2D.IgnoreLayerCollision(8, 10, false);
        GameObject bgr = Instantiate(LevelGenerator.background);
        bgr.transform.position = new Vector3(0, -3, 0);
        bgr.transform.SetParent(gameObject.transform);
        (player = Instantiate(LevelGenerator.player).GetComponentInChildren<Player>()).name = "Player";
        tail = Instantiate(LevelGenerator.tail).GetComponent<Tail>();
        offset = transform.position - player.transform.position;
        if (Settings.sound != null)
        {
            if (Settings.sound.audioSources.Length < 3)
            {
                AudioSource[] a = Settings.sound.audioSources;
                Settings.sound.audioSources = new AudioSource[3];
                Settings.sound.audioSources[0] = a[0];
                Settings.sound.audioSources[1] = a[1];
            }
            (Settings.sound.audioSources[2] = tail.gameObject.GetComponent<AudioSource>()).volume = Settings.soundVolume;
        }
        AudioListener.pause = false;
        switch (Settings.difficulty)
        {
            case 0:
                damageModifier = 1;
                gravityModifier = .53f;
                tailGravityModifier = .64f;
                break;
            case 1:
                damageModifier = 1.5f;
                gravityModifier = .94f;
                tailGravityModifier = 1.15f;
                break;
            case 2:
                damageModifier = 1.8f;
                gravityModifier = 1.4f;
                tailGravityModifier = 1.8f;
                break;
            case 3:
                damageModifier = 2.3f;
                gravityModifier = 1.8f;
                tailGravityModifier = 2.35f;
                break;
            case 4:
                damageModifier = 4;
                gravityModifier = 2.5f;
                tailGravityModifier = 3.2f;
                break;
        }
        gravity = 9.81f * gravityModifier;
        Physics2D.gravity = new Vector2(0, -gravity);
        Physics.gravity = Physics2D.gravity;
        tutorialPlayer.sprite = player.GetComponent<SpriteRenderer>().sprite;
    }

    void Update()
    {
        if (!isPaused && hasStarted && !player.dead && Time.timeScale != 0)
            gameTime += Time.unscaledDeltaTime;
        if (-player.transform.position.y / 8.5f > distance)
            distance = -player.transform.position.y / 8.5f;
        if (!isPaused && hasStarted)
        {
            if (powerup1)
            {
                powerups[0].display.GetComponent<Image>().fillAmount = 1 - (gameTime - powerups[0].startTime) / powerups[0].maxTime;
            }
        }
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, health, 9 * Time.unscaledDeltaTime);
        if (powerups[0].running && gameTime > powerups[0].startTime + powerups[0].maxTime - 3 / gravityModifier)
        {
            SetTimeScale(Mathf.Lerp(Time.timeScale, 1, Time.deltaTime));
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Unpause();
            else if (!tutorialPlayer.IsActive())
                Pause();
        }
        if (pressedContinue && !Settings.touchInput)
        {
            Vector2 v = Input.acceleration;
            if (v.y < 0)
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg + 180);
                Physics2D.gravity = v * gravity / v.magnitude;
            }
            else
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, v.x > 0 ? 180 : 0);
                Physics2D.gravity = new Vector2(Mathf.Sign(v.x) * gravity, 0);
            }
        }
    }

    void FixedUpdate()
    {
        if (player.dead)
            if (Time.timeScale > 0.01f * gravityModifier)
            {
                Time.timeScale -= 0.01f * gravityModifier;
                Time.fixedDeltaTime = 1f / 60f * Time.timeScale;
            }
            else
            {
                Time.timeScale = 0;
            }

    }

    void LateUpdate()
    {
        if (!isPaused)
        {
            float x = transform.position.x,
                y = Mathf.Min(transform.position.y, tail.transform.position.y - 10),
                z = offset.z;
            if (!player.dead)
            {
                if (!ignoreTouches)
                    if (Settings.touchInput)
                    {
                        if (isTouching)
                        {
                            Vector2 v = (Vector2)Input.mousePosition - new Vector2(Screen.width / 2, Screen.height / 2 * (1 + (player.transform.position.y - transform.position.y) / 10));
                            if (v.y < 0)
                                Physics2D.gravity = v * gravity / v.magnitude;
                            else
                                Physics2D.gravity = new Vector2(Mathf.Sign(v.x) * gravity, 0);
                            Physics.gravity = Physics2D.gravity;
                        }
                    }
                    else
                    {
                        Vector2 v = Input.acceleration;
                        if (v.y < 0)
                            Physics2D.gravity = v * gravity / v.magnitude;
                        else
                            Physics2D.gravity = new Vector2(Mathf.Sign(v.x) * gravity, 0);
                        Physics.gravity = Physics2D.gravity;
                    }
                x = player.transform.position.x + offset.x;
                y = Mathf.Min(player.transform.position.y + offset.y, tail.transform.position.y - 10);
                z = offset.z;
            }
            transform.position = new Vector3(x, y, z);
        }
        if (reviving)
        {
            float x = player.transform.position.x + offset.x;
            float y = player.transform.position.y + offset.y;
            float z = offset.z;
            transform.position = new Vector3(Mathf.Lerp(transform.position.x, x, 3 * Time.unscaledDeltaTime), Mathf.Lerp(transform.position.y, y, 3 * Time.unscaledDeltaTime), z);
        }
    }

    void OnGUI()
    {
        if (hasStarted)
        {
            distanceCounter.text = ((int)distance).ToString() + " m";
            starCounter.text = stars.ToString();
        }
        //GUI.Label(new Rect(0, 200, 250, 100), Everyplay.IsRecordingSupported() + " " + Everyplay.IsSupported() + " " + Everyplay.IsReadyForRecording() + " " + Everyplay.IsRecording() + " " + Init.uploading);
    }

    IEnumerator UnignoreTouches()
    {
        yield return new WaitForEndOfFrame();
        ignoreTouches = false;
    }

    public void Pause()
    {
        //if (Settings.music != null)
        //Settings.music.Pause();
        if (player.dead)
            return;
        canRestart = true;
        if (unpauseTimer != null)
            StopCoroutine(unpauseTimer);
        if (Settings.sound != null)
            Settings.sound.Pause();
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.GetComponent<Rigidbody2D>().angularVelocity = 0;
        _continue.transform.GetChild(1).gameObject.SetActive(false);
        _continue.SetActive(false);
        _continue.transform.GetChild(2).gameObject.SetActive(false);
        _continue.transform.GetChild(0).gameObject.SetActive(true);
        /*if (Settings.googlePlayLogin)
            PlayGamesPlatform.Instance.Authenticate(Init.GPLogin, true);*/
        Statistics.AddScore(LevelGenerator.seed, Settings.difficulty, distance);
        Statistics.AddStars(LevelGenerator.seed, Settings.difficulty, stars);
        Statistics.PostTimePlayed(gameTime - time);
        time = gameTime;
        Statistics.Save();
        Time.timeScale = 0f;
        isPaused = true;
        overlay.SetActive(false);
        startOverlay.SetActive(false);
        menu.SetActive(true);
        _continue.transform.GetChild(1).gameObject.SetActive(false);
    }

    public void Unpause()
    {
        if (!hasStarted)
            startOverlay.SetActive(true);
        else
        {
            //Time.timeScale = defTimeScale;
            _continue.SetActive(true);
        }
        /*if (Settings.music != null)
            Settings.music.Unpause();*/
        overlay.SetActive(true);
        menu.SetActive(false);
        isPaused = false;
        ignoreTouches = true;
        pressedContinue = false;
        StartCoroutine(UnignoreTouches());
    }
    public void PressContinue(GameObject arrow)
    {
        if (!pressedContinue)
            unpauseTimer = StartCoroutine(ContinueTimer(arrow));
        pressedContinue = true;
        arrow.SetActive(true);
        if (!Settings.touchInput)
            return;
        Vector2 v = (Vector2)Input.mousePosition - new Vector2(Screen.width / 2, Screen.height / 2 * (1 + (player.transform.position.y - transform.position.y) / 10));
        if (v.y < 0)
        {
            arrow.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg + 180);
            Physics2D.gravity = v * gravity / v.magnitude;
        }
        else
        {
            arrow.transform.eulerAngles = new Vector3(0, 0, v.x > 0 ? 180 : 0);
            Physics2D.gravity = new Vector2(Mathf.Sign(v.x) * gravity, 0);
        }
        Physics.gravity = Physics2D.gravity;
    }

    IEnumerator ContinueTimer(GameObject arrow)
    {
        _continue.transform.GetChild(0).gameObject.SetActive(false);
        _continue.transform.GetChild(2).gameObject.GetComponent<Text>().text = "3";
        _continue.transform.GetChild(2).gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(.9f);
        _continue.transform.GetChild(2).gameObject.GetComponent<Text>().text = "2";
        yield return new WaitForSecondsRealtime(.9f);
        _continue.transform.GetChild(2).gameObject.GetComponent<Text>().text = "1";
        yield return new WaitForSecondsRealtime(.9f);
        reviving = false;
        Physics2D.IgnoreLayerCollision(12, 0, false);
        Physics2D.IgnoreLayerCollision(8, 9, false);
        Physics2D.IgnoreLayerCollision(8, 10, false);
        SetTimeScale(defTimeScale);
        player.dead = false;
        arrow.SetActive(false);
        _continue.SetActive(false);
        _continue.transform.GetChild(2).gameObject.SetActive(false);
        _continue.transform.GetChild(0).gameObject.SetActive(true);
        if (Settings.sound != null)
            Settings.sound.Unpause();
        AudioSource a = tail.GetComponent<AudioSource>();
        if (a != null)
            a.UnPause();
    }

    public void Restart()
    {
        if (canRestart)
            StartCoroutine(_Restart());
    }

    IEnumerator _Restart()
    {
        blackScreen.SetTrigger("Hide");
        blackScreen.gameObject.GetComponent<RawImage>().raycastTarget = true;
        yield return new WaitForSecondsRealtime(.3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PostGameOver()
    {
        StopAllCoroutines();
        canRestart = false;
        foreach (Powerup p in powerups)
            p.StopAllCoroutines();
        if (Settings.sound != null)
        {
            Settings.sound.audioSources[2].Pause();
            AudioSource[] a = Settings.sound.audioSources;
            Settings.sound.audioSources = new AudioSource[2];
            Settings.sound.audioSources[0] = a[0];
            Settings.sound.audioSources[1] = a[1];
        }
        gameOver.SetActive(true);
        level.replay.SetActive(true);
        level.shareReplay.SetActive(Init.isRecordingReady && Settings.recordReplays);
        level.mainMenu.SetActive(true);
        gameOverDistance.text = "<size=55>Distance</size>\n" + Mathf.FloorToInt(distance) + " meters";
        gameOverStars.text = "<size=55>Stars</size>\n" + stars;
        string color = "";
        switch (Settings.difficulty)
        {
            case 0:
                color = "#00A0FF";
                break;
            case 1:
                color = "#00A738";
                break;
            case 2:
                color = "#FFFF00";
                break;
            case 3:
                color = "#FF9900";
                break;
            case 4:
                color = "#FF0000";
                break;
        }
        metadata.text = "Level <color=white><b>#" + LevelGenerator.seed.ToString() + "</b></color> on <color=" + color + "><b>" + Settings.GetDifficultyName(Settings.difficulty) + "</b></color>";
#if UNITY_EDITOR
#else
        /*if (Settings.googlePlayLogin)
            PlayGamesPlatform.Instance.Authenticate(Init.GPLogin, true);*/
#endif
        Statistics.AddScore(LevelGenerator.seed, Settings.difficulty, distance);
        Statistics.AddStars(LevelGenerator.seed, Settings.difficulty, stars);
        Statistics.PostTimePlayed(gameTime - time);
        time = gameTime;
        Statistics.Save();
        float bd = Statistics.GetScore(LevelGenerator.seed, Settings.difficulty), bs = Statistics.GetStars(LevelGenerator.seed, Settings.difficulty);
        bestScore.text = "<size=55>Best</size>\n" + Mathf.FloorToInt(bd) + " meters";
        bestStars.text = "<size=55>Best</size>\n" + (int)bs;
        if (distance == bd)
            newBestScore.gameObject.SetActive(true);
        if (stars == bs)
            newBestStars.gameObject.SetActive(true);
        overlay.SetActive(false);
        player.StartCoroutine(DeathOccurred());
    }

    public static void SetTimeScale(float newTimeScale)
    {
        defTimeScale = newTimeScale;
        Time.fixedDeltaTime = 1f / 60f * defTimeScale;
        if (!isPaused && hasStarted)
            Time.timeScale = defTimeScale;
    }

    void OnApplicationFocus(bool focus)
    {
        if (!focus && !player.dead && hasStarted)
            Pause();
    }

    public void OnStartPlaying()
    {
        if (!isPaused)
        {
            isTouching = true;
            LateUpdate();
            isTouching = false;
            if (Settings.sound != null)
                Settings.sound.Unpause();
            Time.timeScale = defTimeScale;
            startOverlay.SetActive(false);
            hasStarted = true;
        }
    }

    public void PowerupUsed(Powerup powerup)
    {
        StartCoroutine(powerup.Apply());
    }
    public void OnTouch()
    {
        isTouching = true;
    }

    public void OnRelease()
    {
        isTouching = false;
    }

    public static void ApplyDamage(float damage, int type = 0)
    {
        if (!level.powerups[1].running || type == 1)
        {
            health -= damage * damageModifier;
            player.particleSystem.Play();
        }
        else
            level.powerups[1].running = false;
        if (health <= 0f)
        {
            if (!player.dead)
            {
                player.dead = true;
                level.reviving = false;
                Physics2D.IgnoreLayerCollision(12, 0);
                Physics2D.IgnoreLayerCollision(8, 9);
                Physics2D.IgnoreLayerCollision(8, 10);
                level.PostGameOver();
            }
        }
    }

    void RevivePrompt()
    {
        if (Settings.sound != null)
        {
            if (Settings.sound.audioSources[2] != null)
                Settings.sound.audioSources[2].Pause();
            if (Settings.sound.audioSources[1] != null)
                Settings.sound.audioSources[1].Pause();
        }
        revive.SetActive(true);
    }

    public void Death()
    {
        revive.SetActive(false);
        PostGameOver();
    }

    public void Revive()
    {
        StartCoroutine(_Revive());
    }

    IEnumerator _Revive()
    {
        yield return new WaitUntil(() => reviveApproved);
        Time.timeScale = 0;
        health = 1;
        reviving = true;
        revive.SetActive(false);
        usedRevive = true;
        Physics2D.gravity = Physics.gravity = Vector2.zero;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.GetComponent<Rigidbody2D>().angularVelocity = 0;
        if (reviver != null)
            player.transform.position = reviver.position;
        tail.Reset();
        tail.GetComponent<AudioSource>().clip = tail.normalClip;
        tail.GetComponent<AudioSource>().Play();
        Unpause();
    }

    IEnumerator DeathOccurred()
    {
        yield return new WaitForSecondsRealtime(1);
        Time.timeScale = 0;
        canRestart = true;
    }

    public static void OnCollect()
    {
        level.stars++;
#if UNITY_EDITOR
        return;
#endif
        /*if (Settings.googlePlayLogin)
            PlayGamesPlatform.Instance.Authenticate(Init.GPLogin, true);
        if (!Init.loggedInGooglePlay)
            return;
        Statistics.UpdateStarAchievements(Settings.difficulty, level.stars);*/
    }
}
