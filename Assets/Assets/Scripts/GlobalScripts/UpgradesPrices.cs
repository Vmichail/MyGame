using UnityEngine;

public class UpgradePrices : MonoBehaviour
{
    public static UpgradePrices Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Prevent duplicates
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Optional: persist across scenes
    }

    public int attackDamageUpgradePrice = 10;
    public int attackSpeedUpgradePrice = 10;

}
