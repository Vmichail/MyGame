using UnityEngine;

public class DeveloperModeLogic : MonoBehaviour
{
    [SerializeField] private GameObject developerModePanel;
    private void Start()
    {
        GlobalVariables.Instance.OnDeveloperModeChanged += ShowHideDeveloperPanel;
        ShowHideDeveloperPanel(GlobalVariables.Instance.developerMode);

    }
    private void ShowHideDeveloperPanel(bool show)
    {
        if (developerModePanel == null)
        {
            Debug.LogError("DeveloperModePanel reference is not set in DeveloperModeLogic.");
            return;
        }
        if (show)
        {
            developerModePanel.SetActive(true);
        }
        else
        {
            developerModePanel.SetActive(false);
        }
    }
}
