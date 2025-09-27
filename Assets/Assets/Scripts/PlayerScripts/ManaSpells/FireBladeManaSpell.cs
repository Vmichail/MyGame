using UnityEngine;

public class FireBladeManaSpell : PlayerSpellBaseScript
{
    public override float Speed => GlobalVariables.Instance.fireBladeManaSpellSpeed;
    public override float Damage => GlobalVariables.Instance.playerAttackDamage * GlobalVariables.Instance.fireBladeManaSpellDamageMutli;
    public override int Bounces => GlobalVariables.Instance.fireBladeManaSpellBounces + GlobalVariables.Instance.defaultSpellBounces;
    public override float KnockbackForce => GlobalVariables.Instance.fireBladeManaSpellKnockbackForce;
    public override float CriticalChance => GlobalVariables.Instance.fireBladeManaSpellCriticalChance;
    public override float CriticalMultiplier => GlobalVariables.Instance.fireBladeManaSpellCriticalMultiplier;
    public override Color BaseColor => GlobalVariables.Instance.fireBladeManaSpellDefaultColor;

    public override float ManaCost => GlobalVariables.Instance.fireBladeManaSpellManaCost;
    public override int Piercing => GlobalVariables.Instance.fireBladeManaSpellPiercing;

}
