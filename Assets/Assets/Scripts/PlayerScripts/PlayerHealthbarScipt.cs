using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerHealthbarScipt : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI manaText;
    [SerializeField] TextMeshProUGUI expText;
    [SerializeField] Slider healthSlider;


    // Update is called once per frame
    void Update()
    {
        healthText.text = (int)GlobalVariables.Instance.playerCurrentHealth + "/" + (int)GlobalVariables.Instance.playerMaxHealth;
        float normalized = GlobalVariables.Instance.playerCurrentHealth / GlobalVariables.Instance.playerMaxHealth;
        healthSlider.maxValue = GlobalVariables.Instance.playerMaxHealth;
        healthSlider.value = GlobalVariables.Instance.playerCurrentHealth;
    }
}
