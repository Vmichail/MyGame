using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class CollectableBaseScript : MonoBehaviour, ICollectable
{
    protected Transform player;
    protected bool IsCollected { get; set; }

    public float speed = 6f;
    public float speedIncreaseRate = 1f;
    public float maxSpeed = 80f;
    public float stopDistance = 0.1f;
    public bool isAffectedByMagnet = false;

    protected BoxCollider2D boxCollider;

    private Tween idleTween;

    protected virtual void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    protected virtual void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        boxCollider.enabled = true;
        IsCollected = false;
    }

    protected virtual void Update()
    {
        if (player == null) return;

        if (isAffectedByMagnet && GlobalVariables.Instance.magnetIsActive)
        {
            Collect();
        }

        if (!IsCollected)
            return;

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

    public void Initialize(Vector3 spawnPos)
    {
        transform.position = spawnPos;

        // Kill old tween safely
        idleTween?.Kill();
        transform.DOKill();

        idleTween = transform
            .DOMoveY(spawnPos.y + 0.3f, 0.8f)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo)
            .SetTarget(this);
    }

    private void OnDisable()
    {
        // DOTween safety: always kill
        idleTween?.Kill();
        transform.DOKill();
    }

    public virtual void Collect()
    {
        if (IsCollected)
            return;

        IsCollected = true;
        boxCollider.enabled = false;

        idleTween?.Kill();
        transform.DOKill();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Collectable"))
            return;

        Vector2 direction = (Vector2)(transform.position - other.transform.position);
        direction = direction == Vector2.zero
            ? Random.insideUnitCircle.normalized
            : direction.normalized;

        float pushStrength = 0.2f;
        transform.position += (Vector3)(direction * pushStrength);
    }

    protected abstract void CollectEnds();
}