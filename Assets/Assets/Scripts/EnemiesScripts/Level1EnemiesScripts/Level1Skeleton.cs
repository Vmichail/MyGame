using UnityEngine;

public class Level1Skeleton : EnemyBaseScript
{

    protected override void Start()
    {
        base.Start();
        MaxHealth = GlobalVariables.Instance.skeletonHealth;
        knockbackResistance = GlobalVariables.Instance.skeletonKnockbackResistance;
        CurrentHealth = MaxHealth;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        MaxHealth = GlobalVariables.Instance.skeletonHealth;
        CurrentHealth = MaxHealth;
    }

    public override GlobalVariables.EnemyTypes EnemyType
    {
        get => GlobalVariables.EnemyTypes.Level1Skeleton;
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

    public override float MinExp
    {
        get => GlobalVariables.Instance.skeletonMinExp;
    }

    public override float MaxExp
    {
        get => GlobalVariables.Instance.skeletonMaxExp;
    }

    public override string DeathSoundClip
    {
        get => "skeletonDeadSound";
    }

}