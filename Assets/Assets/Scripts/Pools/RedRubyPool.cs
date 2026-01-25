using UnityEngine;

public class RedRubyPool : BasePool<RedRubyPool>
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