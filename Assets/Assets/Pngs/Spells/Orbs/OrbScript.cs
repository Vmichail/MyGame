using UnityEngine;

public class OrbScript : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private float baseAttackInterval = 2f;
    [SerializeField] private GameObject spellPrefab;
    [SerializeField] private bool isTemporaryOrb = false;
    [SerializeField] private float temporaryOrbTime = 10f;
    private float temporaryOrbCurrentTimer = 0;

    private float attackTimer;

    [SerializeField] private PlayerRangeDetector playerRangeDetector;

    private void OnEnable()
    {
        temporaryOrbCurrentTimer = temporaryOrbTime;
    }

    private float ResetAttackTimer()
    {
        float attackSpeed =
            PlayerStatsManager.Instance.RuntimeStats.Get(
                PlayerStatType.Attack_AttackSpeed
            );

        // Safety clamp
        attackSpeed = Mathf.Max(attackSpeed, 0.05f);
        return baseAttackInterval / attackSpeed;
    }

    private void Update()
    {
        attackTimer -= Time.deltaTime;
        temporaryOrbCurrentTimer -= Time.deltaTime;

        if (attackTimer <= 0f)
        {
            TryCastAttack();
            attackTimer = ResetAttackTimer();
        }
        if (isTemporaryOrb && temporaryOrbCurrentTimer <= 0f)
        {
            gameObject.SetActive(false);
        }
    }

    private void TryCastAttack()
    {
        if (playerRangeDetector == null)
            return;

        GameObject target = playerRangeDetector.ClosestEnemy;

        if (target == null)
            return;

        CastAttack(target.transform.position, spellPrefab);
    }

    private void CastAttack(Vector3 enemyPosition, GameObject spell)
    {
        GameObject newSpell = PoolManager.Instance.Get(spell, transform.position, Quaternion.identity, PoolCategory.Player);

        AudioManager.Instance.PlaySoundFX("playerAttack", transform.position, 0.2f, 0.80f, 1.25f);

        Vector2 direction = (enemyPosition - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        newSpell.transform.rotation = Quaternion.Euler(0, 0, angle);

        PlayerSpellBaseScript spellBase = newSpell.GetComponent<PlayerSpellBaseScript>();
        if (spellBase != null)
            spellBase.SetVelocity(direction, true);
    }
}
