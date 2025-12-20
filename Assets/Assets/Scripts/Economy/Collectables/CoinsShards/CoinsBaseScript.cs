using Game.Enums;
using UnityEngine;
using UnityEngine.EventSystems;

public class CoinsBaseScript : CollectableBaseScript, ICollectable
{
    protected virtual float CoinValue { get => GlobalVariables.Instance.yellowCoinValue; }
    protected virtual float ExpValue { get => 0; }
    protected virtual string AudioSound { get => "coinSound"; }
    protected virtual CoinSpecialEffect[] SpecialEffects => new CoinSpecialEffect[] { CoinSpecialEffect.None };

    protected override void CollectEnds()
    {
        // Play coin sound
        AudioManager.Instance.PlaySoundFX(AudioSound, transform.position, 0.4f, 0.75f, 1.25f);

        // Add coin value
        GlobalVariables.Instance.coinsCollected += CoinValue;

        GlobalVariables.Instance.currentExp += ExpValue;

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
                    GlobalVariables.Instance.coinsCollected += CoinValue;
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
        GlobalVariables.Instance.ActivateMagnet();
    }

    private void ApplyHealEffect()
    {
        AudioManager.Instance.PlaySoundFX("bottle", transform.position, 1f, 0.75f, 1.25f);

        PlayerHealthAndManaRegen.ApplyHeal(
            GlobalVariables.Instance.playerMaxHealth,
            HealEffectSelector.PlayerHealEffectType.HealthPotion
        );
        PlayerHealthAndManaRegen.ApplyMana(
           GlobalVariables.Instance.playerMaxMana,
           HealEffectSelector.PlayerHealEffectType.HealthPotion
       );
    }

    private void ApplyExplosionEffect()
    {
    }


}
