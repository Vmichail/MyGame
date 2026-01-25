using Unity.VisualScripting;
using UnityEngine;

public class darkballScript : PlayerSpellBaseScript
{
    public override float Speed => GlobalVariables.Instance.darkballSpeed;
    public override float Damage => PlayerStatsManager.Instance.RuntimeStats.Get(PlayerStatType.Attack_Attack) * GlobalVariables.Instance.darkballDamageMulti;
    public override float KnockbackForce => GlobalVariables.Instance.darkballKnockbackForce;
    public override float CriticalChance => GlobalVariables.Instance.darkballCriticalChance;
    public override float CriticalMultiplier => GlobalVariables.Instance.darkballCriticalMultiplier;
    public override Color BaseColor => GlobalVariables.Instance.darkballDefaultColor;

}
