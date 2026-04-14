using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Rendering.Universal;
using static GlobalVariables;
using static UnityEngine.EventSystems.EventTrigger;
using Random = UnityEngine.Random;

[Serializable]
public class EnemySpawnConfig
{
    public EnemyTypes enemyType;
    public GameObject prefab;
}


public class EnemySpawningScript : MonoBehaviour
{
    [SerializeField] private int maxAllowedSpawnedCats = 5;
    [Header("Enemy Spawning Configurations")]
    [SerializeField] private float spawnTimeDefault;
    [SerializeField] private int maxAliveEnemies = 120;
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
    [SerializeField] private float loopBossSpawningTimeIncreaseDecreaseTime;


    [SerializeField] private EnemySpawnConfig[] enemySpawnConfigs;

    [SerializeField] private Transform parent;
    [SerializeField] private GameObject spawnIndicatorPrefab;
    [SerializeField] private GameObject spawnBossIndicatorPrefab;
    [SerializeField] private Transform[] specificEnemyPosition;
    [SerializeField] private Light2D globalLight;

    private Dictionary<EnemyTypes, ObjectPool<GameObject>> enemyPools = new();
    [Header("Collectable Pets")]
    [SerializeField] private Transform collectableParent;
    [SerializeField] private GameObject collectableCat;
    [SerializeField] private Transform[] collectableCatSpawnPotitions;
    [SerializeField] private float collectableSpawnDelay = 20f;
    [SerializeField] private bool canSpawnSkeletons = true;
    [SerializeField] private bool changeVampireMiniBossMusic = true;
    [SerializeField] private bool canSpawnSkeletonArchers = true;
    [SerializeField] private bool canSpawnBlackArmoredSkeletons = true;
    [SerializeField] private bool canSpawnWhiteArmoredSkeletons = true;
    [SerializeField] private bool canSpawnVampires = true;
    private bool skeletonsEnabled;
    private bool skeletonArchersEnabled;
    private bool blackArmoredSkeletonEnabled;
    private bool whiteArmoredSkeletonEnabled;
    private bool vampiresEnabled;
    private bool firstBossSpawned;
    private bool secondBossSpawned;
    private bool thirdBossSpawned;
    private bool vampireMiniBossActivated;
    private Sequence spawnSequence;
    //
    private float currentSkeletonSpawnTime;
    private float minSkeletonSpawnTime = 0.5f;
    //
    private float currentSkeletonArchersSpawnTime;
    private float minSkeletonArchersSpawnTime = 1f;
    //
    private float currentBlackArmoredSkeletonSpawnTime;
    private float minBlackArmoredSkeletonSpawnTime = 2f;
    //
    private float currentWhiteArmoredSkeletonSpawnTime;
    private float minWhiteArmoredSkeletonSpawnTime = 5f;

    void Start()
    {
        currentSkeletonSpawnTime = skeletonsSpawningTime;
        currentSkeletonArchersSpawnTime = skeletonArchersSpawningTime;
        currentBlackArmoredSkeletonSpawnTime = blackArmoredSkeletonSpawningTime;
        currentWhiteArmoredSkeletonSpawnTime = whiteArmoredSkeletonSpawningTime;

        foreach (var config in enemySpawnConfigs)
        {
            EnemyTypes type = config.enemyType;
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
        StartCoroutine(SpawnIndicatorThenEnemy(enemyPools[EnemyTypes.VampireBoss], new Vector2(Random.Range(-10f, 10f), Random.Range(-10f, 10f)), true));
    }


    private void SpawnSkeletonKingBoss()
    {
        if (changeVampireMiniBossMusic)
            AudioManager.Instance.PlayMusic(3);
        changeVampireMiniBossMusic = false;
        globalLight.intensity = 0.7f;
        globalLight.color = new Color(0.9f, 0.75f, 0.65f);
        StartCoroutine(SpawnIndicatorThenEnemy(enemyPools[EnemyTypes.SkeletonKing], new Vector2(Random.Range(-5f, 5f), Random.Range(-5f, 5f)), true));
    }

    private void SpawnSpecificEnemyWithEffect(ObjectPool<GameObject> pool, Vector2 position)
    {
        AudioManager.Instance.PlaySoundFX("sharp-pop", transform.position, 0.4f, 0.75f, 1.25f);
        CinemachineScript.Instance.Shake(0.15f, 0.15f);
        StartCoroutine(SpawnIndicatorThenEnemy(pool, position, false));
    }

    private void Update()
    {
        if (canSpawnSkeletons && skeletonsEnabled == false)
        {
            skeletonsEnabled = true;
            StartCoroutine(SpawnSpecificMobs(EnemyTypes.Level1Skeleton));
        }

        if (canSpawnSkeletonArchers && Instance.gameTime > skeletonArchersSpawningTime && skeletonArchersEnabled == false)
        {
            skeletonArchersEnabled = true;
            StartCoroutine(SpawnSpecificMobs(EnemyTypes.SkeletonArcher));
        }

        if (canSpawnBlackArmoredSkeletons && Instance.gameTime > blackArmoredSkeletonSpawningTime && blackArmoredSkeletonEnabled == false)
        {
            blackArmoredSkeletonEnabled = true;
            StartCoroutine(SpawnSpecificMobs(EnemyTypes.BlackArmoredSkeleton));
        }

        if (canSpawnWhiteArmoredSkeletons && Instance.gameTime > whiteArmoredSkeletonSpawningTime && whiteArmoredSkeletonEnabled == false)
        {
            whiteArmoredSkeletonEnabled = true;
            StartCoroutine(SpawnSpecificMobs(EnemyTypes.WhiteArmoredSkeleton));
        }



        if (canSpawnVampires && Instance.gameTime > 120 && vampiresEnabled == false)
        {
            vampiresEnabled = true;
            StartCoroutine(SpawnSpecificMobs(EnemyTypes.VampireNormal));
        }

        if (Instance.gameTime >= vampireMiniBossSpawningTime && !firstBossSpawned)
        {
            firstBossSpawned = true;
            SpawnVampireMiniBoss();
            currentSkeletonSpawnTime = Mathf.Max(minSkeletonSpawnTime, currentSkeletonSpawnTime - 0.2f);
        }

        if (Instance.gameTime >= skeletonKingBossSpawningTime && !secondBossSpawned)
        {
            secondBossSpawned = true;
            SpawnSkeletonKingBoss();
            currentSkeletonSpawnTime = Mathf.Max(minSkeletonSpawnTime, currentSkeletonSpawnTime - 0.2f);
        }

        if (Instance.gameTime >= thirdBossSpawningTime && !thirdBossSpawned)
        {
            thirdBossSpawned = true;
            SpawnVampireMiniBoss();
            SpawnSkeletonKingBoss();

        }

        if (Instance.gameTime >= loopBossSpawningTime)
        {
            Instance.endlessModeOn = true;
            loopBossSpawningTime += loopBossSpawningTimeIncrease;
            loopBossSpawningTimeIncrease = Mathf.Max(40, loopBossSpawningTimeIncrease - loopBossSpawningTimeIncreaseDecreaseTime);
            SpawnVampireMiniBoss();
            SpawnSkeletonKingBoss();
            //
            currentSkeletonArchersSpawnTime = Mathf.Max(minSkeletonArchersSpawnTime, currentSkeletonArchersSpawnTime - 0.3f);
            currentBlackArmoredSkeletonSpawnTime = Mathf.Max(minBlackArmoredSkeletonSpawnTime, currentBlackArmoredSkeletonSpawnTime - 1.5f);
            currentWhiteArmoredSkeletonSpawnTime = Mathf.Max(minWhiteArmoredSkeletonSpawnTime, currentWhiteArmoredSkeletonSpawnTime - 2);
        }

        if (Instance.endlessModeOn && !vampireMiniBossActivated)
        {
            vampireMiniBossActivated = true;
            StartCoroutine(SpawnSpecificMobs(EnemyTypes.VampireBoss));
        }
    }

    private GameObject CreateEnemy(GameObject prefab, EnemyTypes type)
    {
        GameObject mob = Instantiate(prefab, parent);
        mob.SetActive(false);

        if (mob.TryGetComponent(out EnemyBaseScript enemyBase))
        {
            enemyBase.SetPool(enemyPools[type]);
        }
        else
        {
            Debug.LogWarning("No enemyBaseScript was found!");
        }

        return mob;
    }

    private void OnGet(GameObject mob)
    {


        if (mob.TryGetComponent<EnemyBaseScript>(out var enemyBase))
        {
            EnemyManagerScript.Instance.RegisterEnemy(mob, enemyBase.EnemyType);
            if (enemyBase.isGeneratedByPool && enemyBase._pool == null)
            {
                Debug.LogError($"{mob.name} pulled from pool but has no pool reference!");
            }

        }
        mob.SetActive(true);
    }

    //private IEnumerator SpawnRandomMobs()
    //{
    //    while (Instance.spawningMobsIsEnabled)
    //    {
    //        yield return new WaitForSeconds(Instance.mobsSpawningTime);
    //        // Choose pool index
    //        int index = Random.Range(0, allowedLength > 0 ? allowedLength : enemyPools.Count);
    //        Vector2 position = new(Random.Range(-20f, 20f), Random.Range(-17f, 17f));
    //        StartCoroutine(SpawnIndicatorThenEnemy(enemyPools[index], position));
    //    }
    //}

    private IEnumerator SpawnSpecificMobs(EnemyTypes enemyType, float maxWidth = 20f, float minWidth = -20f, float maxHeight = 17f, float minHeight = -17f)
    {
        Vector2 position = new(Random.Range(minWidth, maxWidth), Random.Range(minHeight, maxHeight));
        StartCoroutine(SpawnIndicatorThenEnemy(enemyPools[enemyType], position, false));
        while (Instance.spawningMobsIsEnabled)
        {
            if (Instance.aliveEnemies >= maxAliveEnemies)
            {
                yield return new WaitForSeconds(1f);
                continue;
            }
            position = new(Random.Range(minWidth, maxWidth), Random.Range(minHeight, maxHeight));
            float spawningTime = GetSpawnTime(enemyType);
            yield return new WaitForSeconds(spawningTime);
            StartCoroutine(SpawnIndicatorThenEnemy(enemyPools[enemyType], position, false));
        }
    }

    private float GetSpawnTime(EnemyTypes enemyType)
    {
        return enemyType switch
        {
            EnemyTypes.Level1Skeleton => currentSkeletonSpawnTime,
            EnemyTypes.BlackArmoredSkeleton => currentBlackArmoredSkeletonSpawnTime,
            EnemyTypes.WhiteArmoredSkeleton => currentWhiteArmoredSkeletonSpawnTime,
            EnemyTypes.SkeletonArcher => currentSkeletonArchersSpawnTime,
            EnemyTypes.VampireNormal => vampireNormalSpawningTime,
            EnemyTypes.VampireBoss => vampireBossSpawningTimeEndlessMode,
            _ => spawnTimeDefault
        };
    }

    private IEnumerator SpawnIndicatorThenEnemy(ObjectPool<GameObject> pool, Vector2 position, bool isBoss = false)
    {
        if (isBoss && spawnBossIndicatorPrefab == null)
        {
            Debug.LogError("spawnBossIndicatorPrefab is not assigned!");
            yield break;
        }

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

        while (Instance.isSpawningCollectablePets && !token.IsCancellationRequested && Instance.spawnedPets < maxAllowedSpawnedCats)
        {
            await UniTask.Delay(
                (int)(collectableSpawnDelay * 1000),
                DelayType.DeltaTime,
                PlayerLoopTiming.Update,
                token
            );
            Instance.spawnedPets++;
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
