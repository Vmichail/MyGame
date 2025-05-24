using UnityEngine;

public class PoisonCircleScript : PlayerSpellBaseScript
{

    protected override void Start()
    {
        base.Start();
        speed = GlobalVariables.Instance.poisonCircleSpeed;
        damage = GlobalVariables.Instance.poisonCircleDamage;
        bounces = GlobalVariables.Instance.poisonCircleBounces;
        knockbackForce = GlobalVariables.Instance.poisonCircleKnockbackForce;
        criticalChance = GlobalVariables.Instance.poisonCircleCriticalChance + GlobalVariables.globalCriticalChance;
        criticalMultiplier = GlobalVariables.Instance.poisonCircleCriticalMultiplier + GlobalVariables.globalCriticalMultiplier;
        baseColor = GlobalVariables.Instance.poisonCircleDefaultColor;
    }

}
