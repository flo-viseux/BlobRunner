using UnityEngine;

public class ParallaxObject : MonoBehaviour
{
    #region Serialized fields
    [SerializeField] private float width = 4f;
    #endregion

    #region API
    public float Width => width;

    public void Move(float speed)
    {
        transform.position = transform.position + Vector3.left * speed * Time.deltaTime;
    }
    #endregion

    #region Unity methods
    private void OnDrawGizmosSelected()
    {
        Vector3 start = transform.childCount > 0 && transform.GetChild(0).name == "Renderer" ? transform.GetChild(0).position : transform.position;
        Vector3 end = start + Vector3.right * width;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(start, end);
    }
    #endregion
}
