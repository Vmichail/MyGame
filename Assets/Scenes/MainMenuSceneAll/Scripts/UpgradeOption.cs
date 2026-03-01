using Coffee.UIEffects;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeOption : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerClickHandler
{
    private GameObject selectedGO;
    [SerializeField] private UpgradeDataSO upgradeData;
    [SerializeField] private Image iconImage;
    private Image buttonImage;
    [SerializeField] private UpgradeDataSO requiredUpgradeData;
    [SerializeField] private Image line1;
    [SerializeField] private Image line2;
    [SerializeField] private Image line3;
    [SerializeField] private Image line4;
    [Header("UI Effector")]
    private UIEffect uiEffect;
    private UIEffectTweener uiEffectTweener;
    private bool isRefundOption = false;

    [Header("Cursor")]
    [SerializeField] private Vector2 cursorHotspot = Vector2.zero;

    private void Awake()
    {
        if (upgradeData.upgradeId.Equals("Refund"))
            isRefundOption = true;

        if (transform.childCount == 0)
        {
            Debug.LogError($"{name} has no children but expects pointer as first child.", this);
            return;
        }
        buttonImage = GetComponent<Image>();
        selectedGO = transform.GetChild(0).gameObject;
        selectedGO.SetActive(false);
        if (iconImage == null)
        {
            iconImage = transform.GetChild(1).gameObject.GetComponent<Image>();
        }
        iconImage.sprite = upgradeData.icon;
        uiEffect = GetComponent<UIEffect>();
        uiEffectTweener = GetComponent<UIEffectTweener>();

    }

    private void OnEnable()
    {
        UpgradeLoadManager.OnUpgradesChanged += OnUpgradesChanged;
        OnUpgradesChanged();
    }

    private void OnDisable()
    {
        UpgradeLoadManager.OnUpgradesChanged -= OnUpgradesChanged;
        if (selectedGO != null)
            selectedGO.SetActive(false);
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (isRefundOption)
        {
            RefundAll();
            return;
        }

        if (upgradeData.upgradeId.Equals("TBD")
            || UpgradeLoadManager.Instance.IsPurchased(upgradeData.upgradeId)
            || (requiredUpgradeData != null && !SaveSystem.Data.purchasedUpgrades.Contains(requiredUpgradeData.upgradeId)))
        {
            UpgradesMessengerManager.instance.ShakeDescription();
            AudioManager.Instance.PlaySoundFX("uiDeny", Vector3.zero, 0.5f, 0.9f, 1.1f);
            Debug.Log("Upgrade already purchased");
            return;
        }
        else if (!CurrencyManager.instance.CanAfford(upgradeData.price))
        {
            UpgradesMessengerManager.instance.ShakeDescription();
            AudioManager.Instance.PlaySoundFX("uiDeny", Vector3.zero, 0.5f, 0.9f, 1.1f);
            Debug.Log("Not enough currency to purchase this upgrade");
            return;
        }
        BuyOption();
    }


    //Animations
    public void OnPointerEnter(PointerEventData eventData)
    {
        bool canUpgrade = requiredUpgradeData == null || SaveSystem.Data.purchasedUpgrades.Contains(requiredUpgradeData.upgradeId);
        selectedGO.SetActive(true);
        CursorManagerScript.SetBuyPointer();
        UpgradesMessengerManager.instance.ShowMessage(upgradeData, canUpgrade);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        selectedGO.SetActive(false);
        CursorManagerScript.SetDefault();
        UpgradesMessengerManager.instance.HideMessage();
    }

    private void BuyOption()
    {
        AudioManager.Instance.PlaySoundFX("UpgradeBoughtSound", Vector3.zero, 0.5f, 0.9f, 1.1f);
        CurrencyManager.instance.Spend(upgradeData.price);
        UpgradeLoadManager.Instance.Purchase(upgradeData);
        PlayerStatsManager.Instance.RuntimeStats.AddPermanentUpdate(upgradeData.playerStatType, upgradeData.upgradeAmount);
        Debug.Log($"Purchased upgrade: {upgradeData.title}");
        UpgradesMessengerManager.instance.ShowMessage(upgradeData, true);
    }

    private void RefundAll()
    {
        IReadOnlyCollection<string> purchasedUpgrades = UpgradeLoadManager.Instance.GetPurchasedUpgrades();
        AudioManager.Instance.PlaySoundFX("UpgradeBoughtSound", Vector3.zero, 0.6f, 0.95f, 1.05f);
        if (purchasedUpgrades.Count == 0)
            return;

        int totalRefund = 0;

        foreach (string upgrade in purchasedUpgrades)
        {
            UpgradeDataSO upgradeData = PlayerStatsManager.Instance.upgradeDatabase.Get(upgrade);

            if (upgradeData == null)
            {
                Debug.LogError($"Upgrade data not found for upgrade ID: {upgrade}");
                continue;
            }

            totalRefund += upgradeData.price;

            // Remove stat bonus
            PlayerStatsManager.Instance.RuntimeStats
                .RemovePermanentUpdate(upgradeData.playerStatType, -upgradeData.upgradeAmount);
        }

        CurrencyManager.instance.Add(totalRefund);
        Debug.Log($"Before Refund - total count: {purchasedUpgrades.Count}");
        UpgradeLoadManager.Instance.ClearAllPurchases();
        Debug.Log($"Refunded {purchasedUpgrades.Count} upgrades for {totalRefund} gold.");
    }

    //Event
    private void OnUpgradesChanged()
    {
        if (SaveSystem.Data.purchasedUpgrades.Contains(upgradeData.upgradeId) || isRefundOption)
        {
            buttonImage.color = Color.white;
            iconImage.color = Color.white;
            uiEffect.enabled = true;
            uiEffectTweener.enabled = true;
            EnableLines(true);
        }
        else
        {
            buttonImage.color = Color.gray;
            iconImage.color = Color.gray;
            EnableLines(false);
            uiEffect.enabled = false;
            uiEffectTweener.enabled = false;
        }
    }

    private void EnableLines(bool enabled)
    {
        if (enabled)
        {
            if (line1 != null) line1.color = Color.yellow;
            if (line2 != null) line2.color = Color.yellow;
            if (line3 != null) line3.color = Color.yellow;
            if (line4 != null) line4.color = Color.yellow;
        }
        else
        {
            if (line1 != null) line1.color = Color.gray;
            if (line2 != null) line2.color = Color.gray;
            if (line3 != null) line3.color = Color.gray;
            if (line4 != null) line4.color = Color.gray;
        }
    }
}