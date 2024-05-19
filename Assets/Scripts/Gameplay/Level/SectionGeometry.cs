using UnityEngine;

public class SectionGeometry : MonoBehaviour
{
    #region Serialized fields
    [SerializeField] private float width = 4f;

    [SerializeField] private SectionSlot[] startSlots = null;

    [SerializeField] private SectionSlot[] endSlots = null;

    [SerializeField] private float startLeftY = 5.06f;
    [SerializeField] private float endLeftY = 8.5f;
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
        Vector3 end = start + Vector3.right * Width;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(start, end);
    }

    private void OnDrawGizmos()
    {
        Vector3 startLeft = transform.position;
        startLeft -= Vector3.up * startLeftY;
        Vector3 startRight = startLeft + Vector3.right * (width + 0.5f);

        startLeft += 0.5f * Vector3.left;

        Vector3 endLeft = startLeft + Vector3.up * endLeftY;
        float height = (endLeft.y - startLeft.y) / 2f;

        foreach (SectionSlot slot in startSlots)
        {
            int slotIndex = GetSlotHeight(slot);

            Vector3 slotStart = startLeft + Vector3.up * (height * (slotIndex + 0.1f));
            Vector3 slotEnd = slotStart + Vector3.up * (height * 0.8f);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(slotStart, slotEnd);
        }

        foreach (SectionSlot slot in endSlots)
        {
            int slotIndex = GetSlotHeight(slot);

            Vector3 slotStart = startRight + Vector3.up * (height * (slotIndex + 0.1f));
            Vector3 slotEnd = slotStart + Vector3.up * (height * 0.8f);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(slotStart, slotEnd);
        }
    }
    #endregion

    #region Private
    private int GetSlotHeight(SectionSlot slot)
    {
        switch (slot)
        {
            case SectionSlot.Bottom:
                return 0;

            /*case SectionSlot.MiddleBottom:
                return 1;

            case SectionSlot.MiddleTop:
                return 2;*/

            case SectionSlot.Top:
                return 1;
        }

        return 0;
    }
    #endregion
}
