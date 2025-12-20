using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{

    public static GlobalVariables Instance { get; private set; }

    private void Update()
    {
        if (pauseReasonsList.Count == 0 && Time.timeScale != 1)
        {
            Time.timeScale = 1;
        }
        gameTime += Time.deltaTime;

    }
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Prevent duplicates
            return;
        }

        Instance = this;
        CacheInitialValues();
    }
    [Header("Sound-Music values")]
    public float masterVolume = 1f;
    public float musicVolume = 1f;
    public float SFXVolume = 1f;
    [Header("Sound-Music Generic Clip Names")]
    public string[] linaAnnouncementsClips = { "linaGame", "linaFireTime", "linaIamOnFire", "linaLetsGetAFireGoing" };
    public string[] gameOverClips = { "gameOver1", "gameOver2", "gameOver3", "gameOver4", "gameOver5" };
    [Header("Upgrade Generic Values")]
    public int maxRefreshBuyOptions = 1;
    public bool isUpgradeOption4Unlocked = false;
    public bool isUpgradeOption5Unlocked = false;
    public int upgradeStartingCost = 0;
    public int additionUpdateCost = 5;
    public int upgradeOption4UnlockPrice = 25;
    public int upgradeOption5UnlockPrice = 50;
    [Header("Player Generic Values")]
    public bool playerIsAlive = true;
    public List<InvulnerableReasonEnum> playerInvulnerableReasons = new();
    public List<PauseReasonEnum> pauseReasonsList = new();
    public bool gameIsPaused = false;
    public float gameTime = 0f;
    public float upgradeEnemiesTimer = 40f;
    public float upgradeEnemiesTimerIncreaseValue = 5f;
    [Header("Player Collectables")]
    public bool magnetIsActive = false;
    public bool isSpawningCollectablePets = true;
    public int catsCollected = 0;
    public float coinsCollected = 100;
    public int permanentCoinsCollected = 100;
    public int diamondsCollected = 100;
    public float yellowCoinValue = 1;
    public float greenRubyCoinValue = 10;
    public float greenRubyExpValue = 10;
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
    [Header("Player Health-Speed")]
    public float playerMaxHealth = 100;
    public float playerSpeed = 5;
    public bool healthRegenIsActive = true;
    public float playerHealthRegen = 1;
    public float playerHealthRegenInterval = 1;
    public float playerArmor = 1;
    public float playerCurrentHealth = 100;
    [Header("Exp and Mana Values")]
    public float shardExp = 0.5f;
    public float currentExp = 0;
    public float maxExp = 100;
    public int level = 1;
    public float playerCurrentMana = 100;
    public float playerMaxMana = 100;
    public bool manaRegenIsActive = true;
    public float playerManaRegenInterval = 1;
    public float playerManaRegen = 1;
    //Potions
    public float manaPotionMana = 10;
    public float healthPotionHealth = 20;


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
    public int defaultSpellPiercing = 0;
    public bool defaultSpellOnDestroyEffect = false;
    public float defaultKnockbackforce = 10f;
    public Color defaultColor = Color.yellow;
    [Header("--!!Enemies!!--")]
    [Header("--!!Generic Spawning Variables")]
    public bool spawningMobsIsEnabled = true;
    public float mobsSpawningTime = 2f;
    public float skeletonsSpawningTime = 2f;
    public float skeletonArchersSpawningTime = 2f;
    public float vampireType3SpawningTime = 15f;
    public float vampireMiniBossSpawningTime = 15f;
    public float skeletonKingBossSpawningTime = 15f;
    public float thirdBossSpawningTime = 15f;
    public int aliveEnemies = 0;
    public int killedEnemies = 0;
    public int score = 0;
    public int spawnedSkeletons = 0;
    public int spawnedSkeletonArchers = 0;
    public bool skeletonArchersEnabled = false;
    public bool vampiresType3Enabled = false;
    public bool level1BossActive = false;
    [Header("Skeleton")]
    public float skeletonSpeed = 3f;
    public float skeletonHealth = 250f;
    public float skeletonKnockbackResistance = 5f;
    public float skeletonDamage = 1f;
    public float skeletonAttackCooldown = 0.5f;
    public float skeleonCoinDropChance = 0.1f;
    public float skeletonHealthPotionChance = 0.05f;
    public float skeletonManaPotionChance = 0.01f;
    public CoinDropEnum skeletonCoinEnum = CoinDropEnum.Yellow;
    public Color skeletonDefaultColor = Color.white;
    public float skeletonMinExp = 1f;
    public float skeletonMaxExp = 10f;
    [Header("SkeletonArcher")]
    public float skeletonArcherSpeed = 3f;
    public float skeletonArcherHealth = 250f;
    public float skeletonArcherKnockbackResistance = 5f;
    public float skeletonArcherDamage = 2f;
    public float skeletonArcherAttackCooldown = 1f;
    public float skeletonArcherCoinDropChance = 0.1f;
    public float skeletonArcherHealthPotionChance = 0.1f;
    public float skeletonArcherManaPotionChance = 0.02f;
    public CoinDropEnum skeletonArcherCoinEnum = CoinDropEnum.Red;
    public Color skeletonArcherDefaultColor = Color.white;
    public float skeletonArcherProjectileSpeed = 10f;
    public float skeletonArcherRange = 10f;
    public float skeletonArcherExp = 20f;
    public float skeletonArcherMultipleAttackChance = 0.9f;
    [Header("Vampire")]
    public float vampireType3Speed = 3f;
    public float vampireType3Health = 250f;
    public float vampireType3KnockbackResistance = 5f;
    public float vampireType3Damage = 2f;
    public float vampireType3AttackCooldown = 2f;
    public float vampireType3CoinDropChance = 0.8f;
    public float vampireType3HealthPotionChance = 0.5f;
    public float vampireType3ManaPotionChance = 0.5f;
    public CoinDropEnum vampireType3CoinEnum = CoinDropEnum.Red;
    public Color vampireType3DefaultColor = Color.white;
    public float vampireType3ProjectileSpeed = 10f;
    public float vampireType3Range = 10f;
    public float vampireType3Exp = 20f;
    [Header("Vampire Type 2")]
    public float vampireType2Speed = 3f;
    public float vampireType2Health = 250f;
    public float vampireType2KnockbackResistance = 5f;
    public float vampireType2Damage = 2f;
    public float vampireType2AttackCooldown = 2f;
    public float vampireType2CoinDropChance = 1f;
    public float vampireType2HealthPotionChance = 1f;
    public float vampireType2ManaPotionChance = 1f;
    public CoinDropEnum vampireType2CoinEnum = CoinDropEnum.Red;
    public Color vampireType2DefaultColor = Color.white;
    public float vampireType2ProjectileSpeed = 10f;
    public float vampireType2Range = 10f;
    public float vampireType2Exp = 20f;
    public float vampireType2MultipleAttackChance = 0.3f;
    [Header("SkeletonKing")]
    public float skeletonKingSpeed = 3f;
    public float skeletonKingHealth = 250f;
    public float skeletonKingKnockbackResistance = 5f;
    public float skeletonKingDamage = 2f;
    public float skeletonKingAttackCooldown = 2f;
    public float skeletonKingCoinDropChance = 1f;
    public float skeletonKingHealthPotionChance = 1f;
    public float skeletonKingManaPotionChance = 1f;
    public CoinDropEnum skeletonKing2CoinEnum = CoinDropEnum.Red;
    public Color skeletonKingDefaultColor = Color.white;
    public float skeletonKingProjectileSpeed = 16f;
    public float skeletonKingRange = 8f;
    public float skeletonKingExp = 20f;
    public float skeletonKingMultipleAttackChance = 0.3f;
    [Header("GoblinTorch")]
    public float goblinTorchSpeed = 3f;
    public float goblinTorchHealth = 250f;
    public float goblinTorchKnockbackResistance = 5f;
    public float goblinTorchDamage = 2f;
    public float goblinTorchAttackCooldown = 2f;
    public float goblinTorchCoinDropChance = 0.7f;
    public float goblinTorchHealthPotionChance = 0.5f;
    public float goblinTorchManaPotionChance = 0.5f;
    public CoinDropEnum goblinTorchCoinEnum = CoinDropEnum.Red;
    public Color goblinTorchDefaultColor = Color.white;
    public float goblinTorchExp = 10f;
    [Header("GoblinTNT")]
    public float goblinTNTSpeed = 3f;
    public float goblinTNTHealth = 250f;
    public float goblinTNTKnockbackResistance = 5f;
    public float goblinTNTDamage = 2f;
    public float goblinTNTAttackCooldown = 2f;
    public float goblinTNTCoinDropChance = 0.6f;
    public float goblinTNTHealthPotionChance = 0.5f;
    public float goblinTNTManaPotionChance = 0.5f;
    public CoinDropEnum goblinTNTCoinEnum = CoinDropEnum.Red;
    public Color goblinTNTDefaultColor = Color.white;
    public float goblinTNTProjectileSpeed = 10f;
    public float goblinTNTRange = 3f;
    public float goblinTNTExp = 20f;
    public float goblinTNTMultipleAttackChance = 0f;
    [Header("!!Spells!!")]
    [Header("!!ManaSpells!!")]
    [Header("!!FireBlade!!")]
    public float fireBladeManaSpellSpeed = 5f;
    public float fireBladeManaSpellDamageMutli = 2f;
    public int fireBladeManaSpellBounces = 0;
    public float fireBladeManaSpellKnockbackForce = 20f;
    public float fireBladeManaSpellCriticalChance = 0f;
    public float fireBladeManaSpellCriticalMultiplier = 1.1f;
    public Color fireBladeManaSpellDefaultColor = new(255, 255, 0);
    public float fireBladeManaSpellManaCost = 1f;
    public int fireBladeManaSpellPiercing = 25;
    public int fireBladeManaTotal = 1;
    public float fireBladeDelay = 0.5f;
    public float fireBladeCooldown = 5f;
    public string fireBladeCastSound = "fireBladeSound";
    [Header("!!OrbidBlade!!")]
    public float orbidBladeSpellSpeed = 5f;
    public float orbidBladeSpellDamageMutli = 2f;
    public int orbidBladeSpellBounces = 0;
    public float orbidBladeSpellKnockbackForce = 20f;
    public float orbidBladeSpellCriticalChance = 0f;
    public float orbidBladeSpellCriticalMultiplier = 1.1f;
    public Color orbidBladeSpellDefaultColor = new(255, 255, 0);
    public float orbidBladeSpellManaCost = 1f;
    public int orbidBladeSpellPiercing = 25;
    public float orbidBladeCooldown = 5f;
    public string orbidBladeCastSound = "orbidBladeCastSound";
    //Orbit
    public float orbitBladeRadius = 1.5f;
    public float orbitBladeRotationSpeed = 50f; // degrees per second
    public float orbitBladeDuration = 5f;
    public int totalOrbitBlades = 3;
    public string orbitCouroutineSound = "rotatingBlades";
    [Header("!!Shield!!")]
    public float shieldSpellDuration = 3f;
    public float shieldSpellSpeed = 1.5f;
    public float shieldSpellDamage = 1;
    public float shieldSpellKnockbackForce = 20f;
    public float shieldSpellCriticalChance = 0f;
    public float shieldSpellCriticalMultiplier = 1.1f;
    public Color shieldSpellDefaultColor = new(255, 255, 0);
    public float shieldSpellManaCost = 1f;
    public int shieldSpellPiercing = 5;
    public int shieldManaTotal = 1;
    public float shieldDelay = 0.5f;
    public float shieldCooldown = 5f;
    public string shieldCastSound = "fireBladeSound";
    public float shieldSpellSpeedMultiply = 1.3f;
    [Header("!!AttackSpellsSpells!!")]
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
    public float bladeSpeed = 35f;
    public float bladeDamageMulti = 1.1f;
    public int bladeBounces = 3;
    public float bladeKnockbackForce = 20f;
    public float bladeCriticalChance = 0.7f;
    public float bladeCriticalMultiplier = 1f;
    public Color bladeDefaultColor = Color.white;
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


    public enum SpellCode
    {
        //Shoule be same with UpgradeCode
        FireBlade,
        RotatingBlades,
        Shield,
    }

    public enum CoinDropEnum
    {
        None,
        Yellow,
        Red,
        Diamond,
    }

    public enum InvulnerableReasonEnum
    {
        Whatever,
        ShieldSpell,
        Potion,
        Diamond,
    }

    public enum EnemyTypes
    {
        Level1Skeleton,
        SkeletonArcher,
        VampireType3,
        VampireBoss,
        SkeletonKing,
        GoblinTourch,
        GoblinTNT,
    }

    public enum PauseReasonEnum
    {
        PlayerIsDead,
        HeroBuyMenu,
        LevelUpPanel,
        GameMenu,
        GameReset,
    }

    public void PauseTime(PauseReasonEnum pauseReason)
    {
        Debug.Log("Paused Time with reason:" + pauseReason);
        if (!pauseReasonsList.Contains(pauseReason))
        {
            pauseReasonsList.Add(pauseReason);
            if (Time.timeScale > 0)
            {
                gameIsPaused = true;
                Time.timeScale = 0;
                AudioManager.Instance.PauseAllSFX();
            }
        }
    }
    public void UnPauseTime(PauseReasonEnum pauseReason)
    {
        Debug.Log("UnPaused Time with reason:" + pauseReason);
        if (pauseReasonsList.Remove(pauseReason))
        {
            // Only unpause if no other reasons remain
            if (pauseReasonsList.Count == 0 && Time.timeScale == 0)
            {
                Time.timeScale = 1;
                gameIsPaused = false;
                AudioManager.Instance.ResumeAllSFX();
            }
        }
    }

    //BACKUP VALUES 
    private Dictionary<string, object> _initialValues = new();
    private void CacheInitialValues()
    {
        _initialValues.Clear();

        var fields = GetType().GetFields(
            System.Reflection.BindingFlags.Instance |
            System.Reflection.BindingFlags.Public |
            System.Reflection.BindingFlags.NonPublic
        );

        foreach (var field in fields)
        {
            if (typeof(UnityEngine.Object).IsAssignableFrom(field.FieldType))
                continue;

            _initialValues[field.Name] = field.GetValue(this);
        }
    }
    public void ResetValues()
    {
        foreach (var kvp in _initialValues)
        {
            var field = GetType().GetField(kvp.Key,
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.NonPublic);
            field?.SetValue(this, kvp.Value);
        }
    }

    public void ActivateMagnet()
    {
        StartCoroutine(DisableMagnetAfterDelay(1f)); // 1 second delay
    }

    private IEnumerator DisableMagnetAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        magnetIsActive = false;
    }


}
