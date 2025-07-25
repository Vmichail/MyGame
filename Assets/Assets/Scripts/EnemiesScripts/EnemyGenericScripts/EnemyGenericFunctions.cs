using UnityEngine;

public static class EnemyGenericFunctions
{
    static float minDamage = 1f; // Always deal at least 1 damage

    public static float DamagePlayer(float damage)
    {
        float armor = GlobalVariables.Instance.playerArmor;
        float damageMultiplier = 1 - ((0.06f * armor) / (1 + 0.06f * Mathf.Abs(armor)));
        float finalDamage = damage * damageMultiplier;

        GlobalVariables.Instance.playerCurrentHealth -= finalDamage;

        Debug.Log($"Incoming: {damage}, Armor: {armor}, Final: {finalDamage}");
        return finalDamage;
    }
}