using DG.Tweening;
using UnityEngine;

public class SpawnIndicator : MonoBehaviour
{
    public Vector3 targetScale = new Vector3(0.3f, 0.3f, 1f);
    public float scaleTime = 0.5f;
    public int loopCount = 3;

    public bool IsReadyToSpawn { get; private set; }

    private Tween scaleTween;

    private void OnEnable()
    {
        IsReadyToSpawn = false;
        transform.localScale = Vector3.zero;
        scaleTween?.Kill();

        scaleTween = transform
            .DOScale(targetScale, scaleTime)
            .SetEase(Ease.InOutSine)
            .SetLoops(loopCount * 2, LoopType.Yoyo) // full pulses
            .OnComplete(() =>
            {
                IsReadyToSpawn = true;
            });
    }

    private void OnDisable()
    {
        scaleTween?.Kill();
    }
}
