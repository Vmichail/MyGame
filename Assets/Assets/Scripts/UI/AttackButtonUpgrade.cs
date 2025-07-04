using TMPro;
using UnityEngine;

public class AttackUpdateButtonPressed : UpgradeButtonBase
{
    [SerializeField] private TextMeshProUGUI attackDamageValueText;
    [SerializeField] private TextMeshProUGUI attackDamagePriceText;
    [SerializeField] private TextMeshProUGUI attackSpeedDamageValueText;
    [SerializeField] private TextMeshProUGUI attackSpeedPriceText;

    public void Start()
    {
        InitializeValues();
    }


    public enum AttackUpgrade
    {
        AttackDamage,
        AttackSpeed,
    }
    public void BuyAttackUpgrade(AttackUpgrade attackUpgrade)
    {
        switch (attackUpgrade)
        {
            case AttackUpgrade.AttackDamage:
                if (GlobalVariables.Instance.coinsCollected >= UpgradePrices.Instance.attackDamageUpgradePrice)
                {
                    GlobalVariables.Instance.coinsCollected -= UpgradePrices.Instance.attackDamageUpgradePrice;
                    GlobalVariables.Instance.playerAttackDamage += 1;
                    UpgradePrices.Instance.attackDamageUpgradePrice += (int)(10 + UpgradePrices.Instance.attackDamageUpgradePrice * 0.4f);
                    attackDamageValueText.text = "Damage:" + GlobalVariables.Instance.playerAttackDamage.ToString();
                    attackDamagePriceText.text = UpgradePrices.Instance.attackDamageUpgradePrice.ToString();
                    TriggerBuff();
                }
                break;

            case AttackUpgrade.AttackSpeed:
                if (GlobalVariables.Instance.coinsCollected >= UpgradePrices.Instance.attackSpeedUpgradePrice)
                {
                    GlobalVariables.Instance.coinsCollected -= UpgradePrices.Instance.attackSpeedUpgradePrice;
                    GlobalVariables.Instance.playerAttackSpeed += 0.1f;
                    UpgradePrices.Instance.attackSpeedUpgradePrice += (int)(10 + UpgradePrices.Instance.attackSpeedUpgradePrice * 1.1f);
                    attackSpeedDamageValueText.text = "Attack Sp.:" + GlobalVariables.Instance.playerAttackSpeed.ToString();
                    attackSpeedPriceText.text = UpgradePrices.Instance.attackSpeedUpgradePrice.ToString();
                    TriggerBuff();
                }
                break;

            default:
                Debug.LogWarning("Unknown upgrade type: " + attackUpgrade);
                break;
        }
    }

    public void BuyAttackDamage()
    {
        BuyAttackUpgrade(AttackUpgrade.AttackDamage);
    }

    public void BuyAttackSpeed()
    {
        BuyAttackUpgrade(AttackUpgrade.AttackSpeed);
    }


    private void InitializeValues()
    {
        attackDamageValueText.text = "Damage:" + GlobalVariables.Instance.playerAttackDamage.ToString();
        attackDamagePriceText.text = UpgradePrices.Instance.attackDamageUpgradePrice.ToString();
        attackSpeedDamageValueText.text = "Attack Sp.:" + GlobalVariables.Instance.playerAttackSpeed.ToString();
        attackSpeedPriceText.text = UpgradePrices.Instance.attackSpeedUpgradePrice.ToString();
    }
}
