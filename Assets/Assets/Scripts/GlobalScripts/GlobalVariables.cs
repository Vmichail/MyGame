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

    //GreenMultiplier
    public float greenHealthMultiplier = 3f;
    public float greenAttackMultiplier = 2f;
    public float greenScaleMultiplier = 1.2f;
    public float greenKnockbackMultiplier = 1.2f;
    //Player Collectables
    public int coinsCollected = 100;
    public int permanentCoinsCollected = 100;
    public int diamondsCollected = 100;
    //Player Stats
    public float globalCriticalChance = 0.1f;
    public float globalCriticalMultiplier = 1f;
    public float playerMaxHealth = 100;
    public float playerCurrentHealth = 100;
    public float playerAttackSpeed = 1;
    //Default Values
    public float defaultEnemySpeed = 1.5f;
    public float defaultKnockbackResistance = 10f;
    public float defaultEnemyHealth = 9f;
    public float defaultSpellDamage = 4f;
    public float defaultSpellSpeed = 4f;
    public int defaultSpellBounces = 0;
    public float defaultKnockbackforce = 10f;
    public Color defaultColor = Color.yellow;
    //Level1Skeleton
    public float skeletonSpeed = 3f;
    public float skeletonHealth = 250f;
    public float skeletonKnockbackResistance = 5f;
    public float skeletonDamage = 1f;
    public float skeletonAttackCooldown = 2f;
    public Color skeletonDefaultColor = Color.white;
    //Level1Tortch
    public float goblinTorchSpeed = 3f;
    public float goblinTorchHealth = 250f;
    public float goblinTorchKnockbackResistance = 5f;
    public float goblinDamage = 2f;
    public float goblinAttackCooldown = 2f;
    public Color goblinTorchDefaultColor = Color.white;
    //Level1Fireball
    public float fireballSpeed = 5f;
    public float fireballDamage = 5f;
    public int fireballBounces = 1;
    public float fireballKnockbackForce = 15f;
    public float fireballCriticalChance = 0.5f;
    public float fireballCriticalMultiplier = 1f;
    public Color fireballDefaultColor = new(255, 255, 0);
    //Level1Darkball
    public float darkballSpeed = 8f;
    public float darkballDamage = 4f;
    public int darkballBounces = 2;
    public float darkballKnockbackForce = 10f;
    public float darkballCriticalChance = 0.7f;
    public float darkballCriticalMultiplier = 1f;
    public Color darkballDefaultColor = Color.cyan;
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
    //EnemySpawner
    public float enemyScore = 0;
    public float spawnTime = 2f;

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    public enum EnemyRarity
    {
        None,
        Green,
        Purple,
        Orange,
    }
}
