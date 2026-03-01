using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager instance;

    public int Gold { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            if (GlobalVariables.Instance.testMode)
            {
                Gold = 100000;
            }
            else
            {
                Gold = SaveSystem.Data.gold;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool CanAfford(int amount) => Gold >= amount;

    public void Add(int amount)
    {
        Gold += amount;
        Save();
    }

    public void Reduce(int amount)
    {
        Gold -= amount;
        Save();
    }

    public void Spend(int amount)
    {
        if (!CanAfford(amount))
            return;

        Gold -= amount;
        Save();
    }

    private void Save()
    {
        SaveSystem.Data.gold = Gold;
        SaveSystem.Save();
    }
}