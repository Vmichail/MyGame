using UnityEngine;
using System.Collections;
using UnityEngine.Pool;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class EnemySpawningScript : MonoBehaviour
{
    [SerializeField] private EnemyTypesScriptableObject[] enemyTypes;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] Transform parent;

    private Dictionary<GlobalVariables.EnemyTypes, ObjectPool<GameObject>> enemyPools;

    void Start()
    {
        enemyPools = new Dictionary<GlobalVariables.EnemyTypes, ObjectPool<GameObject>>();

        foreach (var entry in enemyTypes)
        {
            var type = entry.type;
            var prefab = entry.prefab;

            enemyPools[type] = new ObjectPool<GameObject>(
                () => CreateFunction(prefab, type),
                mob => ActionOnGet(mob),
                mob => ActionOnRelease(mob),
                mob => ActionOnDestroy(mob),
                false,
                10,
                50
            );
        }

        StartCoroutine(SpawnMobs());
    }

    private GameObject CreateFunction(GameObject prefab, GlobalVariables.EnemyTypes type)
    {
        GameObject mob = Instantiate(prefab);
        mob.transform.SetParent(parent, false);

        if (mob.TryGetComponent<EnemyBaseScript>(out var enemyBase))
        {
            enemyBase.SetPool(enemyPools[type]);
        }

        return mob;
    }

    private void ActionOnGet(GameObject mob)
    {
        if (mob.TryGetComponent<EnemyBaseScript>(out var enemyBase))
        {
            enemyBase.RestoreHealth();
            enemyBase.isDead = false;
            enemyBase.knockBackEffect = false;
        }
        mob.SetActive(true);
    }

    private void ActionOnRelease(GameObject mob)
    {
        mob.SetActive(false);
    }

    private void ActionOnDestroy(GameObject mob)
    {
        Destroy(mob);
    }

    IEnumerator SpawnMobs()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);

            var entry = enemyTypes[Random.Range(0, enemyTypes.Length)];
            var pool = enemyPools[entry.type];

            GameObject mob = pool.Get();
            Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];

            Vector2 spawnPositionAdjustment = Mathf.Abs(spawn.position.y) > Mathf.Abs(spawn.position.x)
                ? new Vector2(Random.Range(-17f, 17f), 0)
                : new Vector2(0, Random.Range(-9.5f, 9.5f));

            mob.transform.position = (Vector2)spawn.position + spawnPositionAdjustment;
        }
    }
}
