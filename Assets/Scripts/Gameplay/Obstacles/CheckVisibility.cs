using System.Linq;
using UnityEngine;

public class CheckVisibility : MonoBehaviour
{
    #region API
    public static bool IsVisible(GameObject target)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        return planes.All(plane => plane.GetDistanceToPoint(target.transform.position) >= 0);
    }
    #endregion
}
