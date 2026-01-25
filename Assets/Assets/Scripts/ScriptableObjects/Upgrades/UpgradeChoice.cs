using UnityEngine;

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
    [SerializeField] private PlayerStatCategory upgradeCategory;
    [SerializeField] private PlayerStatType upgradeCode;

    [Header("Bonus Values")]
    [SerializeField] private UpgradeValueData upgradeValueData;

    public Sprite Image => image;
    public Sprite Icon => icon;
    public int Cost => cost;
    public bool Locked => locked;
    public int UnlockCost => unlockCost;
    public PlayerStatCategory UpgradeCategory => upgradeCategory;
    public PlayerStatType UpgradeCode => upgradeCode;
    public UpgradeValueData UpgradeValue => upgradeValueData;

    // 👇 Nested serializable class
    [System.Serializable]
    public class UpgradeValueData
    {
        [Header("Value Settings")]
        public float value;
        public bool percentage;
    }
}
