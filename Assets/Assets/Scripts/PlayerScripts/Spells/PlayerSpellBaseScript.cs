using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerSpellBaseScript : MonoBehaviour
{

    public virtual float Damage => GlobalVariables.Instance.defaultSpellDamage;
    public virtual float Speed => GlobalVariables.Instance.defaultSpellSpeed;
    public virtual float Bounces => GlobalVariables.Instance.defaultSpellBounces;
    public virtual float KnockbackForce => GlobalVariables.Instance.defaultKnockbackforce;
    public virtual float CriticalChance => GlobalVariables.Instance.globalCriticalChance;
    public virtual float CriticalMultiplier => GlobalVariables.Instance.globalCriticalMultiplier;
    public virtual Color BaseColor => GlobalVariables.Instance.defaultColor;


    private Rigidbody2D rb;
    private int currentBounces = 0;
    protected Color baseColor;
    [SerializeField] private GameObject particles;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
            if (Bounces > 0 && currentBounces < Bounces)
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

    public void SetVelocity(Vector2 direction, bool bounce)
    {
        if (rb == null)
        {
            Debug.LogWarning("Rb is null??");
            return;
        }
        Vector2 desiredVelocity = direction * Speed;

        if (desiredVelocity.magnitude < Speed - 2)
        {
            desiredVelocity = direction.normalized * Speed;
        }

        rb.linearVelocity = desiredVelocity;
        /*Debug.Log("linearVelocity:" + desiredVelocity + "\tBounce:" + bounce + "\trb.linearVelocity:" + rb.linearVelocity + "\tSpeed:" + Speed);*/
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
            SetVelocity(newDirection, true);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
