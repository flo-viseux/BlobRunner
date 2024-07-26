using TMPro;
using UnityEngine;

public class VictoryUI : MonoBehaviour
{
    #region SerialiedFields
    [SerializeField] private TMP_Text label;
    #endregion

    #region UnityMethods
    public void OnEnable()
    {
        label.text = SaveFileController.LastScore + " / " + SaveFileController.LastMaxScore;
    }
    #endregion
}
