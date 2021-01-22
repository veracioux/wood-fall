using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class Play : MonoBehaviour
{
    public Text seedAvailable, difficulty, placeholder;
    public InputField seedSource;
    public UIAnim seedImg;
    int currentTheme;
    public GameObject[] themes;
    public static bool hasBeenSet = false;

    void Start()
    {
        difficulty.text = Settings.GetDifficultyName(Settings.difficulty);
        placeholder.text = LevelGenerator.seed.ToString();
        themes[currentTheme = LevelGenerator.themeID].SetActive(true);
        if (!hasBeenSet) return;
        seedSource.text = LevelGenerator.seed.ToString();
        SetSeed();
        seedImg.transform.eulerAngles = new Vector3(0, 0, 180);
        for (int i = 0; i < seedImg.disable.Length; i++)
            seedImg.disable[i].enabled = true;
    }
    public void SetSeed()
    {
        if (seedSource.text == "")
            seedSource.text = LevelGenerator.seed.ToString();
        int s = Convert.ToInt32(seedSource.text);
        //seedSource.text = s.ToString();
        LevelGenerator.seed = s;
        hasBeenSet = true;
        PlayerPrefs.SetInt("seed", s);
    }

    public void NextSeed()
    {
        seedSource.text = (LevelGenerator.seed + 1).ToString();
        SetSeed();
    }
    public void PrevSeed()
    {
        if (LevelGenerator.seed < 1)
            return;
        seedSource.text = (LevelGenerator.seed - 1).ToString();
        SetSeed();
    }

    public void NextDifficulty()
    {
        Settings.difficulty++;
        Settings.difficulty %= Settings.nDifficulties;
        PlayerPrefs.SetInt("difficulty", Settings.difficulty);
        difficulty.text = Settings.GetDifficultyName(Settings.difficulty);
    }

    public void PrevDifficulty()
    {
        Settings.difficulty += Settings.nDifficulties - 1;
        Settings.difficulty %= Settings.nDifficulties;
        PlayerPrefs.SetInt("difficulty", Settings.difficulty);
        difficulty.text = Settings.GetDifficultyName(Settings.difficulty);
    }

    void ThemeChange(int next)
    {
        themes[currentTheme].SetActive(false);
        themes[currentTheme = (currentTheme + next + themes.Length) % themes.Length].SetActive(true);
        //if (PlayerData.themes[currentTheme])
        LevelGenerator.themeID = currentTheme;
    }

    public void ThemeNext()
    {
        ThemeChange(1);
    }

    public void ThemePrev()
    {
        ThemeChange(-1);
    }
}
