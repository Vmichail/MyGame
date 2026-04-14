using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyProjectileBaseScript : MonoBehaviour
{
    public float speed = 5f;
    public float lifeTime = 5f;
    public float lifeTimeMax = 4f;
    [SerializeField] GameObject particles;
    private Transform player;
    public bool hasDirection = false;
    [SerializeField] protected string[] exposionClipNames = { };
    public float projectileLifes = 1f;
    public bool spawnChildrenOnDestroy = false;
    public GameObject childGameObject;
    private Rigidbody2D rb;
    private bool wallHit = false;
    private Coroutine lifetimeRoutine;
    public virtual float Damage { get; set; }
    public virtual float Speed { get; set; }
    private Vector2 direction;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        wallHit = false;

        if (lifeTimeMax > 0)
            lifeTime = Random.Range(lifeTime, lifeTimeMax);

        if (Speed <= 0)
            Speed = speed;

        if (!hasDirection)
        {
            player = GlobalVariables.Instance.playerTransform;

            if (player != null)
                direction = (player.position - transform.position).normalized;
        }

        if (lifetimeRoutine != null)
            StopCoroutine(lifetimeRoutine);

        lifetimeRoutine = StartCoroutine(DestroyProjectileByLifetime());
    }

    private IEnumerator DestroyProjectileByLifetime()
    {
        yield return new WaitForSeconds(lifeTime);

        DestroyProjectile(true);
    }

    private void FixedUpdate()
    {
        if (direction != Vector2.zero)
            rb.linearVelocity = direction * Speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.Instance.PlaySoundFX("playerProjectileDestroy", transform.position, 0.2f, 0.9f, 1.1f);

            wallHit = true;

            DestroyProjectile();

            EnemyGenericFunctionsForPlayer.Instance.DamagePlayer(Damage);
        }
        else if (other.CompareTag("PlayerSpell"))
        {
            projectileLifes--;

            if (projectileLifes <= 0)
            {
                AudioManager.Instance.PlaySoundFX("playerProjectileDestroy", transform.position, 0.5f, 0.9f, 1.1f);

                DestroyProjectile();
            }
        }
        else if (other.CompareTag("Wall"))
        {
            AudioManager.Instance.PlaySoundFX("wallHitSpell", transform.position, 0.8f, 0.9f, 1.1f, false, true);

            wallHit = true;

            DestroyProjectile();
        }
    }

    private void DestroyProjectile(bool wasLifeTimeDestroy = false)
    {
        if (exposionClipNames.Length > 0)
            AudioManager.Instance.PlayRandomSoundFX(exposionClipNames, transform.position, 0.5f, 0.9f, 1.1f);

        if (spawnChildrenOnDestroy && childGameObject && !wallHit && wasLifeTimeDestroy)
        {
            if (Random.value > 0.5f)
                SpawnDiagonalPattern();
            else
                SpawnCrossPattern();
        }

        PoolManager.Instance.Get(particles, transform.position, Quaternion.identity, PoolCategory.Particles);

        PoolManager.Instance.Return(gameObject);
    }

    private void SpawnCrossPattern()
    {
        Vector2[] directions =
        {
            Vector2.up, Vector2.down, Vector2.left, Vector2.right
        };

        foreach (var dir in directions)
            SpawnProjectile(dir);
    }

    private void SpawnDiagonalPattern()
    {
        Vector2[] directions =
        {
            new Vector2(1,1).normalized,
            new Vector2(-1,1).normalized,
            new Vector2(1,-1).normalized,
            new Vector2(-1,-1).normalized
        };

        foreach (var dir in directions)
            SpawnProjectile(dir);
    }

    private void SpawnProjectile(Vector2 direction)
    {
        GameObject proj = PoolManager.Instance.Get(childGameObject, transform.position, Quaternion.identity, PoolCategory.Enemy);

        proj.transform.localScale *= 0.6f;

        float finalSpeed = Mathf.Max(0f, Speed);

        if (proj.TryGetComponent<Rigidbody2D>(out var rb))
            rb.linearVelocity = direction * finalSpeed;

        if (proj.TryGetComponent<EnemyProjectileBaseScript>(out var projectileScript))
        {
            projectileScript.Damage = Damage;
            projectileScript.Speed = Speed;
            projectileScript.hasDirection = true;
            projectileScript.spawnChildrenOnDestroy = false;
        }
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir;
    }
}