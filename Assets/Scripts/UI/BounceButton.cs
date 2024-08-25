using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;


public class BounceButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public float bounceScale = 1.2f;
    public float bounceDuration = 0.3f;

    private Vector3 originalScale;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(bounceScale, bounceDuration).SetEase(Ease.OutBounce);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(originalScale, bounceDuration).SetEase(Ease.OutBounce);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        transform.DOScale(originalScale, bounceDuration).SetEase(Ease.OutBounce);
    }
}