using UnityEngine;

public class ObstacleVerticalMovement : MonoBehaviour
{
    #region Serialized fields
    [SerializeField] private float minY = -4.5f;

    [SerializeField] private float maxY = 4.5f;

    [SerializeField] private float speed = 4.5f;

    [SerializeField] private float offset = 0f;
    #endregion

    #region Unity methods
    private void Update()
    {
        float currentOffset = offset - transform.parent.position.x * speed * 0.02f;
        float iterations = currentOffset / (maxY - minY);

        float remaining = iterations - Mathf.FloorToInt(iterations);
        bool goingUp = Mathf.FloorToInt(iterations) % 2 == 0;

        float y = goingUp ? Mathf.Lerp(minY, maxY, remaining) : Mathf.Lerp(maxY, minY, remaining);

        Vector3 position = transform.position;
        transform.position = new Vector3(position.x, y, position.z);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Vector3 position = transform.position;
        Gizmos.DrawLine(new Vector3(position.x, minY, position.z), new Vector3(position.x, maxY, position.z));
    }
    #endregion
}
