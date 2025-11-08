using System.Buffers.Text;
using UnityEngine;
using UnityEngine.EventSystems;

public class ManaPotionScript : CollectableBaseScript, ICollectable
{
    public override void Collect()
    {
        if (GlobalVariables.Instance.playerCurrentMana < GlobalVariables.Instance.playerMaxMana)
        {
            base.Collect();
        }
    }

    protected override void CollectEnds()
    {
        AudioManager.Instance.PlaySoundFX("bottle", transform.position, 1f, 0.75f, 1.25f);
        PlayerHealthAndManaRegen.ApplyMana(
           GlobalVariables.Instance.manaPotionMana,
           HealEffectSelector.PlayerHealEffectType.ManaPotion
       );
        gameObject.SetActive(false);
        IsCollected = false;
    }
}
