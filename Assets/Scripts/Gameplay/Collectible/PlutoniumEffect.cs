
using UnityEngine;


public class PlutoniumEffect : CollectibleEffect
{
    #region Serialized fields
    [SerializeField] private int score = 1000;
    #endregion

    #region API
    public override void Effect()
    {
        GameManager.Instance.playerDatas.IncreaseScore(score);
        GameManager.Instance.playerDatas.IncreaseHealth();
    }
    #endregion
}
