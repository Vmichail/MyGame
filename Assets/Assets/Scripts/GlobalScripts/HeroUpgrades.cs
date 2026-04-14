using UnityEngine;

public class HeroUpgrades : MonoBehaviour
{
    [Header("SerializeFields")]
    [SerializeField] private Transform playerRangeTransform;
    [SerializeField] private PlayerScript playerScript;
    [SerializeField] private GameObject playerMagnetColliderGO;

    [Header("Collectable Values")]
    public int yellowCoinValue = 1;
    public int redRubyExpValue = 10;
    public int redRubyCoinValue = 5;
    public int greenRubyCoinValue = 10;
    public int greenRubyExpValue = 100;
    public int shardExp = 1;
    //Potions
    public float manaPotionMana = 10;
    public float healthPotionHealth = 20;

    public static HeroUpgrades Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Prevent duplicates
            return;
        }
        Instance = this;
        RecalculateRange();
    }

    public static string GetUpgradeTitle(PlayerStatType code)
        => PlayerStatsManager.Instance.Definitions[code].displayName.ToString();

    // ================= SPELL FLAGS =================
    private bool fireBladeSpellEnabled = false;
    private bool rotatingBladesSpellEnabled = false;
    private bool shieldSpellEnabled = false;

    // ================= Shards Values =================
    private float expMultiplier = 1f;


    public float ExpMultiplier()
    {
        return expMultiplier;
    }

    // ================= CORE FUNCTION =================
    public void UpgradeHero(UpgradeChoice uc)
    {
        var gv = GlobalVariables.Instance;
        var statsManager = PlayerStatsManager.Instance;
        float upgradeValue = uc.UpgradeValue.value;
        PlayerStatType upgradeCode = uc.UpgradeCode;
        //Debug.Log($"Applying upgrade: {upgradeCode} with value: {upgradeValue}");
        switch (upgradeCode)
        {
            // ================= ATTACK =================
            case PlayerStatType.Attack_AttackRange:
                {
                    statsManager.RuntimeStats.AddFlat(upgradeCode, upgradeValue);
                    RecalculateRange();
                    break;
                }

            case PlayerStatType.Attack_Attack:
            case PlayerStatType.Attack_Bounce:
            case PlayerStatType.Attack_CriticalChance:
            case PlayerStatType.Attack_CriticalDamage:
            case PlayerStatType.Attack_AttackSpeed:
                {
                    Debug.Log($"Applying flat upgrade: {upgradeCode} with value: {upgradeValue}");
                    PlayerStatsManager.Instance.RuntimeStats.AddFlat(upgradeCode, upgradeValue);
                    break;
                }

            case PlayerStatType.Defence_MovementSpeed:
                {
                    Debug.Log($"Applying flat upgrade: {upgradeCode} with value: {upgradeValue}");
                    PlayerStatsManager.Instance.RuntimeStats.AddFlat(upgradeCode, upgradeValue);
                    UpdatePlayerSpeed(upgradeValue);
                    break;
                }

            case PlayerStatType.Defence_Health:
                {
                    PlayerStatsManager.Instance.IncreaseMaxHealthFromFlat((int)upgradeValue);
                    break;
                }

            case PlayerStatType.Defence_HealthRegenValue:
                {
                    PlayerStatsManager.Instance.RuntimeStats.AddFlat(upgradeCode, upgradeValue);
                    break;
                }
            case PlayerStatType.Defence_HealthRegenInterval:
                {
                    PlayerStatsManager.Instance.RuntimeStats.AddFlat(upgradeCode, upgradeValue);
                    break;
                }

            case PlayerStatType.Defence_Armor:
                {
                    PlayerStatsManager.Instance.RuntimeStats.AddFlat(upgradeCode, upgradeValue);
                    break;
                }

            case PlayerStatType.Defence_Mana:
                {
                    PlayerStatsManager.Instance.RuntimeStats.AddFlat(PlayerStatType.Defence_Mana, upgradeValue);
                    PlayerStatsManager.Instance.CurrentMana += (int)upgradeValue;
                    break;
                }

            case PlayerStatType.Defence_ManaRegen:
                {
                    PlayerStatsManager.Instance.RuntimeStats.AddFlat(upgradeCode, upgradeValue);
                    break;
                }

            case PlayerStatType.Defence_ManaRegenInterval:
                {
                    if (PlayerStatsManager.Instance.RuntimeStats.Get(PlayerStatType.Defence_ManaRegenInterval) <= 2)
                    {
                        PlayerStatsManager.Instance.RuntimeStats.AddFlat(upgradeCode, upgradeValue / 2);
                    }
                    PlayerStatsManager.Instance.RuntimeStats.AddFlat(upgradeCode, upgradeValue);
                    break;
                }

            case PlayerStatType.Defence_BetterPotions:
                {
                    healthPotionHealth += upgradeValue;
                    manaPotionMana += upgradeValue;
                    break;
                }

            // ================= ECONOMY =================
            case PlayerStatType.Economy_CoinsValue:
                {
                    yellowCoinValue += (int)upgradeValue;
                    break;
                }

            case PlayerStatType.Economy_MoreCoins:
                {
                    IncreaseCoinDropChance();
                    break;
                }

            case PlayerStatType.Economy_LessExpToLvl:
                {
                    Debug.LogError("Less EXP to Level upgrade is not yet implemented.");
                    break;
                }

            case PlayerStatType.Economy_MoreShardExp:
                {
                    IncreaseShardsExp(upgradeValue);
                    break;
                }

            case PlayerStatType.Economy_PickUpRange:
                {
                    playerMagnetColliderGO.transform.localScale = new(playerMagnetColliderGO.transform.localScale.x + upgradeValue, playerMagnetColliderGO.transform.localScale.y + upgradeValue);
                    break;
                }

            // ================= SPELLS =================

            case PlayerStatType.Spells_FireBlade:
                {
                    PlayerStatsManager.Instance.RuntimeStats.AddFlat(upgradeCode, 1);
                    if (!fireBladeSpellEnabled)
                    {
                        fireBladeSpellEnabled = true;
                        playerScript.EnableUpgradeSpell(upgradeCode);
                        return;
                    }
                    else
                    {
                        gv.fireBladeManaTotal += 2;

                        if (gv.fireBladeDelay > 0.2f)
                            gv.fireBladeDelay -= 0.05f;

                        gv.fireBladeManaSpellDamageMutli += 0.2f;
                        gv.fireBladeManaSpellCriticalChance += 0.1f;
                        gv.fireBladeManaSpellCriticalMultiplier += 0.1f;
                        gv.fireBladeManaSpellPiercing += 5;
                    }
                    break;
                }

            case PlayerStatType.Spells_RotatingBlades:
                {
                    PlayerStatsManager.Instance.RuntimeStats.AddFlat(upgradeCode, 1);
                    if (!rotatingBladesSpellEnabled)
                    {
                        rotatingBladesSpellEnabled = true;
                        playerScript.EnableUpgradeSpell(upgradeCode);
                        return;
                    }
                    else
                    {
                        gv.totalOrbitBlades += 2;
                        gv.orbitBladeDuration += 0.3f;
                        if (gv.orbitBladeRotationSpeed < 150f)
                            gv.orbitBladeRotationSpeed += 20f;
                        gv.orbitBladeRadius += 0.4f;
                    }
                    break;
                }

            case PlayerStatType.Spells_Shield:
                {
                    PlayerStatsManager.Instance.RuntimeStats.AddFlat(upgradeCode, 1);
                    if (!shieldSpellEnabled)
                    {
                        shieldSpellEnabled = true;
                        playerScript.EnableUpgradeSpell(upgradeCode);
                        return;
                    }
                    else
                    {
                        gv.shieldSpellDamage += 1;
                        gv.shieldSpellDuration += 1f;
                        gv.shieldSpellPiercing += 2;
                        gv.shieldSpellSpeed += 0.2f;
                    }
                    break;
                }

            case PlayerStatType.Spells_CooldownReduction:
                {
                    Debug.Log($"Pending Implementation for {upgradeCode}");
                    break;
                }

            // ================= DEFAULT =================
            default:
                {
                    Debug.LogWarning($"No upgradeCode was found for {upgradeCode}");
                    break;
                }
        }
    }

    public void RecalculateRange()
    {
        playerRangeTransform.localScale = new Vector3(
            PlayerStatsManager.Instance.RuntimeStats.Get(PlayerStatType.Attack_AttackRange),
            PlayerStatsManager.Instance.RuntimeStats.Get(PlayerStatType.Attack_AttackRange),
            1f
        );
    }

    // ================= HELPERS =================
    private void IncreaseCoinDropChance()
    {
        DifficultyManager.Instance.EnemyCoinDropMultiplier *= 1.2f;
    }

    public void UpdatePlayerSpeed(float speedIncrease, bool decrease = false)
    {
        if (decrease)
            PlayerStatsManager.Instance.RuntimeStats.AddFlat(PlayerStatType.Defence_MovementSpeed, -speedIncrease);
        else
            PlayerStatsManager.Instance.RuntimeStats.AddFlat(PlayerStatType.Defence_MovementSpeed, speedIncrease);
        Debug.Log($"Player movement speed updated by {(decrease ? -speedIncrease : speedIncrease)}. New speed: {PlayerStatsManager.Instance.RuntimeStats.Get(PlayerStatType.Defence_MovementSpeed)}");
    }

    private void IncreaseShardsExp(float upgradeValue)
    {
        expMultiplier += upgradeValue;
        Debug.Log($"Shard EXP multiplier increased by {upgradeValue}. New multiplier: {expMultiplier}");
    }

}
