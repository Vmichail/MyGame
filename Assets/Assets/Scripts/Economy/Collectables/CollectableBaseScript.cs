using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering.Universal.Internal;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class CollectableBaseScript : MonoBehaviour, ICollectable
{
    protected Transform playerTransform;
    protected bool IsCollected { get; set; }
    private static readonly string sortingLayerName = "Ground";
    public float speed = 6f;
    public float initialSpeed = 6f;
    public float speedIncreaseRate = 1f;
    public float maxSpeed = 80f;
    public float stopDistance = 0.1f;
    public bool isAffectedByMagnet = false;
    [SerializeField] private CollectableType collectableType;

    [SerializeField] private SpriteRenderer spriteRenderer;

    protected BoxCollider2D boxCollider;

    private Tween idleTween;

    protected virtual void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        initialSpeed = speed;
    }

    protected virtual void OnEnable()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            ApplySorting();
        }
        speed = initialSpeed;
        playerTransform = GlobalVariables.Instance.playerTransform;
        boxCollider.enabled = true;
        IsCollected = false;
    }

    protected virtual void Update()
    {
        if (playerTransform == null) return;

        if (isAffectedByMagnet && GlobalVariables.Instance.magnetIsActive)
        {
            Collect();
        }

        if (!IsCollected)
            return;

        speed = Mathf.Min(speed + speedIncreaseRate * Time.deltaTime, maxSpeed);

        transform.position = Vector3.MoveTowards(
            transform.position,
            playerTransform.position,
            speed * Time.deltaTime
        );

        if ((transform.position - playerTransform.position).sqrMagnitude <= stopDistance * stopDistance)
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
            .SetTarget(this)
            .SetLink(gameObject, LinkBehaviour.KillOnDisable);
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

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (!other.CompareTag("Collectable"))
    //        return;

    //    Vector2 direction = (Vector2)(transform.position - other.transform.position);
    //    direction = direction == Vector2.zero
    //        ? Random.insideUnitCircle.normalized
    //        : direction.normalized;

    //    float pushStrength = 0.2f;
    //    transform.position += (Vector3)(direction * pushStrength);
    //}

    protected abstract void CollectEnds();

    private void OnDestroy()
    {
        // DOTween safety: always kill
        idleTween?.Kill();
        transform.DOKill();
    }

    private void ApplySorting()
    {
        spriteRenderer.sortingLayerName = sortingLayerName;
        switch (collectableType)
        {
            case CollectableType.ExpShard:
                spriteRenderer.sortingOrder = 101;
                break;
            case CollectableType.Coin:
                spriteRenderer.sortingOrder = 102;
                break;
            case CollectableType.ManaPotion:
                spriteRenderer.sortingOrder = 103;
                break;
            case CollectableType.HealthPotion:
                spriteRenderer.sortingOrder = 104;
                break;
            case CollectableType.RedRuby:
                spriteRenderer.sortingOrder = 105;
                break;
            case CollectableType.GreenRuby:
                spriteRenderer.sortingOrder = 106;
                break;
            case CollectableType.Cat:
                spriteRenderer.sortingOrder = 107;
                break;
        }
    }
}