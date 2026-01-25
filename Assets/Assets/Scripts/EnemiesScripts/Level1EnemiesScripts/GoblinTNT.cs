public class GoblinTNT : EnemyBaseScript
{
    protected override void Start()
    {
        base.Start();
        //ApplyBaseAndDifficulty();
        hasAttackAnimation = true;
    }

    // Important for pooled enemies
    protected override void OnEnable()
    {
        base.OnEnable();
        //ApplyBaseAndDifficulty();
        CurrentHealth = MaxHealth;
    }

}