using UnityEngine;
using System.Collections;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public class EnemySpawningScript : MonoBehaviour
{
    [SerializeField] GameObject mobPrefab;
    [SerializeField] Transform[] spawnPoints;

    private ObjectPool<GameObject> mobPool;

    void Start()
    {
        mobPool = new ObjectPool<GameObject>(
            CreateFunction,
            ActionOnGet,
           ActionOnRelease,
            ActionOnDestroy,
            false,
            10,
            50
        );

        StartCoroutine(SpawnMobs());
    }

    private GameObject CreateFunction()
    {
        GameObject mob = Instantiate(mobPrefab);

        if (mob.TryGetComponent<EnemyBaseScript>(out var enemyBase))
        {
            enemyBase.SetPool(mobPool);
        }

        return mob;
    }

    private void ActionOnGet(GameObject mob)
    {
        if (mob.TryGetComponent<EnemyBaseScript>(out var enemyBase))
        {
            enemyBase.RestoreHealth();
        }
        mob.SetActive(true);
    }

    private void ActionOnRelease(GameObject mob)
    {
        // Optional: Reset position, animation, velocity, etc.
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

            Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject mob = mobPool.Get();
            Vector2 spawnPositionAdjustment = Vector2.zero;

            if (Mathf.Abs(spawn.position.y) > Mathf.Abs(spawn.position.x))
            {
                spawnPositionAdjustment = new Vector2(Random.Range(-17f, 17f), 0);
            }
            else
            {
                spawnPositionAdjustment = new Vector2(0, Random.Range(-9.5f, 9.5f));
            }
            mob.transform.position = (Vector2)spawn.position + spawnPositionAdjustment;

        }
    }

}
