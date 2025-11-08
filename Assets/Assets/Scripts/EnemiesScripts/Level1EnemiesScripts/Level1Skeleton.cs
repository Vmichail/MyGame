public class Level1Skeleton : EnemyBaseScript
{
    // Convenience accessor with null safety
    private static DifficultyManager DM => DifficultyManager.Instance;

    private static float EnemyHealthMult => (DM != null) ? DM.enemyHealthMultiplier : 1f;
    private static float EnemyDamageMult => (DM != null) ? DM.enemyDamageMultiplier : 1f;
    private static float EnemySpeedMult => (DM != null) ? DM.enemySpeedMultiplier : 1f;

    protected override void Start()
    {
        base.Start();
        MaxHealth = GlobalVariables.Instance.skeletonHealth * EnemyHealthMult;
        knockbackResistance = GlobalVariables.Instance.skeletonKnockbackResistance;
        CurrentHealth = MaxHealth;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        MaxHealth = GlobalVariables.Instance.skeletonHealth * EnemyHealthMult;
        CurrentHealth = MaxHealth;
    }

    public override GlobalVariables.EnemyTypes EnemyType
        => GlobalVariables.EnemyTypes.Level1Skeleton;

    public override float Speed
        => GlobalVariables.Instance.skeletonSpeed * EnemySpeedMult;

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
        => GlobalVariables.Instance.skeletonDamage * EnemyDamageMult;

    public override float AttackCooldown
        => GlobalVariables.Instance.skeletonAttackCooldown;

    public override float CoinDropChance
        => GlobalVariables.Instance.skeleonCoinDropChance;

    public override float HealthPotionChance
        => GlobalVariables.Instance.skeletonHealthPotionChance;

    public override float ManaPotionChance
        => GlobalVariables.Instance.skeletonManaPotionChance;

    public override GlobalVariables.CoinDropEnum CoinDropEnum
        => GlobalVariables.Instance.skeletonCoinEnum;

    public override float MinExp
        => GlobalVariables.Instance.skeletonMinExp;

    public override float MaxExp
        => GlobalVariables.Instance.skeletonMaxExp;

    public override string DeathSoundClip
        => "skeletonDeadSound";
}