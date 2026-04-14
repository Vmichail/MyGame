using DG.Tweening;
using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;


public class UIGenericFunctions : MonoBehaviour
{
    [Header("Game Menus")]
    [SerializeField] private GameObject wholeGameMenuPanels;
    [SerializeField] private GameObject pauseOrGameOverPanel;
    [SerializeField] private GameObject settingsOptionsPanel;
    [SerializeField] private GameObject difficultyPanel;
    [SerializeField] private GameObject statsPanel;

    [Header("Background Animation")]
    [SerializeField] private Image backgroundImage;
    private Vector2 centerPosition;
    private Vector2 leftPosition;
    private bool isCentered = false;


    private void Start()
    {
        // Initialize Game Menu
        backgroundImage.enabled = false;
        centerPosition = wholeGameMenuPanels.transform.position;
        leftPosition = new Vector2(-1250f, centerPosition.y);
        transform.position = leftPosition;
        if (!DifficultyManager.Instance.startingDifficultySet)
        {
            Debug.Log("Showing Difficulty Menu at Start");
            GlobalVariables.Instance.PauseTime(GlobalVariables.PauseReasonEnum.GameMenu);
            ShowDifficultyMenu();
        }
        else
        {
            Debug.Log("Hiding All Menus at Start");
            HideAllMenus();
        }
    }

    private void Update()
    {
        if (PlayerStatsManager.Instance.CurrentHealth < 1 && GlobalVariables.Instance.playerIsAlive)
        {
            FirebaseScoreWriter.WriteUserScore(
                "SkeletonMap",
                DifficultyManager.Instance.CurrentDifficulty.ToString(),
                GlobalVariables.Instance.score,
                GlobalVariables.Instance.selectedCharacter,
                (int)GlobalVariables.Instance.gameTime
            );
            Debug.Log("Player is Dead - Triggering Game Over with sounds");
            GlobalVariables.Instance.playerIsAlive = false;
            AudioManager.Instance.PlayRandomSoundFX(GlobalVariables.Instance.gameOverClips, transform.position, 1f, 1f, 1.25f);
            AudioManager.Instance.PlaySoundFX("gameOverGeneric", transform.position, 1f, 1f, 1.25f);
            GlobalVariables.Instance.PauseTime(GlobalVariables.PauseReasonEnum.PlayerIsDead);
        }
        else if (PlayerStatsManager.Instance.CurrentHealth > 1 && !GlobalVariables.Instance.playerIsAlive)
        {
            GlobalVariables.Instance.playerIsAlive = true;
        }

        //GameOverMenu
        if (!GlobalVariables.Instance.playerIsAlive && !pauseOrGameOverPanel.activeSelf)
        {
            ShowPauseMenu();
        }

        //PauseUnPauseButton 
        if ((Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Tab)) && DifficultyManager.Instance.startingDifficultySet)
        {
            TogglePause();
        }

    }

    private void TogglePause()
    {
        if (GlobalVariables.Instance.playerIsAlive && !pauseOrGameOverPanel.activeSelf && !settingsOptionsPanel.activeSelf && !GlobalVariables.Instance.gameIsPaused)
        {
            GlobalVariables.Instance.PauseTime(GlobalVariables.PauseReasonEnum.GameMenu);
            ShowPauseMenu();
        }
        else
        {
            Unpause();
        }
    }

    private void Unpause()
    {
        if (GlobalVariables.Instance.playerIsAlive && (GlobalVariables.Instance.pauseReasonsList.Contains(GlobalVariables.PauseReasonEnum.GameMenu)))
        {
            HideAllMenus();
        }
    }

    public void Continue()
    {
        Unpause();
    }

    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GlobalVariables.Instance.ResetValues();
        GlobalVariables.Instance.UnPauseTime(GlobalVariables.PauseReasonEnum.GameReset);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Exit()
    {
        Debug.Log("Exiting Game from " + GetType().Name);
        Process.GetCurrentProcess().Kill();
    }

    public enum GameMenuType
    {
        None,
        PauseOrGameOver,
        Music,
        Settings,
        Difficulty,
        Stats
    }

    public void ShowGameMenu(GameMenuType menuType)
    {
        // Turn off all menus first
        pauseOrGameOverPanel.SetActive(false);
        settingsOptionsPanel.SetActive(false);
        difficultyPanel.SetActive(false);
        statsPanel.SetActive(false);
        bool show = menuType != GameMenuType.None;
        // Then enable only the requested one
        switch (menuType)
        {
            case GameMenuType.PauseOrGameOver:
                pauseOrGameOverPanel.SetActive(true);
                break;
            case GameMenuType.Settings:
                settingsOptionsPanel.SetActive(true);
                break;
            case GameMenuType.Difficulty:
                difficultyPanel.SetActive(true);
                break;
            case GameMenuType.Stats:
                statsPanel.SetActive(true);
                break;
            case GameMenuType.None:
                GlobalVariables.Instance.UnPauseTime(GlobalVariables.PauseReasonEnum.GameMenu);
                break;
            default:
                // do nothing, all are off
                break;
        }
        AnimateMenu(show);
    }

    public void ShowPauseMenu() => ShowGameMenu(GameMenuType.PauseOrGameOver);
    public void HideStats()
    {
        if (GlobalVariables.Instance.pauseReasonsList.Contains(GlobalVariables.PauseReasonEnum.LevelUpPanel))
            HideAllMenus();
        else
            ShowGameMenu(GameMenuType.PauseOrGameOver);
    }
    public void ShowMusicMenu() => ShowGameMenu(GameMenuType.Music);
    public void ShowSettingsMenu() => ShowGameMenu(GameMenuType.Settings);
    public void ShowDifficultyMenu() => ShowGameMenu(GameMenuType.Difficulty);
    public void ShowStatsMenu() => ShowGameMenu(GameMenuType.Stats);
    public void HideAllMenus() => ShowGameMenu(GameMenuType.None);


    //==================== MENU ANIMATION ====================//
    private void AnimateMenu(bool show)
    {
        wholeGameMenuPanels.transform.DOKill();

        if (show && !isCentered)
        {
            backgroundImage.enabled = true;

            wholeGameMenuPanels.transform
                .DOMove(centerPosition, 0.25f)
                .SetEase(Ease.OutCubic)
                .SetUpdate(true);

            isCentered = true;
        }
        else if (!show && isCentered)
        {
            wholeGameMenuPanels.transform
                .DOMove(leftPosition, 0.25f)
                .SetEase(Ease.InCubic)
                .SetUpdate(true)
                .OnComplete(() => backgroundImage.enabled = false);

            isCentered = false;
        }
    }
}
