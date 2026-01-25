using UnityEngine;

public class FireballScript : PlayerSpellBaseScript
{
    public override float Speed => GlobalVariables.Instance.fireballSpeed;
    public override float Damage => PlayerStatsManager.Instance.RuntimeStats.Get(PlayerStatType.Attack_Attack) * GlobalVariables.Instance.fireballDamage;
    public override float KnockbackForce => GlobalVariables.Instance.fireballKnockbackForce;
    public override float CriticalChance => GlobalVariables.Instance.fireballCriticalChance;
    public override float CriticalMultiplier => GlobalVariables.Instance.fireballCriticalMultiplier;
    public override Color BaseColor => GlobalVariables.Instance.fireballDefaultColor;


}
