using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using UnityEngine;

public class SaveFileController : MonoBehaviour
{
    #region Attributes
    private static Dictionary<int, int> scores = new Dictionary<int, int>();
    private static Dictionary<int, int> maxScores = new Dictionary<int, int>();
    #endregion

    #region API

    public static void AddScore(int level, int score)
    {
        if (scores.TryGetValue(level, out int scoreRecord))
        {
            if (scoreRecord < score)
                scores[level] = score;
        }
        else
            scores[level] = score;
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
                maxScores[level] = maxScore;
        }
        else
            maxScores[level] = maxScore;
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

        int[] scoreDatas = scores.Values.ToArray();

        binaryFormatter.Serialize(file, scoreDatas);

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
        int[] scoreDatas = LoadScoreDatas();

        if (scoreDatas == null)
            return;

        for (int i = 0; i < scoreDatas.Length; ++i)
        {
            scores[i] = scoreDatas[i];
        }
    }

    private static int[] LoadScoreDatas()
    {
        int[] scoreDatas = null;

        try
        {
            FileStream file = File.Open(Application.persistentDataPath + "/save.data", FileMode.Open);
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            scoreDatas = binaryFormatter.Deserialize(file) as int[];

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

        return scoreDatas;
    }

    private static void Delete()
    {
        File.Delete(Application.persistentDataPath + "/save.data");
    }
    #endregion
}