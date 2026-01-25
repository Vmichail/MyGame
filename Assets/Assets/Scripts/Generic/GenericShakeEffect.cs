using DG.Tweening;
using UnityEngine;

public class GenericShakeEffect : MonoBehaviour
{
    [Header("Shake Settings")]
    [SerializeField] private float duration = 0.3f;
    [SerializeField] private Vector3 strength = new Vector3(10f, 10f, 0f);
    [SerializeField] private int vibrato = 20;
    [SerializeField] private float randomness = 90f;
    [SerializeField] private bool snapping = false;
    [SerializeField] private bool fadeOut = true;

    private Vector3 originalPosition;

    void Awake()
    {
        originalPosition = transform.localPosition;
    }

    public void Shake()
    {
        transform.DOKill();
        transform.localPosition = originalPosition;

        transform.DOShakePosition(
            duration,
            strength,
            vibrato,
            randomness,
            snapping,
            fadeOut
        ).OnComplete(() =>
        {
            transform.localPosition = originalPosition;
        });
    }
}
