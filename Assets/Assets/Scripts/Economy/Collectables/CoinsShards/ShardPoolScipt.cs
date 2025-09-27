using UnityEngine;
using UnityEngine.Pool;

public class ShardPoolScript : MonoBehaviour
{
    public static ShardPoolScript Instance;

    [SerializeField] private GameObject shardPrefab;
    [SerializeField] private Transform parent;

    private ObjectPool<GameObject> shardPool;

    void Awake()
    {
        //Singleton
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Multiple ShardPool instances found! Destroying duplicate.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        shardPool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(shardPrefab),
            actionOnGet: shard => shard.SetActive(true),
            actionOnRelease: shard => shard.SetActive(false),
            actionOnDestroy: Destroy,
            collectionCheck: false,
            defaultCapacity: 20
        );
        DontDestroyOnLoad(gameObject);
    }


    public GameObject GetShard(int minShards, int maxShards)
    {
        GameObject shard = shardPool.Get();

        if (parent != null)
        {
            shard.transform.SetParent(parent, false);
        }
        else
        {
            Debug.LogWarning("Shards parent GameObject not found in scene.");
        }
        if (shard.TryGetComponent(out ParentShardScript parentShardScript))
        {
            parentShardScript.InitializeShards(minShards, maxShards);
        }
        return shard;
    }

    public void ReleaseShard(GameObject shard)
    {
        shardPool.Release(shard);
    }
}
