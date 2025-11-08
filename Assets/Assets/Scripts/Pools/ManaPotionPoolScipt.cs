using UnityEngine;

public class ManaPotionPool : BasePool<ManaPotionPool>
{
    public GameObject GetManaPotion()
    {
        return Get();
    }

    public void ReleaseManaPotion(GameObject potion)
    {
        Release(potion);
    }
}