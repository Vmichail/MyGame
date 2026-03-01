using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class UpgradeDescriptionScript : MonoBehaviour
{
    [Header("Intro")]
    [SerializeField] private float startScale = 0.85f;
    [SerializeField] private float duration = 0.18f;
    [SerializeField] private Ease ease = Ease.OutBack;

    [Header("Shake")]
    [SerializeField] private float shakeDuration = 0.15f;
    [SerializeField] private float shakeStrength = 8f;
    [SerializeField] private int shakeVibrato = 12;

    private CanvasGroup canvasGroup;
    private Sequence introSeq;
    private Tween shakeTween;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        // Reset state
        transform.localScale = Vector3.one * startScale;
        canvasGroup.alpha = 0f;

        introSeq?.Kill();

        introSeq = DOTween.Sequence()
            .Append(canvasGroup.DOFade(1f, duration))
            .Join(transform.DOScale(1f, duration).SetEase(ease))
            .SetUpdate(true); // ignore Time.timeScale
    }

    private void OnDisable()
    {
        introSeq?.Kill();
        shakeTween?.Kill();
    }

    public void Shake()
    {
        shakeTween?.Kill();

        shakeTween = transform.DOShakePosition(
                shakeDuration,
                shakeStrength,
                shakeVibrato,
                randomness: 90,
                snapping: false,
                fadeOut: true
            )
            .SetUpdate(true);
    }
}