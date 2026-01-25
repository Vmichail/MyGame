using TMPro;
using UnityEngine;

public class AchievementUnlockManager : MonoBehaviour
{
    public static AchievementUnlockManager Instance { get; private set; }
    [SerializeField] private TextMeshProUGUI achievementText;
    [SerializeField] private float displayDuration = 2f;

    [Header("Animation")]
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float moveDistance = 40f;
    [SerializeField] private float fadeInDuration = 0.2f;

    [Header("Unlock Timers")]
    [SerializeField] private float unlockDifficultyTimer = 10f;

    [SerializeField] private GameObject achievementGameObject;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 startPos;

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Multiple AchievementUnlockedSrcipt instances found! Destroying duplicate.");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        rectTransform = achievementGameObject.GetComponent<RectTransform>();

        canvasGroup = achievementGameObject.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = achievementGameObject.AddComponent<CanvasGroup>();

        startPos = rectTransform.anchoredPosition;
    }

    public void ShowAchievement(UnlockKey unlockKey)
    {
        SetText(unlockKey);
        // Reset state
        achievementGameObject.SetActive(true);
        LeanTween.cancel(canvasGroup.gameObject);
        rectTransform.anchoredPosition = startPos;
        canvasGroup.alpha = 0f;
        PlayAnimation();
    }

    private void SetText(UnlockKey unlockKey)
    {
        if (unlockKey == UnlockKey.DifficultyHard)
            achievementText.text = "Hard Difficulty Unlocked!";
        else if (unlockKey == UnlockKey.DifficultyNormal)
            achievementText.text = "Normal Difficulty Unlocked!";
        else if (unlockKey == UnlockKey.DifficultyInsane)
            achievementText.text = "Insane Difficulty Unlocked!";
        else if (unlockKey == UnlockKey.RegisterAccount)
            achievementText.text = "Account Registered Successfully!";
        else
            achievementText.text = "Achievement Unlocked!";
    }

    private void PlayAnimation()
    {
        // 🔊 Sound on show
        AudioManager.Instance.PlaySoundFX("UnlockSound", transform.position, 2f, 1f, 1f);

        // 👁️ Fade IN
        LeanTween.alphaCanvas(canvasGroup, 1f, fadeInDuration)
            .setEaseOutSine()
            .setIgnoreTimeScale(true);

        // Wait → fade out + move down
        LeanTween.delayedCall(displayDuration, () =>
        {
            LeanTween.alphaCanvas(canvasGroup, 0f, fadeDuration)
                .setIgnoreTimeScale(true);

            LeanTween.moveY(rectTransform, startPos.y - moveDistance, fadeDuration)
                .setEaseInSine()
                .setIgnoreTimeScale(true)
                .setOnComplete(() =>
                {
                    achievementGameObject.SetActive(false);
                });
        }).setIgnoreTimeScale(true);
    }


    private void Update()
    {
        // For testing purposes: press U to show achievement
        if (Input.GetKeyDown(KeyCode.O) && Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift))
        {
            SaveUnlockKey(UnlockKey.None);
        }
        if (GlobalVariables.Instance.gameTime > unlockDifficultyTimer && DifficultyManager.Instance.CurrentDifficulty.Equals(DifficultyLevel.Easy))
        {
            SaveUnlockKey(UnlockKey.DifficultyNormal);
        }
        if (GlobalVariables.Instance.gameTime > unlockDifficultyTimer && DifficultyManager.Instance.CurrentDifficulty.Equals(DifficultyLevel.Normal))
        {
            SaveUnlockKey(UnlockKey.DifficultyHard);
        }
        if (GlobalVariables.Instance.gameTime > unlockDifficultyTimer && DifficultyManager.Instance.CurrentDifficulty.Equals(DifficultyLevel.Hard))
        {
            SaveUnlockKey(UnlockKey.DifficultyInsane);
        }
        if (Input.GetKeyDown(KeyCode.D) && Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift))
        {
            ResetAllUnlockKeys();
        }
    }

    public void SaveUnlockKey(UnlockKey unlockKey)
    {
        string key = unlockKey.ToString();

        if (PlayerPrefs.GetInt(key, 0) == 1)
        {
            return;
        }
        ShowAchievement(unlockKey);
        if (unlockKey == UnlockKey.None)
        {
            return;
        }
        PlayerPrefs.SetInt(key, 1);
        PlayerPrefs.Save();
    }

    public bool IsLocked(UnlockKey unlockKey)
    {
        Debug.Log($"Checking unlock status for {unlockKey}: " +
            $"{(PlayerPrefs.GetInt(unlockKey.ToString(), 0) != 1 ? "Locked" : "Unlocked")}");
        return PlayerPrefs.GetInt(unlockKey.ToString(), 0) != 1;
    }

    private void ResetAllUnlockKeys()
    {
        foreach (UnlockKey key in System.Enum.GetValues(typeof(UnlockKey)))
        {
            if (key == UnlockKey.None)
                continue;

            PlayerPrefs.DeleteKey(key.ToString());
            Debug.Log($"🗑 Deleted PlayerPref: {key}");
        }

        PlayerPrefs.Save();
        Debug.Log("🔥 All achievement unlocks have been reset");
    }
}
