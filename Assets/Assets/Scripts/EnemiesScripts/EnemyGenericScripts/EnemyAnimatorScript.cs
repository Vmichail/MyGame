using UnityEngine;

public class EnemyAnimatorScript : MonoBehaviour
{
    private EnemyBaseScript enemyBaseScript;

    private void OnEnable()
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

    public void SummonStarts()
    {
        enemyBaseScript.SummonStarts();
    }

    public void SummonEndsSound()
    {
        enemyBaseScript.SummonEndsSound();
    }

    public void NormalAttackFX()
    {
        enemyBaseScript.NormalAttackFX();
    }

    public void PreAttackSound()
    {
        enemyBaseScript.PreAttackSound();
    }

    public void SpecialAttackFX()
    {
        enemyBaseScript.SpecialAttackFX();
    }

}
