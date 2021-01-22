using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;
using System.Collections;
/*using GooglePlayGames;
using GooglePlayGames.BasicApi.SavedGame;
using GooglePlayGames.BasicApi;*/
using System;

public class Statistics : MonoBehaviour
{

    public static float timePlayed = 0;
    public static float[] recordScore = { 0, 0, 0, 0, 0, 0, 0, 0 }, recordStars = { 0, 0, 0, 0, 0, 0, 0, 0 };
    public static float[,] bestScores, bestStars;
    public Text _timePlayed, _bestScore, _bestStars, difficultyDisplay, seedDisplay;
    public InputField seedInputField;
    int difficulty = 0, seed = 0;
    public static string[] distanceLeaderboardIds = { "CgkIot63j_wSEAIQAw", "CgkIot63j_wSEAIQBA", "CgkIot63j_wSEAIQBQ", "CgkIot63j_wSEAIQBg", "CgkIot63j_wSEAIQBw", "CgkIot63j_wSEAIQEw" },
        starsLeaderboardIds = { "CgkIot63j_wSEAIQCA", "CgkIot63j_wSEAIQCQ", "CgkIot63j_wSEAIQCg", "CgkIot63j_wSEAIQCw", "CgkIot63j_wSEAIQDA", "CgkIot63j_wSEAIQFA" };

    void Start()
    {
        //File.Delete(Application.persistentDataPath + "/playerstats.dat");
        difficulty = Settings.difficulty;
        seed = LevelGenerator.seed;
        Refresh();
        if (difficultyDisplay != null)
            difficultyDisplay.text = Settings.GetDifficultyName(difficulty);
    }

    public void NextDifficulty()
    {
        difficulty++;
        difficulty %= Settings.nDifficulties;
        difficultyDisplay.text = Settings.GetDifficultyName(difficulty);
        Refresh();
    }

    public void PrevDifficulty()
    {
        difficulty += Settings.nDifficulties - 1;
        difficulty %= Settings.nDifficulties;
        difficultyDisplay.text = Settings.GetDifficultyName(difficulty);
        Refresh();
    }

    public void NextLevel()
    {
        if (bestScores == null || bestStars == null)
            return;
        int current = seed, index = 0;
        for (int i = 0; i < bestScores.GetLength(0); i++)
            if (bestScores[i, 0] > seed)
            {
                current = (int)bestScores[i, 0];
                index = i;
                break;
            }
        for (int i = index; i < bestScores.GetLength(0); i++)
        {
            if (bestScores[i, 0] > seed && bestScores[i, 0] < current)
                current = (int)bestScores[i, 0];
        }
        seed = current;
        Refresh();
    }

    public void PrevLevel()
    {
        if (seed == 0 || bestScores == null || bestStars == null)
            return;
        int current = seed, index = 0;
        for (int i = 0; i < bestScores.GetLength(0); i++)
            if (bestScores[i, 0] < seed)
            {
                current = (int)bestScores[i, 0];
                index = i;
                break;
            }
        for (int i = index; i < bestScores.GetLength(0); i++)
        {
            if (bestScores[i, 0] < seed && bestScores[i, 0] > current)
                current = (int)bestScores[i, 0];
        }
        seed = current;
        Refresh();
    }

    public void SetLevel()
    {
        seed = System.Convert.ToInt32(seedInputField.text);
        Refresh();
    }

    public void SetLevelOverall()
    {
        seed = -1;
        Refresh();
    }

    public void Refresh()
    {
        if (seedDisplay != null)
            seedDisplay.text = seed != -1 ? seed.ToString() : "";
        if (seedInputField != null)
            seedInputField.text = seed != -1 ? seed.ToString() : "";
        if (_timePlayed != null)
        {
            if (timePlayed < 60)
                _timePlayed.text = "<size=50>Time Played</size>\n" + (int)timePlayed + " second" + ((int)timePlayed == 1 ? "" : "s");
            else if (timePlayed < 36000)
                _timePlayed.text = "<size=50>Time Played</size>\n" + (int)(timePlayed / 60) + " minute" + ((int)(timePlayed / 60) == 1 ? "" : "s");
            else
                _timePlayed.text = "<size=50>Time Played</size>\n" + (int)(timePlayed / 3600) + " hour" + ((int)(timePlayed / 3600) == 1 ? "" : "s");
        }
        int score = seed == -1 ? Mathf.FloorToInt(recordScore[difficulty]) : Mathf.FloorToInt(GetScore(seed, difficulty)),
            stars = seed == -1 ? (int)recordStars[difficulty] : (int)GetStars(seed, difficulty);
        if (_bestScore != null)
            _bestScore.text = "<size=50> Best Distance </size>\n" + score + " meters";
        if (_bestStars != null)
            _bestStars.text = "<size=50> Most Stars </size>\n" + stars + " stars";
    }

    public static void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerstats.dat");
        Stats s = new Stats();
        s.recordScore = recordScore;
        s.recordStars = recordStars;
        s.timePlayed = timePlayed;
        s.bestScores = bestScores;
        s.bestStars = bestStars;
        bf.Serialize(file, s);
        file.Close();
        /*if (Init.loggedInGooglePlay)
        {
            CloudSave.ReadSavedGame("playerstats", (SavedGameRequestStatus status, ISavedGameMetadata game) =>
            {
                if (status == SavedGameRequestStatus.Success)
                {
                    SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder().WithUpdatedPlayedTime(game.TotalTimePlayed.Add(new TimeSpan(1000)));
                    SavedGameMetadataUpdate updatedMetadata = builder.Build();
                    MemoryStream ms = new MemoryStream();
                    bf.Serialize(ms, s);
                    byte[] data = ms.ToArray();
                    ms.Close();
                    PlayGamesPlatform.Instance.SavedGame.CommitUpdate(game, updatedMetadata, data, (SavedGameRequestStatus status1, ISavedGameMetadata game1) => { });
                }
            });
        }*/
    }

    public static void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/playerstats.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerstats.dat", FileMode.Open);
            Stats s = (Stats)bf.Deserialize(file);
            file.Close();
            recordScore = s.recordScore;
            recordStars = s.recordStars;
            timePlayed = s.timePlayed;
            bestScores = s.bestScores;
            bestStars = s.bestStars;
        }
    }

    /*public static void GPLoad()
    {
        if (Init.loggedInGooglePlay)
            CloudSave.ReadSavedGame("playerstats", (SavedGameRequestStatus status, ISavedGameMetadata game) =>
            {
                if (status == SavedGameRequestStatus.Success)
                {
                    PlayGamesPlatform.Instance.SavedGame.ReadBinaryData(game, (SavedGameRequestStatus status1, byte[] data) =>
                    {
                        if (status1 != SavedGameRequestStatus.Success)
                            return;
                        BinaryFormatter bf = new BinaryFormatter();
                        MemoryStream ms = new MemoryStream(data);
                        try
                        {
                            Stats s = (Stats)bf.Deserialize(ms);
                            ms.Close();
                            float[] temp = new float[Mathf.Max(recordScore.Length, s.recordScore.Length)];
                            for (int i = 0; i < temp.Length; i++)
                                temp[i] = Mathf.Max(i < recordScore.Length ? recordScore[i] : 0, i < s.recordScore.Length ? s.recordScore[i] : 0);
                            recordScore = temp;
                            temp = new float[Mathf.Max(recordStars.Length, s.recordStars.Length)];
                            for (int i = 0; i < temp.Length; i++)
                                temp[i] = Mathf.Max(i < recordStars.Length ? recordStars[i] : 0, i < s.recordStars.Length ? s.recordStars[i] : 0);
                            recordStars = temp;
                            timePlayed = Mathf.Max(timePlayed, s.timePlayed);
                            float[,] _temp;
                            _temp = s.bestScores == null ? bestScores : (bestScores == null ? s.bestScores : new float[Mathf.Max(bestScores.GetLength(0), s.bestScores.GetLength(0)), Mathf.Max(bestScores.GetLength(1), s.bestScores.GetLength(1))]);
                            if (bestScores != null && s.bestScores != null)
                                for (int i = 0; i < _temp.GetLength(0); i++)
                                    for (int j = 0; j < _temp.GetLength(1); j++)
                                        _temp[i, j] = Mathf.Max(i < bestScores.GetLength(0) && j < bestScores.GetLength(1) ? bestScores[i, j] : 0, i < s.bestScores.GetLength(0) && j < s.bestScores.GetLength(1) ? s.bestScores[i, j] : 0);
                            bestScores = _temp;
                            _temp = s.bestStars == null ? bestStars : (bestStars == null ? s.bestStars : new float[Mathf.Max(bestStars.GetLength(0), s.bestStars.GetLength(0)), Mathf.Max(bestStars.GetLength(1), s.bestStars.GetLength(1))]);
                            if (bestStars != null && s.bestStars != null)
                                for (int i = 0; i < _temp.GetLength(0); i++)
                                    for (int j = 0; j < _temp.GetLength(1); j++)
                                        _temp[i, j] = Mathf.Max(i < bestStars.GetLength(0) && j < bestStars.GetLength(1) ? bestStars[i, j] : 0, i < s.bestStars.GetLength(0) && j < s.bestStars.GetLength(1) ? s.bestStars[i, j] : 0);
                            bestStars = _temp;
                        }
                        catch (Exception e) { }
                    });
                }
            });
    }*/

    public static float GetScore(int seed, int difficulty)
    {
        if (bestScores != null)
            for (int i = 0; i < bestScores.GetLength(0); i++)
                if ((int)bestScores[i, 0] == seed)
                    return bestScores.GetLength(1) > difficulty + 1 ? bestScores[i, difficulty + 1] : 0;
        return 0;
    }

    public static float GetStars(int seed, int difficulty)
    {
        if (bestStars != null)
            for (int i = 0; i < bestStars.GetLength(0); i++)
                if ((int)bestStars[i, 0] == seed)
                    return bestStars.GetLength(1) > difficulty + 1 ? bestStars[i, difficulty + 1] : 0;
        return 0;
    }

    public static void AddScore(int seed, int difficulty, float score)
    {

        if (recordScore.Length < Settings.nDifficulties)
        {
            float[] _temp = new float[Settings.nDifficulties];
            for (int i = 0; i < _temp.Length; i++)
                if (i < recordScore.Length)
                    _temp[i] = recordScore[i];
                else
                    _temp[i] = 0;
            recordScore = _temp;
        }
        if (score > recordScore[difficulty])
        {
            recordScore[difficulty] = score;
        }
#if UNITY_EDITOR
#else
        /*if (Init.loggedInGooglePlay)
        {
            PlayGamesPlatform.Instance.ReportScore((long)(score * 10), distanceLeaderboardIds[5], (bool success) => { });
            PlayGamesPlatform.Instance.ReportScore((long)(score * 10), distanceLeaderboardIds[difficulty], (bool success) => { });
        }*/
#endif

        if (bestScores != null)
            for (int i = 0; i < bestScores.GetLength(0); i++)
                if ((int)bestScores[i, 0] == seed)
                {
                    if (bestScores.GetLength(1) > Settings.nDifficulties)
                    {
                        if (score > bestScores[i, difficulty + 1])
                        {
                            bestScores[i, difficulty + 1] = score;
                        }
                    }
                    else
                    {
                        float[,] _temp = new float[bestScores.GetLength(0), Settings.nDifficulties + 1];
                        for (int j = 0; j < _temp.GetLength(0); j++)
                            for (int k = 0; k < _temp.GetLength(1); k++)
                                if (k < bestScores.GetLength(1))
                                    _temp[j, k] = bestScores[j, k];
                                else
                                    _temp[j, k] = 0;
                        _temp[i, difficulty + 1] = score;
                        bestScores = _temp;
                    }
                    return;
                }

        float[,] temp = new float[bestScores != null ? bestScores.GetLength(0) + 1 : 1, Settings.nDifficulties + 1];
        for (int i = 0; i < temp.GetLength(0) - 1; i++)
            for (int j = 0; j < temp.GetLength(1); j++)
                if (j < bestScores.GetLength(1))
                    temp[i, j] = bestScores[i, j];
                else
                    temp[i, j] = 0;

        for (int i = 1; i <= Settings.nDifficulties; i++)
            temp[temp.GetLength(0) - 1, i] = 0;
        temp[temp.GetLength(0) - 1, 0] = seed;
        temp[temp.GetLength(0) - 1, difficulty + 1] = score;
        bestScores = temp;
    }

    public static void AddStars(int seed, int difficulty, float score)
    {
        if (recordStars.Length < Settings.nDifficulties)
        {
            float[] _temp = new float[Settings.nDifficulties];
            for (int i = 0; i < _temp.Length; i++)
                if (i < recordStars.Length)
                    _temp[i] = recordStars[i];
                else
                    _temp[i] = 0;
            recordStars = _temp;
        }

        if (score > recordStars[difficulty])
        {
            recordStars[difficulty] = score;
        }
#if UNITY_EDITOR
#else
        /*if (Init.loggedInGooglePlay)
        {
            PlayGamesPlatform.Instance.ReportScore((long)score, starsLeaderboardIds[5], (bool success) => { });
            PlayGamesPlatform.Instance.ReportScore((long)score, starsLeaderboardIds[difficulty], (bool success) => { });
        }*/
#endif

        if (bestStars != null)
            for (int i = 0; i < bestStars.GetLength(0); i++)
                if ((int)bestStars[i, 0] == seed)
                {
                    if (bestStars.GetLength(1) > Settings.nDifficulties)
                    {
                        if (score > bestStars[i, difficulty + 1])
                            bestStars[i, difficulty + 1] = score;
                    }
                    else
                    {
                        float[,] _temp = new float[bestStars.GetLength(0), Settings.nDifficulties + 1];
                        for (int j = 0; j < _temp.GetLength(0); j++)
                            for (int k = 0; k < _temp.GetLength(1); k++)
                                if (k < bestStars.GetLength(1))
                                    _temp[j, k] = bestStars[j, k];
                                else
                                    _temp[j, k] = 0;
                        _temp[i, difficulty + 1] = score;
                        bestStars = _temp;
                    }
                    return;
                }

        float[,] temp = new float[bestStars != null ? bestStars.GetLength(0) + 1 : 1, Settings.nDifficulties + 1];
        for (int i = 0; i < temp.GetLength(0) - 1; i++)
            for (int j = 0; j < temp.GetLength(1); j++)
                if (j < bestStars.GetLength(1))
                    temp[i, j] = bestStars[i, j];
                else
                    temp[i, j] = 0;

        for (int i = 1; i <= Settings.nDifficulties; i++)
            temp[temp.GetLength(0) - 1, i] = 0;
        temp[temp.GetLength(0) - 1, 0] = seed;
        temp[temp.GetLength(0) - 1, difficulty + 1] = score;
        bestStars = temp;
    }

    public static void PostTimePlayed(float time)
    {
        timePlayed += time;
#if UNITY_EDITOR
#else
        /*if (Init.loggedInGooglePlay)
            PlayGamesPlatform.Instance.ReportScore((long)(timePlayed * 1000), "CgkIot63j_wSEAIQDQ", (bool success) => { });*/
#endif
    }
    
    public static void UpdateDistanceAchievements(int difficulty = -1, float distance = -1)
    {
        if (distance == -1 && difficulty != -1)
            distance = recordScore[difficulty];
        /*if (difficulty == -1)
        {
            if (recordScore[0] >= 40)
                PlayGamesPlatform.Instance.ReportProgress("CgkIot63j_wSEAIQAg", 100.0f, (bool success) => { });
            if (recordScore[0] >= 80)
                PlayGamesPlatform.Instance.ReportProgress("CgkIot63j_wSEAIQGQ", 100.0f, (bool success) => { });
            if (recordScore[0] >= 160)
                PlayGamesPlatform.Instance.ReportProgress("CgkIot63j_wSEAIQIw", 100.0f, (bool success) => { });
            if (recordScore[1] >= 35)
                PlayGamesPlatform.Instance.ReportProgress("CgkIot63j_wSEAIQDw", 100.0f, (bool success) => { });
            if (recordScore[1] >= 65)
                PlayGamesPlatform.Instance.ReportProgress("CgkIot63j_wSEAIQGw", 100.0f, (bool success) => { });
            if (recordScore[1] >= 125)
                PlayGamesPlatform.Instance.ReportProgress("CgkIot63j_wSEAIQJQ", 100.0f, (bool success) => { });
            if (recordScore[2] >= 30)
                PlayGamesPlatform.Instance.ReportProgress("CgkIot63j_wSEAIQEQ", 100.0f, (bool success) => { });
            if (recordScore[2] >= 50)
                PlayGamesPlatform.Instance.ReportProgress("CgkIot63j_wSEAIQHQ", 100.0f, (bool success) => { });
            if (recordScore[2] >= 90)
                PlayGamesPlatform.Instance.ReportProgress("CgkIot63j_wSEAIQJw", 100.0f, (bool success) => { });
            if (recordScore[3] >= 20)
                PlayGamesPlatform.Instance.ReportProgress("CgkIot63j_wSEAIQFQ", 100.0f, (bool success) => { });
            if (recordScore[3] >= 35)
                PlayGamesPlatform.Instance.ReportProgress("CgkIot63j_wSEAIQHw", 100.0f, (bool success) => { });
            if (recordScore[3] >= 55)
                PlayGamesPlatform.Instance.ReportProgress("CgkIot63j_wSEAIQKQ", 100.0f, (bool success) => { });
            if (recordScore[4] >= 10)
                PlayGamesPlatform.Instance.ReportProgress("CgkIot63j_wSEAIQFw", 100.0f, (bool success) => { });
            if (recordScore[4] >= 15)
                PlayGamesPlatform.Instance.ReportProgress("CgkIot63j_wSEAIQIQ", 100.0f, (bool success) => { });
            if (recordScore[4] >= 25)
                PlayGamesPlatform.Instance.ReportProgress("CgkIot63j_wSEAIQKw", 100.0f, (bool success) => { });
            return;
        }
        switch (Settings.difficulty)
        {
            case 0:
                if (distance >= 40)
                    PlayGamesPlatform.Instance.ReportProgress("CgkIot63j_wSEAIQAg", 100.0f, (bool success) => { });
                if (distance >= 80)
                    PlayGamesPlatform.Instance.ReportProgress("CgkIot63j_wSEAIQGQ", 100.0f, (bool success) => { });
                if (distance >= 160)
                    PlayGamesPlatform.Instance.ReportProgress("CgkIot63j_wSEAIQIw", 100.0f, (bool success) => { });
                break;
            case 1:
                if (distance >= 35)
                    PlayGamesPlatform.Instance.ReportProgress("CgkIot63j_wSEAIQDw", 100.0f, (bool success) => { });
                if (distance >= 65)
                    PlayGamesPlatform.Instance.ReportProgress("CgkIot63j_wSEAIQGw", 100.0f, (bool success) => { });
                if (distance >= 125)
                    PlayGamesPlatform.Instance.ReportProgress("CgkIot63j_wSEAIQJQ", 100.0f, (bool success) => { });
                break;
            case 2:
                if (distance >= 30)
                    PlayGamesPlatform.Instance.ReportProgress("CgkIot63j_wSEAIQEQ", 100.0f, (bool success) => { });
                if (distance >= 50)
                    PlayGamesPlatform.Instance.ReportProgress("CgkIot63j_wSEAIQHQ", 100.0f, (bool success) => { });
                if (distance >= 90)
                    PlayGamesPlatform.Instance.ReportProgress("CgkIot63j_wSEAIQJw", 100.0f, (bool success) => { });
                break;
            case 3:
                if (distance >= 20)
                    PlayGamesPlatform.Instance.ReportProgress("CgkIot63j_wSEAIQFQ", 100.0f, (bool success) => { });
                if (distance >= 35)
                    PlayGamesPlatform.Instance.ReportProgress("CgkIot63j_wSEAIQHw", 100.0f, (bool success) => { });
                if (distance >= 55)
                    PlayGamesPlatform.Instance.ReportProgress("CgkIot63j_wSEAIQKQ", 100.0f, (bool success) => { });
                break;
            case 4:
                if (distance >= 10)
                    PlayGamesPlatform.Instance.ReportProgress("CgkIot63j_wSEAIQFw", 100.0f, (bool success) => { });
                if (distance >= 15)
                    PlayGamesPlatform.Instance.ReportProgress("CgkIot63j_wSEAIQIQ", 100.0f, (bool success) => { });
                if (distance >= 25)
                    PlayGamesPlatform.Instance.ReportProgress("CgkIot63j_wSEAIQKw", 100.0f, (bool success) => { });
                break;
        }*/
    }
    public static void UpdateStarAchievements(int difficulty = -1, float stars = -1)
    {
        if (stars == -1 && difficulty != -1)
            stars = recordStars[difficulty];
        /*if (difficulty == -1)
        {
            if (recordStars[0] >= 10)
                PlayGamesPlatform.Instance.ReportProgress("CgkIot63j_wSEAIQDg", 100.0f, (bool success) => { });
            if (recordStars[0] >= 20)
                PlayGamesPlatform.Instance.ReportProgress("CgkIot63j_wSEAIQGg", 100.0f, (bool success) => { });
            if (recordStars[0] >= 40)
                PlayGamesPlatform.Instance.ReportProgress("CgkIot63j_wSEAIQJA", 100.0f, (bool success) => { });
            if (recordStars[1] >= 8)
                PlayGamesPlatform.Instance.ReportProgress("CgkIot63j_wSEAIQEA", 100.0f, (bool success) => { });
            if (recordStars[1] >= 16)
                PlayGamesPlatform.Instance.ReportProgress("CgkIot63j_wSEAIQHA", 100.0f, (bool success) => { });
            if (recordStars[1] >= 32)
                PlayGamesPlatform.Instance.ReportProgress("CgkIot63j_wSEAIQJg", 100.0f, (bool success) => { });
            if (recordStars[2] >= 6)
                PlayGamesPlatform.Instance.ReportProgress("CgkIot63j_wSEAIQEg", 100.0f, (bool success) => { });
            if (recordStars[2] >= 12)
                PlayGamesPlatform.Instance.ReportProgress("CgkIot63j_wSEAIQHg", 100.0f, (bool success) => { });
            if (recordStars[2] >= 22)
                PlayGamesPlatform.Instance.ReportProgress("CgkIot63j_wSEAIQKA", 100.0f, (bool success) => { });
            if (recordStars[3] >= 4)
                PlayGamesPlatform.Instance.ReportProgress("CgkIot63j_wSEAIQFg", 100.0f, (bool success) => { });
            if (recordStars[3] >= 8)
                PlayGamesPlatform.Instance.ReportProgress("CgkIot63j_wSEAIQIA", 100.0f, (bool success) => { });
            if (recordStars[3] >= 14)
                PlayGamesPlatform.Instance.ReportProgress("CgkIot63j_wSEAIQKg", 100.0f, (bool success) => { });
            if (recordStars[4] >= 2)
                PlayGamesPlatform.Instance.ReportProgress("CgkIot63j_wSEAIQGA", 100.0f, (bool success) => { });
            if (recordStars[4] >= 4)
                PlayGamesPlatform.Instance.ReportProgress("CgkIot63j_wSEAIQIg", 100.0f, (bool success) => { });
            if (recordStars[4] >= 6)
                PlayGamesPlatform.Instance.ReportProgress("CgkIot63j_wSEAIQLA", 100.0f, (bool success) => { });
            return;
        }
        switch (Settings.difficulty)
        {
            case 0:
                if (stars >= 10)
                    PlayGamesPlatform.Instance.ReportProgress("CgkIot63j_wSEAIQDg", 100.0f, (bool success) => { });
                if (stars >= 20)
                    PlayGamesPlatform.Instance.ReportProgress("CgkIot63j_wSEAIQGg", 100.0f, (bool success) => { });
                if (stars >= 40)
                    PlayGamesPlatform.Instance.ReportProgress("CgkIot63j_wSEAIQJA", 100.0f, (bool success) => { });
                break;
            case 1:
                if (stars >= 8)
                    PlayGamesPlatform.Instance.ReportProgress("CgkIot63j_wSEAIQEA", 100.0f, (bool success) => { });
                if (stars >= 16)
                    PlayGamesPlatform.Instance.ReportProgress("CgkIot63j_wSEAIQHA", 100.0f, (bool success) => { });
                if (stars >= 32)
                    PlayGamesPlatform.Instance.ReportProgress("CgkIot63j_wSEAIQJg", 100.0f, (bool success) => { });
                break;
            case 2:
                if (stars >= 6)
                    PlayGamesPlatform.Instance.ReportProgress("CgkIot63j_wSEAIQEg", 100.0f, (bool success) => { });
                if (stars >= 12)
                    PlayGamesPlatform.Instance.ReportProgress("CgkIot63j_wSEAIQHg", 100.0f, (bool success) => { });
                if (stars >= 22)
                    PlayGamesPlatform.Instance.ReportProgress("CgkIot63j_wSEAIQKA", 100.0f, (bool success) => { });
                break;
            case 3:
                if (stars >= 4)
                    PlayGamesPlatform.Instance.ReportProgress("CgkIot63j_wSEAIQFg", 100.0f, (bool success) => { });
                if (stars >= 8)
                    PlayGamesPlatform.Instance.ReportProgress("CgkIot63j_wSEAIQIA", 100.0f, (bool success) => { });
                if (stars >= 14)
                    PlayGamesPlatform.Instance.ReportProgress("CgkIot63j_wSEAIQKg", 100.0f, (bool success) => { });
                break;
            case 4:
                if (stars >= 2)
                    PlayGamesPlatform.Instance.ReportProgress("CgkIot63j_wSEAIQGA", 100.0f, (bool success) => { });
                if (stars >= 4)
                    PlayGamesPlatform.Instance.ReportProgress("CgkIot63j_wSEAIQIg", 100.0f, (bool success) => { });
                if (stars >= 6)
                    PlayGamesPlatform.Instance.ReportProgress("CgkIot63j_wSEAIQLA", 100.0f, (bool success) => { });
                break;
        }
        if (!PlayGamesPlatform.Instance.IsAuthenticated())
        {
            Init.loggedInGooglePlay = Settings.googlePlayLogin = false;
            PlayerPrefs.SetInt("play", 0);
        }*/
    }

    /*public void ShowLeaderboards()
    {
        PlayGamesPlatform.Instance.Authenticate(Init.GPLogin, false);
        Settings.googlePlayLogin = true;
        PlayerPrefs.SetInt("play", 1);
        if (Init.loggedInGooglePlay)
            PlayGamesPlatform.Instance.ReportScore((long)(timePlayed * 1000), "CgkIot63j_wSEAIQDQ", (bool success) => { });
        if (Init.loggedInGooglePlay)
        {
            foreach (float f in recordScore)
                PlayGamesPlatform.Instance.ReportScore((long)(f * 10), distanceLeaderboardIds[5], (bool _success) => { });
            foreach (float f in recordStars)
                PlayGamesPlatform.Instance.ReportScore((long)f, starsLeaderboardIds[5], (bool _success) => { });
            PlayGamesPlatform.Instance.ReportScore((long)(recordScore[difficulty] * 10), distanceLeaderboardIds[difficulty], (bool _success) => { });
            PlayGamesPlatform.Instance.ReportScore((long)recordStars[difficulty], starsLeaderboardIds[difficulty], (bool _success) => { });
        }
        PlayGamesPlatform.Instance.ShowLeaderboardUI();
    }

    public void ShowAchievements()
    {
        PlayGamesPlatform.Instance.Authenticate(Init.GPLogin, false);
        Settings.googlePlayLogin = true;
        PlayerPrefs.SetInt("play", 1);
        PlayGamesPlatform.Instance.ShowAchievementsUI();
    }*/
}

[System.Serializable]
public class Stats
{
    public float timePlayed;
    public float[] recordScore, recordStars;
    public float[,] bestScores, bestStars;
}
