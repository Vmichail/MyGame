using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsPanelScript : MonoBehaviour
{
    [SerializeField] private Toggle showEnemiesHealthToggle;
    [SerializeField] private Toggle developerModeToggle;
    [SerializeField] private GameObject musicButton;
    [SerializeField] private GameObject otherButton;
    [SerializeField] private GameObject musicSettingsPanel;
    [SerializeField] private GameObject otherSettingsPanel;
    [SerializeField] private Color textUnselectedColor;
    [SerializeField] private Color textSelectedColor;
    [SerializeField] private Color imageUnselectedColor;
    [SerializeField] private Color imageSelectedColor;
    public void ShowMusicSettings()
    {
        musicSettingsPanel.SetActive(true);
        otherSettingsPanel.SetActive(false);
        SelectUnSelectButtons();
    }

    public void ShowOtherSettings()
    {
        musicSettingsPanel.SetActive(false);
        otherSettingsPanel.SetActive(true);
        SelectUnSelectButtons();
    }

    private void OnEnable()
    {
        SelectUnSelectButtons();
        showEnemiesHealthToggle.SetIsOnWithoutNotify(GlobalVariables.Instance.showAllEnemiesHealth);
        developerModeToggle.SetIsOnWithoutNotify(GlobalVariables.Instance.developerMode);
    }

    private void SelectUnSelectButtons()
    {
        if (musicSettingsPanel.activeSelf)
        {
            musicButton.GetComponent<Image>().color = imageSelectedColor;
            musicButton.GetComponentInChildren<TMP_Text>().color = textSelectedColor;
        }
        else
        {
            musicButton.GetComponent<Image>().color = imageUnselectedColor;
            musicButton.GetComponentInChildren<TMP_Text>().color = textUnselectedColor;
        }

        if (otherSettingsPanel.activeSelf)
        {
            otherButton.GetComponent<Image>().color = imageSelectedColor;
            otherButton.GetComponentInChildren<TMP_Text>().color = textSelectedColor;
        }
        else
        {
            otherButton.GetComponent<Image>().color = imageUnselectedColor;
            otherButton.GetComponentInChildren<TMP_Text>().color = textUnselectedColor;
        }
    }

    public void SetShowAllEnemiesHealth(bool isOn)
    {
        ToggleGeneric(isOn);
        GlobalVariables.Instance.SetShowAllEnemiesHealth(isOn);
    }

    public void SetDeveloperMode(bool isOn)
    {
        ToggleGeneric(isOn);
        GlobalVariables.Instance.SetDeveloperMode(isOn);
    }

    private void ToggleGeneric(bool isOn)
    {
        if (isOn)
            AudioManager.Instance.PlaySoundFX("buttonClickSound", transform.position, 1f, 1f, 1.25f);
        else
            AudioManager.Instance.PlaySoundFX("uiDeny", transform.position, 1f, 1f, 1.25f);
    }

}
