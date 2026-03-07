using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class PauseGameOverSwitch : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject replayButton;
    [SerializeField] GameObject pauseOptions;
    [SerializeField] GameObject gameOverOptions;
    [SerializeField] GameObject colorWheelImage;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] private GameObject starPrefab;
    [SerializeField] private GameObject starEmptyPrefab;
    [SerializeField] private Transform starParent;
    [SerializeField] private int maxStars = 7;

    [Header("Score Counter")]
    [SerializeField] private float pauseScoreDuration = 1.5f;
    [SerializeField] private Ease pauseScoreEase = Ease.OutCubic;
    [SerializeField] private float gameOverScoreDuration = 3.5f;
    [SerializeField] private Ease gameOverScoreEase = Ease.OutCubic;

    [Header("Color Wheel")]
    [SerializeField] private float colorWheelRotationDuration = 10f;

    [Header("Stars")]
    [SerializeField] private float starScaleDuration = 0.5f;
    [SerializeField] private Ease starScaleEase = Ease.OutBack;
    [SerializeField] private float starDelayBetween = 0.3f;

    private int previousScore = 0;

    private void OnEnable()
    {
        if (GlobalVariables.Instance.playerIsAlive)
        {
            DOTween.To(() => previousScore, x => scoreText.text = x.ToString(),
                GlobalVariables.Instance.score, pauseScoreDuration)
                .SetEase(pauseScoreEase)
                .SetUpdate(true);

            previousScore = GlobalVariables.Instance.score;
            gameOverOptions.SetActive(false);
            pauseOptions.SetActive(true);
            StartCoroutine(SelectNextFrame(continueButton));
        }
        else
        {
            pauseOptions.SetActive(false);
            colorWheelImage.SetActive(true);

            colorWheelImage.transform
                .DORotate(new Vector3(0, 0, -360f), colorWheelRotationDuration, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Incremental)
                .SetUpdate(true);

            UpdateStars(GlobalVariables.Instance.score);
            gameOverOptions.SetActive(true);

            DOTween.To(() => 0f, x => scoreText.text = Mathf.RoundToInt(x).ToString(),
                GlobalVariables.Instance.score, gameOverScoreDuration)
                .SetEase(gameOverScoreEase)
                .SetUpdate(true);

            StartCoroutine(SelectNextFrame(replayButton));
        }
    }

    public void UpdateStars(int score)
    {
        int filledStars = Mathf.Clamp(score / 100, 0, maxStars);

        foreach (Transform child in starParent)
            Destroy(child.gameObject);

        for (int i = 0; i < maxStars; i++)
        {
            bool isFilled = i < filledStars;
            GameObject prefabToUse = isFilled ? starPrefab : starEmptyPrefab;
            GameObject star = Instantiate(prefabToUse, starParent);
            star.transform.localScale = Vector3.zero;

            float delay = i * starDelayBetween;

            star.transform
                .DOScale(Vector3.one, starScaleDuration)
                .SetEase(starScaleEase)
                .SetDelay(delay)
                .SetUpdate(true);

            string soundName = isFilled ? "levelUpSound" : "starFailSound";
            DOVirtual.DelayedCall(delay, () =>
            {
                AudioManager.Instance.PlaySoundFX(soundName, transform.position, 0.2f, 0.75f, 1.25f);
            }, ignoreTimeScale: true);
        }
    }

    private System.Collections.IEnumerator SelectNextFrame(GameObject go)
    {
        yield return null;
        EventSystem.current.SetSelectedGameObject(go);
    }
}