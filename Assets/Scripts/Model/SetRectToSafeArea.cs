using UnityEngine;

public class AdaptToSafeArea : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Rect Saferect = ScreenController.GetSafeRect();
        RectTransform rect = GetComponent<RectTransform>();

        rect.offsetMin = new Vector2(Screen.safeArea.xMin, 0);
        rect.sizeDelta = new Vector2(Screen.safeArea.width, Screen.safeArea.height + Screen.safeArea.yMin);
    }
}
