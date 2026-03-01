using UnityEngine;
using DG.Tweening;

public class IconSelectedEffect : MonoBehaviour
{
    [SerializeField] private float targetScale = 1.3f;
    [SerializeField] private float loopDuration = 0.4f;

    private Tween scaleTween;

    private void OnEnable()
    {
        transform.localScale = Vector3.one;

        scaleTween = transform
            .DOScale(targetScale, loopDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo)
            .SetUpdate(true);
    }

    private void OnDisable()
    {
        scaleTween?.Kill();
        transform.localScale = Vector3.one;
    }
}