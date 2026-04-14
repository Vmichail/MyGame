using System.Collections.Generic;
using UnityEngine;
public enum PoolCategory
{
    None,
    Player,
    Enemy,
    Particles
}
public class PoolManager : MonoBehaviour
{
    [Header("Pool Roots")]
    [SerializeField] private Transform playerPoolsRoot;
    [SerializeField] private Transform enemyPoolsRoot;
    [SerializeField] private Transform particlesPoolsRoot;


    private Transform GetRoot(PoolCategory category)
    {
        return category switch
        {
            PoolCategory.Player => playerPoolsRoot,
            PoolCategory.Enemy => enemyPoolsRoot,
            PoolCategory.Particles => particlesPoolsRoot,
            _ => null
        };
    }
    public static PoolManager Instance;

    private Dictionary<GameObject, Queue<GameObject>> pools = new();
    private Dictionary<GameObject, Transform> prefabParents = new();
    private Dictionary<GameObject, GameObject> instanceToPrefab = new();

    private void Awake()
    {
        Instance = this;
    }

    public GameObject Get(GameObject prefab, Vector3 position, Quaternion rotation, PoolCategory category = PoolCategory.None)
    {
        if (!pools.ContainsKey(prefab))
        {
            pools[prefab] = new Queue<GameObject>();

            Transform root = GetRoot(category);

            if (root != null)
            {
                GameObject container = new($"[{prefab.name}]_Pool");

                container.transform.SetParent(root);

                prefabParents[prefab] = container.transform;
            }

            Debug.Log($"Created pool for {prefab.name}");
        }

        GameObject obj;

        if (pools[prefab].Count > 0)
        {
            obj = pools[prefab].Dequeue();
        }
        else
        {
            Transform parent = null;

            if (prefabParents.ContainsKey(prefab))
                parent = prefabParents[prefab];

            obj = Instantiate(prefab, parent);

            instanceToPrefab[obj] = prefab;
        }

        obj.transform.SetPositionAndRotation(position, rotation);
        obj.SetActive(true);

        return obj;
    }

    public void Return(GameObject obj)
    {
        if (!instanceToPrefab.ContainsKey(obj))
        {
            Debug.LogError($"PoolManager: Unknown object returned: {obj.name}");
            return;
        }

        GameObject prefab = instanceToPrefab[obj];

        obj.SetActive(false);

        pools[prefab].Enqueue(obj);
    }
}