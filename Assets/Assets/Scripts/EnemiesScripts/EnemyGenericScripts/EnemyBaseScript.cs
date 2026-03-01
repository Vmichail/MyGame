using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;


public abstract class EnemyBaseScript : MonoBehaviour
{
    [SerializeField] protected EnemyConfig config;
    protected EnemyStats stats;
    [SerializeField] GameObject particles;
    [SerializeField] bool hasDamageParticles = false;
    [SerializeField] GameObject damageParticles;
    [SerializeField] GameObject criticalParticles;
    [SerializeField] GameObject receivedDamagePopUp;
    [SerializeField] GameObject playerWasHitDamage;
    [SerializeField] protected GlobalVariables.EnemyRarity rarity = GlobalVariables.EnemyRarity.None;
    [SerializeField] protected bool hasAttackAnimation = false;
    [SerializeField] private bool spriteIsFacingLeft;
    [Header("--!!RangeAtributes!!--")]
    private EnemyRangeAttackScript rangeScript;
    private bool hasRangeAttack = false;
    [SerializeField] protected GameObject projectile;
    [SerializeField] Transform projectilePosition;
    [SerializeField] Transform projectileParent;
    [SerializeField] private bool projectileCanRotate = true;
    [SerializeField] private float randomSpreadAngle = 10f;
    protected float attackCooldownTimer = 0f;
    [Header("Special attacks - mostly bosses")]
    [SerializeField] protected GameObject normalAttackFX;
    [SerializeField] protected GameObject specialAttackFX;
    [SerializeField] public bool isGeneratedByPool = true;
    private bool KnockbackEffect = false;

    protected Transform player;
    public bool hasReachedPlayer;
    [SerializeField] protected Animator animator;
    public void SetAnimatorValue(string animatorValue, bool status)
    {
        animator.SetBool(animatorValue, status);
    }

    public ObjectPool<GameObject> _pool;

    public void SetPool(ObjectPool<GameObject> pool)
    {
        _pool = pool;
    }

    private Rigidbody2D rb;
    private protected Transform spriteTransform;
    private Coroutine damageCoroutine;
    protected float maxHealth;
    protected float currentHealth;
    private float cameraZ;

    public virtual bool IsDead { get; set; }
    public virtual float MaxHealth
    {
        get => maxHealth;
        set
        {
            maxHealth = value;
            if (currentHealth > maxHealth)
                currentHealth = maxHealth;
        }
    }
    public virtual float CurrentHealth
    {
        get => currentHealth;
        set => currentHealth = Mathf.Clamp(value, 0, MaxHealth);
    }

    //Stats forwarded from ScriptableObject
    public virtual float Damage => stats.damage * DifficultyManager.Instance.FinalEnemyDamageMultiplier;
    public virtual float Speed => Mathf.Min(stats.speed * DifficultyManager.Instance.FinalEnemySpeedMultiplier, 10f);
    public virtual float ProjectileSpeed => stats.projectileSpeed * DifficultyManager.Instance.FinalEnemySpeedMultiplier;
    public virtual float KnockbackResistance => stats.knockbackResistance;
    public virtual bool CanApplyKnockback => stats.canBeKnockedBack;
    public virtual float AttackCooldown => stats.attackCooldown;

    public virtual GlobalVariables.CoinDropEnum CoinDropEnum => stats.coinDropEnum;
    public virtual float MinExp => stats.minExp;
    public virtual float MaxExp => stats.maxExp;
    public virtual float AttackRange => stats.attackRange;
    public virtual float MultipleAttackChance => stats.multipleAttackChance;
    //Collectables 
    public virtual float CoinDropChance => stats.coinDropChance * DifficultyManager.Instance.EnemyCoinDropMultiplier;
    public virtual float HealthPotionChance => stats.healthPotionChance;
    public virtual float ManaPotionChance => stats.manaPotionChance;
    public virtual float GreenRubyChance => stats.greenRubyChance * DifficultyManager.Instance.EnemyCoinDropMultiplier;
    public virtual float RedRubyChance => stats.redRubyChance * DifficultyManager.Instance.EnemyCoinDropMultiplier;
    public virtual GlobalVariables.EnemyTypes EnemyType => config.enemyType;
    public virtual string[] DeathSoundClips => stats.deathSoundClips;

    public virtual string[] AttackSoundClips => stats.attackSoundClips;

    protected virtual string[] HurtSounds => stats.hurtSounds ?? Array.Empty<string>();
    //


    private String criticalHitSound = "criticalHitSound";


    private AnimatorStateInfo stateInfo;

    public virtual void RestoreHealth()
    {
        CurrentHealth = MaxHealth;
    }

    private enum AttackDirection
    {
        Up,
        Down,
        Middle
    }

    protected virtual void Awake()
    {
        cameraZ = Camera.main.transform.position.z;
    }

    public bool IsMovementLocked { get; private set; }

    public void LockMovement(bool locked)
    {
        IsMovementLocked = locked;
        if (locked && rb != null)
            rb.linearVelocity = Vector2.zero;
    }

    public Transform GetPlayerTransform()
    {
        return player;
    }

    public Animator GetEnemyAnimator()
    {
        return animator;
    }

    protected virtual void OnEnable()
    {
        BuildStats();
        IsDead = false;
        KnockbackEffect = false;
        hasReachedPlayer = false;
        //
        rangeScript = GetComponentInChildren<EnemyRangeAttackScript>();
        hasRangeAttack = rangeScript != null;
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();

        if (spriteRenderer != null)
            spriteTransform = spriteRenderer.transform;
        else
            Debug.LogWarning("No Sprite was found!");
        FindPlayer();


        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Start()
    {
    }

    private void FindPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    protected virtual void Update()
    {
        if (attackCooldownTimer > 0f)
            attackCooldownTimer -= Time.deltaTime;

        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (CurrentHealth < 0.5 && !IsDead)
        {
            OnDeath();
        }
    }

    protected virtual void OnDeath()
    {
        IsDead = true;
        DropCollectables();
        Instantiate(particles, transform.position, transform.rotation);
        AudioManager.Instance.PlayRandomSoundFX(DeathSoundClips, transform.position, 1f, 0.75f, 1.25f);
        EnemyManagerScript.Instance.UnregisterEnemy(gameObject);
        if (isGeneratedByPool && _pool != null)
            _pool.Release(gameObject);
        else
        {
            if (isGeneratedByPool && _pool == null)
                Debug.LogWarning("Enemy is marked as generated by pool but no pool was assigned!");
            Destroy(gameObject);
        }
    }

    protected virtual void FixedUpdate()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }

        MoveTowardPlayer();

        if (hasReachedPlayer && damageCoroutine == null && hasAttackAnimation == false)
        {
            damageCoroutine = StartCoroutine(CheckBeforeDamage());
        }
        else if (!hasReachedPlayer && damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
            damageCoroutine = null;
        }
    }

    IEnumerator CheckBeforeDamage()
    {
        yield return new WaitForSeconds(AttackCooldown);

        if (hasReachedPlayer)
        {
            DoDamage();
        }

        damageCoroutine = null;
    }

    public void DoDamage()
    {
        EnemyGenericFunctionsForPlayer.Instance.DamagePlayer(Damage);
    }

    public void DoAdditionalDamage(int damage)
    {
        EnemyGenericFunctionsForPlayer.Instance.DamagePlayer(Damage + damage);
    }

    protected void MoveTowardPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        if (KnockbackEffect || IsMovementLocked)
            return;
        else if (hasReachedPlayer || !stateInfo.IsName("Walk"))
        {
            rb.linearVelocity = Vector2.zero;
        }
        else
        {
            rb.linearVelocity = direction * Speed;
        }
        RotateEnemy(direction);
        if (hasAttackAnimation && !hasRangeAttack && attackCooldownTimer <= 0 && !stateInfo.IsName("Attack"))
        {
            /*GetAttackDirection(direction);*/
            animator.SetBool("Attack", true);
        }
    }

    private void RotateEnemy(Vector2 direction)
    {
        if (rb.linearVelocity != Vector2.zero)
        {
            if (spriteIsFacingLeft)
            {
                if (direction.x < 0)
                {
                    spriteTransform.rotation = Quaternion.Euler(0, 0, 0);
                    RotateAlsoFX(0);
                }

                else
                {
                    spriteTransform.rotation = Quaternion.Euler(0, 180, 0);
                    RotateAlsoFX(180);
                }
            }
            else
            {
                if (direction.x < 0)
                {
                    spriteTransform.rotation = Quaternion.Euler(0, 180, 0);
                    RotateAlsoFX(180);

                }
                else
                {
                    spriteTransform.rotation = Quaternion.Euler(0, 0, 0);
                    RotateAlsoFX(0);
                }
            }
        }
    }

    /*    private void GetAttackDirection(Vector2 dir)
        {
            if (Mathf.Abs(dir.y) > Mathf.Abs(dir.x))
            {
                if (dir.y > 0)
                    SetAttackDirection(AttackDirection.Up);
                else
                    SetAttackDirection(AttackDirection.Down);
            }
            else
            {
                SetAttackDirection(AttackDirection.Middle);
            }
        }

        private void SetAttackDirection(AttackDirection direction)
        {
            if (attackCooldownTimer > 0) return;
            if (AttackDirection.Up.Equals(direction))
            {
                animator.SetBool("Attack", true);
                animator.SetBool("AttackUp", false);
                animator.SetBool("AttackDown", false);
            }
            else if (AttackDirection.Down.Equals(direction))
            {
                animator.SetBool("Attack", true);
                animator.SetBool("AttackUp", false);
                animator.SetBool("AttackDown", false);
            }
            else
            {
                animator.SetBool("Attack", true);
                animator.SetBool("AttackUp", false);
                animator.SetBool("AttackDown", false);
            }
        }*/
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //MeleeAttack
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!hasRangeAttack)
            {
                hasReachedPlayer = true;
                animator.SetBool("hasReachedPlayer", true);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("PlayerSpell"))
        {
            if (!collision.gameObject.TryGetComponent<PlayerSpellBaseScript>(out var spellScript))
                Debug.LogWarning("PlayerSpellBaseScript component not found on PlayerSpell object!");
            else
            {
                ReceiveDamage(spellScript.Damage, criticalChance: PlayerStatsManager.Instance.RuntimeStats.Get(PlayerStatType.Attack_CriticalChance), criticalMultiplier: PlayerStatsManager.Instance.RuntimeStats.Get(PlayerStatType.Attack_CriticalDamage), color: spellScript.BaseColor, hitSound: spellScript.OnHitSound);
                if (CanApplyKnockback)
                    ApplyKnockback(collision, spellScript.KnockbackForce - KnockbackResistance);
            }

        }
        //RangeAttack
        RangeAttackDetector(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        RangeAttackDetector(collision);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (!hasRangeAttack && collision.gameObject.CompareTag("Player"))
        {
            hasReachedPlayer = false;
            animator.SetBool("hasReachedPlayer", false);
        }
    }

    public void ReceiveDamage(float spellDamage, float criticalChance, float criticalMultiplier, Color color, String hitSound)
    {
        bool isCritical = false;

        if (Random.value <= criticalChance)
        {
            isCritical = true;
            spellDamage *= criticalMultiplier;
        }

        CurrentHealth -= spellDamage;
        //Debug.Log($"CurrentHealth is {CurrentHealth} and MaxHealth is {MaxHealth} and enemyType is {EnemyType} from {this}");

        Vector2 randomOffset = new(Random.Range(-0.3f, 0.3f), Random.Range(0.5f, 1.0f));
        Vector2 spawnPosition = (Vector2)transform.position + randomOffset;

        if (isCritical)
        {
            AudioManager.Instance.PlaySoundFX(criticalHitSound, transform.position, 0.7f, 0.7f, 1.3f);
        }
        else if (hitSound != null)
        {
            AudioManager.Instance.PlaySoundFX(hitSound, transform.position, 0.7f, 0.7f, 1.3f);
        }
        else
        {
            Debug.LogWarning("hitSound is null!");
        }
        if (HurtSounds.Length > 0)
        {
            AudioManager.Instance.PlayRandomSoundFX(HurtSounds, transform.position, 1.5f, 0.75f, 1.25f);
        }
        if (!GlobalVariables.Instance.mainMenuScene)
        {
            GameObject dmgText = Instantiate(receivedDamagePopUp, spawnPosition, Quaternion.identity);
            DamageTextScript dt = dmgText.GetComponent<DamageTextScript>();
            dt.SetDamage(spellDamage, isCritical, color);
        }
        if (hasDamageParticles)
            Instantiate(damageParticles, transform.position, Quaternion.identity);
        if (isCritical)
            Instantiate(criticalParticles, transform.position, Quaternion.identity);

    }

    private IEnumerator ResetVelocityAfterDelay(float delay)
    {
        KnockbackEffect = true;
        yield return new WaitForSeconds(delay);
        KnockbackEffect = false;
    }

    private void ApplyKnockback(Collider2D collision, float knockbackForce)
    {
        if (knockbackForce < 0)
            return;
        Vector2 knockbackDir = (transform.position - collision.transform.position).normalized;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(knockbackDir * knockbackForce, ForceMode2D.Impulse);
        StartCoroutine(ResetVelocityAfterDelay(0.5f));
    }


    private void DropCollectables()
    {
        if (GlobalVariables.Instance.mainMenuScene)
            return;

        Vector3 dropPosition = transform.position;
        //Coins
        if (Random.value <= CoinDropChance)
        {
            if (GlobalVariables.CoinDropEnum.Yellow.Equals(CoinDropEnum))
            {
                GameObject coin = CoinPool.Instance.GetCoin();
                if (coin.TryGetComponent(out YellowCoinScript yellowCoinScript))
                {
                    yellowCoinScript.Initialize(dropPosition);
                }
            }
        }
        //Mana Potion
        if (Random.value <= HealthPotionChance)
        {
            GameObject manaPotion = ManaPotionPool.Instance.GetManaPotion();
            if (manaPotion.TryGetComponent(out ManaPotionScript manaPotionScript))
            {
                manaPotionScript.Initialize(dropPosition);
            }
            else
            {
                Debug.LogWarning("manaPotionScript component not found on the manaPotion prefab!");
            }
        }
        //Health Potion
        if (Random.value <= ManaPotionChance)
        {
            GameObject healthPotion = HealthPotionPool.Instance.GetHealthPotion();
            if (healthPotion.TryGetComponent(out HealthPotionScript healthPotionScript))
            {
                healthPotionScript.Initialize(dropPosition);
            }
            else
            {
                Debug.LogWarning("HealthPotion component not found on the healthPotion prefab!");
            }
        }
        //Green Ruby Drop Chance
        if (Random.value <= GreenRubyChance)
        {
            GameObject greenRuby = GreenRubyPool.Instance.Get();
            if (greenRuby.TryGetComponent(out CollectableBaseScript collectableBaseScript))
            {
                collectableBaseScript.Initialize(dropPosition);
            }
            else
            {
                Debug.LogWarning("GreenRubyScript component not found on the healthPotion prefab!");
            }
        }
        //Red Ruby Drop Chance
        if (Random.value <= GreenRubyChance)
        {
            GameObject redRuby = RedRubyPool.Instance.Get();
            if (redRuby.TryGetComponent(out CollectableBaseScript collectableBaseScript))
            {
                collectableBaseScript.Initialize(dropPosition);
            }
            else
            {
                Debug.LogWarning("GreenRubyScript component not found on the healthPotion prefab!");
            }
        }
        //Shards
        GameObject shard = ShardPool.Instance.GetShard((int)MinExp, (int)MaxExp);
        if (shard.TryGetComponent(out ParentShardScript parentShardScript))
        {
            parentShardScript.Initialize(dropPosition);
        }
    }

    //RangeAttack
    public void RangeAttackProjectile()
    {
        AudioManager.Instance.PlayRandomSoundFX(AttackSoundClips, transform.position, 0.4f, 0.75f, 1.25f);

        GameObject rangeInstance = Instantiate(projectile, projectilePosition.position, Quaternion.identity);
        rangeInstance.transform.SetParent(projectileParent, false);

        if (projectileCanRotate && player != null)
        {
            Vector2 direction = (player.position - rangeInstance.transform.position).normalized;
            float spreadAngle = Random.Range(-randomSpreadAngle, randomSpreadAngle);
            float baseAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            float finalAngle = baseAngle + spreadAngle;
            rangeInstance.transform.rotation = Quaternion.Euler(0f, 0f, finalAngle);
        }

        if (rangeInstance.TryGetComponent<EnemyProjectileBaseScript>(out var projectileScript))
        {
            projectileScript.Damage = Damage;
            projectileScript.speed = ProjectileSpeed;
        }
        else
        {
            Debug.LogWarning("Projectile script not found on instantiated object!");
        }
    }

    public virtual void EndAttackAnimation()
    {
        if (Random.value > MultipleAttackChance)
        {
            attackCooldownTimer = AttackCooldown;
        }
        animator.SetBool("Attack", false);
        animator.SetBool("hasReachedPlayer", false);
        hasReachedPlayer = false;
    }

    //Used for spells like midas etc
    public void MarkedToDie()
    {
        CurrentHealth = -1;
    }

    private void RangeAttackDetector(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (hasRangeAttack && rangeScript.IsPlayerInRange && attackCooldownTimer <= 0)
            {
                hasReachedPlayer = true;
                animator.SetBool("hasReachedPlayer", true);
                animator.SetBool("Attack", true);
            }
        }
    }

    //SpecialAttack
    public virtual void SpecialAttack()
    {
        Debug.LogWarning("SpecialAttack must be overrided for now");
    }

    public virtual void SummonStarts()
    {
        Debug.LogWarning("SummonStarts must be overrided for now");
    }

    public virtual void SummonEndsSound()
    {
        Debug.LogWarning("SummonEndsSound must be overrided for now");
    }
    public virtual void PreAttackSound()
    {
        Debug.LogWarning("PreAttackFX must be overrided for now");
    }
    public virtual void NormalAttackFX()
    {
        normalAttackFX.SetActive(true);
    }

    public virtual void SpecialAttackFX()
    {
        specialAttackFX.SetActive(true);
    }


    private void RotateAlsoFX(int yRotation)
    {
        if (normalAttackFX != null)
        {
            normalAttackFX.transform.rotation = Quaternion.Euler(0, yRotation, 0);
        }
        if (specialAttackFX != null)
        {
            specialAttackFX.transform.rotation = Quaternion.Euler(0, yRotation, 0);
        }
        if (projectilePosition != null)
        {
            projectilePosition.rotation = Quaternion.Euler(0, yRotation, 0);
        }
    }

    protected virtual void BuildStats()
    {
        if (config == null)
        {
            Debug.LogError($"{name} has no EnemyConfig assigned!", this);
            enabled = false;
            return;
        }

        stats = new EnemyStats(config.baseStats);
        MaxHealth = stats.maxHealth * DifficultyManager.Instance.FinalEnemyHealthMultiplier + DifficultyManager.Instance.CurrentTier;
        CurrentHealth = MaxHealth;
    }
}
