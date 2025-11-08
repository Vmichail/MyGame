using UnityEngine;

public class VampireType3 : EnemyBaseScript
{
    // Shortcuts with null safety
    private static DifficultyManager DM => DifficultyManager.Instance;
    private static float EnemyHealthMult => (DM != null) ? DM.enemyHealthMultiplier : 1f;
    private static float EnemyDamageMult => (DM != null) ? DM.enemyDamageMultiplier : 1f;
    private static float EnemySpeedMult => (DM != null) ? DM.enemySpeedMultiplier : 1f;

    protected override void Start()
    {
        base.Start();
        ApplyBaseAndDifficulty();
        hasAttackAnimation = true;
    }

    // Important for pooled enemies
    protected override void OnEnable()
    {
        base.OnEnable();
        ApplyBaseAndDifficulty();
        CurrentHealth = MaxHealth;
    }

    private void ApplyBaseAndDifficulty()
    {
        // Base (raw) stats
        maxHealth = GlobalVariables.Instance.vampireType3Health;
        knockbackResistance = GlobalVariables.Instance.vampireType3KnockbackResistance;

        // Rarity first so multipliers stack intuitively
        if (GlobalVariables.EnemyRarity.Green.Equals(rarity))
        {
            maxHealth *= GlobalVariables.Instance.greenHealthMultiplier;
            knockbackResistance *= GlobalVariables.Instance.greenKnockbackMultiplier;
            spriteTransform.localScale *= GlobalVariables.Instance.greenScaleMultiplier;
        }

        // Difficulty
        maxHealth *= EnemyHealthMult;

        currentHealth = maxHealth;
    }

    public override float Speed
        => GlobalVariables.Instance.vampireType3Speed * EnemySpeedMult;

    public override float MaxHealth
    {
        get => maxHealth;
        set => maxHealth = value;
    }

    public override float CurrentHealth
    {
        get => currentHealth;
        set => currentHealth = value;
    }

    public override float Damage
        => GlobalVariables.Instance.vampireType3Damage * EnemyDamageMult;

    public override float AttackCooldown
        => GlobalVariables.Instance.vampireType3AttackCooldown;

    public override float CoinDropChance
        => GlobalVariables.Instance.vampireType3CoinDropChance;

    public override float HealthPotionChance
        => GlobalVariables.Instance.vampireType3HealthPotionChance;

    public override float ManaPotionChance
        => GlobalVariables.Instance.vampireType3ManaPotionChance;

    public override float ProjectileSpeed
        => GlobalVariables.Instance.vampireType3ProjectileSpeed * EnemySpeedMult;

    public override GlobalVariables.CoinDropEnum CoinDropEnum
        => GlobalVariables.Instance.vampireType3CoinEnum;

    public override float MinExp
        => GlobalVariables.Instance.vampireType3Exp;

    public override float AttackRange
        => GlobalVariables.Instance.vampireType3Range;

    public override string DeathSoundClip
        => "vampireDeadSound"; // fixed: don’t return a literal with a typo

    public override string[] AttackSoundClip
        => new string[] { "vampireAttackSound1", "vampireAttackSound2" };
}