using Game.Enums;
using UnityEngine;

public class GreenRubyScript : CoinsBaseScript
{
    protected override int CoinValue => GlobalVariables.Instance.greenRubyCoinValue;
    protected override int ExpValue => GlobalVariables.Instance.greenRubyExpValue;
    protected override string AudioSound => "GreenRubySound";
    protected override CoinSpecialEffect[] SpecialEffects =>
    new CoinSpecialEffect[] { CoinSpecialEffect.Heal, CoinSpecialEffect.Magnet };
}
