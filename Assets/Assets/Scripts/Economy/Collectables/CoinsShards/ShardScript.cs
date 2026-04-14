using UnityEngine;
using UnityEngine.EventSystems;

public class ShardScript : CollectableBaseScript, ICollectable
{
    private float ShardExp { get => HeroUpgrades.Instance.shardExp * HeroUpgrades.Instance.ExpMultiplier(); }
    private ParentShardScript parentShardScript;

    protected override void CollectEnds()
    {
        AudioManager.Instance.PlaySoundFX("shardSound", transform.position, 0.4f, 0.75f, 1.25f);
        PlayerStatsManager.Instance.CurrentExp += ShardExp;
        gameObject.SetActive(false);
        IsCollected = false;
        parentShardScript.CheckToDisable();
    }

    public void SetParentShardScript(ParentShardScript parent)
    {
        parentShardScript = parent;
    }
}
