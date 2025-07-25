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
        CacheInitialValues();
        DontDestroyOnLoad(gameObject); // Optional: persist across scenes
    }
    [Header("Player Generic Values")]
    public bool playerIsAlive = true;
    public bool gameIsPaused = true;
    [Header("Player Collectables")]
    public int coinsCollected = 100;
    public int permanentCoinsCollected = 100;
    public int diamondsCollected = 100;
    public int yellowCoinValue = 1;
    //Player Stats
    [Header("Player Stats")]
    [Header("Player Attack")]
    public float playerAttackDamage = 1;
    public float globalCriticalChance = 0.1f;
    public float globalCriticalMultiplier = 1f;
    public float playerAttackSpeed = 1;
    public float playerAttackRangeBaseScale = 3;
    public float playerAttackRangeBase = 100;
    public float playerAttackRange = 100;
    [Header("Player Health")]
    public float playerMaxHealth = 100;
    public bool regenIsActive = true;
    public float playerHealthRegen = 1;
    public float playerHealthRegenInterval = 1;
    public float playerArmor = 1;
    public float playerCurrentHealth = 100;

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
    public float goblinTNTRange = 3f;
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

    //
    // BACKUP variables
    private bool _initialPlayerIsAlive;
    private bool _initialGameIsPaused;
    private int _initialCoinsCollected;
    private int _initialPermanentCoinsCollected;
    private int _initialDiamondsCollected;
    private int _initialYellowCoinValue;
    private float _initialPlayerAttackDamage;
    private float _initialGlobalCriticalChance;
    private float _initialGlobalCriticalMultiplier;
    private float _initialPlayerAttackSpeed;
    private float _initialPlayerAttackRangeBaseScale;
    private float _initialPlayerAttackRangeBase;
    private float _initialPlayerAttackRange;
    private float _initialPlayerMaxHealth;
    private float _initialPlayerCurrentHealth;
    private float _initialPlayerArmor;
    private bool _initialRegenIsActive;
    private float _initialPlayerHealthRegen;
    private float _initialPlayerHealthRegenInterval;
    private float _initialGreenHealthMultiplier;
    private float _initialGreenAttackMultiplier;
    private float _initialGreenScaleMultiplier;
    private float _initialGreenKnockbackMultiplier;
    private float _initialEnemyScore;
    private float _initialSpawnTime;
    private bool _initialSecondSpellEnabled;
    private bool _initialThirdSpellEnabled;
    private bool _initialForthSpellEnabled;

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

    private void CacheInitialValues()
    {
        _initialPlayerIsAlive = playerIsAlive;
        _initialGameIsPaused = gameIsPaused;
        _initialCoinsCollected = coinsCollected;
        _initialPermanentCoinsCollected = permanentCoinsCollected;
        _initialDiamondsCollected = diamondsCollected;
        _initialYellowCoinValue = yellowCoinValue;
        _initialPlayerAttackDamage = playerAttackDamage;
        _initialGlobalCriticalChance = globalCriticalChance;
        _initialGlobalCriticalMultiplier = globalCriticalMultiplier;
        _initialPlayerAttackSpeed = playerAttackSpeed;
        _initialPlayerAttackRangeBaseScale = playerAttackRangeBaseScale;
        _initialPlayerAttackRangeBase = playerAttackRangeBase;
        _initialPlayerAttackRange = playerAttackRange;
        _initialPlayerMaxHealth = playerMaxHealth;
        _initialPlayerCurrentHealth = playerCurrentHealth;
        _initialPlayerArmor = playerArmor;
        _initialRegenIsActive = regenIsActive;
        _initialPlayerHealthRegen = playerHealthRegen;
        _initialPlayerHealthRegenInterval = playerHealthRegenInterval;
        _initialGreenHealthMultiplier = greenHealthMultiplier;
        _initialGreenAttackMultiplier = greenAttackMultiplier;
        _initialGreenScaleMultiplier = greenScaleMultiplier;
        _initialGreenKnockbackMultiplier = greenKnockbackMultiplier;
        _initialEnemyScore = enemyScore;
        _initialSpawnTime = spawnTime;
        _initialSecondSpellEnabled = SecondSpellEnabled;
        _initialThirdSpellEnabled = ThirdSpellEnabled;
        _initialForthSpellEnabled = ForthSpellEnabled;
    }

    public void ResetValues()
    {
        playerIsAlive = _initialPlayerIsAlive;
        gameIsPaused = _initialGameIsPaused;
        coinsCollected = _initialCoinsCollected;
        permanentCoinsCollected = _initialPermanentCoinsCollected;
        diamondsCollected = _initialDiamondsCollected;
        yellowCoinValue = _initialYellowCoinValue;
        playerAttackDamage = _initialPlayerAttackDamage;
        globalCriticalChance = _initialGlobalCriticalChance;
        globalCriticalMultiplier = _initialGlobalCriticalMultiplier;
        playerAttackSpeed = _initialPlayerAttackSpeed;
        playerAttackRangeBaseScale = _initialPlayerAttackRangeBaseScale;
        playerAttackRangeBase = _initialPlayerAttackRangeBase;
        playerAttackRange = _initialPlayerAttackRange;
        playerMaxHealth = _initialPlayerMaxHealth;
        playerCurrentHealth = _initialPlayerCurrentHealth;
        playerArmor = _initialPlayerArmor;
        regenIsActive = _initialRegenIsActive;
        playerHealthRegen = _initialPlayerHealthRegen;
        playerHealthRegenInterval = _initialPlayerHealthRegenInterval;
        greenHealthMultiplier = _initialGreenHealthMultiplier;
        greenAttackMultiplier = _initialGreenAttackMultiplier;
        greenScaleMultiplier = _initialGreenScaleMultiplier;
        greenKnockbackMultiplier = _initialGreenKnockbackMultiplier;
        enemyScore = _initialEnemyScore;
        spawnTime = _initialSpawnTime;
        SecondSpellEnabled = _initialSecondSpellEnabled;
        ThirdSpellEnabled = _initialThirdSpellEnabled;
        ForthSpellEnabled = _initialForthSpellEnabled;
    }
}
