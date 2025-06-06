using UnityEngine;

public class BladeScript : PlayerSpellBaseScript
{
    public override float Speed => GlobalVariables.Instance.bladeSpeed;
    public override float Damage => GlobalVariables.Instance.bladeDamage;
    public override float Bounces => GlobalVariables.Instance.bladeBounces;
    public override float KnockbackForce => GlobalVariables.Instance.bladeKnockbackForce;
    public override float CriticalChance => GlobalVariables.Instance.bladeCriticalChance;
    public override float CriticalMultiplier => GlobalVariables.Instance.bladeCriticalMultiplier;
    public override Color BaseColor => GlobalVariables.Instance.bladeDefaultColor;

}
