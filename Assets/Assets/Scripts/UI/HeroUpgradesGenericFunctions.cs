using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class HeroUpgradesGenericFunctions : MonoBehaviour
{
    [Header("Game Menus")]
    [SerializeField] private GameObject gameMenuPanel;
    [SerializeField] private GameObject gameOverOptions;
    [SerializeField] private GameObject pauseOptions;
    [Header("Buy Menus")]
    [SerializeField] private GameObject heroBuyMenu;
    [SerializeField] private GameObject heroAttackMaximized;
    [SerializeField] private GameObject heroHealthMaximized;
    [SerializeField] RectTransform buyMenuChoices;
    [SerializeField] private Color selectedColor;
    [SerializeField] private Color normalColor;
    private int currentTabIndex = 0;
    [Header("Images")]
    [SerializeField] private Image attackImage;
    [SerializeField] private Image defenceImage;
    [SerializeField] private Image utilityImage;
    [SerializeField] private Image spellsImage;
    [Header("Button Scripts")]
    [SerializeField] private ButtonsChangeColorScript attackButtonScript;
    [SerializeField] private ButtonsChangeColorScript defenceButtonScript;
    [SerializeField] private ButtonsChangeColorScript utilityButtonScript;
    [SerializeField] private ButtonsChangeColorScript spellsButtonScript;

    private void Start()
    {
        Time.timeScale = 1.0f;
        SetChoicesToBottom();
        SetGameMenuPanelOff();
    }

    private void Update()
    {
        //HeroBuyMenu
        if (GlobalVariables.Instance.playerIsAlive && !heroBuyMenu.activeSelf)
        {
            heroBuyMenu.SetActive(true);
        }
        else if (!GlobalVariables.Instance.playerIsAlive && heroBuyMenu.activeSelf)
        {
            heroBuyMenu.SetActive(false);
        }

        //PauseUnPause
        if (Time.timeScale == 0f && !GlobalVariables.Instance.gameIsPaused)
        {
            GlobalVariables.Instance.gameIsPaused = true;
        }
        else if (Time.timeScale != 0f && GlobalVariables.Instance.gameIsPaused)
        {
            GlobalVariables.Instance.gameIsPaused = false;
        }

        //AliveOrDead
        if (GlobalVariables.Instance.playerCurrentHealth <= 0 && GlobalVariables.Instance.playerIsAlive)
        {
            GlobalVariables.Instance.playerIsAlive = false;
            Time.timeScale = 0f;
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

    public void SetChoicesToTop(float topOffset = 0f)
    {
        buyMenuChoices.anchorMin = new Vector2(0.5f, 1f);
        buyMenuChoices.anchorMax = new Vector2(0.5f, 1f);
        buyMenuChoices.pivot = new Vector2(0.5f, 1f);
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
        attackImage.color = normalColor;
        defenceImage.color = normalColor;
        utilityImage.color = normalColor;
        spellsImage.color = normalColor;
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


    private void UpdateUI(int index)
    {
        attackButtonScript.ResetColors();
        defenceButtonScript.ResetColors();
        utilityButtonScript.ResetColors();
        spellsButtonScript.ResetColors();

        if (index == 1)
        {

            heroAttackMaximized.SetActive(true);
            heroHealthMaximized.SetActive(false);

            attackImage.color = selectedColor;
            attackButtonScript.SetClicked(true);
            attackButtonScript.ForceHighlight();

            defenceImage.color = normalColor;
            utilityImage.color = normalColor;
            spellsImage.color = normalColor;
        }
        else if (index == 2)
        {

            heroAttackMaximized.SetActive(false);
            heroHealthMaximized.SetActive(true);

            defenceImage.color = selectedColor;
            defenceButtonScript.SetClicked(true);
            defenceButtonScript.ForceHighlight();

            attackImage.color = normalColor;
            utilityImage.color = normalColor;
            spellsImage.color = normalColor;
        }
        currentTabIndex = index;
        SetChoicesToTop();
    }

    private void TogglePause()
    {
        if (!GlobalVariables.Instance.gameIsPaused && Time.timeScale != 0f && GlobalVariables.Instance.playerIsAlive)
        {
            Time.timeScale = 0f;
            pauseOptions.SetActive(true);
        }
        else
        {
            Unpause();
        }
    }

    private void Unpause()
    {
        if (GlobalVariables.Instance.gameIsPaused && GlobalVariables.Instance.playerIsAlive)
        {
            Time.timeScale = 1f;
            pauseOptions.SetActive(false);
        }
    }

    private void SetGameMenuPanelOff()
    {
        gameOverOptions.SetActive(false);
        pauseOptions.SetActive(false);
    }

    //Button Functions

    public void Continue()
    {
        Unpause();
    }

    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GlobalVariables.Instance.ResetValues();
        UpgradePrices.Instance.ResetValues();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GlobalVariables.Instance.ResetValues();
        UpgradePrices.Instance.ResetValues();
    }

    public void Exit()
    {
        Application.Quit();
    }


}
