using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BaseButtonScript : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler,
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
    [SerializeField] private float moveAmount = 5f; // how far it moves (in pixels)
    [SerializeField] private float moveDuration = 1.5f; // time for each move cycle

    private Vector3 originalScale;
    private Transform highlightTarget; // either this or parent

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

        if (animateButton)
        {
            StartIdleAnimation();
        }
    }

    // ===== MOUSE HOVER =====
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!enableSelectedEffect) return;
        HighlightButton();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!IsClicked) ResetScale();
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

        LeanTween.cancel(highlightTarget.gameObject);
        AudioManager.Instance.PlaySoundFX("buttonHighlight1Sound", transform.position, 0.3f, 0.75f, 1.25f);
        if (originalScale.x < 0.1)
        {
            originalScale = Vector3.one;
        }
        LeanTween.scale(highlightTarget.gameObject, originalScale * highlightScaleMultiplier, highlightTweenDuration)
            .setEaseInOutSine()
            .setIgnoreTimeScale(true);
    }

    private void ResetScale()
    {
        LeanTween.cancel(highlightTarget.gameObject);
        highlightTarget.localScale = originalScale;
    }

    // ===== CLICK =====
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!destroyOnClick) return;

        if (TryGetComponent(out Button button))
            button.interactable = false;

        IsClicked = true;

        LeanTween.cancel(highlightTarget.gameObject);
        LeanTween.scale(highlightTarget.gameObject, Vector3.zero, clickTweenDuration)
            .setEaseInBack()
            .setIgnoreTimeScale(true)
            .setOnComplete(() =>
            {
                gameObject.SetActive(false);
                if (TryGetComponent(out Button b)) b.interactable = true;
                IsClicked = false;
                ResetScale();
            });
    }

    private void StartIdleAnimation()
    {
        Vector3 startPos = highlightTarget.localPosition;

        LeanTween.moveLocalY(highlightTarget.gameObject, startPos.y + moveAmount, moveDuration)
            .setEaseInOutSine()
            .setLoopPingPong()
            .setIgnoreTimeScale(true);
    }

    private void OnDisable()
    {
        LeanTween.cancel(highlightTarget.gameObject);
    }

}