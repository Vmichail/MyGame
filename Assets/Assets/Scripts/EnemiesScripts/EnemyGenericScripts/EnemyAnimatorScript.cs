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

}
