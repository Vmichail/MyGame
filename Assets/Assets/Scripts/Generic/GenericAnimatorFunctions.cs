using UnityEngine;

public class GenericAnimatorFunctions : MonoBehaviour
{
    private Animator animator;
    private CircleCollider2D circleCol;
    private float damage = 0f;
    [SerializeField] private GameObject receivedDamagePopUp;
    [SerializeField] private bool explosionDealsDamage;
    [SerializeField] GameObject explosionFire;

    private void OnEnable()
    {
        // Get the Animator from this GameObject
        if (animator == null)
            animator = GetComponent<Animator>();

        if (circleCol == null)
            circleCol = GetComponent<CircleCollider2D>();

        if (animator == null)
        {
            Debug.LogWarning($"No Animator found on {gameObject.name}");
            return;
        }
    }

    private void AnimationEnds()
    {
        if (explosionFire)
        {
            Instantiate(explosionFire, transform.position, Quaternion.identity);
        }
        gameObject.SetActive(false);
    }

    private void EnableCollider()
    {
        if (circleCol != null)
            circleCol.enabled = true;
    }

    private void DisableCollider()
    {
        if (circleCol != null)
            circleCol.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && explosionDealsDamage)
        {
            AudioManager.Instance.PlaySoundFX("playerProjectileDestroy", transform.position, 0.2f, 0.9f, 1.1f);
            Destroy(gameObject);
            EnemyGenericFunctionsForPlayer.Instance.DamagePlayer(damage);
        }
    }

    private void AnimationEndsDestroy()
    {
        if (explosionFire)
        {
            Instantiate(explosionFire, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
