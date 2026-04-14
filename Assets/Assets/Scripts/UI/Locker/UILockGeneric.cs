using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UILockGeneric : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    ISelectHandler,
    IDeselectHandler
{
    [Header("Lock Visual")]
    [SerializeField] private RectTransform lockIcon;
    [SerializeField] private GameObject lockGO;



    [Header("Unlock Condition")]
    [SerializeField] private UnlockKey unlockKey;

    [Header("Animation")]
    [SerializeField] private float scaleMultiplier = 1.15f;
    [SerializeField] private float scaleDuration = 0.2f;
    [SerializeField] private float rotationAmount = 10f;
    [SerializeField] private float rotationDuration = 0.1f;

    [Header("Audio")]
    [SerializeField] private string denySound = "uiDeny";

    private Vector3 originalScale;
    private bool isLocked;

    private void Awake()
    {

        if (lockIcon == null)
            lockIcon = GetComponentInChildren<RectTransform>();

        originalScale = lockIcon.localScale;

        if (unlockKey.Equals(UnlockKey.None))
        {
            Debug.LogError($"{name}: Unlock condition does not implement IUnlockCondition");
        }
    }

    private void Start()
    {
        RefreshLockState();
    }

    public void RefreshLockState()
    {
        isLocked = AchievementUnlockManager.Instance.IsLocked(unlockKey);

        Selectable selectable = GetComponentInParent<Selectable>();
        if (selectable != null)
            selectable.interactable = !isLocked;

        lockIcon.DOKill();
        lockGO.transform.DOKill();

        if (isLocked)
        {
            lockGO.SetActive(true);
            lockIcon.localScale = originalScale;
            lockIcon.localRotation = Quaternion.identity;
        }
        else
        {
            lockGO.SetActive(true);

            // Scale pop animation
            lockIcon
                .DOScale(originalScale * scaleMultiplier, scaleDuration + scaleDuration)
                .SetEase(Ease.OutBack)
                .SetUpdate(true)
                .SetLink(lockIcon.gameObject, LinkBehaviour.KillOnDisable);

            // Rotation ping-pong
            lockIcon
                .DORotate(new Vector3(0, 0, rotationAmount), rotationDuration)
                .SetLoops(2, LoopType.Yoyo)
                .SetUpdate(true)
                .SetLink(lockIcon.gameObject, LinkBehaviour.KillOnDisable);

            // Lock disappearing animation
            lockGO.transform
                .DOScale(Vector3.zero, 2.5f)
                .SetDelay(0.3f)
                .SetEase(Ease.InBack)
                .SetUpdate(true)
                .SetLink(lockGO, LinkBehaviour.KillOnDisable)
                .OnComplete(() =>
                {
                    lockGO.SetActive(false);
                });
        }
    }

    /* =========================
     * POINTER / SELECTION
     * ========================= */
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isLocked)
            PlayHoverAnimation();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ResetAnimation();
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (isLocked)
            PlayHoverAnimation();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        ResetAnimation();
    }

    /* =========================
     * ANIMATION + AUDIO
     * ========================= */
    private void PlayHoverAnimation()
    {
        lockIcon.DOKill();

        AudioManager.Instance.PlaySoundFX(denySound, transform.position, 0.8f, 0.9f, 1.1f);

        lockIcon
            .DOScale(originalScale * scaleMultiplier, scaleDuration)
            .SetEase(Ease.OutBack)
            .SetUpdate(true);

        lockIcon
            .DORotate(new Vector3(0, 0, rotationAmount), rotationDuration)
            .SetLoops(1, LoopType.Yoyo)
            .SetUpdate(true);
    }

    private void ResetAnimation()
    {
        lockIcon.DOKill();

        lockIcon
            .DOScale(originalScale, scaleDuration)
            .SetEase(Ease.InOutSine)
            .SetUpdate(true);

        lockIcon
            .DORotate(Vector3.zero, rotationDuration)
            .SetUpdate(true);
    }
}