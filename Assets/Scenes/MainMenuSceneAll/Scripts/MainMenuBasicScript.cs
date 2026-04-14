using DG.Tweening;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;


public class MainMenuBasicScript : MonoBehaviour
{
    public enum MainMenuOptions
    {
        MainMenu,
        Music,
        Upgrades,
        Login,
        CharacterSelection,
        Leaderboard
    }

    [SerializeField] private GameObject LoginPage;
    [SerializeField] private GameObject WholeMainMenuScreen;
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject MusicPanel;
    [SerializeField] private GameObject UpgradesPanel;
    [SerializeField] private GameObject LeaderboardGO;
    [SerializeField] private GameObject logoutButton;
    [SerializeField] private GameObject linkAccountButton;
    [SerializeField] private GameObject CharacterSelectionScreen;
    [SerializeField] private GameObject HideButtonGO;
    [SerializeField] private GameObject CoinsGO;

    [SerializeField] private Image hideButton;
    [SerializeField] private Sprite hideImage;
    [SerializeField] private Sprite showImage;
    [SerializeField] private TextMeshProUGUI welcomeTMP;
    [SerializeField]
    private List<string> mainMenuMusic = new()
    {
    "MainMenuHallOfTheKing",
    "MainMenuInTheCastle",
    "MainMenuMagicalForest"
    };


    void Start()
    {
        GlobalVariables.Instance.mainMenuScene = true;
        GlobalVariables.Instance.pauseReasonsList.Clear();
        GlobalVariables.Instance.UnPauseTime(GlobalVariables.PauseReasonEnum.GameMenu);
        PlayerStatsManager.Instance.Initialize();
        AudioManager.Instance.PlayMusic(mainMenuMusic[Random.Range(0, mainMenuMusic.Count)]);
    }

    private void Update()
    {
        if (GlobalVariables.Instance.pauseReasonsList.Count > 0)
        {
            GlobalVariables.Instance.pauseReasonsList.Clear();
            GlobalVariables.Instance.UnPauseTime(GlobalVariables.PauseReasonEnum.GameMenu);
        }
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1f;
        }
    }


    private void HideOrShowSpecificMenu(MainMenuOptions option)
    {
        MainMenu.SetActive(false);
        MusicPanel.SetActive(false);
        UpgradesPanel.SetActive(false);
        LoginPage.SetActive(false);
        CharacterSelectionScreen.SetActive(false);
        LeaderboardGO.SetActive(false);
        HideButtonGO.SetActive(true);
        CoinsGO.SetActive(true);
        GlobalVariables.Instance.canZoomMap = true;
        if (option.Equals(MainMenuOptions.MainMenu))
        {
            MainMenu.SetActive(true);
            if (FirebaseInitializer.auth.CurrentUser != null)
            {
                if (FirebaseInitializer.auth.CurrentUser.IsAnonymous)
                {
                    linkAccountButton.SetActive(true);
                }
                else
                {
                    linkAccountButton.SetActive(false);
                }
                logoutButton.SetActive(true);
            }
            else
            {
                linkAccountButton.SetActive(false);
                logoutButton.SetActive(false);
            }
        }
        else if (option.Equals(MainMenuOptions.Music))
        {
            MusicPanel.SetActive(true);
        }
        else if (option.Equals(MainMenuOptions.Upgrades))
        {
            UpgradesPanel.SetActive(true);
            GlobalVariables.Instance.canZoomMap = false;
        }
        else if (option.Equals(MainMenuOptions.Login))
        {
            LoginPage.SetActive(true);
        }
        else if (option.Equals(MainMenuOptions.Leaderboard))
        {
            LeaderboardGO.SetActive(true);
            GlobalVariables.Instance.canZoomMap = false;
        }
        else if (option.Equals(MainMenuOptions.CharacterSelection))
        {
            CharacterSelectionScreen.SetActive(true);
            HideButtonGO.SetActive(false);
            CoinsGO.SetActive(false);
        }
    }

    public void ShowLogin()
    {
        welcomeTMP.text = "Please Sign In!";
        HideOrShowSpecificMenu(MainMenuOptions.Login);
    }

    public void ShowMainMenu()
    {
        welcomeTMP.text = $"Welcome home {FirebaseInitializer.PlayerUsername}!";
        HideOrShowSpecificMenu(MainMenuOptions.MainMenu);
    }

    public void ShowLeaderboards()
    {
        HideOrShowSpecificMenu(MainMenuOptions.Leaderboard);
    }

    public void ShowMusicPanel()
    {
        HideOrShowSpecificMenu(MainMenuOptions.Music);
    }

    public void ShowUpgrades()
    {
        HideOrShowSpecificMenu(MainMenuOptions.Upgrades);
    }

    public void HideAll()
    {
        WholeMainMenuScreen.SetActive(!WholeMainMenuScreen.activeSelf);
        hideButton.sprite = WholeMainMenuScreen.activeSelf ? showImage : hideImage;
    }


    public void CharacterSelection()
    {
        HideOrShowSpecificMenu(MainMenuOptions.CharacterSelection);
    }

    public void StartGame()
    {
        GlobalVariables.Instance.mainMenuScene = false;
        SceneManager.LoadScene(1);
    }

    public void StartGameWithChar(int hero)
    {
        if (hero == -1)
        {
            AudioManager.Instance.PlaySoundFX("uiDeny", transform.position, 1f, 1f, 1f);
            return;
        }
        GlobalVariables.Instance.mainMenuScene = false;
        if (hero == 0)
        {
            GlobalVariables.Instance.SetSelectedCharacter(CharacterSprite.LinaSprite.ToString());
        }
        else if (hero == 1)
        {
            GlobalVariables.Instance.SetSelectedCharacter(CharacterSprite.MiranaSprite.ToString());
        }
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Debug.Log("Exiting Game from " + GetType().Name);
        Process.GetCurrentProcess().Kill();
    }
}
