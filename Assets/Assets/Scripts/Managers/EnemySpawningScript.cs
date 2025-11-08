using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

public class EnemySpawningScript : MonoBehaviour
{
    /*
     * 0 -> Skeleons
     * 1 -> Skeleton archer(Boss)
     * 2 -> Vampire type 3
     * 3 -> Vampire typ2 2(Boss)
     */
    [SerializeField] private GameObject[] enemyPrefabs;

    [SerializeField] private Transform parent;
    [SerializeField] private int allowedLength;
    [SerializeField] private GameObject spawnIndicatorPrefab;
    [SerializeField] private Transform[] specificEnemyPosition;
    [SerializeField] private Light2D globalLight;
    private int enemyUpgradeCounter = 0;

    private List<ObjectPool<GameObject>> enemyPools = new();
    [Header("Collectable Pets")]
    [SerializeField] private Transform collectableParent;
    [SerializeField] private GameObject collectableCat;
    [SerializeField] private Transform[] collectableCatSpawnPotitions;
    [SerializeField] private float collectableSpawnDelay = 20f;
    void Start()
    {
        foreach (var prefab in enemyPrefabs)
        {
            var pool = new ObjectPool<GameObject>(
                () => CreateEnemy(prefab),
                mob => OnGet(mob),
                mob => mob.SetActive(false),
                mob => Destroy(mob),
                false,
                10,
                100
            );

            enemyPools.Add(pool);
        }
        StartCoroutine(SpawnSpecificMobs(0));

        // Start async spawning loop for collectable cats
        SpawnCollectableCatsLoop().Forget();

    }

    public void SpawnMobsOnSpecificPosition(Transform[] mobPositions, int index)
    {

        if (mobPositions == null)
        {
            mobPositions = specificEnemyPosition;
        }

        if (mobPositions == null || mobPositions.Length == 0)
        {
            return;
        }

        LTSeq seq = LeanTween.sequence();

        foreach (Transform pos in mobPositions)
        {
            if (pos == null) continue;

            // Add 0.3s delay before each spawn
            seq.append(0.3f);
            // Add the spawn action
            seq.append(() =>
            {
                SpawnSpecificEnemyWithEffect(enemyPools[index], pos.position);
            });
        }
    }


    private void SpawnBoss()
    {
        if (!GlobalVariables.Instance.level1BossActive)
        {
            AudioManager.Instance.PlayMusic(3);
            GlobalVariables.Instance.level1BossActive = true;
            GlobalVariables.Instance.spawningMobsIsEnabled = false;
            globalLight.intensity = 0.8f;
            globalLight.color = new Color(1f, 0.7f, 0.7f);
            StartCoroutine(SpawnIndicatorThenEnemy(enemyPools[3], new Vector2(0, 0)));
        }
    }

    private void SpawnSpecificEnemyWithEffect(ObjectPool<GameObject> pool, Vector2 position)
    {
        AudioManager.Instance.PlaySoundFX("sharp-pop", transform.position, 0.4f, 0.75f, 1.25f);
        CinemachineScript.Instance.Shake(0.15f, 0.15f);
        StartCoroutine(SpawnIndicatorThenEnemy(pool, position));
    }

    private void Update()
    {
        //Skeleton Archer spawn
        if (GlobalVariables.Instance.spawnedSkeletons > 1 && GlobalVariables.Instance.skeletonArchersEnabled == false)
        {
            GlobalVariables.Instance.skeletonArchersEnabled = true;
            StartCoroutine(SpawnSpecificMobs(1));
        }


        if (GlobalVariables.Instance.gameTime > GlobalVariables.Instance.upgradeEnemiesTimer && GlobalVariables.Instance.vampiresType3Enabled == false)
        {
            GlobalVariables.Instance.vampiresType3Enabled = true;
            StartCoroutine(SpawnSpecificMobs(2));
        }

        if (GlobalVariables.Instance.gameTime >= GlobalVariables.Instance.upgradeEnemiesTimer)
        {
            if (enemyUpgradeCounter > 3)
            {
                SpawnBoss();
            }
            UpgradeEnemies();
            GlobalVariables.Instance.upgradeEnemiesTimer += GlobalVariables.Instance.upgradeEnemiesTimerIncreaseValue;
        }
    }

    private GameObject CreateEnemy(GameObject prefab)
    {
        GameObject mob = Instantiate(prefab, parent);

        if (mob.TryGetComponent<EnemyBaseScript>(out var enemyBase))
        {
            // Assign pool so the mob can return itself
            enemyBase.SetPool(enemyPools[(int)enemyBase.EnemyType]);
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

    private IEnumerator SpawnRandomMobs()
    {
        while (GlobalVariables.Instance.spawningMobsIsEnabled)
        {
            yield return new WaitForSeconds(GlobalVariables.Instance.mobsSpawningTime);
            // Choose pool index
            int index = Random.Range(0, allowedLength > 0 ? allowedLength : enemyPools.Count);
            Vector2 position = new(Random.Range(-20f, 20f), Random.Range(-17f, 17f));
            StartCoroutine(SpawnIndicatorThenEnemy(enemyPools[index], position));
        }
    }

    private IEnumerator SpawnSpecificMobs(int index, float maxWidth = 20f, float minWidth = -20f, float maxHeight = 17f, float minHeight = -17f)
    {
        Vector2 position = new(Random.Range(minWidth, maxWidth), Random.Range(minHeight, maxHeight));
        StartCoroutine(SpawnIndicatorThenEnemy(enemyPools[index], position));
        float spawningTime = GlobalVariables.Instance.spawnTime;
        if (index == 0)
        {
            spawningTime = GlobalVariables.Instance.skeletonsSpawningTime;
        }
        else if (index == 1)
        {
            spawningTime = GlobalVariables.Instance.skeletonArchersSpawningTime;
        }
        else if (index == 2)
        {
            spawningTime = GlobalVariables.Instance.vampireType3SpawningTime;
        }
        while (GlobalVariables.Instance.spawningMobsIsEnabled)
        {
            position = new(Random.Range(minWidth, maxWidth), Random.Range(minHeight, maxHeight));
            yield return new WaitForSeconds(spawningTime);
            StartCoroutine(SpawnIndicatorThenEnemy(enemyPools[index], position));
        }
    }

    private IEnumerator SpawnIndicatorThenEnemy(ObjectPool<GameObject> pool, Vector2 position)
    {
        GameObject indicator = Instantiate(spawnIndicatorPrefab, position, Quaternion.identity, parent);
        SpawnIndicator indicatorScript = indicator.GetComponent<SpawnIndicator>();

        yield return new WaitUntil(() => indicatorScript.IsReadyToSpawn);

        Destroy(indicator);

        GameObject mob = pool.Get();
        mob.transform.position = position;
    }


    private void UpgradeEnemies()
    {
        enemyUpgradeCounter++;
        //Skeleton
        GlobalVariables.Instance.skeletonHealth += 1;
        GlobalVariables.Instance.skeletonDamage += 1;
        //Skeleton Archer
        GlobalVariables.Instance.skeletonArcherHealth += 1;
        GlobalVariables.Instance.skeletonArcherAttackCooldown -= 0.1f;
        //Vampire Type 3
        GlobalVariables.Instance.vampireType3Health += 5;
    }


    private async UniTaskVoid SpawnCollectableCatsLoop()
    {
        // Cancel automatically if GameObject destroyed
        var token = this.GetCancellationTokenOnDestroy();

        while (GlobalVariables.Instance.isSpawningCollectablePets && !token.IsCancellationRequested)
        {
            await UniTask.Delay(
                (int)(collectableSpawnDelay * 1000),
                DelayType.DeltaTime,
                PlayerLoopTiming.Update,
                token
            );

            if (collectableCat == null || collectableCatSpawnPotitions == null || collectableCatSpawnPotitions.Length == 0)
                continue;

            Transform randomPos = collectableCatSpawnPotitions[Random.Range(0, collectableCatSpawnPotitions.Length)];

            if (randomPos == null)
                continue;

            GameObject collactableCat = CollectableCatPool.Instance.GetCat();
            if (collactableCat.TryGetComponent(out CollectablePetScript collectablePetScript))
            {
                collectablePetScript.Initialize(randomPos.position);
            }
            else
            {
                Debug.LogWarning("CollectablePetScript component not found on the CollectableCat prefab!");
            }

            // Optional: play effect or sound
            //AudioManager.Instance.PlaySoundFX("spawn-puff", randomPos.position, 0.6f, 0.9f, 1.1f, applyDistance: true);
            // Optional: small visual feedback
            //CinemachineScript.Instance.Shake(0.1f, 0.1f);
        }
    }
}
