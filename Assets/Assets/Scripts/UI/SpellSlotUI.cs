using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SpellSlotUI : MonoBehaviour
{
    private Sprite spellSmallIcon;
    public Sprite SpellSmallIcon
    {
        get => spellSmallIcon;
        set
        {
            spellSmallIcon = value;
            var img = GetComponent<Image>();
            if (img != null)
            {
                img.sprite = value;
            }
        }
    }

    private Image cooldownMask;
    private Coroutine cooldownRoutine;
    [SerializeField] private GlobalSpellVariables.SpellCodeEnum spellCodeEnum;
    public GlobalSpellVariables.SpellCodeEnum SpellCodeEnum => spellCodeEnum;
    private PlayerSpellManagerScript spellManager;
    public void Init(GlobalSpellVariables.SpellCodeEnum spellCodeEnum, PlayerSpellManagerScript spellManager)
    {
        this.spellCodeEnum = spellCodeEnum;
        this.spellManager = spellManager;
    }

    private void Awake()
    {
        if (transform.childCount > 0)
        {
            cooldownMask = transform.GetChild(0).GetComponent<Image>();
            if (cooldownMask == null)
            {
                Debug.LogWarning("CooldownMask Image component not found on first child!");
            }
        }
        else
        {
            Debug.LogWarning("No children found to get cooldownMask Image from!");
        }

        if (cooldownMask != null)
        {
            cooldownMask.gameObject.SetActive(false);
            cooldownMask.fillAmount = 0f;
        }
    }

    public void StartCooldown(float cooldown)
    {
        cooldownMask.gameObject.SetActive(true);
        if (cooldownRoutine != null) StopCoroutine(cooldownRoutine);
        cooldownRoutine = StartCoroutine(CooldownRoutine(cooldown));
    }

    private IEnumerator CooldownRoutine(float cooldown)
    {
        float timer = cooldown;
        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            cooldownMask.fillAmount = timer / cooldown;
            yield return null;
        }
        spellManager.SetCooldownState(spellCodeEnum, false);
        cooldownMask.fillAmount = 0f;
        cooldownMask.gameObject.SetActive(false);
    }
}