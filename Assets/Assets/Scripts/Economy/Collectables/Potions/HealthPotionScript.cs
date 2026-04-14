using UnityEngine;
using UnityEngine.EventSystems;

public class HealthPotionScript : CollectableBaseScript, ICollectable
{
    public override void Collect()
    {
        if (!PlayerStatsManager.Instance.IsFullHealth())
        {
            base.Collect();
        }
    }

    protected override void CollectEnds()
    {
        AudioManager.Instance.PlaySoundFX("bottle", transform.position, 1f, 0.75f, 1.25f);
        PlayerHealthAndManaRegen.ApplyHeal(
            (int)HeroUpgrades.Instance.healthPotionHealth,
            HealEffectSelector.PlayerHealEffectType.HealthPotion
        );
        gameObject.SetActive(false);
        IsCollected = false;
    }
}
