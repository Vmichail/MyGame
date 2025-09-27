using System.Collections.Generic;
using UnityEngine;

public class EnemyManagerScript : MonoBehaviour
{
    public static EnemyManagerScript Instance;

    public List<GameObject> ActiveEnemies { get; private set; } = new List<GameObject>();

    void Awake()
    {
        Instance = this;
    }

    public void RegisterEnemy(GameObject enemy, GlobalVariables.EnemyTypes enemyType)
    {
        ActiveEnemies.Add(enemy);
        GlobalVariables.Instance.aliveEnemies++;
        AddSpecificType(enemyType);
    }

    public void UnregisterEnemy(GameObject enemy)
    {
        ActiveEnemies.Remove(enemy);
        GlobalVariables.Instance.aliveEnemies--;
        GlobalVariables.Instance.killedEnemies++;
        GlobalVariables.Instance.score++;
    }

    private void AddSpecificType(GlobalVariables.EnemyTypes enemyType)
    {
        if (GlobalVariables.EnemyTypes.Level1Skeleton.Equals(enemyType))
        {
            GlobalVariables.Instance.spawnedSkeletons++;
        }
        else if (GlobalVariables.EnemyTypes.SkeletonArcher.Equals(enemyType))
        {
            GlobalVariables.Instance.spawnedSkeletonArchers++;
        }
        else
        {
            Debug.Log("Uknown Enemy Type:" + enemyType);
        }
    }
}
