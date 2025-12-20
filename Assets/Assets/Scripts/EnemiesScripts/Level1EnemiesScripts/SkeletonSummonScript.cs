using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

public class SkeletonSummonScript : MonoBehaviour
{
    [SerializeField] private GameObject skeletonBossPrefab;

    [Header("Spawn timings")]
    [SerializeField] private float minRespawnIfDead = 30f;
    [SerializeField] private float maxRespawnIfDead = 45f;
    [SerializeField] private float respawnIfNotDead = 150f;

    [SerializeField] private bool isDead = true;

    private CancellationTokenSource loopCts;

    private void OnEnable()
    {
        loopCts?.Cancel();
        loopCts = new CancellationTokenSource();
        SpawnLoop(loopCts.Token).Forget();
    }

    private void OnDisable()
    {
        loopCts?.Cancel();
        loopCts = null;
    }

    private async UniTaskVoid SpawnLoop(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            float wait = isDead
                ? Random.Range(minRespawnIfDead, maxRespawnIfDead)
                : respawnIfNotDead;

            // Delay in seconds, cancellable
            await UniTask.Delay(
                System.TimeSpan.FromSeconds(wait),
                cancellationToken: token
            );

            if (token.IsCancellationRequested) break;

            SpawnBoss();
        }
    }

    private void SpawnBoss()
    {
        if (skeletonBossPrefab == null)
        {
            Debug.LogError("SkeletonSummonScript: No boss prefab assigned!");
            return;
        }

        Instantiate(skeletonBossPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}