public static class EnemyGenericFunctions
{
    public static float DamagePlayer(float damage)
    {
        GlobalVariables.Instance.playerCurrentHealth -= damage;
        return damage;
    }
}