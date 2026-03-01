using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.TextCore.Text;

public enum CharacterSprite
{
    LinaSprite,
    MiranaSprite
}

[Serializable]
public class SpellDataFull
{
    [Header("UI & Prefab")]
    [SerializeField] public GameObject UICooldownGO;
    [SerializeField] public UICooldownScript ui;
    [SerializeField] public GameObject spellPrefab;
    [SerializeField] public SpellData spellData;
    [Header("Runtime State")]
    public bool IsOnCooldown;
    public bool IsActive;
}

[Serializable]
public class CharacterEntry
{
    public CharacterSprite characterName;
    public GameObject characterRoot;
    public MainHeroConfig mainHeroConfig;
}

public class PlayerScript : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    [SerializeField] private GameObject SpritesGO;
    [SerializeField] private CharacterEntry[] heroesConfig;

    [Header("--!!Mana Spell Variables!!--")]
    [SerializeField] private Light2D manaSpellTarget;
    private List<GameObject> activeBlades = new();
    [Header("Spells Setup")]
    [SerializeField] private SpellDataFull[] manaSpells;
    [SerializeField] private Transform spellSpawnPoint;

    [SerializeField] private PlayerRangeDetector rangeDetector;
    [Header("--!!Attack Spells!!--")]
    [SerializeField] private bool canAttack = true;
    [SerializeField] private GameObject firstSpell;
    [SerializeField] private GameObject secondSpell;
    [SerializeField] private GameObject thirdSpell;
    [SerializeField] private GameObject forthSpell;


    private GameObject closestEnemy;
    private GameObject secondClosestEnemy;
    private GameObject thirdClosestEnemy;
    private GameObject forthClosestEnemy;

    private Transform spriteTransform;
    [Header("--!!Move Functionallity!!--")]
    public bool playerCanMove = true;
    private Vector2 movementInput;
    private Rigidbody2D rb;

    [Header("--!!ETC!!--")]
    [SerializeField] private GameObject target;
    [SerializeField] private float rotatationFlatFix = -0.305f;

    public GameObject ClosestEnemy => closestEnemy;



    private void Start()
    {
        if (GlobalVariables.Instance != null && !GlobalVariables.Instance.mainMenuScene)
        {
            InitializePlayer();
        }
        SetupCharacter();
    }

    void Update()
    {

        if (Time.timeScale == 0)
            return;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            CastManaSpell(0);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            CastManaSpell(1);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CastManaSpell(2);
        }

        if (playerCanMove)
        {
            movementInput.x = Input.GetAxisRaw("Horizontal");
            movementInput.y = Input.GetAxisRaw("Vertical");
        }
        else
        {
            movementInput = Vector2.zero;
        }
        if (movementInput != Vector2.zero)
            animator.SetBool("Moving", true);
        else
            animator.SetBool("Moving", false);

        if (rangeDetector.ClosestEnemy != null && canAttack)
        {
            closestEnemy = rangeDetector.ClosestEnemy;
            secondClosestEnemy = rangeDetector.SecondClosestEnemy;
            thirdClosestEnemy = rangeDetector.ThirdClosestEnemy;
            forthClosestEnemy = rangeDetector.FourthClosestEnemy;
            animator.ResetTrigger("Idle");
            SetTriggetAttack();

        }
        else if (movementInput == Vector2.zero)
        {
            animator.ResetTrigger("Attack");
            animator.SetTrigger("Idle");
        }
        else if (Mathf.Abs(movementInput.x) != 0)
        {
            animator.ResetTrigger("Attack");
            animator.SetTrigger("Idle");
            RotatePlayer();
        }
        ActivateDeactivateTarget();
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0); // 0 = Base Layer
        if (stateInfo.IsName("Attack"))
            animator.speed = 1f * PlayerStatsManager.Instance.RuntimeStats.Get(PlayerStatType.Attack_AttackSpeed);
        else
            animator.speed = 1f;

    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movementInput.normalized * PlayerStatsManager.Instance.RuntimeStats.Get(PlayerStatType.Defence_MovementSpeed) * Time.fixedDeltaTime);
    }

    private void RotatePlayer()
    {
        if (movementInput.x < 0)
        {
            spriteTransform.SetPositionAndRotation(
                transform.position + new Vector3(rotatationFlatFix, 0, 0),
                Quaternion.Euler(0, 180, 0));
        }
        else
        {
            spriteTransform.SetPositionAndRotation(
                transform.position,
                Quaternion.Euler(0, 0, 0));
        }
    }

    public void RotatePlayerToEnemy()
    {
        if (closestEnemy == null)
            return;

        if (closestEnemy.transform.position.x < transform.position.x)
        {
            spriteTransform.SetPositionAndRotation(
                transform.position + new Vector3(rotatationFlatFix, 0, 0),
                Quaternion.Euler(0, 180, 0));
        }
        else
        {
            spriteTransform.SetPositionAndRotation(
                transform.position,
                Quaternion.Euler(0, 0, 0));
        }

    }

    private void SetTriggetAttack()
    {
        if (canAttack)
            animator.SetTrigger("Attack");
    }

    public void CastFirstSpell()
    {
        RotatePlayerToEnemy();
        if (closestEnemy == null)
            return;


        if (GlobalVariables.Instance.SecondSpellEnabled)
            CastSecondSpell();
        if (GlobalVariables.Instance.ThirdSpellEnabled)
            CastThirdSpell();
        if (GlobalVariables.Instance.ForthSpellEnabled)
            CastForthSpell();


        CastAttack(closestEnemy.transform.position, firstSpell);

    }



    public void CastSecondSpell()
    {
        Vector3 enemyPosition;
        if (secondClosestEnemy != null)
        {
            enemyPosition = secondClosestEnemy.transform.position;
        }
        else if (closestEnemy != null)
        {
            enemyPosition = closestEnemy.transform.position;
        }
        else
        {
            return;
        }

        CastAttack(enemyPosition, secondSpell);

    }

    public void CastThirdSpell()
    {
        Vector3 enemyPosition;
        if (thirdClosestEnemy != null)
        {
            enemyPosition = secondClosestEnemy.transform.position;
        }
        else if (secondClosestEnemy != null)
        {
            enemyPosition = secondClosestEnemy.transform.position;
        }
        else if (closestEnemy != null)
        {
            enemyPosition = closestEnemy.transform.position;
        }
        else
        {
            return;
        }

        CastAttack(enemyPosition, thirdSpell);

    }

    public void CastForthSpell()
    {
        Vector3 enemyPosition;
        if (forthClosestEnemy != null)
        {
            enemyPosition = forthClosestEnemy.transform.position;
        }
        else if (closestEnemy != null)
        {
            enemyPosition = closestEnemy.transform.position;
        }
        else if (secondClosestEnemy != null)
        {
            enemyPosition = secondClosestEnemy.transform.position;
        }
        else if (thirdClosestEnemy != null)
        {
            enemyPosition = thirdClosestEnemy.transform.position;
        }
        else
        {
            return;
        }

        CastAttack(enemyPosition, forthSpell);

    }

    public void CastManaSpell(int index)
    {
        if (GlobalVariables.Instance.gameIsPaused || GlobalVariables.Instance.blockGameplayInput || GlobalVariables.Instance.mainMenuScene)
        {
            return;
        }
        if (manaSpells[index].IsOnCooldown || PlayerStatsManager.Instance.CurrentMana < manaSpells[index].spellData.startingManaCost || !manaSpells[index].IsActive)
        {
            AudioManager.Instance.PlaySoundFX("uiDeny", transform.position, 0.8f, 0.9f, 1.1f);
            return;
        }
        CinemachineScript.Instance.Shake(0.5f, 0.15f);
        SpellDataFull spell = manaSpells[index];
        PlayerStatsManager.Instance.CurrentMana -= (int)spell.spellData.startingManaCost;
        Debug.Log($"Casting mana spell {spell.spellData.SpellCode}, mana cost: {spell.spellData.startingManaCost}, cooldown:{spell.spellData.cooldownTime}");
        AudioManager.Instance.PlaySoundFX(spell.spellData.castSound, transform.position, 0.6f, 0.8f, 1.25f);
        if (GlobalVariables.SpellCode.FireBlade.Equals(spell.spellData.SpellCode))
        {
            HealEffectSelector.SelectHealEffect(HealEffectSelector.PlayerHealEffectType.Fire);
            FireBladeCast(spell);
        }
        else if (GlobalVariables.SpellCode.RotatingBlades.Equals(spell.spellData.SpellCode))
        {
            HealEffectSelector.SelectHealEffect(HealEffectSelector.PlayerHealEffectType.White);
            CastRotatingBlades(spell);
        }
        else if (GlobalVariables.SpellCode.Shield.Equals(spell.spellData.SpellCode))
            CastShield(spell);
        else
        {
            Debug.LogWarning($"Spell code {spell.spellData.SpellCode} not recognized!");
        }
        StartCoroutine(StartSpellCooldown(index));
    }

    private void FireBladeCast(SpellDataFull spell)
    {
        StartCoroutine(CastMultipleCouroutine(spell));
    }

    private IEnumerator CastMultipleCouroutine(SpellDataFull spell)
    {
        for (int i = 0; i < GlobalVariables.Instance.fireBladeManaTotal; i++)
        {
            AudioManager.Instance.PlaySoundFX(spell.spellData.castSound, transform.position, 0.5f, 0.80f, 1.25f);
            Vector3 spawnPos = manaSpellTarget.transform.position;
            GameObject newSpell = Instantiate(spell.spellPrefab, spawnPos, Quaternion.identity);

            if (newSpell.TryGetComponent<PlayerSpellBaseScript>(out var playerSpellBaseScript))
            {
                Vector2 dir = manaSpellTarget.transform.up;
                playerSpellBaseScript.SetVelocity(dir, false);
                Debug.Log("FireBlade casted! with speed: " + playerSpellBaseScript.Speed);
            }
            else
            {
                Debug.LogWarning("FireBlade prefab is missing PlayerSpellBaseScript!");
            }

            yield return new WaitForSeconds(GlobalVariables.Instance.fireBladeDelay);
        }
        manaSpellTarget.gameObject.SetActive(false);
    }

    private void CastAttack(Vector3 enemyPosition, GameObject spell)
    {
        GameObject newSpell = Instantiate(spell, transform.position, Quaternion.identity);
        AudioManager.Instance.PlaySoundFX("playerAttack", transform.position, 0.2f, 0.80f, 1.25f);
        Vector2 direction = (enemyPosition - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        newSpell.transform.rotation = Quaternion.Euler(0, 0, angle);
        PlayerSpellBaseScript playerSpellBaseScript = newSpell.GetComponent<PlayerSpellBaseScript>();
        if (playerSpellBaseScript)
            playerSpellBaseScript.SetVelocity(direction, true);
    }

    private void ActivateDeactivateTarget()
    {
        if (rangeDetector.ClosestEnemy != null)
        {
            target.SetActive(true);
            target.transform.position = rangeDetector.ClosestEnemy.transform.position;
        }
        else
        {
            target.SetActive(false);
        }
    }

    public void CastRotatingBlades(SpellDataFull spell)
    {
        foreach (GameObject blade in activeBlades)
        {
            if (blade != null)
                Destroy(blade);
        }
        activeBlades.Clear();
        AudioManager.Instance.PlaySoundFX(spell.spellData.castSound, transform.position, 0.5f, 0.80f, 1.25f);
        for (int i = 0; i < GlobalVariables.Instance.totalOrbitBlades; i++)
        {
            // calculate starting angle so they spread evenly
            float startAngle = i * Mathf.PI * 2f / GlobalVariables.Instance.totalOrbitBlades;
            GameObject blade = Instantiate(spell.spellPrefab, transform.position, Quaternion.identity);

            // set blade's starting angle & radius
            PlayerSpellBaseScript orbit = blade.GetComponent<PlayerSpellBaseScript>();
            orbit.SetAngle(startAngle);
            activeBlades.Add(blade);
        }
    }

    public void CastShield(SpellDataFull spell)
    {
        GameObject shield = Instantiate(spell.spellPrefab, transform.position, Quaternion.identity, transform);
        PlayerSpellBaseScript shieldSpellScript = shield.GetComponent<PlayerSpellBaseScript>();
    }

    private IEnumerator StartSpellCooldown(int index)
    {
        manaSpells[index].IsOnCooldown = true;
        manaSpells[index].ui.StartCooldown(manaSpells[index].spellData.cooldownTime);

        yield return new WaitForSeconds(manaSpells[index].spellData.cooldownTime);

        manaSpells[index].IsOnCooldown = false;
        if (GlobalVariables.SpellCode.FireBlade.Equals(manaSpells[index].spellData.SpellCode))
        {
            manaSpellTarget.gameObject.SetActive(true);
        }
    }

    public void EnableUpgradeSpell(PlayerStatType upgradeCode)
    {
        if (PlayerStatType.Spells_FireBlade.Equals(upgradeCode))
        {
            manaSpells[0].IsActive = true;
            manaSpells[0].ui.ActivateSpell();
            manaSpellTarget.gameObject.SetActive(true);
        }
        else if (PlayerStatType.Spells_RotatingBlades.Equals(upgradeCode))
        {
            manaSpells[1].IsActive = true;
            manaSpells[1].ui.ActivateSpell();
        }
        else if (PlayerStatType.Spells_Shield.Equals(upgradeCode))
        {
            manaSpells[2].IsActive = true;
            manaSpells[2].ui.ActivateSpell();
        }
        else
        {
            Debug.LogWarning($"Upgrade code {upgradeCode} not recognized! can not enable ManaSpell!");
        }
    }



    private void InitializePlayer()
    {
        manaSpellTarget.gameObject.SetActive(false);
        // Auto-fetch UI cooldown script from each prefab
        int spellCount = manaSpells.Length;
        for (int i = 0; i < spellCount; i++)
        {
            manaSpells[i].IsOnCooldown = false;
            if (manaSpells[i].UICooldownGO.TryGetComponent(out UICooldownScript uiCooldownScript))
            {
                manaSpells[i].ui = uiCooldownScript;
            }
            else
            {
                Debug.LogWarning($"Spell prefab {manaSpells[i].UICooldownGO.name} is missing UICooldownScript!");
            }
        }
    }

    private void SetupCharacter()
    {
        rb = GetComponent<Rigidbody2D>();
        if (GlobalVariables.Instance.mainMenuScene)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        target.transform.SetParent(null);
        string selected = GlobalVariables.Instance.selectedCharacter;

        foreach (CharacterEntry entry in heroesConfig)
        {
            bool isSelected = entry.characterName.ToString() == selected;

            entry.characterRoot.SetActive(isSelected);

            if (!isSelected)
                continue;

            spriteRenderer = entry.characterRoot.GetComponentInChildren<SpriteRenderer>(true);
            animator = entry.characterRoot.GetComponentInChildren<Animator>(true);

            rotatationFlatFix = entry.mainHeroConfig.rotationFlatFix;
            canAttack = entry.mainHeroConfig.canAttack;
        }

        if (animator == null)
        {
            Debug.LogError("Selected character missing Animator");
        }

        if (spriteRenderer != null)
            spriteTransform = spriteRenderer.transform;
        else
            Debug.LogWarning("No Sprite was found!");
    }

}
