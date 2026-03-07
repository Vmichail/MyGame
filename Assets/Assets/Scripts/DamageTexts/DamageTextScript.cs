using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageTextScript : MonoBehaviour
{
    [Header("Fonts")]
    [SerializeField] private TMP_FontAsset normalFont;
    [SerializeField] private TMP_FontAsset criticalFont;

    private TextMeshPro damageText;
    public float fadeDuration = 0.8f;
    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        damageText = GetComponent<TextMeshPro>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        StartFade();
    }

    public void SetDamage(float damage, bool isCritical, Color color)
    {
        int dmg = Mathf.RoundToInt(damage);

        if (isCritical)
        {
            // Switch to critical font
            if (criticalFont != null)
                damageText.font = criticalFont;
            rb.linearVelocity = new Vector2(0, 2f);
            // Gold color
            damageText.color = new Color(1f, 0.84f, 0f);

            // Bigger text
            damageText.fontSize *= 1.2f;

            // Bold + symbols for stronger feel
            damageText.text = "<b>" + dmg + "!</b>";

            // Pop scale effect
            transform.localScale = Vector3.one * 1.1f;
        }
        else
        {
            rb.linearVelocity = Vector2.up;
            if (color == default)
                color = Color.white;

            damageText.color = color;
            damageText.text = dmg.ToString();
        }
    }



    // Call this to start fading and destroy after
    public void StartFade()
    {
        StartCoroutine(FadeOutAndDestroy());
    }

    private IEnumerator FadeOutAndDestroy()
    {
        float elapsed = 0f;
        Color initialColor = damageText.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            Color newColor = new(initialColor.r, initialColor.g, initialColor.b, alpha);
            damageText.color = newColor;
            yield return null;
        }
        damageText.color = new(initialColor.r, initialColor.g, initialColor.b, 0f);
        Destroy(gameObject);
    }

}
