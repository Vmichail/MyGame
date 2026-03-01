using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradesMessengerManager : MonoBehaviour
{
    public static UpgradesMessengerManager instance;
    [SerializeField] private UpgradeDescriptionScript upgradeDescriptionScript;

    public Texture2D hoverCursor;
    [Header("All Upgrade Messager Infos")]
    [SerializeField] private GameObject messageGO;
    [SerializeField] private Image leftIcon;
    [SerializeField] private Image rightIcon;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private GameObject price;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private GameObject purchased;
    [SerializeField] private GameObject locked;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        messageGO.SetActive(false);
    }

    public void ShowMessage(UpgradeDataSO upgradeData, bool available)
    {

        messageGO.SetActive(true);
        leftIcon.sprite = upgradeData.icon;
        rightIcon.sprite = upgradeData.icon;
        title.text = upgradeData.title;
        description.text = upgradeData.description;
        if (UpgradeLoadManager.Instance.IsPurchased(upgradeData.upgradeId))
        {
            purchased.SetActive(true);
            price.SetActive(false);
            locked.SetActive(false);
        }
        else if (!available)
        {
            purchased.SetActive(false);
            price.SetActive(false);
            locked.SetActive(true);
        }
        else
        {
            purchased.SetActive(false);
            locked.SetActive(false);
            price.SetActive(true);
            priceText.text = upgradeData.price.ToString();
        }
    }

    public void HideMessage()
    {
        messageGO.SetActive(false);
    }

    public void ShakeDescription()
    {
        upgradeDescriptionScript.Shake();
    }
}
