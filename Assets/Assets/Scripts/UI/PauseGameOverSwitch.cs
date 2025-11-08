using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    private int previousScore = 0;
    private void OnEnable()
    {
        if (GlobalVariables.Instance.playerIsAlive)
        {
            LeanTween.value(gameObject, previousScore, GlobalVariables.Instance.score, 1.5f)
                 .setOnUpdate((float val) =>
                 {
                     scoreText.text = Mathf.RoundToInt(val).ToString();
                 })
                 .setEase(LeanTweenType.easeOutCubic)
                 .setIgnoreTimeScale(true);
            previousScore = GlobalVariables.Instance.score;
            gameOverOptions.SetActive(false);
            pauseOptions.SetActive(true);
            StartCoroutine(SelectNextFrame(continueButton));
        }
        else
        {
            pauseOptions.SetActive(false);
            colorWheelImage.SetActive(true);
            LeanTween.rotateAround(colorWheelImage, Vector3.forward, -360f, 10f).setLoopClamp().setIgnoreTimeScale(true);
            UpdateStars(GlobalVariables.Instance.score);
            gameOverOptions.SetActive(true);
            LeanTween.value(gameObject, 0, GlobalVariables.Instance.score, 3.5f)
                 .setOnUpdate((float val) =>
                 {
                     scoreText.text = Mathf.RoundToInt(val).ToString();
                 })
                 .setEase(LeanTweenType.easeOutCubic)
                 .setIgnoreTimeScale(true);
            StartCoroutine(SelectNextFrame(replayButton));
        }
    }

    public void UpdateStars(int score)
    {
        int filledStars = Mathf.Clamp(score / 100, 0, maxStars);

        foreach (Transform child in starParent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < maxStars; i++)
        {
            bool isFilled = i < filledStars;
            GameObject prefabToUse = isFilled ? starPrefab : starEmptyPrefab;
            GameObject star = Instantiate(prefabToUse, starParent);


            star.transform.localScale = Vector3.zero;
            LeanTween.scale(star, Vector3.one, 0.5f)
                     .setEaseOutBack()
                     .setDelay(i * 0.3f).setIgnoreTimeScale(true);

            // Play sound at the same delay as animation
            string soundName = isFilled ? "levelUpSound" : "starFailSound";
            LeanTween.delayedCall(i * 0.3f, () =>
            {
                AudioManager.Instance.PlaySoundFX(soundName, transform.position, 0.2f, 0.75f, 1.25f);
            }).setIgnoreTimeScale(true);
        }
    }

    private System.Collections.IEnumerator SelectNextFrame(GameObject go)
    {
        yield return null; // wait one frame for layout/activation
        var es = EventSystem.current;
        es.SetSelectedGameObject(go);
    }
}
