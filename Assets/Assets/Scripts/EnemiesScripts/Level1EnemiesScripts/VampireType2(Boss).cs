using UnityEngine;

public class VampireType2Boss : EnemyBaseScript
{
    // Shortcuts with null safety
    private static DifficultyManager DM => DifficultyManager.Instance;
    private static float EnemyHealthMult => (DM != null) ? DM.enemyHealthMultiplier : 1f;
    private static float EnemyDamageMult => (DM != null) ? DM.enemyDamageMultiplier : 1f;
    private static float EnemySpeedMult => (DM != null) ? DM.enemySpeedMultiplier : 1f;

    public override GlobalVariables.EnemyTypes EnemyType
    => GlobalVariables.EnemyTypes.VampireBoss;

    protected override void Start()
    {
        base.Start();
        ApplyBaseAndDifficulty();
        hasAttackAnimation = true;
    }

    // Important for pooled bosses: re-apply on enable
    protected override void OnEnable()
    {
        base.OnEnable();
        ApplyBaseAndDifficulty();
        CurrentHealth = MaxHealth;
    }

    private void ApplyBaseAndDifficulty()
    {
        // Base stats
        maxHealth = GlobalVariables.Instance.vampireType2Health;
        knockbackResistance = GlobalVariables.Instance.vampireType2KnockbackResistance;

        // Difficulty adjustments
        maxHealth *= EnemyHealthMult;

        currentHealth = maxHealth;
    }

    protected override void Update()
    {
        base.Update();
        //BossSpecialBehavior();
        if (hasReachedPlayer)
        {
            // boss proximity logic if needed
        }
    }

    public override void EndAttackAnimation()
    {
        animator.SetBool("Attack", false);
        animator.SetBool("hasReachedPlayer", false);
        hasReachedPlayer = false;

        if (Random.value > 0.5f)
            animator.SetTrigger("SpecialAttack");
    }

    // ------- Scaled properties -------
    public override float Speed
        => GlobalVariables.Instance.vampireType2Speed * EnemySpeedMult;

    public override float Damage
        => GlobalVariables.Instance.vampireType2Damage * EnemyDamageMult;

    public override float ProjectileSpeed
        // Reuse speed multiplier for projectile travel (fits current manager)
        => GlobalVariables.Instance.vampireType2ProjectileSpeed * EnemySpeedMult;

    // ------- Unchanged/forwarded properties -------
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

    public override float AttackCooldown
        => GlobalVariables.Instance.vampireType2AttackCooldown;

    // Collectables
    public override float CoinDropChance
        => GlobalVariables.Instance.vampireType2CoinDropChance;

    public override float HealthPotionChance
        => GlobalVariables.Instance.vampireType2HealthPotionChance;

    public override float ManaPotionChance
        => GlobalVariables.Instance.vampireType2ManaPotionChance;

    public override GlobalVariables.CoinDropEnum CoinDropEnum
        => GlobalVariables.Instance.vampireType2CoinEnum;

    public override float MinExp
        => GlobalVariables.Instance.vampireType2Exp;

    public override float MultipleAttackChance
        => GlobalVariables.Instance.vampireType2MultipleAttackChance;

    public override float AttackRange
        => GlobalVariables.Instance.vampireType2Range;

    public override string DeathSoundClip
        => "vampireDeadSound"; // fixed: avoid returning a literal typo

    public override string[] AttackSoundClip
        => new string[] { "vampireAttackSound1", "vampireAttackSound2" };

    // ------- Special Attack -------
    // This local is used only if you force RB velocity directly.
    [SerializeField] private float projectileSpeed = 20f;

    public override void SpecialAttack()
    {
        AudioManager.Instance.PlaySoundFX("Chain_Frost_target_creep", transform.position, 0.4f, 0.75f, 1.25f);

        if (Random.value < 0.15f)
        {
            SpawnCrossPattern();
        }
        else if (Random.value > 0.6f)
        {
            FullSpawnDiagonalPattern();
        }
        else
        {
            SpawnDiagonalPattern();
        }
    }

    private void SpawnCrossPattern()
    {
        Vector2[] directions = {
            Vector2.up, Vector2.down, Vector2.left, Vector2.right
        };
        foreach (var dir in directions) SpawnProjectile(dir);
    }

    private void SpawnDiagonalPattern()
    {
        Vector2[] directions = {
            new Vector2( 1,  1).normalized,
            new Vector2(-1,  1).normalized,
            new Vector2( 1, -1).normalized,
            new Vector2(-1, -1).normalized
        };
        foreach (var dir in directions) SpawnProjectile(dir);
    }

    private void FullSpawnDiagonalPattern()
    {
        Vector2[] directions = {
            Vector2.up, Vector2.down, Vector2.left, Vector2.right,
            new Vector2( 1,  1).normalized,
            new Vector2(-1,  1).normalized,
            new Vector2( 1, -1).normalized,
            new Vector2(-1, -1).normalized
        };
        foreach (var dir in directions) SpawnProjectile(dir);
    }

    private void SpawnProjectile(Vector2 direction)
    {
        GameObject proj = Instantiate(projectile, transform.position, Quaternion.identity);
        proj.transform.localScale *= 2f;

        // Use a single, consistent speed value that includes difficulty
        float finalSpeed = Mathf.Max(0f, ProjectileSpeed); // already includes EnemySpeedMult

        // If the projectile moves via RB velocity, set it:
        if (proj.TryGetComponent<Rigidbody2D>(out var rb))
        {
            rb.linearVelocity = direction * finalSpeed;
        }

        // If the projectile has its own script, set its parameters too
        if (proj.TryGetComponent<EnemyProjectileBaseScript>(out var projectileScript))
        {
            projectileScript.Damage = Damage;      // includes EnemyDamageMult
            projectileScript.Speed = finalSpeed;  // includes EnemySpeedMult
            projectileScript.hasDirection = true;
        }

        // Rotate the *instance* to face its travel direction (fix)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        proj.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}