using UnityEngine;
using System.Collections;
using UnityEngine.Pool;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class EnemySpawningScript : MonoBehaviour
{
    /*
     * 0 -> Skeleons
     * 1 -> Skeleton archer(Boss)
     * 2 -> Vampire(Boss)
     */
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private Transform parent;
    [SerializeField] private int allowedLength;
    [SerializeField] private GameObject spawnIndicatorPrefab;

    private List<ObjectPool<GameObject>> enemyPools = new();

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

    }

    private void Update()
    {
        //Skeleton Archer spawn
        if (GlobalVariables.Instance.spawnedSkeletons > 1 && GlobalVariables.Instance.skeletonArchersEnabled == false)
        {
            GlobalVariables.Instance.skeletonArchersEnabled = true;
            StartCoroutine(SpawnSpecificMobs(1));
        }


        if (GlobalVariables.Instance.gameTime > GlobalVariables.Instance.upgradeEnemiesTimer)
        {
            StartCoroutine(SpawnSpecificMobs(2));
        }

        if (GlobalVariables.Instance.gameTime >= GlobalVariables.Instance.upgradeEnemiesTimer)
        {
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

    private IEnumerator SpawnSpecificMobs(int index)
    {
        Vector2 position = new(Random.Range(-20f, 20f), Random.Range(-17f, 17f));
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
            position = new(Random.Range(-20f, 20f), Random.Range(-17f, 17f));
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
        //Skeleton
        GlobalVariables.Instance.skeletonHealth += 1;
        GlobalVariables.Instance.skeletonDamage += 1;
        //Skeleton Archer
        GlobalVariables.Instance.skeletonArcherHealth += 1;
        GlobalVariables.Instance.skeletonArcherAttackCooldown -= 0.1f;
        //Vampire Type 3
        GlobalVariables.Instance.vampireType3Health += 5;
    }
}
