using UnityEngine;

public class GlobalVariables
{
    //Player Stats
    public static float criticalChance = 0.1f;
    public static float criticalMultiplier = 1f;
    public static float defaultEnemySpeed = 1.5f;
    public static float defaultKnockbackResistance = 10f;
    public static float defaultEnemyHealth = 9f;
    public static float defaultSpellDamage = 4f;
    public static float defaultSpellSpeed = 4f;
    public static int defaultSpellBounces = 2;
    public static float defaultKnockbackforce = 10f;
    //Level1Skeleton
    public static float level1SkeletonSpeed = 3f;
    public static float level1SkeletonHealth = 250f;
    public static float level1SkeletonKnockbackResistance = 5f;
    //Level1Fireball
    public static float level1FireballSpeed = 5f;
    public static float level1FireballDamage = 5f;
    public static int level1FireballBounces = 1;
    public static float level1FireballKnockbackForce = 12f;
    //Level1Darkball
    public static bool DarkballEnabled = true;
    public static float level1DarkballSpeed = 10f;
    public static float level1DarkballDamage = 4f;
    public static int level1DarkballBounces = 4;
    public static float level1DarkballKnockbackForce = 8f;

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
}
