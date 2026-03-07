using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class BaseButtonScript : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler,
    ISelectHandler, IDeselectHandler
{
    public bool IsClicked { get; set; }

    [Header("FX")]
    [SerializeField] private bool destroyOnClick;
    [SerializeField] private bool enableSelectedEffect = true;
    [SerializeField] private bool setOriginalScaleToOne = false;
    [SerializeField] private bool isSlider = false;

    [Header("Tween Settings")]
    [SerializeField, Range(1f, 1.5f)] private float highlightScaleMultiplier = 1.1f;
    [SerializeField, Range(0f, 1f)] private float highlightTweenDuration = 0.1f;
    [SerializeField, Range(0f, 1f)] private float clickTweenDuration = 0.3f;

    [Header("Animated Idle Movement")]
    [SerializeField] private bool animateButton = false;
    [SerializeField] private float moveAmount = 5f;
    [SerializeField] private float moveDuration = 1.5f;

    private Vector3 originalScale;
    private Transform highlightTarget;

    private void Awake()
    {
        highlightTarget = isSlider ? transform.parent : transform;

        if (setOriginalScaleToOne)
            originalScale = Vector3.one;
        else
            originalScale = highlightTarget.localScale;
    }

    private void OnEnable()
    {
        if (highlightTarget == null)
            highlightTarget = isSlider ? transform.parent : transform;

        if (highlightTarget.localScale.sqrMagnitude < 0.1f * 0.1f)
            highlightTarget.localScale = Vector3.one;

        if (highlightTarget.localScale.x > 1)
            ResetScale();

        if (animateButton)
            StartIdleAnimation();
    }

    // ===== MOUSE HOVER =====
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!enableSelectedEffect) return;
        HighlightButton();
        CursorManagerScript.SetPointer();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!IsClicked) ResetScale();
        CursorManagerScript.SetDefault();
    }

    // ===== KEYBOARD / GAMEPAD SELECT =====
    public void OnSelect(BaseEventData eventData)
    {
        if (!enableSelectedEffect) return;
        HighlightButton();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (!IsClicked) ResetScale();
    }

    // ===== VISUAL EFFECTS =====
    public virtual void HighlightButton()
    {
        if (highlightTarget == null)
            highlightTarget = isSlider ? transform.parent : transform;

        highlightTarget.DOKill();

        AudioManager.Instance.PlaySoundFX("buttonHighlight1Sound",
            transform.position,
            0.3f,
            0.75f,
            1.25f);

        if (originalScale.x < 0.1f)
            originalScale = Vector3.one;

        highlightTarget
            .DOScale(originalScale * highlightScaleMultiplier, highlightTweenDuration)
            .SetEase(Ease.InOutSine)
            .SetUpdate(true);
    }

    public virtual void ResetScale()
    {
        highlightTarget.DOKill();
        highlightTarget.localScale = originalScale;
    }

    // ===== CLICK =====
    public void OnPointerClick(PointerEventData eventData)
    {
        CursorManagerScript.SetDefault();
        if (!destroyOnClick) return;
        if (TryGetComponent(out Button button))
        {
            button.interactable = false;
        }
        IsClicked = true;
        highlightTarget.DOKill();
        highlightTarget
            .DOScale(Vector3.zero, clickTweenDuration)
            .SetEase(Ease.InBack)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
                if (TryGetComponent(out Button b))
                    b.interactable = true;
                IsClicked = false;
                ResetScale();
            });
    }

    private void StartIdleAnimation()
    {
        Vector3 startPos = highlightTarget.localPosition;

        highlightTarget.DOKill();

        highlightTarget
            .DOLocalMoveY(startPos.y + moveAmount, moveDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo)
            .SetUpdate(true);
    }

    private void OnDisable()
    {
        if (highlightTarget != null)
            highlightTarget.DOKill();
    }
}