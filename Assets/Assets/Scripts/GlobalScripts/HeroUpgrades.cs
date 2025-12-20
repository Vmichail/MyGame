using System.Collections.Generic;
using UnityEngine;
using static UpgradeChoice;

public class HeroUpgrades : MonoBehaviour
{
    [Header("SerializeFields")]
    [SerializeField] private Transform playerRangeTransform;
    [SerializeField] private PlayerScript playerScript;
    [SerializeField] private CircleCollider2D playerMagnetColliderRadius;
    private bool firstTimeUpgrade = true;

    public static HeroUpgrades Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Prevent duplicates
            return;
        }
        firstTimeUpgrade = true;
        Instance = this;
    }

    // ================= ENUMS =================
    public enum UpgradeCategory
    {
        Attack,
        Defence,
        Economy,
        Spells,
    }

    public enum UpgradeCode
    {
        // Attack
        Attack_Attack,
        Attack_AttackSpeed,
        Attack_AttackRange,
        Attack_CriticalChance,
        Attack_CriticalDamage,
        Attack_Bounce,

        // Defence
        Defence_Health,
        Defence_HealthRegen,
        Defence_Armor,
        Defence_MovementSpeed,
        Defence_Mana,
        Defence_ManaRegen,
        Defence_BetterPotions,

        // Economy
        Economy_CoinsValue,
        Economy_MoreCoins,
        Economy_LessExpToLvl,
        Economy_MoreShardExp,
        Economy_PickUpRange,

        // Spells
        Spells_FireBlade,
        Spells_RotatingBlades,
        Spells_Shield,
        Spells_CooldownReduction,
    }

    // ================= TITLES =================
    public static readonly Dictionary<UpgradeCode, string> UpgradesTitles = new()
    {
        // 🗡️ Attack
        { UpgradeCode.Attack_Attack, "Attack Damage" },
        { UpgradeCode.Attack_AttackSpeed, "Attack Speed" },
        { UpgradeCode.Attack_AttackRange, "Attack Range" },
        { UpgradeCode.Attack_CriticalChance, "Critical Chance" },
        { UpgradeCode.Attack_CriticalDamage, "Critical Damage" },
        { UpgradeCode.Attack_Bounce, "Bounce" },

        // 🛡️ Defence
        { UpgradeCode.Defence_Health, "Health" },
        { UpgradeCode.Defence_HealthRegen, "Health Regen Time" },
        { UpgradeCode.Defence_Armor, "Armor" },
        { UpgradeCode.Defence_MovementSpeed, "Movement Speed" },
        { UpgradeCode.Defence_Mana, "Mana" },
        { UpgradeCode.Defence_ManaRegen, "Mana Regen" },
        { UpgradeCode.Defence_BetterPotions, "Better Potions" },

        // 💰 Economy
        { UpgradeCode.Economy_CoinsValue, "Coin Value" },
        { UpgradeCode.Economy_MoreCoins, "More Coins" },
        { UpgradeCode.Economy_LessExpToLvl, "Less EXP to Level" },
        { UpgradeCode.Economy_MoreShardExp, "More Shard EXP" },
        { UpgradeCode.Economy_PickUpRange, "Pick Up Range" },

        // 🔮 Spells
        { UpgradeCode.Spells_FireBlade, "Fire Blade" },
        { UpgradeCode.Spells_RotatingBlades, "Rotating Blades" },
        { UpgradeCode.Spells_Shield, "Shield" },
        { UpgradeCode.Spells_CooldownReduction, "Spells Cooldown Reduction" },
     };

    public static string GetUpgradeTitle(UpgradeCode code)
        => UpgradesTitles.TryGetValue(code, out var title) ? title : code.ToString();

    // ================= SPELL FLAGS =================
    private bool fireBladeSpellEnabled = false;
    private bool rotatingBladesSpellEnabled = false;
    private bool shieldSpellEnabled = false;

    private int fireBladeUpgrades = 0;
    private int rotatingBladesUpgrades = 0;
    private int shieldUpgrades = 0;

    // ================= CORE FUNCTION =================
    public void UpgradeHero(UpgradeChoice uc)
    {
        if (firstTimeUpgrade)
        {
            InitializeBaseValues();
        }
        var gv = GlobalVariables.Instance;
        float upgradeValue = uc.UpgradeValue.value;
        UpgradeCode upgradeCode = uc.UpgradeCode;
        //Debug.Log($"Applying upgrade: {upgradeCode} with value: {upgradeValue}");
        switch (upgradeCode)
        {
            // ================= ATTACK =================
            case UpgradeCode.Attack_AttackRange:
                {
                    Debug.Log($"Increasing attack range by {playerStartingAttackRange} * {upgradeValue}- from {gv.playerAttackRangeBaseScale} to {gv.playerAttackRangeBaseScale + (playerStartingAttackRange * upgradeValue)}");
                    gv.playerAttackRangeBaseScale += playerStartingAttackRange * upgradeValue;

                    playerRangeTransform.localScale = new Vector3(
                        gv.playerAttackRangeBaseScale,
                        gv.playerAttackRangeBaseScale,
                        1f
                    );
                    break;
                }

            case UpgradeCode.Attack_Attack:
                {
                    gv.playerAttackDamage += upgradeValue;
                    break;
                }

            case UpgradeCode.Attack_AttackSpeed:
                {
                    Debug.Log($"Increasing attack speed by {playerStartingAttackSpeed} * {upgradeValue}- from {gv.playerAttackSpeed} to {playerStartingAttackSpeed * upgradeValue}");
                    gv.playerAttackSpeed += playerStartingAttackSpeed * upgradeValue;
                    break;
                }

            case UpgradeCode.Attack_CriticalChance:
                {
                    Debug.Log($"Increasing critical chance by {playerStartingGlobalCriticalChance} * {upgradeValue}- from {gv.globalCriticalChance} to {playerStartingGlobalCriticalChance * upgradeValue}");
                    gv.globalCriticalChance += playerStartingGlobalCriticalChance * upgradeValue;
                    if (gv.globalCriticalChance < 0.1f)
                    {
                        Debug.Log("Critical chance too low, setting to minimum of 0.1f");
                        gv.globalCriticalChance = 0.1f;
                        playerStartingGlobalCriticalChance = 0.1f;
                    }
                    break;
                }

            case UpgradeCode.Attack_CriticalDamage:
                {
                    Debug.Log($"Increasing critical multiplier by {playerStartingCriticalMultiplier} * {upgradeValue}- from {gv.globalCriticalMultiplier} to {playerStartingCriticalMultiplier * upgradeValue}");
                    gv.globalCriticalMultiplier += playerStartingCriticalMultiplier * upgradeValue;
                    break;
                }

            case UpgradeCode.Attack_Bounce:
                {
                    Debug.Log($"Increasing spell bounces by {upgradeValue}- from {gv.defaultSpellBounces} to {gv.defaultSpellBounces + (int)upgradeValue}");
                    gv.defaultSpellBounces += (int)upgradeValue;
                    break;
                }

            // ================= DEFENCE =================
            case UpgradeCode.Defence_MovementSpeed:
                {
                    Debug.Log($"Increasing movement speed by {playerStartingMovementSpeed} * {upgradeValue}- from {playerScript.moveSpeed} to {playerStartingMovementSpeed * upgradeValue}");
                    UpdatePlayerSpeed(upgradeValue);
                    break;
                }

            case UpgradeCode.Defence_Health:
                {
                    Debug.Log($"Increasing health by {upgradeValue}- from {gv.playerMaxHealth} to {gv.playerMaxHealth + upgradeValue}");
                    gv.playerMaxHealth += upgradeValue;
                    gv.playerCurrentHealth += upgradeValue;
                    break;
                }

            case UpgradeCode.Defence_HealthRegen:
                {
                    Debug.Log($"Increasing health regen by {playerStartingHealthRegenValue} * {upgradeValue}- from {gv.playerHealthRegen} to {playerStartingHealthRegenValue * upgradeValue}");
                    Debug.Log($"Decreasing health regen interval by {playerStartingHealthRegenInterval} * {upgradeValue}- from {gv.playerHealthRegenInterval} to {gv.playerHealthRegenInterval - (playerStartingHealthRegenInterval * upgradeValue)}");
                    gv.playerHealthRegen += 1;
                    if (gv.playerHealthRegenInterval < 0.35f)
                    {
                        Debug.Log("Health regen interval too low, just double upgrading the health regen ");
                        gv.playerHealthRegen += 1;
                    }
                    else
                    {
                        gv.playerHealthRegenInterval -= playerStartingHealthRegenInterval * upgradeValue;
                    }
                    break;
                }

            case UpgradeCode.Defence_Armor:
                {
                    Debug.Log($"Increasing armor by {upgradeValue}- from {gv.playerArmor} to {gv.playerArmor + upgradeValue}");
                    gv.playerArmor += upgradeValue;
                    break;
                }

            case UpgradeCode.Defence_Mana:
                {
                    Debug.Log($"Increasing mana by {playerStartingMana} * {upgradeValue}- from {gv.playerMaxMana} to {gv.playerMaxMana + (playerStartingMana * upgradeValue)}");
                    gv.playerMaxMana += upgradeValue;
                    gv.playerCurrentMana += upgradeValue;
                    break;
                }

            case UpgradeCode.Defence_ManaRegen:
                {
                    Debug.Log($"Increasing mana regen by {playerStartingManaRegenValue} * {upgradeValue}- from {gv.playerManaRegen} to {gv.playerManaRegenInterval - (playerStartingManaRegenValue * upgradeValue)}");
                    gv.playerManaRegen += 1;
                    if (gv.playerManaRegenInterval < 0.35f)
                    {
                        Debug.Log("Mana regen interval too low, just double upgrading the mana regen ");
                        gv.playerManaRegen += 1;
                    }
                    else
                    {
                        gv.playerManaRegenInterval -= playerStartingManaRegenInterval * upgradeValue;
                    }
                    break;
                }

            case UpgradeCode.Defence_BetterPotions:
                {
                    Debug.Log($"Increasing health and mana potion effectiveness by {upgradeValue}- from {gv.healthPotionHealth} to {gv.healthPotionHealth + upgradeValue}");
                    gv.healthPotionHealth += upgradeValue;
                    gv.manaPotionMana += upgradeValue;
                    break;
                }

            // ================= ECONOMY =================
            case UpgradeCode.Economy_CoinsValue:
                {
                    Debug.Log($"Increasing yellow coin value by {startingYellowCoinValue} * {upgradeValue}- from {gv.yellowCoinValue} to {gv.yellowCoinValue + (startingYellowCoinValue * upgradeValue)}");
                    gv.yellowCoinValue += startingYellowCoinValue * upgradeValue;
                    break;
                }

            case UpgradeCode.Economy_MoreCoins:
                {
                    Debug.Log($"Increasing coin drop chance by 20% multiplicatively");
                    IncreaseCoinDropChance();
                    break;
                }

            case UpgradeCode.Economy_LessExpToLvl:
                {
                    Debug.Log($"Decreasing EXP required to level up by {upgradeValue}% - from {gv.maxExp} to {gv.maxExp * (1f - (upgradeValue / 100f))}");
                    // Reduces required EXP to level up
                    gv.maxExp *= 1f - (upgradeValue / 100f);
                    break;
                }

            // ✅ NEW: Economy_MoreShardExp (flat % increase)
            case UpgradeCode.Economy_MoreShardExp:
                {
                    Debug.Log($"Increasing shard EXP gain by {startingShardExp} * {upgradeValue}- from {gv.shardExp} to {startingShardExp * upgradeValue}");
                    gv.shardExp += startingShardExp * upgradeValue;
                    break;
                }

            // ✅ NEW: Economy_PickUpRange (percentage increase)
            case UpgradeCode.Economy_PickUpRange:
                {
                    Debug.Log($"Increasing pickup range({playerMagnetColliderRadius.radius}) by {playerStartingPickupRange} * {upgradeValue}");
                    playerMagnetColliderRadius.radius += playerStartingPickupRange * upgradeValue;
                    break;
                }

            // ================= SPELLS =================
            case UpgradeCode.Spells_FireBlade:
                {
                    if (!fireBladeSpellEnabled)
                    {
                        fireBladeSpellEnabled = true;
                        playerScript.EnableUpgradeSpell(upgradeCode);
                        return;
                    }
                    else
                    {
                        gv.fireBladeManaTotal += 2;

                        if (fireBladeUpgrades % 2 == 0 && gv.fireBladeDelay > 0.2f)
                            gv.fireBladeDelay -= 0.05f;

                        gv.fireBladeManaSpellDamageMutli += 0.2f;
                        gv.fireBladeManaSpellCriticalChance += 0.1f;
                        gv.fireBladeManaSpellCriticalMultiplier += 0.1f;
                        gv.fireBladeManaSpellPiercing += 5;
                    }
                    break;
                }

            case UpgradeCode.Spells_RotatingBlades:
                {
                    if (!rotatingBladesSpellEnabled)
                    {
                        rotatingBladesSpellEnabled = true;
                        playerScript.EnableUpgradeSpell(upgradeCode);
                        return;
                    }
                    else
                    {
                        gv.totalOrbitBlades += 2;
                        gv.orbitBladeDuration += 2f;

                        if (gv.orbitBladeRotationSpeed < 150f)
                            gv.orbitBladeRotationSpeed += 20f;

                        gv.orbitBladeRadius += 0.4f;

                        gv.orbidBladeSpellDamageMutli += 0.5f;
                        gv.orbidBladeSpellCriticalChance += 0.1f;
                        gv.orbidBladeSpellCriticalMultiplier += 0.1f;

                        if (rotatingBladesUpgrades % 2 == 0)
                            gv.orbidBladeSpellPiercing += 1;
                    }
                    break;
                }

            case UpgradeCode.Spells_Shield:
                {
                    if (!shieldSpellEnabled)
                    {
                        shieldSpellEnabled = true;
                        playerScript.EnableUpgradeSpell(upgradeCode);
                        return;
                    }
                    else
                    {
                        gv.shieldSpellDamage += 2;
                        gv.shieldSpellDuration += 1f;
                        gv.shieldSpellPiercing += 2;
                        gv.shieldSpellSpeedMultiply += 0.1f;
                    }
                    break;
                }

            case UpgradeCode.Spells_CooldownReduction:
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

    // ================= HELPERS =================
    private void IncreaseCoinDropChance()
    {
        var gv = GlobalVariables.Instance;
        gv.skeleonCoinDropChance *= 1.2f;
        gv.vampireType3CoinDropChance *= 1.2f;
        gv.goblinTNTCoinDropChance *= 1.2f;
        gv.goblinTorchCoinDropChance *= 1.2f;
    }

    public void UpdatePlayerSpeed(float speedIncrease, bool decrease = false)
    {
        playerScript.UpdatePlayerSpeed(speedIncrease, decrease);
    }

    public float playerStartingGlobalCriticalChance;
    public float playerStartingAttackSpeed;
    public float playerStartingMovementSpeed;
    public float playerStartingCriticalMultiplier;
    public float playerStartingMana;
    public float playerStartingSpeed;
    public float playerStartingHealthRegenValue;
    public float playerStartingHealthRegenInterval;
    public float playerStartingManaRegenValue;
    public float playerStartingManaRegenInterval;
    public float playerStartingArmor;
    public float playerStartingAttackRange;

    //Economy
    public float startingYellowCoinValue;
    public float startingShardExp;
    public float playerStartingPickupRange;
    private void InitializeBaseValues()
    {
        firstTimeUpgrade = false;
        playerStartingAttackRange = GlobalVariables.Instance.playerAttackRangeBaseScale;
        playerStartingAttackSpeed = GlobalVariables.Instance.playerAttackSpeed;
        playerStartingGlobalCriticalChance = GlobalVariables.Instance.globalCriticalChance;
        playerStartingCriticalMultiplier = GlobalVariables.Instance.globalCriticalMultiplier;
        playerStartingMovementSpeed = playerScript.moveSpeed;
        startingYellowCoinValue = GlobalVariables.Instance.yellowCoinValue;
        startingShardExp = GlobalVariables.Instance.shardExp;
        playerStartingPickupRange = playerMagnetColliderRadius.radius;
    }
}
