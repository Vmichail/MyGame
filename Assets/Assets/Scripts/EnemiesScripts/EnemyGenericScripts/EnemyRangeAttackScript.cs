using UnityEngine;

public class EnemyRangeAttackScript : MonoBehaviour
{
    public bool IsPlayerInRange { get; private set; }

    private EnemyBaseScript enemyScript;

    void Start()
    {
        enemyScript = GetComponentInParent<EnemyBaseScript>();
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
            enemyScript.animator.SetBool("hasReachedPlayer", false);
        }
    }
}
