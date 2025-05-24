using UnityEngine;

public class Level1FireballScript : PlayerSpellBaseScript
{
    protected override void Start()
    {
        base.Start();
        speed = GlobalVariables.level1FireballSpeed;
        damage = GlobalVariables.level1FireballDamage;
        bounces = GlobalVariables.level1FireballBounces;
        knockbackForce = GlobalVariables.level1FireballKnockbackForce;
        criticalChance = GlobalVariables.level1FireballCriticalChance + GlobalVariables.globalCriticalChance;
        criticalMultiplier = GlobalVariables.level1FireballCriticalMultiplier + GlobalVariables.globalCriticalMultiplier;
        baseColor = GlobalVariables.Instance.level1FireballDefaultColor;
    }

}
