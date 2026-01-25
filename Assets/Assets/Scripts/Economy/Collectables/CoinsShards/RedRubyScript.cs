using Game.Enums;
using UnityEngine;

public class RedRubyScript : CoinsBaseScript
{
    protected override int CoinValue => GlobalVariables.Instance.redRubyCoinValue;
    protected override int ExpValue => GlobalVariables.Instance.redRubyExpValue;
    protected override string AudioSound => "GreenRubySound";
    protected override CoinSpecialEffect[] SpecialEffects =>
    new CoinSpecialEffect[] { CoinSpecialEffect.None };
}
