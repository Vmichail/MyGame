using System.Collections.Generic;
using UnityEngine;

public class MobPool : MonoBehaviour
{
    [SerializeField] GameObject mobPrefab;
    [SerializeField] int poolSize = 10;

    private List<GameObject> mobPool;

    void Awake()
    {
        mobPool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject mob = Instantiate(mobPrefab);
            mob.SetActive(false);
            mobPool.Add(mob);
        }
    }

    public GameObject GetMob()
    {
        foreach (GameObject mob in mobPool)
        {
            if (!mob.activeInHierarchy)
            {
                return mob;
            }
        }

        // Optionally expand the pool (optional)
        GameObject newMob = Instantiate(mobPrefab);
        newMob.SetActive(false);
        mobPool.Add(newMob);
        return newMob;
    }
}
