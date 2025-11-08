public class SkeletonArcher : EnemyBaseScript
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

    // Important for pooled enemies: re-apply when re-enabled
    protected override void OnEnable()
    {
        base.OnEnable();
        ApplyBaseAndDifficulty();
        CurrentHealth = MaxHealth;
    }

    private void ApplyBaseAndDifficulty()
    {
        // Base stats
        maxHealth = GlobalVariables.Instance.skeletonArcherHealth;
        knockbackResistance = GlobalVariables.Instance.skeletonArcherKnockbackResistance;

        // Rarity adjustments (before difficulty so multipliers stack intuitively)
        if (GlobalVariables.EnemyRarity.Green.Equals(rarity))
        {
            maxHealth *= GlobalVariables.Instance.greenHealthMultiplier;
            knockbackResistance *= GlobalVariables.Instance.greenKnockbackMultiplier;
            spriteTransform.localScale *= GlobalVariables.Instance.greenScaleMultiplier;
        }

        // Difficulty multipliers
        maxHealth *= EnemyHealthMult;
        // (Damage/Speed are applied via their getters below)
        currentHealth = maxHealth;
    }

    public override float Speed
        => GlobalVariables.Instance.skeletonArcherSpeed * EnemySpeedMult;

    public override GlobalVariables.EnemyTypes EnemyType
        => GlobalVariables.EnemyTypes.SkeletonArcher;

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
        => GlobalVariables.Instance.skeletonArcherDamage * EnemyDamageMult;

    public override float AttackCooldown
        => GlobalVariables.Instance.skeletonArcherAttackCooldown;

    public override float CoinDropChance
        => GlobalVariables.Instance.skeletonArcherCoinDropChance;

    public override float HealthPotionChance
        => GlobalVariables.Instance.skeletonArcherHealthPotionChance;

    public override float ManaPotionChance
        => GlobalVariables.Instance.skeletonArcherManaPotionChance;

    public override float ProjectileSpeed
        => GlobalVariables.Instance.skeletonArcherProjectileSpeed;

    public override GlobalVariables.CoinDropEnum CoinDropEnum
        => GlobalVariables.Instance.skeletonArcherCoinEnum;

    public override float MinExp
        => GlobalVariables.Instance.skeletonArcherExp;

    public override float AttackRange
        => GlobalVariables.Instance.skeletonArcherRange;

    public override string DeathSoundClip
        => "skeletonDeadSound";

    public override string[] AttackSoundClip
        => new string[] { "arrowSound2" };

    public override float MultipleAttackChance
        => GlobalVariables.Instance.skeletonArcherMultipleAttackChance;
}