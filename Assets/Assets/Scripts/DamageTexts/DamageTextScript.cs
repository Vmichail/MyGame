using System.Collections;
using TMPro;
using UnityEngine;

public class DamageTextScript : MonoBehaviour
{
    [Header("Fonts")]
    [SerializeField] private TMP_FontAsset normalFont;
    [SerializeField] private TMP_FontAsset criticalFont;

    private TextMeshPro damageText;
    private Rigidbody2D rb;

    public float fadeDuration = 0.8f;

    private Coroutine fadeRoutine;

    void Awake()
    {
        damageText = GetComponent<TextMeshPro>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        StartFade();
    }

    public void SetDamage(float damage, bool isCritical, Color color)
    {
        int dmg = Mathf.RoundToInt(damage);

        transform.localScale = Vector3.one; // reset scale each reuse

        if (isCritical)
        {
            if (criticalFont != null)
                damageText.font = criticalFont;

            rb.linearVelocity = new Vector2(0, 2f);

            damageText.color = new Color(1f, 0.84f, 0f);

            damageText.fontSize *= 1.2f;

            damageText.text = "<b>" + dmg + "!</b>";

            transform.localScale = Vector3.one * 1.1f;
        }
        else
        {
            if (normalFont != null)
                damageText.font = normalFont;

            rb.linearVelocity = Vector2.up;

            if (color == default)
                color = Color.white;

            damageText.color = color;

            damageText.text = dmg.ToString();
        }
    }

    public void StartFade()
    {
        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(FadeOutAndReturn());
    }

    private IEnumerator FadeOutAndReturn()
    {
        float elapsed = 0f;
        Color initialColor = damageText.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;

            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);

            damageText.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);

            yield return null;
        }

        damageText.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);

        PoolManager.Instance.Return(gameObject);
    }
}