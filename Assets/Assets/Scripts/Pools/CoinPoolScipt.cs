using UnityEngine;

public class CoinPool : BasePool<CoinPool>
{
    public GameObject GetCoin()
    {
        return Get();
    }

    public void ReleaseCoin(GameObject coin)
    {
        Release(coin);
    }
}