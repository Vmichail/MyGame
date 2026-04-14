using Game.Enums;
using UnityEngine;

public class RedRubyScript : CoinsBaseScript
{
    protected override int CoinValue => HeroUpgrades.Instance.redRubyCoinValue;
    protected override float ExpValue => HeroUpgrades.Instance.redRubyExpValue * HeroUpgrades.Instance.ExpMultiplier();
    protected override string AudioSound => "GreenRubySound";
    protected override CoinSpecialEffect[] SpecialEffects =>
    new CoinSpecialEffect[] { CoinSpecialEffect.None };
}
