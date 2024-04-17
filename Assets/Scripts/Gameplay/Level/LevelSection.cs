using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelSection : MonoBehaviour
{
    #region SerializedFields
    [SerializeField] float size = 1.0f;

    [SerializeField] Rigidbody2D[] rigidbody2Ds;
    #endregion

    #region API
    public float speed = 1.0f;

    public float Size => size;
    #endregion

    #region UnityMethods
    private void FixedUpdate()
    {
        foreach (var r in rigidbody2Ds)
        {
            r.MovePosition(r.position + new Vector2(-speed * Time.deltaTime, 0));
        }

        /*transform.position = new Vector3 (transform.position.x - (speed * Time.deltaTime), 
                                          transform.position.y, 
                                          transform.position.z);*/
    }
    #endregion
}
