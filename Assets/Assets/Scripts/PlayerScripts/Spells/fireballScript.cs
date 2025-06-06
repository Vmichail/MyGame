using UnityEngine;

public class fireballScript : PlayerSpellBaseScript
{
    public override float Speed => GlobalVariables.Instance.fireballSpeed;
    public override float Damage => GlobalVariables.Instance.fireballDamage;
    public override float Bounces => GlobalVariables.Instance.fireballBounces;
    public override float KnockbackForce => GlobalVariables.Instance.fireballKnockbackForce;
    public override float CriticalChance => GlobalVariables.Instance.fireballCriticalChance;
    public override float CriticalMultiplier => GlobalVariables.Instance.fireballCriticalMultiplier;
    public override Color BaseColor => GlobalVariables.Instance.fireballDefaultColor;


}
