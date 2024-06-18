using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryUI : MonoBehaviour
{
    #region SerialiedFields
    [SerializeField] private Showable showable;
    #endregion

    #region API
    public void Show()
    {
        showable.Show();
    }
    #endregion
}
