public class GoblinTNT : EnemyBaseScript
{
    // Difficulty shortcuts with null-safety
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
        // Base stats
        maxHealth = GlobalVariables.Instance.goblinTNTHealth;
        knockbackResistance = GlobalVariables.Instance.goblinTNTKnockbackResistance;

        // Rarity first (so difficulty stacks after)
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

    public override GlobalVariables.EnemyTypes EnemyType
        => GlobalVariables.EnemyTypes.GoblinTNT;

    public override float Speed
        => GlobalVariables.Instance.goblinTNTSpeed * EnemySpeedMult;

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
        => GlobalVariables.Instance.goblinTNTDamage * EnemyDamageMult;

    public override float AttackCooldown
        => GlobalVariables.Instance.goblinTNTAttackCooldown;

    public override float CoinDropChance
        => GlobalVariables.Instance.goblinTNTCoinDropChance;

    public override float HealthPotionChance
        => GlobalVariables.Instance.goblinTNTHealthPotionChance;

    public override float ManaPotionChance
        => GlobalVariables.Instance.goblinTNTManaPotionChance;

    public override float ProjectileSpeed
        => GlobalVariables.Instance.goblinTNTProjectileSpeed * EnemySpeedMult;

    public override GlobalVariables.CoinDropEnum CoinDropEnum
        => GlobalVariables.Instance.goblinTNTCoinEnum;

    public override float MinExp
        => GlobalVariables.Instance.goblinTNTExp;

    public override float AttackRange
        => GlobalVariables.Instance.goblinTNTRange;
}