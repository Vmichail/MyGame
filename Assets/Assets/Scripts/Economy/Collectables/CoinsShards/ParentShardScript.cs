using DG.Tweening;
using UnityEngine;

public class ParentShardScript : MonoBehaviour
{
    public int MaxShardsToSpawn { get; set; }
    public int MinShardsToSpawn { get; set; }
    private void Update()
    {
        // Only check if parent is active
        if (gameObject.activeSelf)
        {
            bool allInactive = true;

            foreach (Transform child in transform)
            {
                if (child.gameObject.activeSelf)
                {
                    allInactive = false;
                    break;
                }
            }

            // If every child is inactive, disable parent
            if (allInactive)
            {
                MinShardsToSpawn = 0;
                MaxShardsToSpawn = 0;
                ShardPool.Instance.ReleaseShard(gameObject);
            }
        }
    }

    public void InitializeShards(int min, int max)
    {
        MinShardsToSpawn = min;
        MaxShardsToSpawn = max;
        SpawnShards();
    }

    private void SpawnShards()
    {
        int maxShards = transform.childCount;

        int effectiveMax = Mathf.Clamp(MaxShardsToSpawn, 1, maxShards);

        if (MinShardsToSpawn >= effectiveMax)
            MinShardsToSpawn = Mathf.Clamp(effectiveMax - 1, 0, effectiveMax);

        int shardCount = Random.Range(MinShardsToSpawn, effectiveMax + 1);

        // Disable all shards first
        for (int i = 0; i < maxShards; i++)
            transform.GetChild(i).gameObject.SetActive(false);

        // Layout settings
        float radius = 0.55f;   // distance from center
        float jitter = 0.12f;   // small randomness

        for (int i = 0; i < shardCount; i++)
        {
            Transform child = transform.GetChild(i);
            child.gameObject.SetActive(true);

            // Even angular distribution
            float angle = (360f / shardCount) * i;
            float rad = angle * Mathf.Deg2Rad;

            Vector2 basePos = new Vector2(
                Mathf.Cos(rad),
                Mathf.Sin(rad)
            ) * radius;

            Vector2 offset = Random.insideUnitCircle * jitter;

            child.localPosition = basePos + offset;

            // Optional: tiny Z offset to avoid sprite overlap flicker
            child.localPosition += new Vector3(0f, 0f, -i * 0.001f);
        }
    }

    public void Initialize(Vector3 spawnPos)
    {
        transform.position = spawnPos;
        transform.DOKill();
        transform.DOMoveY(0.3f, 0.8f)
                    .SetRelative()
                    .SetEase(Ease.InOutSine)
                    .SetLoops(-1, LoopType.Yoyo);
    }
}