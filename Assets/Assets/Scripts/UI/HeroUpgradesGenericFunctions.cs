using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class HeroUpgradesGenericFunctions : MonoBehaviour
{
    [Header("Game Menus")]
    [SerializeField] private GameObject gameMenuPanel;
    [SerializeField] private GameObject gameOverOptions;
    [SerializeField] private GameObject pauseOptions;
    [SerializeField] private GameObject spellIcons;
    [Header("Buy Menus")]
    [SerializeField] private GameObject heroBuyMenu;
    [SerializeField] private GameObject heroAttackMaximized;
    [SerializeField] private GameObject heroHealthMaximized;
    [SerializeField] RectTransform buyMenuChoices;
    [SerializeField] private Color selectedColor;
    [SerializeField] private Color normalColor;
    private int currentTabIndex = 0;
    [Header("Option Buttons")]
    [SerializeField] private Button attackButton;
    [SerializeField] private Button healthButton;
    [SerializeField] private Button economyButton;
    [SerializeField] private Button spellsButton;


    private void Start()
    {
        SetGameMenuPanelOff();
    }

    private void Update()
    {
        if (GlobalVariables.Instance.playerCurrentHealth <= 0 && GlobalVariables.Instance.playerIsAlive)
        {
            GlobalVariables.Instance.playerIsAlive = false;
            GlobalVariables.PauseTime(GlobalVariables.PauseReasonEnum.PlayerIsDead);
        }
        else if (GlobalVariables.Instance.playerCurrentHealth > 0 && !GlobalVariables.Instance.playerIsAlive)
        {
            GlobalVariables.Instance.playerIsAlive = true;
        }

        //GameOverMenu
        if (!GlobalVariables.Instance.playerIsAlive && !gameOverOptions.activeSelf)
        {
            gameOverOptions.SetActive(true);
        }

        //PauseUnPauseButton
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        //TABShowsMenu
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            EnableDisableBuyMenu();
        }

        //GameMenuPanel
        if (pauseOptions.activeSelf || gameOverOptions.activeSelf)
        {
            gameMenuPanel.SetActive(true);
        }
        else
        {
            gameMenuPanel.SetActive(false);
        }

    }

    public void SetChoicesToLeft(float topOffset = 0f)
    {
        buyMenuChoices.anchorMin = new Vector2(0f, 1f);
        buyMenuChoices.anchorMax = new Vector2(0f, 1f);
        buyMenuChoices.pivot = new Vector2(0f, 1f);
        buyMenuChoices.anchoredPosition = new Vector2(0f, -topOffset);
    }

    public void SetChoicesToBottom(float bottomOffset = 0f)
    {
        heroAttackMaximized.SetActive(false);
        heroHealthMaximized.SetActive(false);
        buyMenuChoices.anchorMin = new Vector2(0.5f, 0f);
        buyMenuChoices.anchorMax = new Vector2(0.5f, 0f);
        buyMenuChoices.pivot = new Vector2(0.5f, 0f);
        buyMenuChoices.anchoredPosition = new Vector2(0f, bottomOffset);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void ChangeTab(int tabIndex)
    {
        if (currentTabIndex == tabIndex)
        {
            SetChoicesToBottom();
            currentTabIndex = 0;
            return;
        }
        UpdateUI(tabIndex);
    }

    private void EnableDisableBuyMenu()
    {
        if (heroBuyMenu.activeInHierarchy == false && !GlobalVariables.gameIsPaused)
        {
            GlobalVariables.PauseTime(GlobalVariables.PauseReasonEnum.HeroBuyMenu);
            heroBuyMenu.SetActive(true);
            spellIcons.SetActive(false);
        }
        else
        {
            heroBuyMenu.SetActive(false);
            spellIcons.SetActive(true);
            GlobalVariables.UnPauseTime(GlobalVariables.PauseReasonEnum.HeroBuyMenu);
        }
    }
    private void UpdateUI(int index)
    {
        if (index == 1)
        {
            EventSystem.current.SetSelectedGameObject(attackButton.gameObject);
            heroAttackMaximized.SetActive(true);
            heroHealthMaximized.SetActive(false);
        }
        else if (index == 2)
        {
            EventSystem.current.SetSelectedGameObject(healthButton.gameObject);
            heroAttackMaximized.SetActive(false);
            heroHealthMaximized.SetActive(true);
        }
        currentTabIndex = index;
        SetChoicesToLeft();
    }

    private void TogglePause()
    {
        if (GlobalVariables.Instance.playerIsAlive && !pauseOptions.activeInHierarchy)
        {
            GlobalVariables.PauseTime(GlobalVariables.PauseReasonEnum.GameMenu);
            pauseOptions.SetActive(true);
        }
        else
        {
            Unpause();
        }
    }

    private void Unpause()
    {
        if (GlobalVariables.Instance.playerIsAlive && pauseOptions.activeInHierarchy)
        {
            GlobalVariables.UnPauseTime(GlobalVariables.PauseReasonEnum.GameMenu);
            pauseOptions.SetActive(false);
        }
    }

    private void SetGameMenuPanelOff()
    {
        gameOverOptions.SetActive(false);
        pauseOptions.SetActive(false);
        heroBuyMenu.SetActive(false);
    }

    public void Continue()
    {
        Unpause();
    }

    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GlobalVariables.Instance.ResetValues();
        UpgradePrices.Instance.ResetValues();
        GlobalVariables.UnPauseTime(GlobalVariables.PauseReasonEnum.GameMenu);
    }

    public void MainMenu()
    {
        Replay();
    }

    public void Exit()
    {
        Application.Quit();
    }


}
