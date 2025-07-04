using UnityEngine;

public class GoblinTNT : EnemyBaseScript
{

    protected override void Start()
    {
        base.Start();
        maxHealth = GlobalVariables.Instance.goblinTNTHealth;
        knockbackResistance = GlobalVariables.Instance.goblinTNTKnockbackResistance;
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
        get => GlobalVariables.Instance.goblinTNTSpeed;
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
        get => GlobalVariables.Instance.goblinTNTDamage;
    }

    public override float AttackCooldown
    {
        get => GlobalVariables.Instance.goblinTNTAttackCooldown;
    }

    public override float CoinDropChance
    {
        get => GlobalVariables.Instance.goblinTNTCoinDropChance;
    }

    public override float ProjectileSpeed
    {
        get => GlobalVariables.Instance.goblinTNTProjectileSpeed;
    }

    public override GlobalVariables.CoinDropEnum CoinDropEnum
    {
        get => GlobalVariables.Instance.goblinTNTCoinEnum;
    }
}