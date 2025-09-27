using UnityEngine;

public class OrbitBlade : PlayerSpellBaseScript
{
    public override float Speed => GlobalVariables.Instance.orbidBladeSpellSpeed;
    public override float Damage => GlobalVariables.Instance.playerAttackDamage * GlobalVariables.Instance.orbidBladeSpellDamageMutli;
    public override int Bounces => GlobalVariables.Instance.orbidBladeSpellBounces + GlobalVariables.Instance.defaultSpellBounces;
    public override float KnockbackForce => GlobalVariables.Instance.orbidBladeSpellKnockbackForce;
    public override float CriticalChance => GlobalVariables.Instance.orbidBladeSpellCriticalChance;
    public override float CriticalMultiplier => GlobalVariables.Instance.orbidBladeSpellCriticalMultiplier;
    public override Color BaseColor => GlobalVariables.Instance.orbidBladeSpellDefaultColor;

    public override float ManaCost => GlobalVariables.Instance.orbidBladeSpellManaCost;
    public override int Piercing => GlobalVariables.Instance.orbidBladeSpellPiercing;
    //Orbit
    public override float Radius => GlobalVariables.Instance.orbitBladeRadius;
    public override float RotationSpeed => GlobalVariables.Instance.orbitBladeRotationSpeed;
    public override float SpellDuration => GlobalVariables.Instance.orbitBladeDuration;

}
