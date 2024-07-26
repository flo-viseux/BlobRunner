using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    #region SerializedFields
    [SerializeField] private int levelIndex = 0;
    [SerializeField] private string levelScene = "";

    [SerializeField] private Button button = null;
    [SerializeField] private TMP_Text label = null;
    #endregion

    #region UnityMethods
    private void Awake()
    {
        button.onClick.AddListener(LoadScene);
    }

    private void OnEnable()
    {
        if (SaveFileController.GetLevel(levelIndex) != null)
            label.text = SaveFileController.GetLevel(levelIndex).score + " / " + SaveFileController.GetLevel(levelIndex).maxScore;
        else
            label.text = "0 / 0";
    }
    #endregion

    #region Private
    private void LoadScene()
    {
        GameManager.Instance.gameSceneName = levelScene;
        GameManager.Instance.levelIndex = levelIndex;
        GameManager.Instance.GoToGame();
    }
    #endregion
}
