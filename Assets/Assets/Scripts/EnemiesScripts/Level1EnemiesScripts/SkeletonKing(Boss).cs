using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using Random = UnityEngine.Random;

public class SkeletonKingBoss : EnemyBaseScript
{
    public override GlobalVariables.EnemyTypes EnemyType
=> GlobalVariables.EnemyTypes.SkeletonKing;
    [SerializeField] private GameObject skeletonBossSpawner;
    [SerializeField] private GameObject health;
    private BoxCollider2D collider;
    // Shortcuts with null safety
    private static DifficultyManager DM => DifficultyManager.Instance;
    private static float EnemyHealthMult => (DM != null) ? DM.enemyHealthMultiplier : 1f;
    private static float EnemyDamageMult => (DM != null) ? DM.enemyDamageMultiplier : 1f;
    private static float EnemySpeedMult => (DM != null) ? DM.enemySpeedMultiplier : 1f;
    [SerializeField] private float idleSoundInterval = 9f;
    private CancellationTokenSource idleCTS;
    [SerializeField]
    private String[] skeletonKingSounds = new string[] {
        "BeholdYourKing",
        "FleeWhileYouCan",
        "TimeToFight",
        "IWalkAmongYou",
        "TimeToFight",
        "LetsGetItOver",
    };
    [SerializeField]
    private String[] skeletonKingDeathSounds = new string[] {
        "IRiseAgain",
        "ButTheDeathless",
        "DeathHasNoGrip",
        "NotEvenDeath",
        "SeeYouSooner",
    };

    protected override String[] HurtSounds => new string[] {
        "skeletonKingBossHurt1",
        "skeletonKingBossHurt2",
        "skeletonKingBossHurt3",
    };



    protected override void Start()
    {
        base.Start();
        ApplyBaseAndDifficulty();
        hasAttackAnimation = true;
        AudioManager.Instance.PlaySoundFX("reincarnation_cast", transform.position, 0.8f, 1f, 1f);
        idleCTS?.Cancel();
        idleCTS = new CancellationTokenSource();
        IdleSoundLoop(idleCTS.Token).Forget();
    }

    // Important for pooled bosses: re-apply on enable
    protected override void OnEnable()
    {
        base.OnEnable();
        collider = GetComponent<BoxCollider2D>();
        ApplyBaseAndDifficulty();
        CurrentHealth = MaxHealth;
    }

    private void ApplyBaseAndDifficulty()
    {
        // Base stats
        maxHealth = GlobalVariables.Instance.skeletonKingHealth;
        knockbackResistance = GlobalVariables.Instance.skeletonKingKnockbackResistance;

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

        if (UnityEngine.Random.value > 0.5f)
            animator.SetTrigger("SpecialAttack");
    }

    // ------- Scaled properties -------
    public override float Speed
        => GlobalVariables.Instance.vampireType2Speed * EnemySpeedMult;

    public override float Damage
        => GlobalVariables.Instance.skeletonKingDamage * EnemyDamageMult;

    public override float ProjectileSpeed
        // Reuse speed multiplier for projectile travel (fits current manager)
        => GlobalVariables.Instance.skeletonKingProjectileSpeed;

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
        => GlobalVariables.Instance.skeletonKingAttackCooldown;

    // Collectables
    public override float CoinDropChance
        => GlobalVariables.Instance.skeletonKingCoinDropChance;

    public override float HealthPotionChance
        => GlobalVariables.Instance.skeletonKingHealthPotionChance;

    public override float ManaPotionChance
        => GlobalVariables.Instance.skeletonKingManaPotionChance;

    public override GlobalVariables.CoinDropEnum CoinDropEnum
        => GlobalVariables.Instance.vampireType2CoinEnum;

    public override float MinExp
        => GlobalVariables.Instance.skeletonKingExp;

    public override float MultipleAttackChance
        => GlobalVariables.Instance.skeletonKingMultipleAttackChance;

    public override float AttackRange
        => GlobalVariables.Instance.skeletonKingRange;

    public override string DeathSoundClip
        => "SkeletonKingDeath";

    public override string[] AttackSoundClip
        => new string[] { "SkeletonKingAttackSound", "SkeletonKingAttackSound" };

    public override void SpecialAttack()
    {
        if (Random.value < 0.3f)
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
        GameObject proj = Instantiate(projectile, specialAttackFX.transform.position, Quaternion.identity);
        proj.transform.localScale *= 1f;

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
        //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //proj.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public override void SummonStarts()
    {
        collider.enabled = false;
        health.SetActive(false);
    }

    public override void SummonEndsSound()
    {
        collider.enabled = true;
        health.SetActive(true);
        AudioManager.Instance.PlaySoundFX("theboneking", transform.position, 0.8f, 1f, 1f);
    }


    public override void NormalAttackFX()
    {
        normalAttackFX.SetActive(true);
        AudioManager.Instance.PlaySoundFX("SkeletonKingAttackSound", transform.position, 0.8f, 1f, 1f);
    }

    public override void PreAttackSound()
    {
        AudioManager.Instance.PlaySoundFX("SkeletonKingPreAttackSound", transform.position, 0.8f, 1f, 1f);
    }

    public override void SpecialAttackFX()
    {
        specialAttackFX.SetActive(true);
        AudioManager.Instance.PlaySoundFX("SkeletonKingSpecialAttackSound", transform.position, 0.8f, 1f, 1f);
    }

    private async UniTaskVoid IdleSoundLoop(CancellationToken token)
    {
        try
        {
            while (true)
            {
                await UniTask.Delay((int)(idleSoundInterval * 1000f), cancellationToken: token);

                if (token.IsCancellationRequested || this == null)
                    return;

                // Play the idle sound
                AudioManager.Instance.PlayRandomSoundFX(skeletonKingSounds, transform.position, 0.7f, 0.9f, 1.1f);
            }
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    protected override void OnDeath()
    {
        idleCTS?.Cancel();
        idleCTS?.Dispose();
        idleCTS = null;
        AudioManager.Instance.PlayRandomSoundFX(skeletonKingDeathSounds, transform.position, 0.7f, 0.9f, 1.1f);
        Instantiate(skeletonBossSpawner, transform.position, Quaternion.identity);
        base.OnDeath();
    }


    private void OnDisable()
    {
        idleCTS?.Cancel();
        idleCTS?.Dispose();
        idleCTS = null;
    }

    // Called when the object is permanently destroyed
    private void OnDestroy()
    {
        idleCTS?.Cancel();
        idleCTS?.Dispose();
        idleCTS = null;
    }
}