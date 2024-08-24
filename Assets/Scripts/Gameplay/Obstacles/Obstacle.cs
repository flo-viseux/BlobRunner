using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private SimpleEventSO HitObstacle;
    
    #region UnityMethods
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision);

        if (collision != null && collision.CompareTag("Player"))
        {
            Debug.Log("Player !");
            HitObstacle.RaiseEvent();
        }
    }
    #endregion
}
