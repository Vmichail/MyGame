using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SocialPlatforms.Impl;

public abstract class EnemyBaseScript : MonoBehaviour
{
    [SerializeField] GameObject particles;
    [SerializeField] GameObject damageText;
    [SerializeField] private bool canApplyKnockback;
    [SerializeField] protected GlobalVariables.EnemyRarity rarity = GlobalVariables.EnemyRarity.None;
    [SerializeField] protected bool hasAttackAnimation = false;

    protected Transform player;
    private bool hasReachedPlayer;
    private Animator animator;

    protected ObjectPool<GameObject> _pool;

    public void SetPool(ObjectPool<GameObject> pool)
    {
        _pool = pool;
    }

    private Rigidbody2D rb;
    private bool knockBackEffect;
    private protected Transform spriteTransform;
    private Coroutine damageCoroutine;
    protected float maxHealth;
    protected float knockbackResistance;
    protected float speed;
    protected float currentHealth;
    protected float damage;

    public virtual float MaxHealth { get; set; }
    public virtual float CurrentHealth { get; set; }
    public virtual float Damage { get => GlobalVariables.Instance.skeletonDamage; }
    public virtual float AttackCooldown { get => GlobalVariables.Instance.skeletonAttackCooldown; }

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

    }

    protected virtual void Start()
    {
        InstantiateVariables();
        EnemyManagerScript.Instance.RegisterEnemy(gameObject);
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();

        if (spriteRenderer != null)
            spriteTransform = spriteRenderer.transform;
        else
            Debug.LogWarning("No Sprite was found!");

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        rb = GetComponent<Rigidbody2D>();
    }

    private void InstantiateVariables()
    {
        maxHealth = GlobalVariables.Instance.defaultEnemyHealth;
        knockbackResistance = GlobalVariables.Instance.defaultKnockbackResistance;
        speed = GlobalVariables.Instance.defaultEnemySpeed;
    }

    protected virtual void Update()
    {
        if (currentHealth <= 0)
        {
            Instantiate(particles, transform.position, transform.rotation);
            EnemyManagerScript.Instance.UnregisterEnemy(gameObject);
            _pool.Release(gameObject);
        }
    }

    protected virtual void FixedUpdate()
    {
        if (player == null)
            return;

        MoveTowardPlayer(speed);

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
        GlobalVariables.Instance.playerCurrentHealth -= Damage;
        /*Debug.Log($"EnemyTypeClass: {GetType()} did {Damage} to player! but Damage should be {GlobalVariables.Instance.goblinDamage}");*/
    }

    protected void MoveTowardPlayer(float speed)
    {
        if (knockBackEffect)
            return;
        else if (hasReachedPlayer)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * this.speed;
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
        if (collision.gameObject.CompareTag("Player"))
        {
            hasReachedPlayer = true;
            animator.SetBool("hasReachedPlayer", true);
        }
        else if (collision.gameObject.CompareTag("PlayerSpell"))
        {
            PlayerSpellBaseScript spellScript = collision.gameObject.GetComponent<PlayerSpellBaseScript>();
            if (spellScript == null)
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
        if (collision.gameObject.CompareTag("Player"))
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

        Vector2 randomOffset = new Vector2(
        Random.Range(-0.3f, 0.3f),
        Random.Range(0.5f, 1.0f));

        Vector2 spawnPosition = (Vector2)transform.position + randomOffset;



        GameObject dmgText = Instantiate(damageText, spawnPosition, Quaternion.identity);


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

}
