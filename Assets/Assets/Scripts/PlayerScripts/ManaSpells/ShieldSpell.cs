using UnityEngine;

public class ShieldSpell : PlayerSpellBaseScript
{
    public override float Speed => GlobalVariables.Instance.shieldSpellSpeed;
    public override float Damage => GlobalVariables.Instance.shieldSpellDamage;
    public override int Bounces => 0;
    public override float KnockbackForce => GlobalVariables.Instance.shieldSpellKnockbackForce;
    public override float CriticalChance => GlobalVariables.Instance.shieldSpellCriticalChance;
    public override float CriticalMultiplier => GlobalVariables.Instance.shieldSpellCriticalMultiplier;
    public override Color BaseColor => GlobalVariables.Instance.shieldSpellDefaultColor;

    public override float ManaCost => GlobalVariables.Instance.shieldSpellManaCost;
    public override int Piercing => GlobalVariables.Instance.shieldSpellPiercing;
    public override float SpellDuration => GlobalVariables.Instance.shieldSpellDuration;
    public override bool OnDestroyEffect => true;
    public override string CouroutineSoundName => "shieldLoopSound";
    public override string ChildSummonSoundName => "shieldBreakSound";
    public override string SpellCastSound => "shieldCastSound";
    public override string OnHitSound => "hitEffect";
    public override bool IsShield => true;

}
