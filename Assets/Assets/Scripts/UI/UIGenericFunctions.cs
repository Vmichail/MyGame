using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class MenuGenericFunctions : MonoBehaviour
{
    [Header("Game Menus")]
    [SerializeField] private GameObject pauseOrGameOverPanel;
    [SerializeField] private GameObject MusicPanel;
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
            AudioManager.Instance.PlayRandomSoundFX(GlobalVariables.Instance.gameOverClips, transform.position, 1f, 1f, 1.25f);
            AudioManager.Instance.PlaySoundFX("GameOverGeneric", transform.position, 1f, 0.75f, 1.25f);
            GlobalVariables.Instance.playerIsAlive = false;
            GlobalVariables.Instance.PauseTime(GlobalVariables.PauseReasonEnum.PlayerIsDead);
        }
        else if (GlobalVariables.Instance.playerCurrentHealth > 0 && !GlobalVariables.Instance.playerIsAlive)
        {
            GlobalVariables.Instance.playerIsAlive = true;
        }

        //GameOverMenu
        if (!GlobalVariables.Instance.playerIsAlive && !pauseOrGameOverPanel.activeSelf)
        {
            pauseOrGameOverPanel.SetActive(true);
        }

        //PauseUnPauseButton
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        //TABShowsMenu
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            TogglePause();
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
        if (heroBuyMenu.activeInHierarchy == false && !GlobalVariables.Instance.gameIsPaused)
        {
            GlobalVariables.Instance.PauseTime(GlobalVariables.PauseReasonEnum.HeroBuyMenu);
            heroBuyMenu.SetActive(true);
        }
        else
        {
            heroBuyMenu.SetActive(false);
            GlobalVariables.Instance.UnPauseTime(GlobalVariables.PauseReasonEnum.HeroBuyMenu);
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
        if (GlobalVariables.Instance.playerIsAlive && !pauseOrGameOverPanel.activeInHierarchy && !MusicPanel.activeInHierarchy)
        {
            GlobalVariables.Instance.PauseTime(GlobalVariables.PauseReasonEnum.GameMenu);
            pauseOrGameOverPanel.SetActive(true);
        }
        else
        {
            Unpause();
        }
    }

    private void Unpause()
    {
        if (GlobalVariables.Instance.playerIsAlive && (pauseOrGameOverPanel.activeInHierarchy || MusicPanel.activeInHierarchy))
        {
            GlobalVariables.Instance.UnPauseTime(GlobalVariables.PauseReasonEnum.GameMenu);
            SetGameMenuPanelOff();
        }
    }

    private void SetGameMenuPanelOff()
    {
        pauseOrGameOverPanel.SetActive(false);
        heroBuyMenu.SetActive(false);
        MusicPanel.SetActive(false);
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
        GlobalVariables.Instance.UnPauseTime(GlobalVariables.PauseReasonEnum.GameReset);
    }

    public void MainMenu()
    {
        Replay();
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void BackButton(bool gameover)
    {
        pauseOrGameOverPanel.SetActive(true);
        MusicPanel.SetActive(false);
    }
    //===============Music================
    public void MusicButton()
    {
        MusicPanel.SetActive(true);
        pauseOrGameOverPanel.SetActive(false);
    }



}
