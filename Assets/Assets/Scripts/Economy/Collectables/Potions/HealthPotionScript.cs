using UnityEngine;
using UnityEngine.EventSystems;

public class HealthPotionScript : CollectableBaseScript, ICollectable
{
    public override void Collect()
    {
        if (GlobalVariables.Instance.playerCurrentHealth < GlobalVariables.Instance.playerMaxHealth)
        {
            base.Collect();
        }
    }

    protected override void CollectEnds()
    {
        AudioManager.Instance.PlaySoundFX("bottle", transform.position, 1f, 0.75f, 1.25f);
        PlayerHealthAndManaRegen.ApplyHeal(
            GlobalVariables.Instance.healthPotionHealth,
            HealEffectSelector.PlayerHealEffectType.HealthPotion
        );
        gameObject.SetActive(false);
        IsCollected = false;
    }
}
