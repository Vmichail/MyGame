using UnityEngine;
using TMPro;
using DG.Tweening;

public class OverflowingScript : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI text;

    [Header("Float Movement")]
    [SerializeField] private float floatHeight = 10f;
    [SerializeField] private float floatDuration = 1.6f;

    [Header("Horizontal Drift")]
    [SerializeField] private float driftAmount = 6f;
    [SerializeField] private float driftDuration = 2.2f;

    [Header("Breathing Scale")]
    //[SerializeField] private float minScale = 0.95f;
    [SerializeField] private float maxScale = 1.05f;
    [SerializeField] private float scaleDuration = 1.8f;

    private RectTransform rect;
    private Sequence sequence;
    private Vector2 startPos;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        startPos = rect.anchoredPosition;

        sequence?.Kill();

        sequence = DOTween.Sequence()
            .SetUpdate(true)
            .SetLoops(-1, LoopType.Yoyo); // ✅ loop the sequence

        // Vertical float
        sequence.Join(
            rect.DOAnchorPosY(startPos.y + floatHeight, floatDuration)
                .SetEase(Ease.InOutSine)
        );

        // Horizontal drift
        sequence.Join(
            DOTween.To(
                () => rect.anchoredPosition.x,
                x => rect.anchoredPosition = new Vector2(x, rect.anchoredPosition.y),
                startPos.x + Random.Range(-driftAmount, driftAmount),
                driftDuration
            )
            .SetEase(Ease.InOutSine)
        );

        // Breathing scale
        sequence.Join(
            rect.DOScale(maxScale, scaleDuration)
                .SetEase(Ease.InOutSine)
        );
    }

    private void OnDisable()
    {
        sequence?.Kill();
        rect.anchoredPosition = startPos;
        rect.localScale = Vector3.one;
    }
}