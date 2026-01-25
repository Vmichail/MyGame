using Game.Enums;
using UnityEngine;
using UnityEngine.EventSystems;

public class CoinsBaseScript : CollectableBaseScript, ICollectable
{
    protected virtual int CoinValue { get => GlobalVariables.Instance.yellowCoinValue; }
    protected virtual int ExpValue { get => 0; }
    protected virtual string AudioSound { get => "coinSound"; }
    protected virtual CoinSpecialEffect[] SpecialEffects => new CoinSpecialEffect[] { CoinSpecialEffect.None };

    protected override void CollectEnds()
    {
        // Play coin sound
        AudioManager.Instance.PlaySoundFX(AudioSound, transform.position, 0.4f, 0.75f, 1.25f);

        // Add coin value
        CurrencyManager.instance.Add(CoinValue);

        PlayerStatsManager.Instance.CurrentExp += ExpValue;

        // Apply effects
        foreach (var effect in SpecialEffects)
        {
            switch (effect)
            {
                case CoinSpecialEffect.None:
                    break;

                case CoinSpecialEffect.Magnet:
                    ApplyMagnetEffect();
                    break;

                case CoinSpecialEffect.Heal:
                    ApplyHealEffect();
                    break;

                case CoinSpecialEffect.DoubleValue:
                    CurrencyManager.instance.Add(CoinValue);
                    break;

                case CoinSpecialEffect.Explosion:
                    ApplyExplosionEffect();
                    break;
            }
        }

        gameObject.SetActive(false);

        IsCollected = false;
    }


    private void ApplyMagnetEffect()
    {
        Debug.Log("Magnet effect applied!");
        GlobalVariables.Instance.ActivateMagnet();
    }

    private void ApplyHealEffect()
    {
        AudioManager.Instance.PlaySoundFX("bottle", transform.position, 1f, 0.75f, 1.25f);

        PlayerHealthAndManaRegen.ApplyHeal(
            PlayerStatsManager.Instance.MaxHealth(),
            HealEffectSelector.PlayerHealEffectType.HealthPotion
        );
        PlayerHealthAndManaRegen.ApplyMana(
           PlayerStatsManager.Instance.MaxMana(),
           HealEffectSelector.PlayerHealEffectType.HealthPotion
       );
    }

    private void ApplyExplosionEffect()
    {
    }


}
