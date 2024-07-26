using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class SaveFileController : MonoBehaviour
{
    #region Attributes
    private static Dictionary<int, LevelData> levels = new Dictionary<int, LevelData>();
    private static Dictionary<int, int> scores = new Dictionary<int, int>();
    private static Dictionary<int, int> maxScores = new Dictionary<int, int>();
    #endregion

    #region API
    public static int LastScore = 0;
    public static int LastMaxScore = 0;

    public static void AddLevel(int level, int score, int maxScore)
    {
        if (!levels.TryGetValue(level, out LevelData levelData))
        {
            levels[level] = new LevelData(level, score, maxScore);
        }
            

        AddScore(level, score);
        AddMaxScore(level, maxScore);
    }
    public static LevelData GetLevel(int level)
    {
        if (levels.TryGetValue(level, out LevelData levelData))
            return levelData;

        return null;
    }

    public static void AddScore(int level, int score)
    {
        if (scores.TryGetValue(level, out int scoreRecord))
        {
            if (scoreRecord < score)
            {
                scores[level] = score;
                levels[level].score = score;
            }

            return;
        }
        
        scores[level] = score;
        levels[level].score = score;
    }
    public static int GetScore(int level)
    {
        if (scores.TryGetValue(level, out int scoreRecord))
            return scoreRecord;

        return 0;
    }

    public static void AddMaxScore(int level, int maxScore)
    {
        if (maxScores.TryGetValue(level, out int maxScoreRecord))
        {
            if (maxScoreRecord < maxScore)
            {
                maxScores[level] = maxScore;
                levels[level].maxScore = maxScore;
            }

            return;
        }

        maxScores[level] = maxScore;
        levels[level].maxScore = maxScore;
    }
    public static int GetMaxScore(int level)
    {
        if (maxScores.TryGetValue(level, out int maxScoreRecord))
            return maxScoreRecord;

        return 0;
    }

    public static void Save()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        FileStream file = File.Open(Application.persistentDataPath + "/save.data", FileMode.Create);

        LevelData[] levelsData = levels.Values.ToArray();

        binaryFormatter.Serialize(file, levelsData);

        file.Close();
    }
    #endregion

    #region Unity methods
    private void Awake()
    {
        Load();
        //Delete();
    }
    #endregion

    #region Private
    private static void Load()
    {
        LevelData[] levelsDatas = LoadLevelDatas();

        if (levelsDatas == null)
            return;

        for (int i = 0; i < levelsDatas.Length; ++i)
        {
            levels[i] = levelsDatas[i];
            scores[i] = levels[i].score;
            maxScores[i] = levels[i].maxScore;
        }

    }

    private static LevelData[] LoadLevelDatas()
    {
        LevelData[] levelDatas = null;

        try
        {
            FileStream file = File.Open(Application.persistentDataPath + "/save.data", FileMode.Open);
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            levelDatas = binaryFormatter.Deserialize(file) as LevelData[];

            file.Close();
        }
        catch (FileNotFoundException)
        {
            Debug.Log("Save file: No save file");
            AddScore(0, 0);
        }
        catch (InvalidCastException)
        {
            Debug.LogError("Save file: Invalid cast");
        }
        catch (SerializationException)
        {
            Debug.LogError("Save file: Impossible to deserialize");
        }
        catch (EndOfStreamException)
        {
            Debug.LogError("Save file: Empty file");
        }

        return levelDatas;
    }

    private static void Delete()
    {
        File.Delete(Application.persistentDataPath + "/save.data");
    }
    #endregion
}
