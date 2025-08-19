using UnityEngine;

public class VampireType3 : EnemyBaseScript
{

    protected override void Start()
    {
        base.Start();
        maxHealth = GlobalVariables.Instance.vampireType3Health;
        knockbackResistance = GlobalVariables.Instance.vampireType3KnockbackResistance;
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
        get => GlobalVariables.Instance.vampireType3Speed;
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
        get => GlobalVariables.Instance.vampireType3Damage;
    }

    public override float AttackCooldown
    {
        get => GlobalVariables.Instance.vampireType3AttackCooldown;
    }

    public override float CoinDropChance
    {
        get => GlobalVariables.Instance.vampireType3CoinDropChance;
    }

    public override float ProjectileSpeed
    {
        get => GlobalVariables.Instance.vampireType3ProjectileSpeed;
    }

    public override GlobalVariables.CoinDropEnum CoinDropEnum
    {
        get => GlobalVariables.Instance.vampireType3CoinEnum;
    }
    public override float Exp
    {
        get => GlobalVariables.Instance.vampireType3Exp;
    }
    public override float AttackRange
    {
        get => GlobalVariables.Instance.vampireType3Range;
    }

}