using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatRowUI : MonoBehaviour
{

    [SerializeField] private CanvasGroup canvasGroup;

    private void Awake()
    {
        // Ensure CanvasGroup exists
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void PlaySpawnAnimation(float delay)
    {
        canvasGroup.alpha = 0f;
        transform.localScale = new Vector3(1f, 0.9f, 1f);

        Sequence seq = DOTween.Sequence();

        seq.AppendInterval(delay);

        seq.Join(canvasGroup.DOFade(1f, 0.25f).SetEase(Ease.OutQuad));
        seq.Join(transform.DOScaleY(1f, 0.25f).SetEase(Ease.OutBack));

        seq.SetUpdate(true)
           .SetLink(gameObject);
    }
    [Header("Category Sprites")]
    [SerializeField] private Image statusImage;
    [SerializeField] private Sprite attackSprite;
    [SerializeField] private Sprite defenceSprite;
    [SerializeField] private Sprite utilitySprite;
    [SerializeField] private Sprite spellsSprite;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI statNameText;
    [SerializeField] private TextMeshProUGUI statStartedValueText;
    [SerializeField] private TextMeshProUGUI statUpgradesValueText;
    [SerializeField] private TextMeshProUGUI statLevelUpValueText;
    [SerializeField] private TextMeshProUGUI statTotalValueText;

    [Header("Row Background")]
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Sprite evenRowSprite;
    [SerializeField] private Sprite oddRowSprite;

    public void Setup(
        PlayerStatDefinition definition,
        PlayerStat stat,
        bool isEvenRow)
    {
        statNameText.text = definition.displayName;
        statStartedValueText.text = Format(stat.baseValue);
        statUpgradesValueText.text = stat.multiplier > 1 ? Format(stat.multiplier) : Format(stat.flatBonus);
        statLevelUpValueText.text = Format(stat.levelBonus);
        statTotalValueText.text = Format(stat.Value);
        statusImage.sprite = GetSpriteForCategory(definition.category);
        backgroundImage.sprite = isEvenRow
            ? evenRowSprite
            : oddRowSprite;
    }

    private string Format(float value)
    {
        if (Mathf.Approximately(value, 0f))
            return "-";

        return value.ToString("0.##");
    }

    private Sprite GetSpriteForCategory(PlayerStatCategory category)
    {
        return category switch
        {
            PlayerStatCategory.Attack => attackSprite,
            PlayerStatCategory.Defence => defenceSprite,
            PlayerStatCategory.Utility => utilitySprite,
            PlayerStatCategory.Spells => spellsSprite,
            _ => backgroundImage.sprite
        };

    }

}