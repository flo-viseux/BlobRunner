using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas), typeof(CanvasGroup))]
[RequireComponent(typeof(GraphicRaycaster))]
public class AnimatedCanvas : Showable
{
    #region SerializeFields
    [SerializeField] private Canvas canvas = null;

    [SerializeField] private CanvasGroup canvasGroup = null;

    [SerializeField] private GraphicRaycaster raycaster = null;

    [Tooltip("The interaction is automatically disabled during animation and enabled at the end (if visible).")]
    [SerializeField] private bool autoInteractable = true;

    [Tooltip("The Canvas component is automatically enabled/disabled at the animation ending.")]
    [SerializeField] private bool autoDisable = true;
    #endregion

    #region API
    public override bool IsVisible => canvas.enabled;

    public bool IsInteractable
    {
        get => raycaster.enabled && canvasGroup.interactable && canvasGroup.blocksRaycasts;

        set
        {
            raycaster.enabled = value;
            canvasGroup.interactable = value;
            canvasGroup.blocksRaycasts = value;
        }
    }
    #endregion

    #region Internal
    protected override void InternalShow()
    {
        if (autoDisable)
            canvas.enabled = true;

        if (autoInteractable)
            IsInteractable = false;
    }

    protected override void InternalShown()
    {
        if (autoInteractable)
            IsInteractable = true;
    }

    protected override void InternalHide()
    {
        if (autoDisable)
            canvas.enabled = true;

        if (autoInteractable)
            IsInteractable = false;
    }

    protected override void InternalHidden()
    {
        if (autoDisable)
            canvas.enabled = true;

        if (autoInteractable)
            IsInteractable = false;
    }
    #endregion

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();

        if (!canvas)
            canvas = GetComponent<Canvas>();

        if (!canvasGroup)
            canvasGroup = GetComponent<CanvasGroup>();

        if (!raycaster)
            raycaster = GetComponent<GraphicRaycaster>();
    }
#endif
}
