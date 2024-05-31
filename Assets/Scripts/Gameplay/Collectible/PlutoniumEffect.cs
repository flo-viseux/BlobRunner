using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PlutoniumEffect : CollectibleEffect
{
    #region Serialized fields
    [SerializeField] private int score = 1000;
    #endregion

    #region API
    public override void Effect()
    {
        PlayerScore.Instance.IncreaseScore(score);
    }
    #endregion
}
