using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SocialPlatforms.Impl;

public abstract class EnemyBaseScript : MonoBehaviour
{
    [SerializeField] GameObject particles;
    [SerializeField] GameObject damageTextPopUp;
    [SerializeField] GameObject receivedDamagePopUp;
    [SerializeField] private bool canApplyKnockback;
    [SerializeField] protected GlobalVariables.EnemyRarity rarity = GlobalVariables.EnemyRarity.None;
    [SerializeField] protected bool hasAttackAnimation = false;
    [Header("--!!RangeAtributes!!--")]
    private EnemyRangeAttackScript rangeScript;
    private bool hasRangeAttack = false;
    [SerializeField] GameObject projectile;
    [SerializeField] Transform projectilePosition;
    [SerializeField] Transform projectileParent;

    public bool isDead = false;
    public virtual float ProjectileSpeed { get => GlobalVariables.Instance.goblinTNTProjectileSpeed; }

    protected Transform player;
    public bool hasReachedPlayer;
    public Animator animator;

    protected ObjectPool<GameObject> _pool;

    public void SetPool(ObjectPool<GameObject> pool)
    {
        _pool = pool;
    }

    private Rigidbody2D rb;
    public bool knockBackEffect;
    private protected Transform spriteTransform;
    private Coroutine damageCoroutine;
    protected float maxHealth;
    protected float knockbackResistance;
    protected float currentHealth;
    protected float damage;
    private float cameraZ;

    public virtual float Speed { get; set; }
    public virtual float MaxHealth { get; set; }
    public virtual float CurrentHealth { get; set; }
    public virtual float Damage { get => GlobalVariables.Instance.skeletonDamage; }
    public virtual float AttackCooldown { get => GlobalVariables.Instance.skeletonAttackCooldown; }
    public virtual float CoinDropChance { get => GlobalVariables.Instance.skeletonAttackCooldown; }
    public virtual GlobalVariables.CoinDropEnum CoinDropEnum { get => GlobalVariables.CoinDropEnum.Yellow; }


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

    protected virtual void Start()
    {
        InstantiateVariables();
        rangeScript = GetComponentInChildren<EnemyRangeAttackScript>();
        hasRangeAttack = rangeScript != null;
        EnemyManagerScript.Instance.RegisterEnemy(gameObject);
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();

        if (spriteRenderer != null)
            spriteTransform = spriteRenderer.transform;
        else
            Debug.LogWarning("No Sprite was found!");
        findPlayer();


        rb = GetComponent<Rigidbody2D>();
    }

    private void findPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    private void InstantiateVariables()
    {
        maxHealth = GlobalVariables.Instance.defaultEnemyHealth;
        knockbackResistance = GlobalVariables.Instance.defaultKnockbackResistance;
        Speed = GlobalVariables.Instance.defaultEnemySpeed;
    }

    protected virtual void Update()
    {
        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            TryDropCoin();
            Instantiate(particles, transform.position, transform.rotation);
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
        float damage = EnemyGenericFunctions.DamagePlayer(Damage);
        Vector2 randomOffset = new(Random.Range(-0.3f, 0.3f), Random.Range(0.5f, 1.0f));
        Vector2 spawnPosition = (Vector2)player.transform.position + randomOffset;
        GameObject dmgText = Instantiate(receivedDamagePopUp, spawnPosition, Quaternion.identity);
        DamageTextScript dt = dmgText.GetComponent<DamageTextScript>();
        dt.SetDamage(damage, false, Color.red);
    }

    protected void MoveTowardPlayer()
    {
        if (knockBackEffect)
            return;
        else if (hasReachedPlayer)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * Speed;
        if (direction.x < 0)
            spriteTransform.rotation = Quaternion.Euler(0, 180, 0);
        else
        {
            spriteTransform.rotation = Quaternion.Euler(0, 0, 0);
        }
        if (hasAttackAnimation)
        {
            GetAttackDirection(direction);
        }
    }

    private void GetAttackDirection(Vector2 dir)
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
        if (AttackDirection.Up.Equals(direction))
        {
            animator.SetBool("Attack", false);
            animator.SetBool("AttackUp", true);
            animator.SetBool("AttackDown", false);
        }
        else if (AttackDirection.Down.Equals(direction))
        {
            animator.SetBool("Attack", false);
            animator.SetBool("AttackUp", false);
            animator.SetBool("AttackDown", true);
        }
        else
        {
            animator.SetBool("Attack", true);
            animator.SetBool("AttackUp", false);
            animator.SetBool("AttackDown", false);
        }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player") && !hasRangeAttack)
        {
            hasReachedPlayer = true;
            animator.SetBool("hasReachedPlayer", true);
        }
        else if (hasRangeAttack && rangeScript.IsPlayerInRange)
        {
            hasReachedPlayer = true;
            animator.SetBool("hasReachedPlayer", true);
        }

        if (collision.gameObject.CompareTag("PlayerSpell"))
        {
            if (!collision.gameObject.TryGetComponent<PlayerSpellBaseScript>(out var spellScript))
                Debug.LogWarning("PlayerSpellBaseScript component not found on PlayerSpell object!");
            else
            {
                DamageEnemy(spellScript.Damage, criticalChance: spellScript.CriticalChance, criticalMultiplier: spellScript.CriticalMultiplier, color: spellScript.BaseColor);
            }
            if (canApplyKnockback)
                ApplyKnockback(collision, spellScript.KnockbackForce - knockbackResistance);

        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!hasRangeAttack && collision.gameObject.CompareTag("Player"))
        {
            hasReachedPlayer = false;
            animator.SetBool("hasReachedPlayer", false);
        }
    }

    private void DamageEnemy(float spellDamage, float criticalChance, float criticalMultiplier, Color color)
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
        GameObject dmgText = Instantiate(damageTextPopUp, spawnPosition, Quaternion.identity);

        DamageTextScript dt = dmgText.GetComponent<DamageTextScript>();
        dt.SetDamage(spellDamage, isCritical, color);

    }

    private IEnumerator ResetVelocityAfterDelay(float delay)
    {
        knockBackEffect = true;
        yield return new WaitForSeconds(delay);
        knockBackEffect = false;
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


    private void TryDropCoin()
    {

        if (Random.value <= CoinDropChance)
        {
            if (GlobalVariables.CoinDropEnum.Yellow.Equals(CoinDropEnum))
            {
                GameObject coin = CoinPoolScript.Instance.GetCoin();
                Vector3 dropPosition = transform.position;
                dropPosition.z = cameraZ;
                coin.transform.position = dropPosition;
            }
        }
    }

    //RangeAttack
    public void RangeAttackProjectile()
    {
        GameObject rangeInstance = Instantiate(projectile, projectilePosition.position, Quaternion.identity);
        rangeInstance.transform.SetParent(projectileParent, false);
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


}
