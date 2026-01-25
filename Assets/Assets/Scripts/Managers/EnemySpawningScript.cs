using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;
using static GlobalVariables;
using Random = UnityEngine.Random;

[Serializable]
public class EnemySpawnConfig
{
    public GlobalVariables.EnemyTypes enemyType;
    public GameObject prefab;
}


public class EnemySpawningScript : MonoBehaviour
{
    [Header("Enemy Spawning Configurations")]
    [SerializeField] private float spawnTimeDefault;
    [SerializeField] private float skeletonsSpawningTime;
    [SerializeField] private float blackArmoredSkeletonSpawningTime;
    [SerializeField] private float whiteArmoredSkeletonSpawningTime;
    [SerializeField] private float skeletonArchersSpawningTime;
    [SerializeField] private float vampireNormalSpawningTime;
    [SerializeField] private float vampireBossSpawningTimeEndlessMode;
    [Header("Other boss Timers")]

    [SerializeField] private float vampireMiniBossSpawningTime;
    [SerializeField] private float skeletonKingBossSpawningTime;
    [SerializeField] private float thirdBossSpawningTime;
    [SerializeField] private float loopBossSpawningTime;
    [SerializeField] private float loopBossSpawningTimeIncrease;
    [SerializeField] private float loopBossSpawningTimeIncreaseDeacreaseTime;


    [SerializeField] private EnemySpawnConfig[] enemySpawnConfigs;

    [SerializeField] private Transform parent;
    [SerializeField] private GameObject spawnIndicatorPrefab;
    [SerializeField] private GameObject spawnBossIndicatorPrefab;
    [SerializeField] private Transform[] specificEnemyPosition;
    [SerializeField] private Light2D globalLight;

    private Dictionary<GlobalVariables.EnemyTypes, ObjectPool<GameObject>> enemyPools = new();
    [Header("Collectable Pets")]
    [SerializeField] private Transform collectableParent;
    [SerializeField] private GameObject collectableCat;
    [SerializeField] private Transform[] collectableCatSpawnPotitions;
    [SerializeField] private float collectableSpawnDelay = 20f;
    [SerializeField] private bool changeVampireMiniBossMusic = true;
    private bool skeletonArchersEnabled;
    private bool blackArmoredSkeletonEnabled;
    private bool whiteArmoredSkeletonEnabled;
    private bool vampiresEnabled;
    private bool firstBossSpawned;
    private bool secondBossSpawned;
    private bool thirdBossSpawned;
    private bool loopBossSpawned;
    private bool vampireMiniBossActivated;
    private Sequence spawnSequence;

    void Start()
    {
        foreach (var config in enemySpawnConfigs)
        {
            GlobalVariables.EnemyTypes type = config.enemyType;
            GameObject prefab = config.prefab;

            var pool = new ObjectPool<GameObject>(
                () => CreateEnemy(prefab, type),
                mob => OnGet(mob),
                mob => mob.SetActive(false),
                mob => Destroy(mob),
                false,
                10,
                100
            );

            enemyPools[type] = pool;
        }
        StartCoroutine(SpawnSpecificMobs(GlobalVariables.EnemyTypes.Level1Skeleton));

        // Start async spawning loop for collectable cats
        SpawnCollectableCatsLoop().Forget();

    }

    public void SpawnMobsOnSpecificPosition(
        Transform[] mobPositions,
        EnemyTypes enemyType)
    {
        if (mobPositions == null)
            mobPositions = specificEnemyPosition;

        if (mobPositions == null || mobPositions.Length == 0)
            return;

        spawnSequence?.Kill();

        spawnSequence = DOTween.Sequence();

        foreach (Transform pos in mobPositions)
        {
            if (pos == null) continue;

            Transform spawnPos = pos; // 👈 capture local copy

            // Delay
            spawnSequence.AppendInterval(0.3f);

            // Spawn action
            spawnSequence.AppendCallback(() =>
            {
                SpawnSpecificEnemyWithEffect(
                    enemyPools[enemyType],
                    spawnPos.position
                );
            });
        }
    }


    private void SpawnVampireMiniBoss()
    {
        changeVampireMiniBossMusic = false;
        globalLight.intensity = 0.8f;
        globalLight.color = new Color(1f, 0.9f, 0.9f);
        StartCoroutine(SpawnIndicatorThenEnemy(enemyPools[GlobalVariables.EnemyTypes.VampireBoss], new Vector2(Random.Range(-10f, 10f), Random.Range(-10f, 10f)), true));
    }


    private void SpawnSkeletonKingBoss()
    {
        if (changeVampireMiniBossMusic)
            AudioManager.Instance.PlayMusic(3);
        changeVampireMiniBossMusic = false;
        globalLight.intensity = 0.7f;
        globalLight.color = new Color(0.9f, 0.75f, 0.65f);
        StartCoroutine(SpawnIndicatorThenEnemy(enemyPools[GlobalVariables.EnemyTypes.SkeletonKing], new Vector2(Random.Range(-5f, 5f), Random.Range(-5f, 5f)), true));
    }

    private void SpawnSpecificEnemyWithEffect(ObjectPool<GameObject> pool, Vector2 position)
    {
        AudioManager.Instance.PlaySoundFX("sharp-pop", transform.position, 0.4f, 0.75f, 1.25f);
        CinemachineScript.Instance.Shake(0.15f, 0.15f);
        StartCoroutine(SpawnIndicatorThenEnemy(pool, position, false));
    }

    private void FixedUpdate()
    {
        //Skeleton Archer spawn
        if (GlobalVariables.Instance.spawnedSkeletons > 45 && blackArmoredSkeletonEnabled == false)
        {
            blackArmoredSkeletonEnabled = true;
            StartCoroutine(SpawnSpecificMobs(GlobalVariables.EnemyTypes.BlackArmoredSkeleton));
        }

        if (GlobalVariables.Instance.spawnedSkeletons > 35 && whiteArmoredSkeletonEnabled == false)
        {
            whiteArmoredSkeletonEnabled = true;
            StartCoroutine(SpawnSpecificMobs(GlobalVariables.EnemyTypes.WhiteArmoredSkeleton));
        }

        if (GlobalVariables.Instance.spawnedSkeletons > 25 && skeletonArchersEnabled == false)
        {
            skeletonArchersEnabled = true;
            StartCoroutine(SpawnSpecificMobs(GlobalVariables.EnemyTypes.SkeletonArcher));
        }

        if (GlobalVariables.Instance.spawnedSkeletons > 100 && vampiresEnabled == false)
        {
            vampiresEnabled = true;
            StartCoroutine(SpawnSpecificMobs(GlobalVariables.EnemyTypes.VampireNormal));
        }

        if (GlobalVariables.Instance.gameTime >= vampireMiniBossSpawningTime && !firstBossSpawned)
        {
            firstBossSpawned = true;
            SpawnVampireMiniBoss();
            StartCoroutine(SpawnSpecificMobs(GlobalVariables.EnemyTypes.Level1Skeleton));
        }

        if (GlobalVariables.Instance.gameTime >= skeletonKingBossSpawningTime && !secondBossSpawned)
        {
            secondBossSpawned = true;
            SpawnSkeletonKingBoss();
            StartCoroutine(SpawnSpecificMobs(GlobalVariables.EnemyTypes.Level1Skeleton));
        }

        if (GlobalVariables.Instance.gameTime >= thirdBossSpawningTime && !thirdBossSpawned)
        {
            thirdBossSpawned = true;
            SpawnVampireMiniBoss();
            SpawnSkeletonKingBoss();

        }

        if (GlobalVariables.Instance.gameTime >= loopBossSpawningTime)
        {
            GlobalVariables.Instance.endlessModeOn = true;
            loopBossSpawningTime += loopBossSpawningTimeIncrease;
            loopBossSpawningTimeIncrease -= loopBossSpawningTimeIncreaseDeacreaseTime;
            SpawnVampireMiniBoss();
            SpawnSkeletonKingBoss();
            //
            StartCoroutine(SpawnSpecificMobs(GlobalVariables.EnemyTypes.WhiteArmoredSkeleton));
            StartCoroutine(SpawnSpecificMobs(GlobalVariables.EnemyTypes.SkeletonArcher));
            StartCoroutine(SpawnSpecificMobs(GlobalVariables.EnemyTypes.BlackArmoredSkeleton));
        }

        if (GlobalVariables.Instance.endlessModeOn && !vampireMiniBossActivated)
        {
            vampireMiniBossActivated = true;
            StartCoroutine(SpawnSpecificMobs(EnemyTypes.VampireBoss));
        }
    }

    private GameObject CreateEnemy(GameObject prefab, GlobalVariables.EnemyTypes type)
    {
        GameObject mob = Instantiate(prefab, parent);
        mob.SetActive(false);

        if (mob.TryGetComponent(out EnemyBaseScript enemyBase))
        {
            enemyBase.SetPool(enemyPools[type]);
        }

        return mob;
    }

    private void OnGet(GameObject mob)
    {
        if (mob.TryGetComponent<EnemyBaseScript>(out var enemyBase))
        {
            EnemyManagerScript.Instance.RegisterEnemy(mob, enemyBase.EnemyType);
        }
        mob.SetActive(true);
    }

    //private IEnumerator SpawnRandomMobs()
    //{
    //    while (GlobalVariables.Instance.spawningMobsIsEnabled)
    //    {
    //        yield return new WaitForSeconds(GlobalVariables.Instance.mobsSpawningTime);
    //        // Choose pool index
    //        int index = Random.Range(0, allowedLength > 0 ? allowedLength : enemyPools.Count);
    //        Vector2 position = new(Random.Range(-20f, 20f), Random.Range(-17f, 17f));
    //        StartCoroutine(SpawnIndicatorThenEnemy(enemyPools[index], position));
    //    }
    //}

    private IEnumerator SpawnSpecificMobs(GlobalVariables.EnemyTypes enemyType, float maxWidth = 20f, float minWidth = -20f, float maxHeight = 17f, float minHeight = -17f)
    {
        Vector2 position = new(Random.Range(minWidth, maxWidth), Random.Range(minHeight, maxHeight));
        StartCoroutine(SpawnIndicatorThenEnemy(enemyPools[enemyType], position, false));
        float spawningTime = GetSpawnTime(enemyType);
        while (GlobalVariables.Instance.spawningMobsIsEnabled && GlobalVariables.Instance.aliveEnemies < 150)
        {
            position = new(Random.Range(minWidth, maxWidth), Random.Range(minHeight, maxHeight));
            yield return new WaitForSeconds(spawningTime);
            StartCoroutine(SpawnIndicatorThenEnemy(enemyPools[enemyType], position, false));
        }
    }

    private float GetSpawnTime(GlobalVariables.EnemyTypes enemyType)
    {
        return enemyType switch
        {
            GlobalVariables.EnemyTypes.Level1Skeleton => skeletonsSpawningTime,
            GlobalVariables.EnemyTypes.BlackArmoredSkeleton => blackArmoredSkeletonSpawningTime,
            GlobalVariables.EnemyTypes.WhiteArmoredSkeleton => whiteArmoredSkeletonSpawningTime,
            GlobalVariables.EnemyTypes.SkeletonArcher => skeletonArchersSpawningTime,
            GlobalVariables.EnemyTypes.VampireNormal => vampireNormalSpawningTime,
            GlobalVariables.EnemyTypes.VampireBoss => vampireBossSpawningTimeEndlessMode,
            _ => spawnTimeDefault
        };
    }

    private IEnumerator SpawnIndicatorThenEnemy(ObjectPool<GameObject> pool, Vector2 position, bool isBoss = false)
    {
        GameObject indicator = null;
        if (isBoss)
            indicator = Instantiate(spawnBossIndicatorPrefab, position, Quaternion.identity, parent);
        else
            indicator = Instantiate(spawnIndicatorPrefab, position, Quaternion.identity, parent);
        SpawnIndicator indicatorScript = indicator.GetComponent<SpawnIndicator>();

        yield return new WaitUntil(() => indicatorScript.IsReadyToSpawn);

        Destroy(indicator);

        GameObject mob = pool.Get();
        //Debug.Log($"Spawned {(isBoss ? "Boss" : "Enemy")}: {mob.name} at {position} with pool index = {index}");
        mob.transform.position = position;
    }


    private async UniTaskVoid SpawnCollectableCatsLoop()
    {
        // Cancel automatically if GameObject destroyed
        var token = this.GetCancellationTokenOnDestroy();

        if (GlobalVariables.Instance.isSpawningCollectablePets)
        {
            SpawnCollectableCat();
        }

        while (GlobalVariables.Instance.isSpawningCollectablePets && !token.IsCancellationRequested)
        {
            await UniTask.Delay(
                (int)(collectableSpawnDelay * 1000),
                DelayType.DeltaTime,
                PlayerLoopTiming.Update,
                token
            );

            SpawnCollectableCat();
        }
    }

    private void SpawnCollectableCat()
    {
        if (collectableCat == null || collectableCatSpawnPotitions == null || collectableCatSpawnPotitions.Length == 0)
            return;

        Transform randomPos = collectableCatSpawnPotitions[Random.Range(0, collectableCatSpawnPotitions.Length)];

        if (randomPos == null)
            return;

        GameObject collactableCat = CollectableCatPool.Instance.GetCat();
        if (collactableCat.TryGetComponent(out CollectablePetScript collectablePetScript))
        {
            collectablePetScript.Initialize(randomPos.position);
        }
        else
        {
            Debug.LogWarning("CollectablePetScript component not found on the CollectableCat prefab!");
        }
    }

}
