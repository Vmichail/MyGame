using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerSlidersScipt : MonoBehaviour
{
    [Header("--!!Texts!!--")]
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI manaText;
    [SerializeField] TextMeshProUGUI expText;
    [Header("--!!Sliders!!--")]
    [SerializeField] Slider healthSlider;
    [SerializeField] Slider expSlider;
    [SerializeField] Slider manaSlider;
    [SerializeField] GameObject levelUpPanel;


    void Update()
    {
        //Health
        healthText.text = (int)GlobalVariables.Instance.playerCurrentHealth + "/" + (int)GlobalVariables.Instance.playerMaxHealth;
        healthSlider.maxValue = GlobalVariables.Instance.playerMaxHealth;
        healthSlider.value = GlobalVariables.Instance.playerCurrentHealth;

        //Exp
        expText.text = (int)GlobalVariables.Instance.currentExp + "/" + (int)GlobalVariables.Instance.maxExp;
        expSlider.maxValue = GlobalVariables.Instance.maxExp;
        expSlider.value = GlobalVariables.Instance.currentExp;
        if (GlobalVariables.Instance.currentExp >= GlobalVariables.Instance.maxExp)
        {
            GlobalVariables.Instance.currentExp = GlobalVariables.Instance.currentExp - GlobalVariables.Instance.maxExp;
            GlobalVariables.Instance.maxExp = (float)(GlobalVariables.Instance.maxExp + 1.2 * GlobalVariables.Instance.level);
            levelUpPanel.SetActive(true);
        }

        //Mana
        manaText.text = (int)GlobalVariables.Instance.playerCurrentMana + "/" + (int)GlobalVariables.Instance.playerMaxMana;
        manaSlider.maxValue = GlobalVariables.Instance.playerMaxMana;
        manaSlider.value = GlobalVariables.Instance.playerCurrentMana;


    }
}
