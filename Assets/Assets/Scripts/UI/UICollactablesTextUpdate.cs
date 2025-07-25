using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UICollactablesTextUpdate : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinsCollectedText;
    [SerializeField] private TextMeshProUGUI permanentCoinsCollectedText;
    [SerializeField] private TextMeshProUGUI diamondsCollectedText;



    void Update()
    {
        coinsCollectedText.text = FormatValues(GlobalVariables.Instance.coinsCollected);
        permanentCoinsCollectedText.text = GlobalVariables.Instance.permanentCoinsCollected.ToString();
        diamondsCollectedText.text = GlobalVariables.Instance.diamondsCollected.ToString();
    }

    public static string FormatValues(int amount)
    {
        if (amount >= 1000000)
            return (amount / 1000000f).ToString("0.#") + "M";  // e.g. 1.5M
        if (amount >= 1000)
            return (amount / 1000f).ToString("0.#") + "K";     // e.g. 1.2K

        return amount.ToString();  // e.g. 999
    }
}
