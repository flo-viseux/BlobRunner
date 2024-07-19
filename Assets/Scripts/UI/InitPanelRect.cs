using UnityEngine;

[ExecuteAlways]
public class InitPanelRect : MonoBehaviour
{
    #region SerializedFields
    [SerializeField] private RectTransform rectTransform;
    #endregion

    #region UnityMethods
    private void Awake()
    {
        Rect safeRect = ScreenController.GetSafeRect();

        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.sizeDelta = Vector2.zero;

        rectTransform.offsetMin = new Vector2(safeRect.x * Screen.width, safeRect.y);
        Debug.Log(safeRect.x + ", "  + Screen.width);
        rectTransform.offsetMax = new Vector2(0, 0);
    }
    #endregion
}
