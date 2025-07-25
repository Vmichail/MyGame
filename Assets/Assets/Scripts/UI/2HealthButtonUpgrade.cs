using TMPro;
using UnityEngine;

public class HealthUpgrateButton : UpgradeButtonBase
{
    [SerializeField] private TextMeshProUGUI healthValueText;
    [SerializeField] private TextMeshProUGUI healthPriceText;
    [SerializeField] private TextMeshProUGUI healthRegenValueText;
    [SerializeField] private TextMeshProUGUI healthRegenPriceText;
    [SerializeField] private TextMeshProUGUI armorIncreaseValueText;
    [SerializeField] private TextMeshProUGUI armorIncreasePriceText;

    public void Start()
    {
        InitializeValues();
    }


    public enum HealthUpgrade
    {
        Health,
        HealthRegen,
        Armor,
    }
    public void BuyHealthUpgrade(HealthUpgrade healthUpgrade)
    {
        switch (healthUpgrade)
        {
            case HealthUpgrade.Health:
                if (GlobalVariables.Instance.coinsCollected >= UpgradePrices.Instance.healthUpgradePrice)
                {
                    GlobalVariables.Instance.coinsCollected -= UpgradePrices.Instance.healthUpgradePrice;
                    GlobalVariables.Instance.playerMaxHealth += 10;
                    GlobalVariables.Instance.playerCurrentHealth += 10;
                    UpgradePrices.Instance.healthUpgradePrice += (int)(10 + UpgradePrices.Instance.healthUpgradePrice * 0.1f);
                    healthValueText.text = "Health:" + GlobalVariables.Instance.playerMaxHealth.ToString();
                    healthPriceText.text = UpgradePrices.Instance.healthUpgradePrice.ToString();
                    TriggerBuff();
                }
                break;

            case HealthUpgrade.HealthRegen:
                if (GlobalVariables.Instance.coinsCollected >= UpgradePrices.Instance.healthRegenUpgradePrice)
                {
                    GlobalVariables.Instance.coinsCollected -= UpgradePrices.Instance.healthRegenUpgradePrice;
                    GlobalVariables.Instance.playerHealthRegen += 1f;
                    UpgradePrices.Instance.healthRegenUpgradePrice += (int)(10 + UpgradePrices.Instance.healthRegenUpgradePrice * 0.3f);
                    healthRegenValueText.text = "Health Regen.:" + GlobalVariables.Instance.playerHealthRegen.ToString();
                    healthRegenPriceText.text = UpgradePrices.Instance.healthRegenUpgradePrice.ToString();
                    TriggerBuff();
                }
                break;

            case HealthUpgrade.Armor:
                if (GlobalVariables.Instance.coinsCollected >= UpgradePrices.Instance.armorUpgradePrice)
                {
                    GlobalVariables.Instance.coinsCollected -= UpgradePrices.Instance.armorUpgradePrice;
                    GlobalVariables.Instance.playerArmor += 1f;
                    UpgradePrices.Instance.armorUpgradePrice += (int)(10 + UpgradePrices.Instance.armorUpgradePrice * 0.3f);
                    armorIncreaseValueText.text = "Armor:" + GlobalVariables.Instance.playerArmor.ToString();
                    armorIncreasePriceText.text = UpgradePrices.Instance.armorUpgradePrice.ToString();
                    TriggerBuff();
                }
                break;

            default:
                Debug.LogWarning("Unknown upgrade type: " + healthUpgrade);
                break;
        }
    }

    public void BuyHealth()
    {
        BuyHealthUpgrade(HealthUpgrade.Health);
    }

    public void BuyHealthRegen()
    {
        BuyHealthUpgrade(HealthUpgrade.HealthRegen);
    }

    public void BuyArmor()
    {
        BuyHealthUpgrade(HealthUpgrade.Armor);
    }


    private void InitializeValues()
    {
        healthValueText.text = "Health:" + GlobalVariables.Instance.playerMaxHealth.ToString();
        healthPriceText.text = UpgradePrices.Instance.healthUpgradePrice.ToString();
        healthRegenValueText.text = "HealthRegen:" + GlobalVariables.Instance.playerHealthRegen.ToString();
        healthRegenPriceText.text = UpgradePrices.Instance.healthRegenUpgradePrice.ToString();
        armorIncreaseValueText.text = "Armor:" + GlobalVariables.Instance.playerArmor.ToString();
        armorIncreasePriceText.text = UpgradePrices.Instance.armorUpgradePrice.ToString();
    }
}
