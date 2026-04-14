using DG.Tweening;
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

    [Header("Locker")]
    [SerializeField] private GameObject locker;

    private RectTransform rt;
    private Image img;
    private Vector2 baseAnchoredPos;
    private Vector3 baseScale;
    private Color originalColor;
    private bool loopsStarted = false;

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
        if (locker != null && locker.activeSelf)
            return;

        StartLoops();
    }

    private void Update()
    {
        if (locker != null && !locker.activeSelf && !loopsStarted)
        {
            StartLoops();
        }
        if (locker != null && locker.activeSelf && loopsStarted)
        {
            StopLoops(resetToBase: true);
        }
    }

    private void OnDisable()
    {
        StopLoops(resetToBase: true);
    }

    private void StartLoops()
    {
        loopsStarted = true;

        // safety: kill any previous tweens
        transform.DOKill();
        rt.DOKill();

        float delay = (randomInitialDelayMax > 0f)
            ? Random.Range(0f, randomInitialDelayMax)
            : 0f;

        if (doScale)
        {
            rt.DOScale(baseScale * (1f + scaleAmount), scaleTime)
              .SetDelay(delay)
              .SetEase(Ease.InOutSine)
              .SetLoops(-1, LoopType.Yoyo)
              .SetUpdate(true);
        }

        if (doBob)
        {
            rt.DOAnchorPosY(baseAnchoredPos.y + bobDistance, bobTime)
              .SetDelay(delay * 0.5f) // tiny desync
              .SetEase(Ease.InOutSine)
              .SetLoops(-1, LoopType.Yoyo)
              .SetUpdate(true);
        }

        if (doColorPulse && img)
        {
            img.DOKill();

            img.DOColor(pulseColor, colorTweenTime)
               .SetDelay(delay)
               .SetEase(Ease.InOutSine)
               .SetLoops(-1, LoopType.Yoyo)
               .SetUpdate(true)
               .SetLink(img.gameObject, LinkBehaviour.KillOnDisable);
        }
    }

    private void StopLoops(bool resetToBase)
    {
        transform.DOKill();
        rt.DOKill();

        if (!resetToBase) return;

        // reset transform
        rt.localScale = baseScale;
        rt.anchoredPosition = baseAnchoredPos;

        // reset color (if any)
        if (img) img.color = originalColor;
    }
}