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
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private GameObject maskedGameObject;
    [SerializeField] private TextMeshProUGUI unlockCostText;


    public void SetUpgradeChoice(UpgradeChoice upgradeChoice, int cost, bool locked, int unlockPrice)
    {
        cardImage.sprite = upgradeChoice.Image;
        iconImage.sprite = upgradeChoice.Icon;
        titleText.text = upgradeChoice.Title;
        costText.text = cost.ToString();
        valueText.text = upgradeChoice.Value.ToString() + (upgradeChoice.Percentage ? "%" : "");
        maskedGameObject.SetActive(locked);
        if (locked)
        {
            IsLocked = true;
            unlockCostText.text = unlockPrice.ToString();
        }
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


