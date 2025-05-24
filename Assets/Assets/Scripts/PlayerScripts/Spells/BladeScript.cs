using UnityEngine;

public class BladeScript : PlayerSpellBaseScript
{

    protected override void Awake()
    {
        base.Awake();
        speed = GlobalVariables.Instance.bladeSpeed;
        damage = GlobalVariables.Instance.bladeDamage;
        bounces = GlobalVariables.Instance.bladeBounces;
        knockbackForce = GlobalVariables.Instance.bladeKnockbackForce;
        criticalChance = GlobalVariables.Instance.bladeCriticalChance + GlobalVariables.globalCriticalChance;
        criticalMultiplier = GlobalVariables.Instance.bladeCriticalMultiplier + GlobalVariables.globalCriticalMultiplier;
        baseColor = GlobalVariables.Instance.bladeDefaultColor;
    }

}
