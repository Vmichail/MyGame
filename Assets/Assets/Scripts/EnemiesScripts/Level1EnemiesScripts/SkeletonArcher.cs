using UnityEngine;

public class SkeletonArcher : EnemyBaseScript
{

    protected override void Start()
    {
        base.Start();
        maxHealth = GlobalVariables.Instance.skeletonArcherHealth;
        knockbackResistance = GlobalVariables.Instance.skeletonArcherKnockbackResistance;
        if (GlobalVariables.EnemyRarity.Green.Equals(rarity))
        {
            maxHealth *= GlobalVariables.Instance.greenHealthMultiplier;
            knockbackResistance *= GlobalVariables.Instance.greenKnockbackMultiplier;
            spriteTransform.localScale *= GlobalVariables.Instance.greenScaleMultiplier;
        }
        currentHealth = maxHealth;
        hasAttackAnimation = true;
    }

    public override float Speed
    {
        get => GlobalVariables.Instance.skeletonArcherSpeed;
    }
    public override GlobalVariables.EnemyTypes EnemyType
    {
        get => GlobalVariables.EnemyTypes.SkeletonArcher;
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

    public override float Damage
    {
        get => GlobalVariables.Instance.skeletonArcherDamage;
    }

    public override float AttackCooldown
    {
        get => GlobalVariables.Instance.skeletonArcherAttackCooldown;
    }

    public override float CoinDropChance
    {
        get => GlobalVariables.Instance.skeletonArcherCoinDropChance;
    }

    public override float ProjectileSpeed
    {
        get => GlobalVariables.Instance.skeletonArcherProjectileSpeed;
    }

    public override GlobalVariables.CoinDropEnum CoinDropEnum
    {
        get => GlobalVariables.Instance.skeletonArcherCoinEnum;
    }
    public override float MinExp
    {
        get => GlobalVariables.Instance.skeletonArcherExp;
    }
    public override float AttackRange
    {
        get => GlobalVariables.Instance.skeletonArcherRange;
    }
    public override string DeathSoundClip
    {
        get => "skeletonDeadSound";
    }
    public override string[] AttackSoundClip
    {
        get => new string[] { "arrowSound2" };
    }
    public override float MultipleAttackChance
    {
        get => GlobalVariables.Instance.skeletonArcherMultipleAttackChance;
    }

}