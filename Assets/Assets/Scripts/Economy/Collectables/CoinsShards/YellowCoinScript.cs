using UnityEngine;

public class YellowCoinScript : CoinsBaseScript
{
    protected override int CoinValue => HeroUpgrades.Instance.yellowCoinValue;
}
