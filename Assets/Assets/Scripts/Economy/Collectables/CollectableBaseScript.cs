using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class CollectableBaseScript : MonoBehaviour, ICollectable
{
    protected Transform player;
    protected bool IsCollected { get; set; }
    public float speed = 6f;
    public float speedIncreaseRate = 1f; // how fast speed increases per second
    public float maxSpeed = 80f;          // limit max speed
    public float stopDistance = 0.1f;
    protected BoxCollider2D boxCollider;
    public bool isAffectedByMagnet = false;

    protected virtual void Awake()
    {
        // Cache reference once
        boxCollider = GetComponent<BoxCollider2D>();
    }

    protected virtual void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        boxCollider.enabled = true;
    }

    protected virtual void Update()
    {
        if (player == null) return;

        if (isAffectedByMagnet && GlobalVariables.Instance.magnetIsActive)
        {
            Collect();
        }

        if (IsCollected)
        {
            // increase speed gradually until maxSpeed
            speed = Mathf.Min(speed + speedIncreaseRate * Time.deltaTime, maxSpeed);

            transform.position = Vector3.MoveTowards(
                transform.position,
                player.position,
                speed * Time.deltaTime
            );

            if (Vector3.Distance(transform.position, player.position) <= stopDistance)
            {
                CollectEnds();
            }
        }
    }

    public void Initialize(Vector3 spawnPos)
    {
        transform.position = spawnPos;
        LeanTween.cancel(gameObject);
        LeanTween.moveY(gameObject, spawnPos.y + 0.3f, 0.8f)
                 .setEaseInOutSine()
                 .setLoopPingPong();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Collectable"))
        {
            Vector2 direction = (transform.position - other.transform.position);

            // If they are at the same position, apply random direction
            if (direction == Vector2.zero)
            {
                direction = Random.insideUnitCircle.normalized;
            }
            else
            {
                direction.Normalize();
            }

            float pushStrength = 0.2f; // adjust as needed
            transform.position += (Vector3)(direction * pushStrength);
        }
    }

    public virtual void Collect()
    {
        //Debug.Log("Collectable collected: " + gameObject.name);
        LeanTween.cancel(gameObject);
        IsCollected = true;
        boxCollider.enabled = false;
    }
    protected abstract void CollectEnds();
}