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
        knowckbackForce = GlobalVariables.level1DarkballKnockbackForce;
    }

}
