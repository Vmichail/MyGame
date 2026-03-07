using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]
public class EnemyFireProjectile : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] private float damagePerTick = 3;
    [SerializeField] private float damageInterval = 1f;

    [Header("Lifetime")]
    [SerializeField] private float lifeTime = 5f;

    private float nextDamageTime;
    private Collider2D fireCollider;
    private Animator animator;
    private bool ending;

    private void Awake()
    {
        fireCollider = GetComponent<BoxCollider2D>();
        fireCollider.isTrigger = true;
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (PlayerStatsManager.Instance.CurrentLevel > 20)
        {
            lifeTime = Random.Range(lifeTime, lifeTime * 2);
        }
        Invoke(nameof(EndProjectile), lifeTime);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (ending || !other.CompareTag("Player"))
            return;

        if (Time.time >= nextDamageTime)
        {
            AudioManager.Instance.PlaySoundFX("burnSoundEffect", transform.position, 0.3f, 0.9f, 1.1f);
            EnemyGenericFunctionsForPlayer.Instance.DamagePlayer(damagePerTick + DifficultyManager.Instance.CurrentTier);
            nextDamageTime = Time.time + damageInterval;
        }
    }

    private void EndProjectile()
    {
        ending = true;
        fireCollider.enabled = false;
        animator.SetBool("End", true);
    }

    // Called from animation event
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}