using Game.Enums;
using UnityEngine;

public class GreenRubyScript : CoinsBaseScript
{
    protected override float CoinValue => GlobalVariables.Instance.greenRubyCoinValue;
    protected override float ExpValue => GlobalVariables.Instance.greenRubyCoinValue;
    protected override string AudioSound => "GreenRubySound";
    protected override CoinSpecialEffect[] SpecialEffects =>
    new CoinSpecialEffect[] { CoinSpecialEffect.Heal, CoinSpecialEffect.Magnet };
}
