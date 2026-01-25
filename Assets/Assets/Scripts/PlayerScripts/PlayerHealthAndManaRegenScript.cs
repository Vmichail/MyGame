using UnityEngine;
using static HealEffectSelector;

public class PlayerHealthAndManaRegen : MonoBehaviour
{
    private float healthRegenTimer;
    private float manaRegenTimer;

    void Update()
    {
        if (PlayerStatsManager.Instance.RegenIsActive)
            RegenerateHealth();
        if (PlayerStatsManager.Instance.RegenIsActive)
            RegenerateMana();
    }

    public void RegenerateHealth()
    {
        healthRegenTimer += Time.deltaTime;
        if (healthRegenTimer >= PlayerStatsManager.Instance.RuntimeStats.Get(PlayerStatType.Defence_HealthRegenInterval))
        {
            ApplyHeal((int)PlayerStatsManager.Instance.RuntimeStats.Get(PlayerStatType.Defence_HealthRegenValue), PlayerHealEffectType.PassiveHeal);
            healthRegenTimer = 0f;
        }
    }

    public void RegenerateMana()
    {
        manaRegenTimer += Time.deltaTime;
        if (manaRegenTimer >= PlayerStatsManager.Instance.RuntimeStats.Get(PlayerStatType.Defence_ManaRegenInterval))
        {
            ApplyMana(PlayerStatsManager.Instance.RuntimeStats.Get(PlayerStatType.Defence_ManaRegen), PlayerHealEffectType.PassiveMana);
            manaRegenTimer = 0f;
        }
    }

    public static void ApplyHeal(int healValue, PlayerHealEffectType effectType)
    {
        PlayerStatsManager.Instance.CurrentHealth =
            Mathf.Min(PlayerStatsManager.Instance.CurrentHealth + healValue, PlayerStatsManager.Instance.MaxHealth());
        SelectHealEffect(effectType);
    }

    public static void ApplyMana(float manaValue, PlayerHealEffectType effectType)
    {
        PlayerStatsManager.Instance.CurrentMana = (int)Mathf.Min(PlayerStatsManager.Instance.CurrentMana + manaValue, PlayerStatsManager.Instance.RuntimeStats.Get(PlayerStatType.Defence_Mana));
        SelectHealEffect(effectType);
    }
}