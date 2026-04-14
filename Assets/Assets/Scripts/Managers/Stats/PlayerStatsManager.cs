using System.Collections.Generic;
using UnityEngine;
public enum PlayerStatCategory
{
    Attack,
    Defence,
    Utility,
    Spells,
}

public enum PlayerStatType
{
    // =====================
    // Attack (0–99)
    // =====================
    Attack_Attack = 0,
    Attack_AttackSpeed = 1,
    Attack_AttackRange = 2,
    Attack_CriticalChance = 3,
    Attack_CriticalDamage = 4,
    Attack_Bounce = 5,

    // =====================
    // Defence (100–199)
    // =====================
    Defence_Health = 100,
    Defence_HealthRegenValue = 101,
    Defence_HealthRegenInterval = 102,
    Defence_Armor = 103,
    Defence_MovementSpeed = 104,
    Defence_Mana = 105,
    Defence_ManaRegen = 106,
    Defence_ManaRegenInterval = 107,
    Defence_BetterPotions = 108,

    // =====================
    // Economy (200–299)
    // =====================
    Economy_CoinsValue = 200,
    Economy_MoreCoins = 201,
    Economy_LessExpToLvl = 202,
    Economy_MoreShardExp = 203,
    Economy_PickUpRange = 204,

    // =====================
    // Spells (300–399)
    // =====================
    Spells_FireBlade = 300,
    Spells_RotatingBlades = 301,
    Spells_Shield = 302,
    Spells_CooldownReduction = 303,


    // =====================
    // Others (400–499)
    // =====================
    Reroll = 400,
    UpgradeOption = 401,
}

public class PlayerStatsManager : MonoBehaviour
{
    [Header("All Upgrades go here")]
    [SerializeField] public UpgradeDatabaseSO upgradeDatabase;
    [Header("Base Stats per Difficulty")]
    [SerializeField] private PlayerStatsSO easyBaseStats;
    [SerializeField] private PlayerStatsSO normalBaseStats;
    [SerializeField] private PlayerStatsSO hardBaseStats;
    [SerializeField] private PlayerStatsSO insaneBaseStats;
    [SerializeField] private PlayerStatsSO MainMenuSceneStats;
    private PlayerStatsSO selectedBaseStats;
    private int _currentHealth; // stores the actual value
    private bool _lowHealthWarningPlayed = false;
    public int CurrentHealth  // the public face of it
    {
        get => _currentHealth;
        set
        {
            _currentHealth = Mathf.Max(0, value);
            OnHealthChanged();
        }
    }
    public int CurrentMana { get; set; }
    //
    public int CurrentLevel = 0;
    public bool RegenIsActive = true;
    public float CurrentExp = 0;
    public int MaxExp = 10;
    public static PlayerStatsManager Instance { get; private set; }

    public PlayerStats BaseStats { get; private set; }
    public PlayerStats RuntimeStats { get; private set; }

    public Dictionary<PlayerStatType, PlayerStatDefinition> Definitions
    { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        Initialize();
        upgradeDatabase.Initialize();
    }

    private void Start()
    {
    }

    private void AddPermanentUpgrades()
    {
        IReadOnlyCollection<string> purchasedUpgrades = UpgradeLoadManager.Instance.GetPurchasedUpgrades();
        foreach (string upgradeId in purchasedUpgrades)
        {
            UpgradeDataSO upgrade = upgradeDatabase.Get(upgradeId);
            if (upgrade != null)
            {
                RuntimeStats.AddPermanentUpdate(upgrade.playerStatType, upgrade.upgradeAmount);
            }
        }
    }

    public void Initialize()
    {
        if (GlobalVariables.Instance.mainMenuScene)
        {
            selectedBaseStats = MainMenuSceneStats;
        }
        else if (DifficultyManager.Instance.startingDifficultySet == false)
        {
            selectedBaseStats = normalBaseStats;
        }
        else
        {
            selectedBaseStats = SelectStatsBasedOnDifficulty();
        }

        BaseStats = new PlayerStats();
        RuntimeStats = new PlayerStats();
        Definitions = new Dictionary<PlayerStatType, PlayerStatDefinition>();

        // DEFENSE
        Register(PlayerStatType.Defence_Health, PlayerStatCategory.Defence, "Max Health");
        Register(PlayerStatType.Defence_HealthRegenInterval, PlayerStatCategory.Defence, "Health Regen");
        Register(PlayerStatType.Defence_HealthRegenValue, PlayerStatCategory.Defence, "Regen Time");
        Register(PlayerStatType.Defence_Armor, PlayerStatCategory.Defence, "Armor");
        Register(PlayerStatType.Defence_MovementSpeed, PlayerStatCategory.Defence, "Movement Speed");
        Register(PlayerStatType.Defence_Mana, PlayerStatCategory.Defence, "Max Mana");
        CurrentMana = MaxMana();
        Register(PlayerStatType.Defence_ManaRegen, PlayerStatCategory.Defence, "Mana Regen.");
        Register(PlayerStatType.Defence_ManaRegenInterval, PlayerStatCategory.Defence, "Mana Regen.");
        Register(PlayerStatType.Defence_BetterPotions, PlayerStatCategory.Defence, "Better Potions");


        // ATTACK
        Register(PlayerStatType.Attack_Attack, PlayerStatCategory.Attack, "Attack Damage");
        Register(PlayerStatType.Attack_AttackSpeed, PlayerStatCategory.Attack, "Attack Speed");
        Register(PlayerStatType.Attack_CriticalChance, PlayerStatCategory.Attack, "Critical Chance");
        Register(PlayerStatType.Attack_CriticalDamage, PlayerStatCategory.Attack, "Critical Damage");
        Register(PlayerStatType.Attack_AttackRange, PlayerStatCategory.Attack, "Attack Range");
        if (HeroUpgrades.Instance != null)
            HeroUpgrades.Instance.RecalculateRange();
        Register(PlayerStatType.Attack_Bounce, PlayerStatCategory.Attack, "Attack Bounce");

        // ECONOMY
        Register(PlayerStatType.Economy_PickUpRange, PlayerStatCategory.Utility, "Pick Up Range");
        Register(PlayerStatType.Economy_CoinsValue, PlayerStatCategory.Utility, "Coins Value");
        Register(PlayerStatType.Economy_MoreCoins, PlayerStatCategory.Utility, "More Coins");
        Register(PlayerStatType.Economy_LessExpToLvl, PlayerStatCategory.Utility, "Less EXP to Level");
        Register(PlayerStatType.Economy_MoreShardExp, PlayerStatCategory.Utility, "More Shard EXP");

        // SPELLS
        Register(PlayerStatType.Spells_FireBlade, PlayerStatCategory.Spells, "Fire Blade");
        Register(PlayerStatType.Spells_RotatingBlades, PlayerStatCategory.Spells, "Rotating Blades");
        Register(PlayerStatType.Spells_Shield, PlayerStatCategory.Spells, "Shield");
        Register(PlayerStatType.Spells_CooldownReduction, PlayerStatCategory.Spells, "Cooldown Reduction");

        //Other 
        Register(PlayerStatType.Reroll, PlayerStatCategory.Utility, "Maximum Rerolls");
        Register(PlayerStatType.UpgradeOption, PlayerStatCategory.Utility, "Upgrade Options");
        AddPermanentUpgrades();
        CurrentHealth = MaxHealth();
        CurrentMana = MaxMana();
    }

    private void Register(
    PlayerStatType type,
    PlayerStatCategory category,
    string displayName)
    {
        float finalBaseValue = 1;

        if (TryGetBaseValueFromSO(type, out float soValue))
        {
            finalBaseValue = soValue;
        }
        else
        {
            Debug.LogError($"[PlayerStatsManager] Stat '{type}' not found in PlayerStatsSO.Using fallback value: {finalBaseValue}");
        }

        Definitions[type] =
            new PlayerStatDefinition(type, category, displayName);

        BaseStats.Register(type, finalBaseValue);
        RuntimeStats.Register(type, finalBaseValue);
    }

    // Roguelike run reset
    public void ResetToBase()
    {
        RuntimeStats = new PlayerStats(BaseStats);
        UpgradeStackTracker.Instance.Reset();
    }

    //Helper methods

    public void IncreaseMaxHealthFromLevels(int amount)
    {
        RuntimeStats.AddLevelValue(PlayerStatType.Defence_Health, amount);
        CurrentHealth += amount;
    }

    public void IncreaseMaxManaFromLevels(int amount)
    {
        RuntimeStats.AddLevelValue(PlayerStatType.Defence_Mana, amount);
        CurrentHealth += amount;
    }

    public void IncreaseMaxHealthFromFlat(int amount)
    {
        RuntimeStats.AddFlat(PlayerStatType.Defence_Health, amount);
        CurrentHealth += amount;
    }

    public bool IsFullHealth()
    {
        return CurrentHealth
            >= MaxHealth();
    }

    public bool IsFullMana()
    {
        return CurrentMana
            >= MaxMana();
    }

    public void LoseHealth(float damage)
    {
        CurrentHealth -= (int)damage;
    }

    public int MaxHealth()
    {
        return (int)RuntimeStats.Get(PlayerStatType.Defence_Health);
    }

    public int MaxMana()
    {
        return (int)RuntimeStats.Get(PlayerStatType.Defence_Mana);
    }

    private bool TryGetBaseValueFromSO(
    PlayerStatType type,
    out float value)
    {
        value = 0f;

        if (selectedBaseStats == null)
            return false;

        foreach (var entry in selectedBaseStats.stats)
        {
            if (entry.type == type)
            {
                value = entry.baseValue;
                return true;
            }
        }

        return false;
    }

    private PlayerStatsSO SelectStatsBasedOnDifficulty()
    {
        return DifficultyManager.Instance.CurrentDifficulty switch
        {
            DifficultyLevel.Easy => easyBaseStats,
            DifficultyLevel.Normal => normalBaseStats,
            DifficultyLevel.Hard => hardBaseStats,
            DifficultyLevel.Insane => insaneBaseStats,
            _ => normalBaseStats,
        };
    }

    private void OnHealthChanged()
    {
        float healthPercent = (float)_currentHealth / MaxHealth();

        if (healthPercent <= 0.2f && _currentHealth >= 1)
        {
            if (!_lowHealthWarningPlayed)
            {
                _lowHealthWarningPlayed = true;
                HurtEffect.Instance.StartLowHealthEffect(0.1f);
            }
        }
        else
        {
            if (_lowHealthWarningPlayed)
            {
                _lowHealthWarningPlayed = false;
                HurtEffect.Instance.StopLowHealthEffect();
            }
        }
    }
}