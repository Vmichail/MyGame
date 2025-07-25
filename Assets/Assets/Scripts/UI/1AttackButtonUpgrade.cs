using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class AttackUpdateButtonPressed : UpgradeButtonBase
{
    [SerializeField] private TextMeshProUGUI attackDamageValueText;
    [SerializeField] private TextMeshProUGUI attackDamagePriceText;
    [SerializeField] private TextMeshProUGUI attackSpeedValueText;
    [SerializeField] private TextMeshProUGUI attackSpeedPriceText;
    [SerializeField] private TextMeshProUGUI attackRangeValueText;
    [SerializeField] private TextMeshProUGUI attackRangePriceText;
    [SerializeField] private Transform rangeTransform;

    public void Start()
    {
        InitializeValues();
    }


    public enum AttackUpgrade
    {
        AttackDamage,
        AttackSpeed,
        AttackRange,
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
                    attackSpeedValueText.text = "Attack Sp.:" + GlobalVariables.Instance.playerAttackSpeed.ToString();
                    attackSpeedPriceText.text = UpgradePrices.Instance.attackSpeedUpgradePrice.ToString();
                    TriggerBuff();
                }
                break;

            case AttackUpgrade.AttackRange:
                if (GlobalVariables.Instance.coinsCollected >= UpgradePrices.Instance.attackRangePrice)
                {
                    GlobalVariables.Instance.coinsCollected -= UpgradePrices.Instance.attackRangePrice;
                    GlobalVariables.Instance.playerAttackRange += 10f;
                    UpgradePrices.Instance.attackRangePrice += (int)(10 + UpgradePrices.Instance.attackRangePrice * 1.4f);
                    attackRangeValueText.text = "Range:" + GlobalVariables.Instance.playerAttackRange.ToString();
                    attackRangePriceText.text = UpgradePrices.Instance.attackRangePrice.ToString();
                    UpgradeRange();
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

    public void BuyAttackRange()
    {
        BuyAttackUpgrade(AttackUpgrade.AttackRange);
    }


    private void InitializeValues()
    {
        attackDamageValueText.text = "Damage:" + GlobalVariables.Instance.playerAttackDamage.ToString();
        attackDamagePriceText.text = UpgradePrices.Instance.attackDamageUpgradePrice.ToString();
        attackSpeedValueText.text = "Attack Sp.:" + GlobalVariables.Instance.playerAttackSpeed.ToString();
        attackSpeedPriceText.text = UpgradePrices.Instance.attackSpeedUpgradePrice.ToString();
        attackRangeValueText.text = "Range:" + GlobalVariables.Instance.playerAttackRange.ToString();
        attackRangePriceText.text = UpgradePrices.Instance.attackRangePrice.ToString();

    }

    private void UpgradeRange()
    {
        float scale = GlobalVariables.Instance.playerAttackRangeBaseScale *
            (GlobalVariables.Instance.playerAttackRange / GlobalVariables.Instance.playerAttackRangeBase);
        rangeTransform.localScale = new Vector3(scale, scale, 1f);
    }

}
