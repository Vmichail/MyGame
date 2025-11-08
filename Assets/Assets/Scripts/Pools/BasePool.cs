using UnityEngine;
using UnityEngine.Pool;

public abstract class BasePool<T> : MonoBehaviour where T : BasePool<T>
{
    public static T Instance { get; private set; }

    [SerializeField] protected GameObject prefab;
    [SerializeField] protected Transform parent;

    protected ObjectPool<GameObject> objectPool;

    protected virtual void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning($"Multiple {typeof(T).Name} instances found! Destroying duplicate.");
            Destroy(gameObject);
            return;
        }
        Instance = (T)this;

        objectPool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(prefab),
            actionOnGet: obj => obj.SetActive(true),
            actionOnRelease: obj => obj.SetActive(false),
            actionOnDestroy: Destroy,
            collectionCheck: false,
            defaultCapacity: 20
        );

        DontDestroyOnLoad(gameObject);
    }

    // Generic get method
    public virtual GameObject Get()
    {
        GameObject obj = objectPool.Get();

        if (parent != null)
            obj.transform.SetParent(parent, false);
        else
            Debug.LogWarning($"{typeof(T).Name} parent GameObject not found in scene.");

        return obj;
    }

    // Generic release method
    public virtual void Release(GameObject obj)
    {
        objectPool.Release(obj);
    }

}