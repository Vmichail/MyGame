using UnityEngine;

public class EnemyRangeAttackScript : MonoBehaviour
{
    public bool IsPlayerInRange { get; private set; }

    private CircleCollider2D circleCollider;

    private EnemyBaseScript enemyScript;

    void Start()
    {
        enemyScript = GetComponentInParent<EnemyBaseScript>();
        circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.radius = enemyScript.AttackRange;
    }

    private void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            IsPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            IsPlayerInRange = false;
            enemyScript.hasReachedPlayer = false;
            enemyScript.SetAnimatorValue("hasReachedPlayer", false);
        }
    }
}
