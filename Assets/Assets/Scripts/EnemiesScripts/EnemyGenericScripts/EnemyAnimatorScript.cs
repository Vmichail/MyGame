using UnityEngine;

public class EnemyAnimatorScript : MonoBehaviour
{
    private EnemyBaseScript enemyBaseScript;

    private void Start()
    {
        enemyBaseScript = GetComponentInParent<EnemyBaseScript>();
    }

    public void CallDoDamage()
    {
        enemyBaseScript.DoDamage();
    }

    public void ProjectileInstantiation()
    {
        enemyBaseScript.RangeAttackProjectile();
    }

    public void EndAttackAnimation()
    {
        enemyBaseScript.EndAttackAnimation();
    }

    public void SpecialAttack()
    {
        enemyBaseScript.SpecialAttack();
    }
}
