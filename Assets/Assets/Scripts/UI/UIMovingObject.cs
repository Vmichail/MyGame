using UnityEngine;
using UnityEngine.UI;

public class MovingUIObject : MonoBehaviour
{
    [Header("Enable/Disable Tracks")]
    [SerializeField] private bool doScale = true;
    [SerializeField] private bool doBob = true;
    [SerializeField] private bool doColorPulse = true;

    [Header("Scale (breathing)")]
    [SerializeField] private float scaleAmount = 0.06f;   // +6%
    [SerializeField] private float scaleTime = 0.9f;

    [Header("Bob (vertical)")]
    [SerializeField] private float bobDistance = 4f;      // px
    [SerializeField] private float bobTime = 1.2f;

    [Header("Color Pulse")]
    [SerializeField] private Color pulseColor = Color.yellow;
    [SerializeField] private float colorTweenTime = 0.3f;

    [Header("Stagger / Desync")]
    [Tooltip("Random initial delay so groups don’t move in perfect sync")]
    [SerializeField] private float randomInitialDelayMax = 0.25f;

    private RectTransform rt;
    private Image img;
    private Vector2 baseAnchoredPos;
    private Vector3 baseScale;
    private Color originalColor;

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
        img = GetComponent<Image>();
        baseAnchoredPos = rt.anchoredPosition;
        baseScale = rt.localScale;
        if (img) originalColor = img.color;
    }

    private void OnEnable()
    {
        StartLoops();
    }

    private void OnDisable()
    {
        StopLoops(resetToBase: true);
    }

    private void StartLoops()
    {
        // safety: kill any previous tweens
        LeanTween.cancel(gameObject);

        float delay = (randomInitialDelayMax > 0f) ? Random.Range(0f, randomInitialDelayMax) : 0f;

        if (doScale)
        {
            LeanTween.scale(rt, baseScale * (1f + scaleAmount), scaleTime)
                     .setDelay(delay)
                     .setEaseInOutSine()
                     .setLoopPingPong()
                     .setIgnoreTimeScale(true);
        }

        if (doBob)
        {
            LeanTween.moveY(rt, baseAnchoredPos.y + bobDistance, bobTime)
                     .setDelay(delay * 0.5f) // tiny desync
                     .setEaseInOutSine()
                     .setLoopPingPong()
                     .setIgnoreTimeScale(true);
        }

        if (doColorPulse && img)
        {
            LeanTween.color(rt, pulseColor, colorTweenTime)
                     .setDelay(delay)
                     .setEaseInOutSine()
                     .setLoopPingPong()
                     .setIgnoreTimeScale(true);
        }
    }

    private void StopLoops(bool resetToBase)
    {
        LeanTween.cancel(gameObject);

        if (!resetToBase) return;

        // reset transform
        rt.localScale = baseScale;
        rt.anchoredPosition = baseAnchoredPos;

        // reset color (if any)
        if (img) img.color = originalColor;
    }
}