using UnityEngine;

public class SaveLevelScore : MonoBehaviour
{
    #region API
    public void OnGameOver(int levelId, int maxScore)
    {
        SaveFileController.AddLevel(levelId, 0, 0);

        SaveFileController.Save();
    }

    public void OnVictory(int levelId, int score, int maxScore)
    {
        SaveFileController.AddLevel(levelId, score, maxScore);

        SaveFileController.Save();
    }
    #endregion
}
