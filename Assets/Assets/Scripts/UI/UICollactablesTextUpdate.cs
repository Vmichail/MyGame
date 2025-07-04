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
        coinsCollectedText.text = GlobalVariables.Instance.coinsCollected.ToString();
        permanentCoinsCollectedText.text = GlobalVariables.Instance.permanentCoinsCollected.ToString();
        diamondsCollectedText.text = GlobalVariables.Instance.diamondsCollected.ToString();
    }
}
