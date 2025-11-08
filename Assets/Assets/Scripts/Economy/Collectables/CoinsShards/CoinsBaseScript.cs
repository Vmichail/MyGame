using UnityEngine;
using UnityEngine.EventSystems;

public class CoinsBaseScript : CollectableBaseScript, ICollectable
{
    protected virtual float CoinValue { get => GlobalVariables.Instance.yellowCoinValue; }

    protected override void CollectEnds()
    {
        AudioManager.Instance.PlaySoundFX("coinSound", transform.position, 0.4f, 0.75f, 1.25f);
        GlobalVariables.Instance.coinsCollected += CoinValue;
        gameObject.SetActive(false);
        IsCollected = false;
    }
}
