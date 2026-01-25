using UnityEngine;

public class PoisonCircleScript : PlayerSpellBaseScript
{
    public override float Speed => GlobalVariables.Instance.poisonCircleSpeed;
    public override float Damage => PlayerStatsManager.Instance.RuntimeStats.Get(PlayerStatType.Attack_Attack) * GlobalVariables.Instance.poisonCircleDamageMulti;
    public override float KnockbackForce => GlobalVariables.Instance.poisonCircleKnockbackForce;
    public override float CriticalChance => GlobalVariables.Instance.poisonCircleCriticalChance;
    public override float CriticalMultiplier => GlobalVariables.Instance.poisonCircleCriticalMultiplier;
    public override Color BaseColor => GlobalVariables.Instance.poisonCircleDefaultColor;

}
