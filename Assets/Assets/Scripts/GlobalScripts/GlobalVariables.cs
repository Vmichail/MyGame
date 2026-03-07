using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;

public class GlobalVariables : MonoBehaviour
{
    private const string SHOW_ENEMY_HEALTH_KEY = "ShowAllEnemiesHealth";
    private const string DEV_MODE_KEY = "DeveloperMode";
    private const string SELECTED_CHAR_KEY = "SelectedCharacter";
    public static GlobalVariables Instance { get; private set; }

    private void Update()
    {
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
        LoadSettings();
        CacheInitialValues();
    }
    private void LoadSettings()
    {
        showAllEnemiesHealth = PlayerPrefs.GetInt(SHOW_ENEMY_HEALTH_KEY, 0) == 1;
        developerMode = PlayerPrefs.GetInt(DEV_MODE_KEY, 0) == 1;
        selectedCharacter = PlayerPrefs.GetString(SELECTED_CHAR_KEY, CharacterSprite.MiranaSprite.ToString());
    }
    public void SetSelectedCharacter(string value)
    {
        PlayerPrefs.SetString(SELECTED_CHAR_KEY, value);
        PlayerPrefs.Save();
    }

    //
    public bool canZoomMap = true;
    public string selectedCharacter;
    public bool testMode = false;
    public bool mainMenuScene = false;
    [Header("Sound-Music values")]
    public float masterVolume = 1f;
    public float musicVolume = 1f;
    public float SFXVolume = 1f;
    [Header("Settings Options")]
    public bool developerMode = false;
    public bool showAllEnemiesHealth = false;
    public event Action<bool> OnShowAllEnemiesHealthChanged;
    public event Action<bool> OnDeveloperModeChanged;
    public void SetShowAllEnemiesHealth(bool value)
    {
        if (showAllEnemiesHealth == value)
            return;

        showAllEnemiesHealth = value;
        PlayerPrefs.SetInt(SHOW_ENEMY_HEALTH_KEY, value ? 1 : 0);
        PlayerPrefs.Save();

        OnShowAllEnemiesHealthChanged?.Invoke(value);
    }

    public void SetDeveloperMode(bool value)
    {
        if (developerMode == value)
            return;

        developerMode = value;
        PlayerPrefs.SetInt(DEV_MODE_KEY, value ? 1 : 0);
        PlayerPrefs.Save();

        OnDeveloperModeChanged?.Invoke(value);
    }

    [Header("Sound-Music Generic Clip Names")]
    public string[] linaAnnouncementsClips = { "linaGame", "linaFireTime", "linaIamOnFire", "linaLetsGetAFireGoing" };
    public string[] gameOverClips = { "gameOver1", "gameOver2", "gameOver3", "gameOver4", "gameOver5" };
    [Header("Player Generic Values")]
    public bool playerIsAlive = true;
    public List<InvulnerableReasonEnum> playerInvulnerableReasons = new();
    public List<PauseReasonEnum> pauseReasonsList = new();
    public bool gameIsPaused = false;
    public float gameTime = 0f;
    [Header("Player Collectables")]
    public bool magnetIsActive = false;
    public bool isSpawningCollectablePets = true;
    public int spawnedPets = 0;
    public int catsCollected = 0;
    public int yellowCoinValue = 1;
    public int greenRubyCoinValue = 10;
    public int greenRubyExpValue = 100;
    public int redRubyCoinValue = 5;
    public int redRubyExpValue = 10;
    [Header("Exp and Mana Values")]
    public int shardExp = 1;

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
    public int defaultSpellPiercing = 0;
    public bool defaultSpellOnDestroyEffect = false;
    public float defaultKnockbackforce = 10f;
    public Color defaultColor = Color.yellow;
    [Header("--!!Enemies!!--")]
    [Header("--!!Generic Spawning Variables")]
    public bool spawningMobsIsEnabled = true;
    public bool endlessModeOn = false;
    public int aliveEnemies = 0;
    public int killedEnemies = 0;
    public int score = 0;
    public int spawnedSkeletons = 0;
    public int spawnedSkeletonArchers = 0;

    [Header("!!Spells!!")]
    [Header("!!ManaSpells!!")]
    [Header("!!FireBlade!!")]
    public float fireBladeManaSpellSpeed = 5f;
    public float fireBladeManaSpellDamageMutli = 2f;
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
    public float orbidBladeSpellKnockbackForce = 20f;
    public Color orbidBladeSpellDefaultColor = Color.aliceBlue;
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
    public float shieldSpellSpeed = 2f;
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
    public float fireballKnockbackForce = 15f;
    public float fireballCriticalChance = 0.5f;
    public float fireballCriticalMultiplier = 1f;
    public Color fireballDefaultColor = new(255, 255, 0);
    [Header("Spell-DarkBall")]
    public float darkballSpeed = 8f;
    public float darkballDamageMulti = 1.1f;
    public float darkballKnockbackForce = 10f;
    public float darkballCriticalChance = 0.7f;
    public float darkballCriticalMultiplier = 1f;
    public Color darkballDefaultColor = Color.cyan;
    [Header("Spell-Blade")]
    public float bladeSpeed = 35f;
    public float bladeKnockbackForce = 20f;
    public float bladeCriticalChance = 0.7f;
    public float bladeCriticalMultiplier = 1f;
    public Color bladeDefaultColor = Color.white;
    [Header("Spell-PoisonCircle")]
    public float poisonCircleSpeed = 14f;
    public float poisonCircleDamageMulti = 1.1f;
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
    public bool blockGameplayInput;

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
        BlackArmoredSkeleton,
        WhiteArmoredSkeleton,
        SkeletonArcher,
        VampireNormal,
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
                if (pauseReason.Equals(PauseReasonEnum.GameMenu))
                {
                    AudioManager.Instance.PauseAllSFX();
                }
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
                blockGameplayInput = true;
                StartCoroutine(ClearInputBlock());
                AudioManager.Instance.ResumeAllSFX();
            }
        }
    }

    private IEnumerator ClearInputBlock()
    {
        yield return null;
        blockGameplayInput = false;
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
        magnetIsActive = true;
        StartCoroutine(DisableMagnetAfterDelay(1f));
    }

    private IEnumerator DisableMagnetAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        magnetIsActive = false;
    }


}
