using UnityEngine;

public class EnemyProjectileBaseScript : MonoBehaviour
{
    public float speed = 5f;
    public float lifeTime = 5f;
    [SerializeField] GameObject particles;
    [SerializeField] GameObject receivedDamagePopUp;
    private GameObject player;

    public virtual float Damage { get; set; }
    public virtual float Speed { get; set; }

    private Vector2 direction;

    void Start()
    {
        Destroy(gameObject, lifeTime);
        if (Speed <= 0)
            Speed = speed;

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

    void Update()
    {
        if (direction != Vector2.zero)
        {
            transform.Translate(Speed * Time.deltaTime * (Vector3)direction, Space.World);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.Instance.PlaySoundFX("playerProjectileDestroy", transform.position, 0.2f, 0.9f, 1.1f);
            Destroy(gameObject);
            Instantiate(particles, transform.position, Quaternion.identity);
            float damage = EnemyGenericFunctions.DamagePlayer(Damage);
            Vector2 randomOffset = new(Random.Range(-0.3f, 0.3f), Random.Range(0.5f, 1.0f));
            Vector2 spawnPosition = (Vector2)player.transform.position + randomOffset;
            GameObject dmgText = Instantiate(receivedDamagePopUp, spawnPosition, Quaternion.identity);
            DamageTextScript dt = dmgText.GetComponent<DamageTextScript>();
            dt.SetDamage(damage, false, Color.red);
        }
        else if (other.CompareTag("PlayerSpell"))
        {
            AudioManager.Instance.PlaySoundFX("playerProjectileDestroy", transform.position, 0.5f, 0.9f, 1.1f);
            Instantiate(particles, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

    }
}