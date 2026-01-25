using UnityEngine;

public class OrbitBlade : PlayerSpellBaseScript
{
    public override float Speed => GlobalVariables.Instance.orbidBladeSpellSpeed;
    public override float Damage => PlayerStatsManager.Instance.RuntimeStats.Get(PlayerStatType.Attack_Attack) * GlobalVariables.Instance.orbidBladeSpellDamageMutli;
    public override float KnockbackForce => GlobalVariables.Instance.orbidBladeSpellKnockbackForce;
    public override float CriticalChance => 0;
    public override float CriticalMultiplier => 0;
    public override Color BaseColor => GlobalVariables.Instance.orbidBladeSpellDefaultColor;

    public override float ManaCost => GlobalVariables.Instance.orbidBladeSpellManaCost;
    public override int Piercing => GlobalVariables.Instance.orbidBladeSpellPiercing;
    //Orbit
    public override float Radius => GlobalVariables.Instance.orbitBladeRadius;
    public override float RotationSpeed => GlobalVariables.Instance.orbitBladeRotationSpeed;
    public override float SpellDuration => GlobalVariables.Instance.orbitBladeDuration;

}
