using UnityEngine;
using UnityEngine.UI;

public class HurtEffect : MonoBehaviour
{
    public static HurtEffect Instance { get; private set; }

    private Image hurtImage;

    [Header("Settings")]
    [SerializeField] private float flashInDuration = 0.1f;
    [SerializeField] private float flashOutDuration = 0.5f;
    [SerializeField] private Color flashColor = new Color(1f, 0f, 0f, 0.5f);

    private void Awake()
    {
        Instance = this;
        hurtImage = GetComponent<Image>();
        hurtImage.color = new Color(1, 0, 0, 0);
    }

    /// <summary>
    /// Triggers a red flash. Optionally scales intensity with damage.
    /// </summary>
    public void Flash(float damage = 1f)
    {
        // Optional: damage intensity → stronger hit = more red
        float intensity = Mathf.Clamp01(damage / 2f); // scale to your game balance
        Color targetColor = new(flashColor.r, flashColor.g, flashColor.b, flashColor.a * intensity);

        // Cancel any active tween
        LeanTween.cancel(gameObject);

        // Fade in to red
        LeanTween.value(gameObject, 0f, targetColor.a, flashInDuration)
            .setOnUpdate((float val) =>
            {
                hurtImage.color = new Color(targetColor.r, targetColor.g, targetColor.b, val);
            })
            .setOnComplete(() =>
            {
                // Fade back out
                LeanTween.value(gameObject, targetColor.a, 0f, flashOutDuration)
                    .setOnUpdate((float val) =>
                    {
                        hurtImage.color = new Color(targetColor.r, targetColor.g, targetColor.b, val);
                    }).setIgnoreTimeScale(true);
            }).setIgnoreTimeScale(true);
    }
}