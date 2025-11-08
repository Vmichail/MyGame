using Unity.Android.Types;
using UnityEngine;

public class CollectableCatPool : BasePool<CollectableCatPool>
{
    public GameObject GetCat()
    {
        return Get();
    }

    public void ReleaseCat(GameObject potion)
    {
        Release(potion);
    }
}