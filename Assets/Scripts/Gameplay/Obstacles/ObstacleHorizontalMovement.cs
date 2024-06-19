using UnityEngine;

public class ObstacleHorizontalMovement : MonoBehaviour
{
    #region Serialized fields
    [SerializeField] private float minX = -4.5f;

    [SerializeField] private float maxX = 4.5f;

    [SerializeField] private float speed = 4.5f;

    [SerializeField] private float offset = 0f;
    #endregion

    #region Unity methods
    private void Update()
    {
        if (!transform.parent && (CheckVisibility.IsVisible(this.gameObject) || CheckVisibility.IsVisible(this.transform.parent.gameObject)))
            return;

        float currentOffset = offset - transform.parent.position.x * speed * 0.02f;
        float iterations = currentOffset / (maxX - minX);

        float remaining = iterations - Mathf.FloorToInt(iterations);
        bool goingUp = Mathf.FloorToInt(iterations) % 2 == 0;

        float x = goingUp ? Mathf.Lerp(minX, maxX, remaining) : Mathf.Lerp(maxX, minX, remaining);

        Vector3 position = transform.localPosition;
        transform.localPosition = new Vector3(x, position.y, position.z);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Vector3 position = transform.position;
        Gizmos.DrawLine(new Vector3(minX, position.y, position.z), new Vector3(maxX, position.y, position.z));
    }
    #endregion
}
