using UnityEngine;

public class Level1FireballScript : PlayerSpellBaseScript
{
    protected override void Start()
    {
        base.Start();
        speed = GlobalVariables.level1FireballSpeed;
        damage = GlobalVariables.level1FireballDamage;
        bounces = GlobalVariables.level1FireballBounces;
    }

}
