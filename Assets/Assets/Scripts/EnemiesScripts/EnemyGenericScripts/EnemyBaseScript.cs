using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseScript : MonoBehaviour
{
    [SerializeField] GameObject particles;
    [SerializeField] GameObject damageText;
    [SerializeField] private bool canApplyKnockback;
    [SerializeField] GlobalVariables.Direction directionEnum;

    protected Transform player;
    private bool hasReachedPlayer;
    private Animator animator;
    protected float maxHealth = GlobalVariables.defaultEnemyHealth;
    protected float currentHealth;
    protected float knockbackResistance = GlobalVariables.defaultKnockbackResistance;

    private Rigidbody2D rb;
    private bool knockBackEffect;
    private Transform spriteTransform;

    public virtual float MaxHealth { get; set; }
    public virtual float CurrentHealth { get; set; }

    protected virtual void Awake()
    {
        EnemyManagerScript.Instance.RegisterEnemy(gameObject);
    }

    protected virtual void Start()
    {
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

    protected virtual void Update()
    {
        if (currentHealth <= 0)
        {
            Instantiate(particles, transform.position, transform.rotation);
            EnemyManagerScript.Instance.UnregisterEnemy(gameObject);
            Destroy(gameObject);
        }
    }

    protected virtual void FixedUpdate()
    {
        if (player != null)
        {
            MoveTowardPlayer(GlobalVariables.defaultEnemySpeed);
        }
    }

    protected virtual void MoveTowardPlayer()
    {
        MoveTowardPlayer(GlobalVariables.defaultEnemySpeed);
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
        rb.linearVelocity = direction * speed;
        if (direction.x < 0)
            spriteTransform.rotation = new Quaternion(0, 180, 0, 0);
        else
        {
            spriteTransform.rotation = new Quaternion(0, 0, 0, 0);
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
            if (canApplyKnockback)
                applyKnockback(collision, spellScript.KnowckbackForce - knockbackResistance);
            if (spellScript == null)
                Debug.LogWarning("PlayerSpellBaseScript component not found on PlayerSpell object!");
            else
            {
                DamageEnemy(spellScript.Damage);
            }
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

    private void DamageEnemy(float spellDamage)
    {
        bool isCritical = false;

        if (Random.value <= GlobalVariables.criticalChance)
        {
            isCritical = true;
            spellDamage *= GlobalVariables.criticalMultiplier;
        }

        CurrentHealth -= spellDamage;

        Vector2 randomOffset = new Vector2(
        Random.Range(-0.3f, 0.3f),
        Random.Range(0.5f, 1.0f));

        Vector2 spawnPosition = (Vector2)transform.position + randomOffset;



        GameObject dmgText = Instantiate(damageText, spawnPosition, Quaternion.identity);


        DamageTextScript dt = dmgText.GetComponent<DamageTextScript>();
        dt.SetDamage(spellDamage, isCritical);

    }

    private IEnumerator ResetVelocityAfterDelay(float delay)
    {
        knockBackEffect = true;
        yield return new WaitForSeconds(delay);
        knockBackEffect = false;
    }

    private void applyKnockback(Collider2D collision, float knockbackForce)
    {
        if (knockbackForce < 0)
            return;
        Vector2 knockbackDir = (transform.position - collision.transform.position).normalized;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(knockbackDir * knockbackForce, ForceMode2D.Impulse);
        StartCoroutine(ResetVelocityAfterDelay(0.5f));
    }

}
