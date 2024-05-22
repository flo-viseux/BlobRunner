using UnityEngine;

[System.Serializable]
public class WeightedSection
{
    #region SerializedFields
    [SerializeField] private Section section = null;

    [SerializeField] private float weight = 1;

    [SerializeField] private int cdDuration = 1;
    #endregion

    #region Attributes
    private int currentCd = -1;
    #endregion

    #region API
    public Section Section => section;
    public float Weight => weight;

    public float GetWeightAt(Section lastSection = null)
    {
        float CdWeight = currentCd < 0 ? 1f : 0f;

        float sectionsValid = 1f;

        if (lastSection != null)
            sectionsValid = section.Geometry.SlotsValid(lastSection) ? 1f : 0f;

        return weight * CdWeight * sectionsValid;
    }

    public void SetCooldown()
    {
        currentCd = cdDuration;
    }

    public void ForceCooldown()
    {
        currentCd = 1;
    }

    public void DecreaseCooldown()
    {
        --currentCd;
    }
    #endregion
}
