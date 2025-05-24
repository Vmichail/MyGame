using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    public static GlobalVariables Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Prevent duplicates
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Optional: persist across scenes
    }


    //Player Stats
    public static float globalCriticalChance = 0.1f;
    public static float globalCriticalMultiplier = 1f;
    public static float defaultEnemySpeed = 1.5f;
    public static float defaultKnockbackResistance = 10f;
    public static float defaultEnemyHealth = 9f;
    public static float defaultSpellDamage = 4f;
    public static float defaultSpellSpeed = 4f;
    public static int defaultSpellBounces = 0;
    public static float defaultKnockbackforce = 10f;
    public Color defaultColor = Color.yellow;
    //Level1Skeleton
    public static float level1SkeletonSpeed = 3f;
    public static float level1SkeletonHealth = 250f;
    public static float level1SkeletonKnockbackResistance = 5f;
    public Color level1SkeletonDefaultColor = Color.white;
    //Level1Fireball
    public static float level1FireballSpeed = 5f;
    public static float level1FireballDamage = 5f;
    public static int level1FireballBounces = 1;
    public static float level1FireballKnockbackForce = 15f;
    public static float level1FireballCriticalChance = 0.5f;
    public static float level1FireballCriticalMultiplier = 1f;
    public Color level1FireballDefaultColor = new(255, 255, 0);
    //Level1Darkball
    public static float level1DarkballSpeed = 8f;
    public static float level1DarkballDamage = 4f;
    public static int level1DarkballBounces = 2;
    public static float level1DarkballKnockbackForce = 10f;
    public static float level1DarkballCriticalChance = 0.7f;
    public static float level1DarkballCriticalMultiplier = 1f;
    public Color level1DarkBallDefaultColor = Color.cyan;
    //Blade
    public float bladeSpeed = 12f;
    public float bladeDamage = 4f;
    public int bladeBounces = 3;
    public float bladeKnockbackForce = 20f;
    public float bladeCriticalChance = 0.7f;
    public float bladeCriticalMultiplier = 1f;
    public Color bladeDefaultColor = Color.gray;
    //PoisonCircle
    public float poisonCircleSpeed = 14f;
    public float poisonCircleDamage = 4f;
    public int poisonCircleBounces = 2;
    public float poisonCircleKnockbackForce = 20f;
    public float poisonCircleCriticalChance = 0.7f;
    public float poisonCircleCriticalMultiplier = 1f;
    public Color poisonCircleDefaultColor = Color.green;
    //
    public bool SecondSpellEnabled = true;
    public bool ThirdSpellEnabled = true;
    public bool ForthSpellEnabled = true;

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
}
