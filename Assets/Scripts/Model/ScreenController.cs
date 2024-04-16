using UnityEngine;

public class ScreenController : MonoBehaviour
{
    public static Rect GetSafeRect()
    {
        float yScreenPercent = Screen.height;
        float yScreenOffset = Screen.safeArea.yMin / Screen.height;
        float xScreenPercent = (Screen.safeArea.width) / Screen.width;
        float xScreenOffset = Screen.safeArea.xMin / Screen.width;
        return new Rect(xScreenOffset, yScreenOffset, xScreenPercent, yScreenPercent);
    }
}
