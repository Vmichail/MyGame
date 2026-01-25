using UnityEngine;

public class BladeScript : PlayerSpellBaseScript
{
    public override float Speed => GlobalVariables.Instance.bladeSpeed;
    public override float Damage => PlayerStatsManager.Instance.RuntimeStats.Get(PlayerStatType.Attack_Attack);
    public override float KnockbackForce => GlobalVariables.Instance.bladeKnockbackForce;
    public override float CriticalChance => GlobalVariables.Instance.bladeCriticalChance;
    public override float CriticalMultiplier => GlobalVariables.Instance.bladeCriticalMultiplier;
    public override Color BaseColor => GlobalVariables.Instance.bladeDefaultColor;
    public override string OnHitSound => "hitEffect";

}
