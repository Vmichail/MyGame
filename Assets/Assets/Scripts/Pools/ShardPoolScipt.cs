using UnityEngine;

public class ShardPool : BasePool<ShardPool>
{
    public GameObject GetShard(int minShards, int maxShards)
    {
        GameObject shard = Get();

        if (shard.TryGetComponent(out ParentShardScript parentShard))
        {
            parentShard.InitializeShards(minShards, maxShards);
        }

        return shard;
    }

    public void ReleaseShard(GameObject shard)
    {
        Release(shard);
    }
}