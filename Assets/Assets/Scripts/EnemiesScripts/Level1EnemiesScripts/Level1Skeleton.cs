using UnityEngine;

public class Level1Skeleton : EnemyBaseScript
{

    protected override void Start()
    {

        maxHealth = GlobalVariables.level1SkeletonHealth;
        currentHealth = maxHealth;
        knockbackResistance = GlobalVariables.level1SkeletonKnockbackResistance;

        base.Start();
    }

    public override float MaxHealth
    {
        get => maxHealth;
        set => maxHealth = value;
    }

    public override float CurrentHealth
    {
        get => currentHealth;
        set => currentHealth = value;
    }

    protected override void MoveTowardPlayer()
    {
        MoveTowardPlayer(GlobalVariables.level1SkeletonSpeed);
    }
}