using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyChargeAttack : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnemyBaseScript enemyBaseScript;
    private Transform player;
    private Animator animator;
    private AnimatorStateInfo animStateInfo;

    [Header("Indicator")]
    [SerializeField] private GameObject indicatorRoot;
    [SerializeField] private Transform maskTransform; // red square mask
    //[SerializeField] private SpriteRenderer arrowRenderer;

    [Header("Charge Settings")]
    [SerializeField] private int chargeAdditionalDamage = 2;
    [SerializeField] private float chargeUpTime = 1.2f;
    [SerializeField] private float chargeSpeed = 14f;
    [SerializeField] private float chargeDuration = 0.5f;
    [SerializeField] private float cooldown = 2.5f;
    [SerializeField] private float triggerDistance = 6f;
    [SerializeField] private float aimRandomAngle = 8f; // degrees

    [Header("Physics Layers")]
    [SerializeField] private string normalLayer = "Enemy";
    [SerializeField] private string chargingLayer = "ChargingEnemy";

    private Rigidbody2D rb;
    private bool isCharging;
    private bool isOnCooldown;
    private Vector2 lockedDirection;
    private bool hasHitPlayer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (enemyBaseScript == null)
        {
            enemyBaseScript = GetComponent<EnemyBaseScript>();
        }
        indicatorRoot.SetActive(false);
    }

    private void Start()
    {
        player = enemyBaseScript.GetPlayerTransform();
        if (player == null)
        {
            Debug.LogError("Player transform not found by EnemyChargeAttack.");
        }
        animator = enemyBaseScript.GetEnemyAnimator();
    }

    void Update()
    {
        if (isCharging || isOnCooldown || player == null)
            return;

        float dist = Vector2.Distance(transform.position, player.position);
        if (dist <= triggerDistance)
        {
            StartCoroutine(ChargeRoutine());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isCharging || hasHitPlayer)
            return;

        if (collision.gameObject.CompareTag("Player"))
        {
            hasHitPlayer = true;
            enemyBaseScript.DoAdditionalDamage(chargeAdditionalDamage);
        }
    }

    private IEnumerator ChargeRoutine()
    {
        hasHitPlayer = false;
        isOnCooldown = true;

        animator.SetBool("isPreparingCharge", true);
        enemyBaseScript.LockMovement(true);

        // 🔥 SWITCH TO CHARGING LAYER
        gameObject.layer = LayerMask.NameToLayer(chargingLayer);

        Vector2 baseDir = (player.position - transform.position).normalized;
        float randomAngle = Random.Range(-aimRandomAngle, aimRandomAngle);
        lockedDirection = Quaternion.Euler(0, 0, randomAngle) * baseDir;

        SetupIndicator(lockedDirection);
        indicatorRoot.SetActive(true);

        float t = 0f;
        while (t < chargeUpTime)
        {
            float fill = t / chargeUpTime;
            maskTransform.localScale = new Vector3(1f, fill, 1f);

            t += Time.deltaTime;
            yield return null;
        }

        maskTransform.localScale = Vector3.one;
        indicatorRoot.SetActive(false);

        // 🚀 START CHARGE
        isCharging = true;
        animator.SetBool("isPreparingCharge", false);
        animator.SetBool("isCharging", true);

        float timer = 0f;
        while (timer < chargeDuration)
        {
            rb.linearVelocity = lockedDirection * chargeSpeed;
            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        // 🛑 END CHARGE
        rb.linearVelocity = Vector2.zero;
        isCharging = false;
        animator.SetBool("isCharging", false);

        // 🔄 RESTORE NORMAL LAYER
        gameObject.layer = LayerMask.NameToLayer(normalLayer);

        enemyBaseScript.LockMovement(false);

        yield return new WaitForSeconds(cooldown);
        isOnCooldown = false;
    }

    void OnDisable()
    {
        // 🔄 Restore layer
        int normal = LayerMask.NameToLayer(normalLayer);
        if (normal != -1)
            gameObject.layer = normal;

        // 🛑 Stop all coroutines
        StopAllCoroutines();

        // 🧠 Reset state flags
        isCharging = false;
        isOnCooldown = false;
        hasHitPlayer = false;

        // 🏃 Stop movement
        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        // 🔓 Unlock base movement
        if (enemyBaseScript != null)
            enemyBaseScript.LockMovement(false);

        // 🎞 Reset animator
        if (animator != null)
        {
            animator.SetBool("isCharging", false);
            animator.SetBool("isPreparingCharge", false);
        }

        // 🚫 Hide indicator + reset fill
        if (indicatorRoot != null)
            indicatorRoot.SetActive(false);

        if (maskTransform != null)
            maskTransform.localScale = new Vector3(0f, 1f, 1f);
    }

    private void SetupIndicator(Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        indicatorRoot.transform.rotation = Quaternion.Euler(0, 0, angle);
        maskTransform.localScale = new Vector3(1f, 0f, 1f);
    }
}