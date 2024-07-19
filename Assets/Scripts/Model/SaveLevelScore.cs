using UnityEngine;

public class SaveLevelScore : MonoBehaviour
{
    #region API
    public void OnGameOver(int levelId, int maxScore)
    {
        SaveFileController.AddScore(levelId, 0);
        SaveFileController.AddMaxScore(levelId, maxScore);

        SaveFileController.Save();
    }

    public void OnVictory(int levelId, int score, int maxScore)
    {
        SaveFileController.AddScore(levelId, score);
        SaveFileController.AddMaxScore(levelId, maxScore);

        SaveFileController.Save();
    }
    #endregion
}
