using UnityEngine;

public class GreenRubyPool : BasePool<GreenRubyPool>
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