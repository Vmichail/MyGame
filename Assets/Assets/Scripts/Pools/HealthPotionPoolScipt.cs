using UnityEngine;

public class HealthPotionPool : BasePool<HealthPotionPool>
{
    public GameObject GetHealthPotion()
    {
        return Get();
    }

    public void ReleaseHealthPotion(GameObject potion)
    {
        Release(potion);
    }
}