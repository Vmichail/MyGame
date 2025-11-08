using Coffee.UIEffects;
using System;
using TMPro;
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
    public GlobalVariables.UpgradeCode UpgradeCode { get; set; }
    public GlobalVariables.UpgradeCategory UpgradeCatecory { get; set; }
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


    public void SetUpgradeChoice(UpgradeChoice upgradeChoice, int cost, bool locked, int unlockPrice)
    {
        cardImage.sprite = upgradeChoice.Image;
        iconImage.sprite = upgradeChoice.Icon;
        costText.text = cost.ToString();
        for (int i = 0; i < upgradeChoice.Bonuses.Count; i++)
        {
            bonuses[i].SetActive(true);
            // Get the first and second child TextMeshPro components
            TextMeshProUGUI titleText = bonuses[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI valueText = bonuses[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>();

            titleText.text = GlobalVariables.GetUpgradeTitle(upgradeChoice.Bonuses[i].upgradeCode);
            valueText.text = upgradeChoice.Bonuses[i].value.ToString() + (upgradeChoice.Bonuses[i].percentage ? "%" : "");
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


