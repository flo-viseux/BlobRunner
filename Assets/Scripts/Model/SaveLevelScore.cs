using UnityEngine;

public class SaveLevelScore : MonoBehaviour
{
    #region API
    public void OnGameOver(int levelId, int maxScore)
    {
        SaveFileController.AddLevel(levelId, 0, 0);
        SaveFileController.LastScore = 0;
        SaveFileController.LastMaxScore = 0;

        SaveFileController.Save();
    }

    public void OnVictory(int levelId, int score, int maxScore)
    {
        SaveFileController.AddLevel(levelId, score, maxScore);
        SaveFileController.LastScore = score;
        SaveFileController.LastMaxScore = maxScore;


        SaveFileController.Save();
    }
    #endregion
}
