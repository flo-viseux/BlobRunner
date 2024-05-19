using UnityEngine;

[CreateAssetMenu(fileName = "LevelSection", menuName = "BlobRunner/LevelSection", order = 0)]
public class Section : ScriptableObject
{
    #region Serialized fields
    [SerializeField] private GameObject geometry;
    #endregion

    #region API
    public GameObject Geometry => geometry;
    #endregion

}
