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
    [Header("Player Collectables")]
    public int coinsCollected = 100;
    public int permanentCoinsCollected = 100;
    public int diamondsCollected = 100;
    public int yellowCoinValue = 1;
    //Player Stats
    [Header("Player Stats")]
    public float playerAttackDamage = 1;
    public float globalCriticalChance = 0.1f;
    public float globalCriticalMultiplier = 1f;
    public float playerMaxHealth = 100;
    public float playerCurrentHealth = 100;
    public float playerAttackSpeed = 1;
    //GreenMultiplier
    public float greenHealthMultiplier = 3f;
    public float greenAttackMultiplier = 2f;
    public float greenScaleMultiplier = 1.2f;
    public float greenKnockbackMultiplier = 1.2f;

    //Default Values
    public float defaultEnemySpeed = 1.5f;
    public float defaultKnockbackResistance = 10f;
    public float defaultEnemyHealth = 9f;
    public float defaultSpellDamage = 1f;
    public float defaultSpellSpeed = 4f;
    public int defaultSpellBounces = 0;
    public float defaultKnockbackforce = 10f;
    public Color defaultColor = Color.yellow;
    [Header("--!!Enemies!!--")]
    [Header("Skeleton")]
    public float skeletonSpeed = 3f;
    public float skeletonHealth = 250f;
    public float skeletonKnockbackResistance = 5f;
    public float skeletonDamage = 1f;
    public float skeletonAttackCooldown = 2f;
    public float skeleonCoinDropChance = 0.1f;
    public CoinDropEnum skeletonCoinEnum = CoinDropEnum.Yellow;
    public Color skeletonDefaultColor = Color.white;
    [Header("GoblinTortch")]
    public float goblinTorchSpeed = 3f;
    public float goblinTorchHealth = 250f;
    public float goblinTorchKnockbackResistance = 5f;
    public float goblinDamage = 2f;
    public float goblinAttackCooldown = 2f;
    public float goblinTorchCoinDropChance = 0.1f;
    public CoinDropEnum goblinCoinEnum = CoinDropEnum.Red;
    public Color goblinTorchDefaultColor = Color.white;
    [Header("GoblinTNT")]
    public float goblinTNTSpeed = 3f;
    public float goblinTNTHealth = 250f;
    public float goblinTNTKnockbackResistance = 5f;
    public float goblinTNTDamage = 2f;
    public float goblinTNTAttackCooldown = 2f;
    public float goblinTNTCoinDropChance = 0.1f;
    public CoinDropEnum goblinTNTCoinEnum = CoinDropEnum.Red;
    public Color goblinTNTDefaultColor = Color.white;
    public float goblinTNTProjectileSpeed = 10f;
    [Header("!!Spells!!")]
    [Header("Spell-FireBall")]
    public float fireballSpeed = 5f;
    public float fireballDamage = 1.1f;
    public int fireballBounces = 1;
    public float fireballKnockbackForce = 15f;
    public float fireballCriticalChance = 0.5f;
    public float fireballCriticalMultiplier = 1f;
    public Color fireballDefaultColor = new(255, 255, 0);
    [Header("Spell-DarkBall")]
    public float darkballSpeed = 8f;
    public float darkballDamageMulti = 1.1f;
    public int darkballBounces = 2;
    public float darkballKnockbackForce = 10f;
    public float darkballCriticalChance = 0.7f;
    public float darkballCriticalMultiplier = 1f;
    public Color darkballDefaultColor = Color.cyan;
    [Header("Spell-Blade")]
    public float bladeSpeed = 12f;
    public float bladeDamageMulti = 1.1f;
    public int bladeBounces = 3;
    public float bladeKnockbackForce = 20f;
    public float bladeCriticalChance = 0.7f;
    public float bladeCriticalMultiplier = 1f;
    public Color bladeDefaultColor = Color.gray;
    [Header("Spell-PoisonCircle")]
    public float poisonCircleSpeed = 14f;
    public float poisonCircleDamageMulti = 1.1f;
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

    public enum CoinDropEnum
    {
        None,
        Yellow,
        Red,
        Diamond,
    }

    public enum EnemyTypes
    {
        Level1Skeleton,
        GoblinTourch,
        GoblinTNT,
    }

}
