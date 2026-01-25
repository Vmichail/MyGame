using UnityEngine;

public class FireBladeManaSpell : PlayerSpellBaseScript
{
    public override float Speed => GlobalVariables.Instance.fireBladeManaSpellSpeed;
    public override float Damage => PlayerStatsManager.Instance.RuntimeStats.Get(PlayerStatType.Attack_Attack) * GlobalVariables.Instance.fireBladeManaSpellDamageMutli;
    public override float KnockbackForce => GlobalVariables.Instance.fireBladeManaSpellKnockbackForce;
    public override float CriticalChance => GlobalVariables.Instance.fireBladeManaSpellCriticalChance;
    public override float CriticalMultiplier => GlobalVariables.Instance.fireBladeManaSpellCriticalMultiplier;
    public override Color BaseColor => GlobalVariables.Instance.fireBladeManaSpellDefaultColor;

    public override float ManaCost => GlobalVariables.Instance.fireBladeManaSpellManaCost;
    public override int Piercing => GlobalVariables.Instance.fireBladeManaSpellPiercing;

}
