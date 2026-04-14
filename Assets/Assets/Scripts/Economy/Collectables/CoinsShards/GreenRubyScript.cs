using Game.Enums;
using UnityEngine;

public class GreenRubyScript : CoinsBaseScript
{
    protected override int CoinValue => HeroUpgrades.Instance.greenRubyCoinValue;
    protected override float ExpValue => HeroUpgrades.Instance.greenRubyExpValue * HeroUpgrades.Instance.ExpMultiplier();
    protected override string AudioSound => "GreenRubySound";
    protected override CoinSpecialEffect[] SpecialEffects =>
    new CoinSpecialEffect[] { CoinSpecialEffect.Magnet };
}
