using UnityEngine;

public class VampireNormal : EnemyBaseScript
{

    protected override void Start()
    {
        base.Start();
        hasAttackAnimation = true;
    }

    // Important for pooled enemies
    protected override void OnEnable()
    {
        base.OnEnable();
    }

}