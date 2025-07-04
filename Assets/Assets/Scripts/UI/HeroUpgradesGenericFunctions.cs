using UnityEngine;

public class HeroUpgradesGenericFunctions : MonoBehaviour
{
    [SerializeField] private GameObject heroPanelMinimized;
    [SerializeField] private GameObject heroPanelMaximized;

    public void MinimizeMaximazeHeroPanel()
    {
        if (heroPanelMaximized.activeInHierarchy)
        {
            heroPanelMaximized.SetActive(false);
            heroPanelMinimized.SetActive(true);
        }
        else
        {
            heroPanelMinimized.SetActive(false);
            heroPanelMaximized.SetActive(true);
        }
    }
}
