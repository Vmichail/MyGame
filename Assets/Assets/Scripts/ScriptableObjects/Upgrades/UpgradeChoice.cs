using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Upgrade Choice", menuName = "Upgrades/NewChoice")]
public class UpgradeChoice : ScriptableObject
{
    [Header("Visuals")]
    [SerializeField] private Sprite image;
    [SerializeField] private Sprite icon;

    [Header("Base Info")]
    [SerializeField] private int cost;
    [SerializeField] private bool locked;
    [SerializeField] private int unlockCost;
    [SerializeField] private GlobalVariables.UpgradeCategory upgradeCategory;
    [SerializeField] private GlobalVariables.UpgradeCode upgradeCode;

    [Header("Bonus Values")]
    [SerializeField] private List<UpgradeValueDataSO> bonuses = new();

    // ✅ Properties
    public Sprite Image => image;
    public Sprite Icon => icon;
    public int Cost => cost;
    public bool Locked => locked;
    public int UnlockCost => unlockCost;
    public GlobalVariables.UpgradeCategory UpgradeCategory => upgradeCategory;
    public GlobalVariables.UpgradeCode UpgradeCode => upgradeCode;

    public List<UpgradeValueDataSO> Bonuses => bonuses;
}