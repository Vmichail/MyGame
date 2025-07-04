using UnityEngine;

public class Level1Skeleton : EnemyBaseScript
{

    protected override void Start()
    {
        base.Start();
        if (GlobalVariables.EnemyRarity.Green.Equals(rarity))
        {
            maxHealth = GlobalVariables.Instance.skeletonHealth * GlobalVariables.Instance.greenHealthMultiplier;
            knockbackResistance = GlobalVariables.Instance.skeletonKnockbackResistance * GlobalVariables.Instance.greenKnockbackMultiplier;
            spriteTransform.localScale *= GlobalVariables.Instance.greenScaleMultiplier;
        }
        else
        {
            maxHealth = GlobalVariables.Instance.skeletonHealth;
            knockbackResistance = GlobalVariables.Instance.skeletonKnockbackResistance;
        }
        currentHealth = maxHealth;
    }

    public override float Speed
    {
        get => GlobalVariables.Instance.skeletonSpeed;
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
        get => GlobalVariables.Instance.skeletonDamage;
    }

    public override float AttackCooldown
    {
        get => GlobalVariables.Instance.skeletonAttackCooldown;
    }

    public override float CoinDropChance
    {
        get => GlobalVariables.Instance.skeleonCoinDropChance;
    }

    public override GlobalVariables.CoinDropEnum CoinDropEnum
    {
        get => GlobalVariables.Instance.skeletonCoinEnum;
    }

}