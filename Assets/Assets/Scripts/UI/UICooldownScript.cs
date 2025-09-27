using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UICooldownScript : MonoBehaviour
{
    [Header("Cooldown Settings")]
    [SerializeField] private Image cooldownMask;
    [SerializeField] private float cooldownTime = 5f;
    public bool isActive;

    private bool isCoolingDown;

    private void Start()
    {
        if (!isActive)
        {
            cooldownMask.fillAmount = 1f;
        }
    }

    public void ActivateSpell()
    {
        isActive = true;
        cooldownMask.fillAmount = 0f;
    }

    public void StartCooldown(float duration)
    {
        if (isCoolingDown) return;
        cooldownTime = duration;
        StartCoroutine(CooldownRoutine());
    }

    private IEnumerator CooldownRoutine()
    {
        isCoolingDown = true;
        float timer = cooldownTime;

        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            cooldownMask.fillAmount = timer / cooldownTime;
            yield return null;
        }

        cooldownMask.fillAmount = 0f;
        isCoolingDown = false;
    }
}