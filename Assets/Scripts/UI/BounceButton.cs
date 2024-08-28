using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;


public class BounceButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    
    [SerializeField] private float bounceScale = 1.2f;
    [SerializeField] private float bounceDuration = 0.3f;

    private Vector3 originalScale;
    private AudioSource _source;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
    }

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
        _source.Play();
        transform.DOScale(originalScale, bounceDuration).SetEase(Ease.OutBounce);
    }
}