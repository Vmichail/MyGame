using UnityEngine;
using UnityEngine.EventSystems;

public class ShardScript : CollectableBaseScript, ICollectable
{
    private int ShardExp { get => GlobalVariables.Instance.shardExp; }

    protected override void CollectEnds()
    {
        AudioManager.Instance.PlaySoundFX("shardSound", transform.position, 0.4f, 0.75f, 1.25f);
        PlayerStatsManager.Instance.CurrentExp += ShardExp;
        gameObject.SetActive(false);
        IsCollected = false;
    }
}
