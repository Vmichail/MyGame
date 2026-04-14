using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HurtEffect : MonoBehaviour
{
    public static HurtEffect Instance { get; private set; }

    private Image hurtImage;

    [Header("Settings")]
    [SerializeField] private float flashInDuration = 0.1f;
    [SerializeField] private float flashOutDuration = 0.5f;
    [SerializeField] private Color flashColor = new Color(1f, 0f, 0f, 0.5f);
    private bool isLowHealthActive = false;
    private AudioSource lowHealthLoopSoundSource;

    private void Awake()
    {
        Instance = this;
        hurtImage = GetComponent<Image>();
        hurtImage.color = new Color(1, 0, 0, 0);
    }

    private Tween lowHealthTween;

    public void StartLowHealthEffect(float intensity = 1f)
    {
        if (isLowHealthActive)
            return;

        isLowHealthActive = true;

        float targetAlpha = flashColor.a * intensity;

        // Prevent stacking
        if (lowHealthTween != null && lowHealthTween.IsActive())
            return;

        lowHealthLoopSoundSource = AudioManager.Instance.PlaySoundFX("lowHealthHeartbeat", transform.position, 1f, 1f, 1f, loop: true);
        lowHealthTween = DOVirtual.Float(0f, targetAlpha, 0.8f, val =>
        {
            hurtImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, val);
        })
        .SetLoops(-1, LoopType.Yoyo)
        .SetUpdate(true);
    }

    public void StopLowHealthEffect()
    {
        if (!isLowHealthActive)
            return;

        isLowHealthActive = false;
        if (lowHealthLoopSoundSource != null)
        {
            AudioManager.Instance.StopSound(lowHealthLoopSoundSource);
            lowHealthLoopSoundSource = null;
        }

        if (lowHealthTween != null)
        {
            lowHealthTween.Kill();
            lowHealthTween = null;
        }

        // Smooth fade out instead of snapping
        hurtImage.DOFade(0f, 0.3f).SetUpdate(true);
    }

    /// <summary>
    /// Triggers a red flash. Optionally scales intensity with damage.
    /// </summary>
    public void Flash(float damage = 1f)
    {
        float intensity = Mathf.Clamp01(damage / 6f);
        Color targetColor = new Color(flashColor.r, flashColor.g, flashColor.b, flashColor.a * intensity);

        // Cancel active tweens on this object
        transform.DOKill();

        // Fade in
        DOVirtual.Float(0f, targetColor.a, flashInDuration, val =>
        {
            hurtImage.color = new Color(targetColor.r, targetColor.g, targetColor.b, val);
        })
        .SetUpdate(true)
        .OnComplete(() =>
        {
            // Fade out
            DOVirtual.Float(targetColor.a, 0f, flashOutDuration, val =>
            {
                hurtImage.color = new Color(targetColor.r, targetColor.g, targetColor.b, val);
            })
            .SetUpdate(true);
        });
    }

    private void OnDisable()
    {
        StopLowHealthEffect();
    }

    private void OnDestroy()
    {
        StopLowHealthEffect();
    }
}