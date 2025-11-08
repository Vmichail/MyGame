using UnityEngine;
using static HealEffectSelector;

public class PlayerHealthAndManaRegen : MonoBehaviour
{
    private float healthRegenTimer;
    private float manaRegenTimer;

    void Update()
    {
        if (GlobalVariables.Instance.healthRegenIsActive)
            RegenerateHealth();
        if (GlobalVariables.Instance.healthRegenIsActive)
            RegenerateMana();
    }

    public void RegenerateHealth()
    {
        healthRegenTimer += Time.deltaTime;
        if (healthRegenTimer >= GlobalVariables.Instance.playerHealthRegenInterval)
        {
            ApplyHeal(GlobalVariables.Instance.playerHealthRegen, PlayerHealEffectType.PassiveHeal);
            healthRegenTimer = 0f;
        }
    }

    public void RegenerateMana()
    {
        manaRegenTimer += Time.deltaTime;
        if (manaRegenTimer >= GlobalVariables.Instance.playerManaRegenInterval)
        {
            ApplyMana(GlobalVariables.Instance.playerManaRegen, PlayerHealEffectType.PassiveMana);
            manaRegenTimer = 0f;
        }
    }


    /// <summary>
    /// Applies health restoration. Can be called from anywhere.
    /// </summary>
    /// <param name="healValue">Amount of HP to restore</param>
    /// <param name="effectType">Type of heal effect (for visuals, logic, etc.)</param>
    public static void ApplyHeal(float healValue, PlayerHealEffectType effectType)
    {
        GlobalVariables.Instance.playerCurrentHealth =
            Mathf.Min(GlobalVariables.Instance.playerCurrentHealth + healValue, GlobalVariables.Instance.playerMaxHealth);
        HealEffectSelector.SelectHealEffect(effectType);
    }

    public static void ApplyMana(float manaValue, PlayerHealEffectType effectType)
    {
        GlobalVariables.Instance.playerCurrentMana =
            Mathf.Min(GlobalVariables.Instance.playerCurrentMana + manaValue, GlobalVariables.Instance.playerMaxMana);
        HealEffectSelector.SelectHealEffect(effectType);
    }
}