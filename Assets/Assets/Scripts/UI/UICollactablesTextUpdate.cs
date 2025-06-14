using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UICollactablesTextUpdate : MonoBehaviour
{
    private TextMeshPro coinsCollectedText;
    private TextMeshPro permanentCoinsCollectedText;
    private TextMeshPro diamondsCollectedText;



    void Update()
    {
        coinsCollectedText.text = GlobalVariables.Instance.coinsCollected.ToString();
        permanentCoinsCollectedText.text = GlobalVariables.Instance.permanentCoinsCollected.ToString();
        diamondsCollectedText.text = GlobalVariables.Instance.diamondsCollected.ToString();
    }
}
