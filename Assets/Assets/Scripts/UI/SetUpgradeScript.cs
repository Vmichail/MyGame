using Coffee.UIEffects;
using System;
using System.Collections.Generic;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SetUpgradeScript : BaseButtonScript
{
    //ScriptableObject values 
    public Sprite Icon { get; set; }
    public Boolean HealthCost { get; set; }
    public string Text { get; set; }
    public HeroUpgrades.UpgradeCode UpgradeCode { get; set; }
    public HeroUpgrades.UpgradeCategory UpgradeCatecory { get; set; }
    //Other public values
    public int Cost { get; set; }
    public bool IsLocked { get; set; }
    public int UnlockValue { get; set; }
    //Private values unchangable
    [SerializeField] private Image cardImage;
    [SerializeField] private Image iconImage;
    [SerializeField] private GameObject maskedGameObject;
    [SerializeField] private TextMeshProUGUI unlockCostText;
    [SerializeField] private Image coinCost;
    [SerializeField] private Image healthCost;
    [SerializeField] private TextMeshProUGUI costText;
    private UIEffect uiEffect;

    [SerializeField] private GameObject[] bonuses;
    private Mask maskComponent;


    public void SetUpgradeChoice(UpgradeChoice upgradeChoice, int cost, bool locked, int unlockPrice, List<UpgradeChoice> additionalChoices)
    {
        cardImage.sprite = upgradeChoice.Image;
        iconImage.sprite = upgradeChoice.Icon;
        if (cost > 0)
        {
            costText.enabled = true;
        }
        else
        {
            costText.enabled = false;
        }
        costText.text = cost.ToString();
        bonuses[0].SetActive(true);

        TextMeshProUGUI titleText = bonuses[0].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI valueText = bonuses[0].transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        if (additionalChoices.Count == 1)
        {
            cardImage.color = new Color(0f, 1f, 0f, 1f);
        }
        else if (additionalChoices.Count == 2)
        {
            cardImage.color = new Color(1f, 1f, 0f, 1f);
        }
        else if (additionalChoices.Count == 3)
        {
            cardImage.color = new Color(0f, 1f, 1f, 1f);
        }
        titleText.text = HeroUpgrades.GetUpgradeTitle(upgradeChoice.UpgradeCode);
        float displayValue = upgradeChoice.UpgradeValue.percentage
        ? upgradeChoice.UpgradeValue.value * 100f
        : upgradeChoice.UpgradeValue.value;

        valueText.text = displayValue.ToString("0.#") + (upgradeChoice.UpgradeValue.percentage ? "%" : "");
        int additionalBonusIndex = 1;
        foreach (UpgradeChoice additionalChoice in additionalChoices)
        {
            if (additionalBonusIndex >= bonuses.Length || additionalBonusIndex > 3)
                break;

            bonuses[additionalBonusIndex].SetActive(true);

            TextMeshProUGUI titleText1 = bonuses[additionalBonusIndex].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI valueText1 = bonuses[additionalBonusIndex].transform.GetChild(1).GetComponent<TextMeshProUGUI>();

            titleText1.text = HeroUpgrades.GetUpgradeTitle(additionalChoice.UpgradeCode);
            float displayValue1 = additionalChoice.UpgradeValue.percentage
             ? additionalChoice.UpgradeValue.value * 100f
             : additionalChoice.UpgradeValue.value;

            valueText1.text = displayValue1.ToString("0.#") + (additionalChoice.UpgradeValue.percentage ? "%" : "");

            additionalBonusIndex++;
        }

        maskedGameObject.SetActive(locked);
        if (locked)
        {
            DisableUIEffect();
            IsLocked = true;
            unlockCostText.text = unlockPrice.ToString();
            maskComponent.enabled = true;
        }
        if (HealthCost)
        {
            coinCost.gameObject.SetActive(false);
            healthCost.gameObject.SetActive(true);
        }
    }

    public void Awake()
    {
        if (!TryGetComponent(out maskComponent))
        {
            Debug.LogWarning($"[{nameof(SetUpgradeScript)}] No Mask component found on '{gameObject.name}'.", this);
        }
        if (!TryGetComponent(out uiEffect))
        {
            Debug.LogWarning($"[{nameof(SetUpgradeScript)}] No UIEffect component found on '{gameObject.name}'.", this);
        }
        foreach (var bonus in bonuses)
        {
            bonus.SetActive(false);
        }
    }


    public void DisableUIEffect()
    {
        if (uiEffect != null)
            uiEffect.enabled = false;
    }

    public void EnableUIEffect()
    {
        if (uiEffect != null)
            uiEffect.enabled = true;
    }

    public void SetPrice(int upgradePrice)
    {
        costText.text = upgradePrice.ToString();
    }

    public void RemoveLock(Button button, int index, int unlockCost)
    {
        if (IsLocked)
        {
            IsLocked = false;
            GlobalVariables.Instance.coinsCollected -= unlockCost;
            if (index == 3)
                GlobalVariables.Instance.isUpgradeOption4Unlocked = true;
            if (index == 4)
                GlobalVariables.Instance.isUpgradeOption5Unlocked = true;
            button.interactable = false;
            if (maskedGameObject == null)
            {
                Debug.LogWarning($"maskedGameObject is null inside {gameObject.name}!!!");
                return;
            }
            EnableUIEffect();
            maskComponent.enabled = false;
            LeanTween.scale(maskedGameObject, Vector3.zero, 0.5f)
                .setIgnoreTimeScale(true)
                .setOnComplete(() => SetActiveAndInteractive(button));
        }
    }

    private void SetActiveAndInteractive(Button button)
    {
        maskedGameObject.SetActive(false);
        button.interactable = true;

    }

}


