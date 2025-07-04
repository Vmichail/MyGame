using UnityEngine;
using UnityEngine.Pool;

public class CoinPoolScript : MonoBehaviour
{
    public static CoinPoolScript Instance;

    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private Transform parent;

    private ObjectPool<GameObject> coinPool;

    void Awake()
    {
        //Singleton
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Multiple CoinPool instances found! Destroying duplicate.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        coinPool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(coinPrefab),
            actionOnGet: coin => coin.SetActive(true),
            actionOnRelease: coin => coin.SetActive(false),
            actionOnDestroy: Destroy,
            collectionCheck: false,
            defaultCapacity: 20
        );
        DontDestroyOnLoad(gameObject);
    }


    public GameObject GetCoin()
    {
        GameObject coin = coinPool.Get();

        if (parent != null)
        {
            coin.transform.SetParent(parent, false);
        }
        else
        {
            Debug.LogWarning("Coins parent GameObject not found in scene.");
        }

        return coin;
    }

    public void ReleaseCoin(GameObject coin)
    {
        coinPool.Release(coin);
    }
}
