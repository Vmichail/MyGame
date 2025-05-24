using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerSpellBaseScript : MonoBehaviour
{
    protected float damage;
    public virtual float Damage => damage;
    public virtual float KnowckbackForce => knowckbackForce;

    private Rigidbody2D rb;
    protected float speed;
    protected int bounces;
    protected float knowckbackForce;
    private int currentBounces = 0;
    public GlobalVariables.Direction moveDirection;
    [SerializeField] private GameObject particles;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        speed = GlobalVariables.defaultSpellSpeed;
        damage = GlobalVariables.defaultSpellDamage;
        bounces = GlobalVariables.defaultSpellBounces;
        knowckbackForce = GlobalVariables.defaultKnockbackforce;
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (particles != null)
            {
                Instantiate(particles, transform.position, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("Particles prefab not assigned!");
            }
            if (bounces > 1 && currentBounces < bounces)
                Bounce(collision.gameObject);
            else
                Destroy(gameObject);
        }

    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Map"))
        {
            Destroy(gameObject);
        }
    }

    public void SetVelocity(Vector2 direction)
    {
        if (rb == null)
        {
            Debug.Log("Rb is null??");
            return;
        }
        rb.linearVelocity = direction * speed;
    }

    private GameObject FindClosestEnemy(GameObject exclude)
    {
        GameObject closest = null;
        float minDistance = float.MaxValue;

        foreach (GameObject enemy in EnemyManagerScript.Instance.ActiveEnemies)
        {
            if (enemy == exclude || enemy == null) continue;

            float dist = Vector2.Distance(transform.position, enemy.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                closest = enemy;
            }
        }

        return closest;
    }


    private void Bounce(GameObject lastHitEnemy)
    {
        currentBounces++;
        GameObject nextEnemy = FindClosestEnemy(exclude: lastHitEnemy);
        if (nextEnemy != null)
        {
            Vector2 newDirection = (nextEnemy.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(newDirection.y, newDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            SetVelocity(newDirection);
        }
    }
}
