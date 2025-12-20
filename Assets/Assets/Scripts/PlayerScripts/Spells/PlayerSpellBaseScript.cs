using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerSpellBaseScript : MonoBehaviour
{
    public virtual bool OnDestroyEffect => GlobalVariables.Instance.defaultSpellOnDestroyEffect;
    public virtual bool IsShield => false;
    public virtual float Damage => GlobalVariables.Instance.defaultSpellDamage;
    public virtual float Speed => GlobalVariables.Instance.defaultSpellSpeed;
    public virtual int Bounces => GlobalVariables.Instance.defaultSpellBounces;
    public virtual float KnockbackForce => GlobalVariables.Instance.defaultKnockbackforce;
    public virtual float CriticalChance => GlobalVariables.Instance.globalCriticalChance;
    public virtual float CriticalMultiplier => GlobalVariables.Instance.globalCriticalMultiplier;
    public virtual float ManaCost => GlobalVariables.Instance.globalCriticalMultiplier;
    public virtual Color BaseColor => GlobalVariables.Instance.defaultColor;
    public virtual int Piercing => GlobalVariables.Instance.defaultSpellPiercing;
    private Transform player;   // who it orbits around
    private float angle;        // current angle around player
    private Rigidbody2D rb;
    private int currentBounces = 0;
    private int currentPiercing = 0;
    protected Color baseColor;
    [SerializeField] private GameObject particles;
    [SerializeField] private GameObject OnDestroyParticles;
    [SerializeField] private ChildSpellSpawner childSpellSpawner;
    [Header("Orbit Settings")]
    public virtual float Radius => GlobalVariables.Instance.orbitBladeRadius;
    public virtual float RotationSpeed => GlobalVariables.Instance.orbidBladeSpellSpeed;
    public virtual float SpellDuration => GlobalVariables.Instance.orbitBladeDuration;
    public bool orbit;
    private float currentRadius = 0f; // start from center
    private Coroutine soundCoroutine;
    public bool couroutineSound = false;

    [SerializeField] bool StayOnPlayer = false;
    public virtual string ChildSummonSoundName => GlobalVariables.Instance.orbitCouroutineSound;
    public virtual string CouroutineSoundName => GlobalVariables.Instance.orbitCouroutineSound;
    public virtual string SpellCastSound => null;
    public virtual string OnHitSound => "hitEffect";
    private float shieldSpeed;

    public void SetAngle(float startAngle)
    {
        angle = startAngle;
    }

    void Update()
    {
        if (player != null && orbit)
        {
            if (currentRadius < Radius)
            {
                currentRadius = Mathf.MoveTowards(currentRadius, Radius, Speed * Time.deltaTime);
            }

            angle += RotationSpeed * Mathf.Deg2Rad * Time.deltaTime;

            Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * currentRadius;
            transform.position = player.position + offset;
        }
        if (StayOnPlayer)
        {
            transform.localPosition = Vector3.zero; // stick to player
        }
    }

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Start()
    {
        if (IsShield)
        {
            GlobalVariables.Instance.playerInvulnerableReasons.Add(GlobalVariables.InvulnerableReasonEnum.ShieldSpell);
            shieldSpeed = GlobalVariables.Instance.shieldSpellSpeed;
            HeroUpgrades.Instance.UpdatePlayerSpeed(shieldSpeed);
        }
        if (SpellCastSound != null)
        {
            AudioManager.Instance.PlaySoundFX(SpellCastSound, transform.position, 0.8f, 0.9f, 1.1f);
        }
        Destroy(gameObject, SpellDuration);
        /*        Debug.Log($"Spell started: {GetType().Name}, duration: {SpellDuration}");*/
        if (orbit)
        {
            if (couroutineSound)
                soundCoroutine = StartCoroutine(PlayLoopSound());
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    private IEnumerator PlayLoopSound()
    {
        while (true)
        {
            AudioManager.Instance.PlaySoundFX(CouroutineSoundName, transform.position, 0.2f, 0.9f, 1.1f);
            yield return new WaitForSeconds(AudioManager.Instance.GetClipLength(CouroutineSoundName));
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("EnemyProjectile"))
        {
            if (particles != null)
            {
                Instantiate(particles, transform.position, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("Particles prefab not assigned!");
            }
            if (Bounces > 0 && currentBounces < Bounces)
                Bounce(collision.gameObject);
            else if (currentPiercing < Piercing)
            {
                currentPiercing++;
            }
            else
            {
                AudioManager.Instance.PlaySoundFX("playerProjectileDestroy", transform.position, 0.5f, 0.6f, 1.1f);
                Destroy(gameObject);
            }
        }

    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            AudioManager.Instance.PlaySoundFX("wallHitSpell", transform.position, 0.8f, 0.9f, 1.1f);
            Destroy(gameObject);
        }
    }

    public void SetVelocity(Vector2 direction, bool bounce)
    {
        if (!bounce)
            currentBounces = Bounces;
        if (rb == null)
        {
            Debug.LogWarning("Rb is null??");
            return;
        }
        Vector2 desiredVelocity = direction * Speed;

        if (desiredVelocity.magnitude < Speed - 2)
        {
            desiredVelocity = direction.normalized * Speed;
        }

        rb.linearVelocity = desiredVelocity;
        /* Debug.Log($"Setting velocity to {desiredVelocity}");*/
    }

    private GameObject FindClosestEnemy(GameObject exclude)
    {
        GameObject closest = null;
        float minDistance = float.MaxValue;

        foreach (GameObject enemy in EnemyManagerScript.Instance.ActiveEnemies)
        {
            if (enemy == exclude || enemy == null) continue;

            float dist = Vector2.Distance(transform.position, enemy.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                closest = enemy;
            }
        }

        return closest;
    }


    private void Bounce(GameObject lastHitEnemy)
    {
        currentBounces++;
        GameObject nextEnemy = FindClosestEnemy(exclude: lastHitEnemy);
        if (nextEnemy != null)
        {
            Vector2 newDirection = (nextEnemy.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(newDirection.y, newDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            SetVelocity(newDirection, true);
        }
        else
        {
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            float angle = Mathf.Atan2(randomDirection.y, randomDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            SetVelocity(randomDirection, true);
        }
    }

    private void OnDestroy()
    {
        if (soundCoroutine != null)
            StopCoroutine(soundCoroutine);
        if (OnDestroyParticles != null)
        {
            Instantiate(OnDestroyParticles, transform.position, Quaternion.identity);
        }
        if (IsShield)
        {
            GlobalVariables.Instance.playerInvulnerableReasons.Remove(GlobalVariables.InvulnerableReasonEnum.ShieldSpell);
            HeroUpgrades.Instance.UpdatePlayerSpeed(shieldSpeed, true);
        }
        if (OnDestroyEffect)
        {
            AudioManager.Instance.PlaySoundFX(ChildSummonSoundName, transform.position, 0.5f, 0.80f, 1.25f);
            childSpellSpawner.SpawnChildSpells(transform.position);
        }
    }
}
