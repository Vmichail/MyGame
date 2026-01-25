using UnityEngine;

public class VampireBoss : EnemyBaseScript
{
    protected override void Start()
    {
        base.Start();
        hasAttackAnimation = true;
    }

    public override void EndAttackAnimation()
    {
        animator.SetBool("Attack", false);
        animator.SetBool("hasReachedPlayer", false);
        hasReachedPlayer = false;

        if (Random.value > 0.5f)
            animator.SetTrigger("SpecialAttack");
    }

    public override void SpecialAttack()
    {
        AudioManager.Instance.PlaySoundFX("Chain_Frost_target_creep", transform.position, 0.4f, 0.75f, 1.25f);

        if (Random.value < 0.15f)
        {
            SpawnCrossPattern();
        }
        else if (Random.value > 0.6f)
        {
            FullSpawnDiagonalPattern();
        }
        else
        {
            SpawnDiagonalPattern();
        }
    }

    private void SpawnCrossPattern()
    {
        Vector2[] directions = {
            Vector2.up, Vector2.down, Vector2.left, Vector2.right
        };
        foreach (var dir in directions) SpawnProjectile(dir);
    }

    private void SpawnDiagonalPattern()
    {
        Vector2[] directions = {
            new Vector2( 1,  1).normalized,
            new Vector2(-1,  1).normalized,
            new Vector2( 1, -1).normalized,
            new Vector2(-1, -1).normalized
        };
        foreach (var dir in directions) SpawnProjectile(dir);
    }

    private void FullSpawnDiagonalPattern()
    {
        Vector2[] directions = {
            Vector2.up, Vector2.down, Vector2.left, Vector2.right,
            new Vector2( 1,  1).normalized,
            new Vector2(-1,  1).normalized,
            new Vector2( 1, -1).normalized,
            new Vector2(-1, -1).normalized
        };
        foreach (var dir in directions) SpawnProjectile(dir);
    }

    private void SpawnProjectile(Vector2 direction)
    {
        GameObject proj = Instantiate(projectile, transform.position, Quaternion.identity);
        proj.transform.localScale *= 2f;

        // Use a single, consistent speed value that includes difficulty
        float finalSpeed = Mathf.Max(0f, ProjectileSpeed); // already includes EnemySpeedMult

        // If the projectile moves via RB velocity, set it:
        if (proj.TryGetComponent<Rigidbody2D>(out var rb))
        {
            rb.linearVelocity = direction * finalSpeed;
        }

        // If the projectile has its own script, set its parameters too
        if (proj.TryGetComponent<EnemyProjectileBaseScript>(out var projectileScript))
        {
            projectileScript.Damage = Damage;      // includes EnemyDamageMult
            projectileScript.Speed = finalSpeed;  // includes EnemySpeedMult
            projectileScript.hasDirection = true;
        }

        // Rotate the *instance* to face its travel direction (fix)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        proj.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}