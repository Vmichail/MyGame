using Unity.VisualScripting;
using UnityEngine;

public class Level1DarkballScript : PlayerSpellBaseScript
{

    protected override void Awake()
    {
        base.Awake();
        speed = GlobalVariables.level1DarkballSpeed;
        damage = GlobalVariables.level1DarkballDamage;
        bounces = GlobalVariables.level1DarkballBounces;
        knockbackForce = GlobalVariables.level1DarkballKnockbackForce;
        criticalChance = GlobalVariables.level1DarkballCriticalChance + GlobalVariables.globalCriticalChance;
        criticalMultiplier = GlobalVariables.level1DarkballCriticalMultiplier + GlobalVariables.globalCriticalMultiplier;
        baseColor = GlobalVariables.Instance.level1DarkBallDefaultColor;
    }

}
