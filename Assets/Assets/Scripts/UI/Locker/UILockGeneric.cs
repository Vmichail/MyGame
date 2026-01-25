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

        LeanTween.cancel(lockIcon.gameObject);

        if (isLocked)
        {
            lockGO.SetActive(true);
            lockIcon.localScale = originalScale;
            lockIcon.localRotation = Quaternion.identity;
        }
        else
        {
            lockGO.SetActive(true);

            LeanTween.scale(lockIcon, originalScale * scaleMultiplier, scaleDuration + scaleDuration)
                .setEaseOutBack()
                .setIgnoreTimeScale(true);

            LeanTween.rotateZ(lockIcon.gameObject, rotationAmount, rotationDuration)
                .setLoopPingPong(2)
                .setIgnoreTimeScale(true);

            LeanTween.scale(lockGO, Vector3.zero, 2.5f)
                .setDelay(0.3f)
                .setEaseInBack()
                .setIgnoreTimeScale(true)
                .setOnComplete(() =>
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
        LeanTween.cancel(lockIcon.gameObject);

        AudioManager.Instance.PlaySoundFX(denySound, transform.position, 0.8f, 0.9f, 1.1f);

        LeanTween.scale(lockIcon, originalScale * scaleMultiplier, scaleDuration)
            .setEaseOutBack()
            .setIgnoreTimeScale(true);

        LeanTween.rotateZ(lockIcon.gameObject, rotationAmount, rotationDuration)
            .setLoopPingPong(1)
            .setIgnoreTimeScale(true);
    }

    private void ResetAnimation()
    {
        LeanTween.cancel(lockIcon.gameObject);

        LeanTween.scale(lockIcon, originalScale, scaleDuration)
            .setEaseInOutSine()
            .setIgnoreTimeScale(true);

        LeanTween.rotateZ(lockIcon.gameObject, 0f, rotationDuration)
            .setIgnoreTimeScale(true);
    }
}