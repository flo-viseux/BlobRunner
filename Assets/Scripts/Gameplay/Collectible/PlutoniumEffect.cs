
using System;
using UnityEngine;


public class PlutoniumEffect : CollectibleEffect
{
    #region Serialized fields
    [SerializeField] private int score = 1000;
    [SerializeField] private VFX_EventSO VFX_Event;
    #endregion

    private AudioSource _source;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
    }

    #region API
    public override void Effect()
    {
        GameManager.Instance.playerDatas.IncreaseCollectiblesCount();
        VFX_Event.RaiseEvent(transform.position, VFX_Manager.EType.Collect);
        _source.PlayOneShot(_source.clip);
    }
    #endregion
}
