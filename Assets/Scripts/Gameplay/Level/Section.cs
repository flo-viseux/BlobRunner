using UnityEngine;

[CreateAssetMenu(fileName = "LevelSection", menuName = "BlobRunner/LevelSection", order = 0)]
public class Section : ScriptableObject
{
    #region Serialized fields
    [SerializeField] private SectionGeometry geometry;
    #endregion

    #region API
    public SectionGeometry Geometry => geometry;
    #endregion
}
