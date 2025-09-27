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
                ShardPoolScript.Instance.ReleaseShard(gameObject);
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
        {
            MinShardsToSpawn = Mathf.Clamp(effectiveMax - 1, 0, effectiveMax);
        }

        int randomNumber = Random.Range(MinShardsToSpawn, effectiveMax + 1);

        for (int i = 0; i < maxShards; i++)
            transform.GetChild(i).gameObject.SetActive(false);

        /*Debug.Log($"Spawning {randomNumber} shards out of {maxShards} available.");*/

        for (int i = 0; i < randomNumber; i++)
        {
            var child = transform.GetChild(i).gameObject;
            child.SetActive(true);
            child.transform.localPosition = Random.insideUnitCircle * 0.9f;
        }
    }
}