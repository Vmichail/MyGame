using UnityEngine;

public class PoisonCircleScript : PlayerSpellBaseScript
{
    public override float Speed => GlobalVariables.Instance.poisonCircleSpeed;
    public override float Damage => GlobalVariables.Instance.playerAttackDamage * GlobalVariables.Instance.poisonCircleDamageMulti;
    public override float Bounces => GlobalVariables.Instance.poisonCircleBounces;
    public override float KnockbackForce => GlobalVariables.Instance.poisonCircleKnockbackForce;
    public override float CriticalChance => GlobalVariables.Instance.poisonCircleCriticalChance;
    public override float CriticalMultiplier => GlobalVariables.Instance.poisonCircleCriticalMultiplier;
    public override Color BaseColor => GlobalVariables.Instance.poisonCircleDefaultColor;

}
