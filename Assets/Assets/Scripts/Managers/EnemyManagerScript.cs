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

    public void RegisterEnemy(GameObject enemy)
    {
        ActiveEnemies.Add(enemy);
    }

    public void UnregisterEnemy(GameObject enemy)
    {
        ActiveEnemies.Remove(enemy);
    }
}
