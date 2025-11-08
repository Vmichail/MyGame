using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SocialPlatforms.Impl;

public abstract class EnemyBaseScript : MonoBehaviour
{
    [SerializeField] GameObject particles;
    [SerializeField] GameObject receivedDamagePopUp;
    [SerializeField] GameObject playerWasHitDamage;
    [SerializeField] private bool canApplyKnockback;
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
    protected float attackCooldownTimer = 0f;
    public virtual float ProjectileSpeed { get => GlobalVariables.Instance.goblinTNTProjectileSpeed; }

    protected Transform player;
    public bool hasReachedPlayer;
    [SerializeField] protected Animator animator;
    public void SetAnimatorValue(string animatorValue, bool status)
    {
        animator.SetBool(animatorValue, status);
    }

    protected ObjectPool<GameObject> _pool;

    public void SetPool(ObjectPool<GameObject> pool)
    {
        _pool = pool;
    }

    private Rigidbody2D rb;
    private protected Transform spriteTransform;
    private Coroutine damageCoroutine;
    protected float maxHealth;
    protected float knockbackResistance;
    protected float currentHealth;
    private float cameraZ;

    public virtual bool IsDead { get; set; }
    public virtual float Speed { get; set; }
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
    public virtual float Damage { get => GlobalVariables.Instance.skeletonDamage; }
    public virtual float AttackCooldown { get => GlobalVariables.Instance.skeletonAttackCooldown; }

    public virtual GlobalVariables.CoinDropEnum CoinDropEnum { get => GlobalVariables.CoinDropEnum.Yellow; }
    public virtual float MinExp { get => GlobalVariables.Instance.skeletonMinExp; }
    public virtual float MaxExp { get => GlobalVariables.Instance.skeletonMaxExp; }
    public virtual float AttackRange { get => GlobalVariables.Instance.goblinTNTRange; }
    public virtual float MultipleAttackChance { get => GlobalVariables.Instance.goblinTNTMultipleAttackChance; }
    public virtual bool KnockbackEffect { get; set; }
    //Collectables 
    public virtual float CoinDropChance { get => GlobalVariables.Instance.skeleonCoinDropChance; }
    public virtual float HealthPotionChance { get => GlobalVariables.Instance.skeleonCoinDropChance; }
    public virtual float ManaPotionChance { get => GlobalVariables.Instance.skeleonCoinDropChance; }
    public virtual GlobalVariables.EnemyTypes EnemyType { get; set; }
    public virtual string DeathSoundClip { get => "skeletonDeadSound"; }
    AnimatorStateInfo stateInfo;

    public virtual string[] AttackSoundClip
    {
        get => new string[] { "vampireAttackSound1", "vampireAttackSound2" };
    }



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

    protected virtual void OnEnable()
    {
        IsDead = false;
        KnockbackEffect = false;
        currentHealth = maxHealth;
        hasReachedPlayer = false;
    }

    protected virtual void Start()
    {
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
        if (CurrentHealth < 1 && !IsDead)
        {
            IsDead = true;
            DropCollectables();
            Instantiate(particles, transform.position, transform.rotation);
            AudioManager.Instance.PlaySoundFX(DeathSoundClip, transform.position, 1f, 0.75f, 1.25f);
            EnemyManagerScript.Instance.UnregisterEnemy(gameObject);
            _pool.Release(gameObject);
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
        if (GlobalVariables.Instance.playerInvulnerableReasons.Count > 0)
            return;
        float damage = EnemyGenericFunctions.DamagePlayer(Damage);
        Vector2 randomOffset = new(Random.Range(-0.3f, 0.3f), Random.Range(0.5f, 1.0f));
        Vector2 spawnPosition = (Vector2)player.transform.position + randomOffset;
        GameObject dmgText = Instantiate(playerWasHitDamage, spawnPosition, Quaternion.identity);
        DamageTextScript dt = dmgText.GetComponent<DamageTextScript>();
        dt.SetDamage(damage, false, Color.red);
    }

    protected void MoveTowardPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        if (KnockbackEffect)
            return;
        else if (hasReachedPlayer || !stateInfo.IsName("Walk"))
        {
            rb.linearVelocity = Vector2.zero;
        }
        else
        {
            rb.linearVelocity = direction * Speed;
        }
        if (spriteIsFacingLeft)
        {
            if (direction.x < 0)
                spriteTransform.rotation = Quaternion.Euler(0, 0, 0);
            else
            {
                spriteTransform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
        else
        {
            if (direction.x < 0)
                spriteTransform.rotation = Quaternion.Euler(0, 180, 0);
            else
            {
                spriteTransform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
        if (hasAttackAnimation && !hasRangeAttack && attackCooldownTimer <= 0 && !stateInfo.IsName("Attack"))
        {
            /*GetAttackDirection(direction);*/
            animator.SetBool("Attack", true);
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
                if (spellScript.OnHitSound != null)
                {
                    AudioManager.Instance.PlaySoundFX(spellScript.OnHitSound, transform.position, 0.4f, 0.75f, 1.25f);
                }
                ReceiveDamage(spellScript.Damage, criticalChance: spellScript.CriticalChance, criticalMultiplier: spellScript.CriticalMultiplier, color: spellScript.BaseColor);
                if (canApplyKnockback)
                    ApplyKnockback(collision, spellScript.KnockbackForce - knockbackResistance);
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

    public void ReceiveDamage(float spellDamage, float criticalChance, float criticalMultiplier, Color color)
    {
        bool isCritical = false;

        if (Random.value <= criticalChance)
        {
            isCritical = true;
            spellDamage *= criticalMultiplier;
        }

        CurrentHealth -= spellDamage;

        Vector2 randomOffset = new(Random.Range(-0.3f, 0.3f), Random.Range(0.5f, 1.0f));
        Vector2 spawnPosition = (Vector2)transform.position + randomOffset;
        GameObject dmgText = Instantiate(receivedDamagePopUp, spawnPosition, Quaternion.identity);

        DamageTextScript dt = dmgText.GetComponent<DamageTextScript>();
        dt.SetDamage(spellDamage, isCritical, color);

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
        AudioManager.Instance.PlayRandomSoundFX(AttackSoundClip, transform.position, 0.4f, 0.75f, 1.25f);

        GameObject rangeInstance = Instantiate(projectile, projectilePosition.position, Quaternion.identity);
        rangeInstance.transform.SetParent(projectileParent, false);

        if (projectileCanRotate && player != null)
        {
            Vector2 direction = (player.position - rangeInstance.transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rangeInstance.transform.rotation = Quaternion.Euler(0f, 0f, angle);
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
        Debug.LogWarning("SpecialAttack most be overrided for now");
    }

}
