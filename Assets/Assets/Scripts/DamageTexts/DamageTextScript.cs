using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageTextScript : MonoBehaviour
{
    private TextMeshPro damageText;
    public float fadeDuration = 1.5f;
    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        damageText = GetComponent<TextMeshPro>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rb.linearVelocity = Vector2.up;
        StartFade();
    }

    public void SetDamage(float damage, bool isCritical)
    {
        damageText.text = ((int)damage).ToString();
        if (isCritical)
        {
            damageText.color = Color.yellow;
            damageText.text = ((int)damage).ToString() + "!";
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

        TextMeshPro tmp = GetComponent<TextMeshPro>();

        Color initialColor = tmp.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            Color newColor = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);
            tmp.color = newColor;
            yield return null;
        }
        tmp.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);
        Destroy(gameObject);
    }

}
