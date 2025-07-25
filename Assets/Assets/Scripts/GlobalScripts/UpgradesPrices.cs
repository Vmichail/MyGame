using UnityEngine;

public class UpgradePrices : MonoBehaviour
{
    public static UpgradePrices Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Prevent duplicates
            return;
        }
        CacheInitialValues();
        Instance = this;
        DontDestroyOnLoad(gameObject); // Optional: persist across scenes
    }
    [Header("Attack Prices")]
    public int attackDamageUpgradePrice = 10;
    public int attackSpeedUpgradePrice = 10;
    public int attackRangePrice = 10;
    [Header("Health Prices")]
    public int healthUpgradePrice = 10;
    public int healthRegenUpgradePrice = 10;
    public int armorUpgradePrice = 10;



    //BackupVariables
    private int _attackDamageUpgradePrice;
    private int _attackSpeedUpgradePrice;
    private int _attackRangePrice;
    private int _healthUpgradePrice;
    private int _healthRegenUpgradePrice;
    private int _armorUpgradePrice;

    private void CacheInitialValues()
    {
        _attackDamageUpgradePrice = attackDamageUpgradePrice;
        _attackSpeedUpgradePrice = attackSpeedUpgradePrice;
        _attackRangePrice = attackRangePrice;
        _healthUpgradePrice = healthUpgradePrice;
        _healthRegenUpgradePrice = healthRegenUpgradePrice;
        _armorUpgradePrice = armorUpgradePrice;
    }

    public void ResetValues()
    {
        attackDamageUpgradePrice = _attackDamageUpgradePrice;
        attackSpeedUpgradePrice = _attackSpeedUpgradePrice;
        attackRangePrice = _attackRangePrice;
        healthUpgradePrice = _healthUpgradePrice;
        healthRegenUpgradePrice = _healthRegenUpgradePrice;
        armorUpgradePrice = _armorUpgradePrice;
    }

}
