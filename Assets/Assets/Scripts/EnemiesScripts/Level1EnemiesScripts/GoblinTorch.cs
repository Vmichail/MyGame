using UnityEngine;

public class GoblinTorchScript : EnemyBaseScript
{

    protected override void Start()
    {
        base.Start();
        maxHealth = GlobalVariables.Instance.goblinTorchHealth;
        knockbackResistance = GlobalVariables.Instance.goblinTorchKnockbackResistance;
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
        get => GlobalVariables.Instance.goblinDamage;
    }

    public override float AttackCooldown
    {
        get => GlobalVariables.Instance.goblinAttackCooldown;
    }

    public override float CoinDropChance
    {
        get => GlobalVariables.Instance.goblinTorchCoinDropChance;
    }

    public override GlobalVariables.CoinDropEnum CoinDropEnum
    {
        get => GlobalVariables.Instance.goblinCoinEnum;
    }
}