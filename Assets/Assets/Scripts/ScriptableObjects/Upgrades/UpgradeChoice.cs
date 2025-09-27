using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade Choice", menuName = "Upgrades/NewChoice")]
public class UpgradeChoice : ScriptableObject
{
    [SerializeField] private Sprite image;
    [SerializeField] private string title;
    [SerializeField] private Sprite icon;
    [SerializeField] private int value;
    [SerializeField] private bool percentage;
    [SerializeField] private int cost;
    [SerializeField] private bool locked;
    [SerializeField] private int unlockCost;
    [SerializeField] private GlobalVariables.UpgradeCategory upgradeCategory;
    [SerializeField] private GlobalVariables.UpgradeCode upgradeCode;

    public Sprite Image => image;
    public string Title => title;
    public Sprite Icon => icon;
    public int Cost => cost;
    public GlobalVariables.UpgradeCategory UpgradeCategory => upgradeCategory;
    public GlobalVariables.UpgradeCode UpgradeCode => upgradeCode;
    public bool Locked => locked;
    public int UnlockCost => unlockCost;
    public int Value => value;
    public bool Percentage => percentage;

}