using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private SimpleEventSO HitObstacle;

    #region UnityMethods

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Player"))
        {
            if (CompareTag("Obstacles"))
            {
                HitObstacle.RaiseEvent();
                return;
            }

            RaycastHit2D hit = Physics2D.CircleCast(collision.transform.position, 0.5f, Vector2.right, 0.1f);
            if (hit != null && hit.collider != null && hit.collider.CompareTag("walkableObstacle"))
            {
                Vector2 normal = hit.normal;
                // Debug.Log($"normal : {normal}");
                // if (Vector3.Dot(normal, Vector3.down) > 0.7f)
                // {
                //     Debug.Log("Touching down the ground");
                //     
                // }

                if (Vector3.Dot(normal, Vector3.up) > 0.7f)
                {
                    //Debug.Log("Touching up the ground");
                    return;
                }

                // else if (Vector3.Dot(normal, Vector3.left) > 0.7f)
                // {
                //     Debug.Log("Touching the left side");
                // }
                //
                // else if (Vector3.Dot(normal, Vector3.right) > 0.7f)
                // {
                //     Debug.Log("Touching the right side");
                // }

                HitObstacle.RaiseEvent();
            }
        }
    }
    
    #endregion
}