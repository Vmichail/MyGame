using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyProjectileBaseScript : MonoBehaviour
{
    public float speed = 5f;
    public float lifeTime = 5f;
    public float lifeTimeMax = 4f;
    [SerializeField] GameObject particles;
    [SerializeField] GameObject receivedDamagePopUp;
    private GameObject player;
    public bool hasDirection = false;
    [SerializeField] protected string[] exposionClipNames = { };
    public float projectileLifes = 1f;
    public bool spawnChildrenOnDestroy = false;
    public GameObject childGameObject;
    private Rigidbody2D rb;
    private bool wallHit = false;

    public virtual float Damage { get; set; }
    public virtual float Speed { get; set; }

    private Vector2 direction;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (lifeTimeMax > 0)
            lifeTime = Random.Range(lifeTime, lifeTimeMax);
        Destroy(gameObject, lifeTime);
        if (Speed <= 0)
            Speed = speed;

        if (!hasDirection)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                direction = (player.transform.position - transform.position).normalized;
            }
            else
            {
                direction = Vector2.zero;
                Debug.LogWarning("Player not found!");
            }
        }
    }

    void FixedUpdate()
    {
        if (rb != null && direction != Vector2.zero)
        {
            rb.linearVelocity = direction * Speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.Instance.PlaySoundFX("playerProjectileDestroy", transform.position, 0.2f, 0.9f, 1.1f);
            wallHit = true;
            Destroy(gameObject);
            EnemyGenericFunctionsForPlayer.Instance.DamagePlayer(Damage);
        }
        else if (other.CompareTag("PlayerSpell"))
        {
            projectileLifes--;
            if (projectileLifes <= 0)
            {
                AudioManager.Instance.PlaySoundFX("playerProjectileDestroy", transform.position, 0.5f, 0.9f, 1.1f);
                Destroy(gameObject);
            }
        }
        else if (other.CompareTag("Wall"))
        {
            AudioManager.Instance.PlaySoundFX("wallHitSpell", transform.position, 0.8f, 0.9f, 1.1f, false, true);
            wallHit = true;
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (exposionClipNames.Length > 0)
            AudioManager.Instance.PlayRandomSoundFX(exposionClipNames, transform.position, 0.5f, 0.9f, 1.1f);
        if (spawnChildrenOnDestroy && childGameObject && !wallHit)
        {
            if (Random.value > 0.5f)
            {
                SpawnDiagonalPattern();
            }
            else
            {
                SpawnCrossPattern();
            }
        }
        Instantiate(particles, transform.position, Quaternion.identity);
    }

    /// <summary>
    /// Child spawn
    /// </summary>

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

    private void SpawnProjectile(Vector2 direction)
    {
        GameObject proj = Instantiate(childGameObject, transform.position, Quaternion.identity);
        proj.transform.localScale *= 0.6f;


        float finalSpeed = Mathf.Max(0f, Speed);

        // If the projectile moves via RB velocity, set it:
        if (proj.TryGetComponent<Rigidbody2D>(out var rb))
        {
            rb.linearVelocity = direction * finalSpeed;
        }

        if (proj.TryGetComponent<EnemyProjectileBaseScript>(out var projectileScript))
        {
            projectileScript.Damage = Damage;
            projectileScript.Speed = Speed;
            projectileScript.hasDirection = true;
            projectileScript.spawnChildrenOnDestroy = false;
        }

        //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //proj.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}