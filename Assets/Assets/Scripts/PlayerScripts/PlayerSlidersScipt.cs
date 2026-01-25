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
    [SerializeField] private Image healthFillImage;
    [SerializeField] private Image manaFillImage;
    [SerializeField] private Image expFillImage;

    [SerializeField] private Color lowHealthColor = new Color(0.8f, 0.2f, 0.2f);
    [SerializeField] private Color lowExpColor = new Color(0.9f, 0.8f, 0.25f); // soft yellow
    [SerializeField] private Color lowManaColor = new Color(0.25f, 0.8f, 0.8f);


    void Update()
    {
        var stats = PlayerStatsManager.Instance;
        //Health
        stats.CurrentHealth = Mathf.Clamp(stats.CurrentHealth, 0, stats.MaxHealth());
        healthText.text = stats.CurrentHealth + "/" + PlayerStatsManager.Instance.MaxHealth();
        float healthPercent = (float)stats.CurrentHealth / stats.MaxHealth();
        Color healthColor = Color.Lerp(Color.yellow, Color.green, healthPercent);
        Color healthColorImage = Color.Lerp(Color.pink, Color.green, healthPercent);
        healthFillImage.color = healthColorImage;
        healthText.color = healthColor;
        healthSlider.maxValue = stats.MaxHealth();
        healthSlider.value = stats.CurrentHealth;

        //Exp
        float expPercent = (float)stats.CurrentExp / stats.MaxExp;
        expPercent = Mathf.Clamp01(expPercent);

        Color expColor = Color.Lerp(lowExpColor, Color.yellow, expPercent);

        expText.text = $"{stats.CurrentExp}/{stats.MaxExp}";

        expSlider.maxValue = stats.MaxExp;
        expSlider.value = stats.CurrentExp;
        expFillImage.color = expColor;
        if (PlayerStatsManager.Instance.CurrentExp >= PlayerStatsManager.Instance.MaxExp && !levelUpPanel.activeSelf)
        {
            PlayerStatsManager.Instance.CurrentExp -= PlayerStatsManager.Instance.MaxExp;
            if (PlayerStatsManager.Instance.MaxExp >= 100)
            {
                PlayerStatsManager.Instance.MaxExp = 100;
            }
            else
            {
                PlayerStatsManager.Instance.MaxExp += 10;
            }
            PlayerStatsManager.Instance.CurrentLevel++;
            levelUpPanel.SetActive(true);
            stats.IncreaseMaxHealthFromLevels(1);
            if (PlayerStatsManager.Instance.CurrentLevel % 4 == 0)
            {
                stats.RuntimeStats.AddLevelValue(PlayerStatType.Attack_Attack, 1);
            }
        }

        //Mana
        float maxMana = stats.RuntimeStats.Get(PlayerStatType.Defence_Mana);
        float manaPercent = stats.CurrentMana / maxMana;
        manaPercent = Mathf.Clamp01(manaPercent);

        Color manaColor = Color.Lerp(lowManaColor, Color.cyan, manaPercent);

        manaText.text = $"{stats.CurrentMana}/{(int)maxMana}";

        manaSlider.maxValue = maxMana;
        manaSlider.value = stats.CurrentMana;
        manaFillImage.color = manaColor;


    }
}
