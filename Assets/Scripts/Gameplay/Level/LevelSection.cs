using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelSection : MonoBehaviour
{
    #region Serialized fields
    [SerializeField] private ParallaxObject parallaxObject = null;

    [SerializeField] private SectionSlot[] startSlots = null;

    [SerializeField] private SectionSlot[] endSlots = null;
    #endregion

    #region API
    public ParallaxObject ParallaxObject => parallaxObject;
    #endregion

    #region Unity methods
    private void OnDrawGizmos()
    {
        Vector3 startLeft = transform.position;
        startLeft -= Vector3.up * 5.06f;
        Vector3 startRight = startLeft + Vector3.right * (parallaxObject.Width + 0.5f);

        startLeft += 0.5f * Vector3.left;

        Vector3 endLeft = startLeft + Vector3.up * 8.5f;
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
