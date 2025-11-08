public class GoblinTorchScript : EnemyBaseScript
{
    // Difficulty multipliers (null-safe)
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
        maxHealth = GlobalVariables.Instance.goblinTorchHealth;
        knockbackResistance = GlobalVariables.Instance.goblinTorchKnockbackResistance;

        // Rarity first (stacking order)
        if (GlobalVariables.EnemyRarity.Green.Equals(rarity))
        {
            maxHealth *= GlobalVariables.Instance.greenHealthMultiplier;
            knockbackResistance *= GlobalVariables.Instance.greenKnockbackMultiplier;
            spriteTransform.localScale *= GlobalVariables.Instance.greenScaleMultiplier;
        }

        // Difficulty multipliers
        maxHealth *= EnemyHealthMult;

        currentHealth = maxHealth;
    }

    public override GlobalVariables.EnemyTypes EnemyType
        => GlobalVariables.EnemyTypes.GoblinTourch; // keeping your enum key

    public override float Speed
        => GlobalVariables.Instance.goblinTorchSpeed * EnemySpeedMult;

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
        => GlobalVariables.Instance.goblinTorchDamage * EnemyDamageMult;

    public override float AttackCooldown
        => GlobalVariables.Instance.goblinTorchAttackCooldown;

    public override float CoinDropChance
        => GlobalVariables.Instance.goblinTorchCoinDropChance;

    public override float HealthPotionChance
        => GlobalVariables.Instance.goblinTorchHealthPotionChance;

    public override float ManaPotionChance
        => GlobalVariables.Instance.goblinTorchManaPotionChance;

    public override GlobalVariables.CoinDropEnum CoinDropEnum
        => GlobalVariables.Instance.goblinTorchCoinEnum;

    public override float MinExp
        => GlobalVariables.Instance.goblinTorchExp;

    // No ranged attack → no ProjectileSpeed override needed
}