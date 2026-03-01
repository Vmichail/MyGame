using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using TMPro;
using UnityEngine;

public class AuthManager : MonoBehaviour
{
    //Login fields
    [SerializeField] private GameObject loginFields;
    [SerializeField] private TMP_InputField email;
    [SerializeField] private TMP_InputField password;
    //Register fields
    [SerializeField] private GameObject registerFields;
    [SerializeField] private TMP_InputField rUsername;
    [SerializeField] private TMP_InputField rEmail;
    [SerializeField] private TMP_InputField rPassword;
    [SerializeField] private TMP_InputField repeatPassword;
    [Header("Info Text")]
    [SerializeField] private GameObject infoGO;
    [SerializeField] private TextMeshProUGUI infoText;
    private GenericShakeEffect shakeEffectScript;
    [Header("Main Menu")]
    [SerializeField] private MainMenuBasicScript mainMenu;
    [Header("Welcome Text")]
    [SerializeField] private TextMeshProUGUI welcomeTMP;
    [Header("Link Account Options")]
    [SerializeField] private GameObject linkAccountOptionsGO;
    [SerializeField] private GameObject LoginRegisterOptionsGO;
    public static AuthManager Instance;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnEnable()
    {
        FirebaseInitializer.OnFirebaseReady += HandleFirebaseReady;
        infoGO.TryGetComponent(out shakeEffectScript);
        InitialState();
    }

    private void Start()
    {

    }

    private void OnDisable()
    {
        FirebaseInitializer.OnFirebaseReady -= HandleFirebaseReady;
        rUsername.text = "";
        rEmail.text = "";
        rPassword.text = "";
        repeatPassword.text = "";
        email.text = "";
        password.text = "";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (loginFields.activeSelf)
            {
                Login();
            }
            else if (registerFields.activeSelf)
            {
                Register();
            }
        }
    }

    public void Register(bool linkAccount = false)
    {
        if (!FirebaseInitializer.IsReady)
        {
            UpdateInfoText(Color.yellow, "Firebase error. Do you have internet?? :D");
            return;
        }

        if (FirebaseInitializer.auth.CurrentUser != null && FirebaseInitializer.auth.CurrentUser.IsAnonymous)
        {
            linkAccount = true;
        }

        if (!registerFields.activeSelf)
        {
            registerFields.SetActive(true);
            loginFields.SetActive(false);
            return;
        }
        if (string.IsNullOrWhiteSpace(rUsername.text)
        || string.IsNullOrWhiteSpace(rEmail.text)
        || string.IsNullOrWhiteSpace(rPassword.text)
        || string.IsNullOrWhiteSpace(repeatPassword.text))
        {
            UpdateInfoText(Color.yellow, "Please fill all register fields");
            return;
        }

        if (rPassword.text != repeatPassword.text)
        {
            UpdateInfoText(Color.yellow, "Passwords do not match");
            return;
        }

        if (!IsValidEmail(rEmail.text))
        {
            UpdateInfoText(Color.yellow, "Invalid email address");
            return;
        }

        CheckUsernameExists(rUsername.text, exists =>
        {
            if (exists)
            {
                UpdateInfoText(Color.yellow, "Username already exists");
                return;
            }

            if (linkAccount)
            {
                Credential credential = EmailAuthProvider.GetCredential(rEmail.text, rPassword.text);
                FirebaseUser currentUser = FirebaseInitializer.auth.CurrentUser;
                currentUser.LinkWithCredentialAsync(credential)
                    .ContinueWithOnMainThread(task =>
                    {
                        if (task.IsFaulted)
                        {
                            HandleInputError(task.Exception);
                            Debug.LogError("❌ Failed to link guest account: " + task.Exception);
                            return;
                        }
                        // 🔒 Username is now PERMANENT
                        WriteUserToDatabase(currentUser.UserId, rUsername.text);
                        FirebaseInitializer.PlayerUsername = rUsername.text;
                        Debug.Log("🔗 Guest account linked to email/password");
                        AudioManager.Instance.PlaySoundFX("levelup2", transform.position, 1f, 1f, 1f);
                        welcomeTMP.text = "Welcome " + rUsername.text + "!";
                        Debug.Log("Is registered user anonymous?: " + FirebaseInitializer.auth.CurrentUser.IsAnonymous);
                        mainMenu.ShowMainMenu();
                    });
            }
            else
            {
                FirebaseInitializer.auth
                    .CreateUserWithEmailAndPasswordAsync(rEmail.text, rPassword.text)
                    .ContinueWithOnMainThread(task =>
                    {
                        if (task.IsFaulted)
                        {
                            HandleInputError(task.Exception);
                            Debug.LogError("❌ Registration failed: " + task.Exception);
                            return;
                        }
                        //If Annonymous user somehow exists, delete it

                        var user = FirebaseInitializer.auth.CurrentUser;
                        // Write username to database
                        WriteUserToDatabase(user.UserId, rUsername.text);
                        Debug.Log("🎉 Registered: " + user.Email);
                        AudioManager.Instance.PlaySoundFX("levelup2", transform.position, 1f, 1f, 1f);
                        AchievementUnlockManager.Instance.SaveUnlockKey(UnlockKey.RegisterAccount);
                        welcomeTMP.text = "Welcome Home " + rUsername.text + "!";
                        FirebaseInitializer.PlayerUsername = rUsername.text;
                        mainMenu.ShowMainMenu();
                    });
            }
        });
    }

    public void LoginAsGuest()
    {
        if (!FirebaseInitializer.IsReady)
        {
            UpdateInfoText(Color.yellow, "Firebase not ready maybe check your internet?");
            return;
        }

        FirebaseInitializer.auth
            .SignInAnonymouslyAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    UpdateInfoText(Color.red, "Guest login failed");
                    Debug.LogError(task.Exception);
                    return;
                }

                FirebaseInitializer.PlayerUsername = "Guest";

                Debug.Log("👤 Logged in as Guest | UID: " + FirebaseInitializer.auth.CurrentUser.UserId);

                welcomeTMP.text = "Welcome Home " + FirebaseInitializer.PlayerUsername + "!";
                mainMenu.ShowMainMenu();
            });
    }

    public void Login()
    {
        if (!FirebaseInitializer.IsReady)
        {
            UpdateInfoText(Color.yellow, "Firebase error. Do you have internet?? :D");
            return;
        }

        if (!loginFields.activeSelf)
        {
            loginFields.SetActive(true);
            registerFields.SetActive(false);
            return;
        }

        if (string.IsNullOrWhiteSpace(email.text)
            || string.IsNullOrWhiteSpace(password.text))
        {
            UpdateInfoText(Color.yellow, "Please fill all register fields");
            return;
        }

        if (!IsValidEmail(email.text))
        {
            UpdateInfoText(Color.yellow, "Invalid email address");
            return;
        }

        // 🔴 DELETE anonymous BEFORE login
        if (FirebaseInitializer.auth.CurrentUser != null && FirebaseInitializer.auth.CurrentUser.IsAnonymous)
        {
            Debug.Log("🗑️ Deleting anonymous user before login");

            FirebaseInitializer.auth.CurrentUser.DeleteAsync()
                .ContinueWithOnMainThread(_ =>
                {
                    FirebaseInitializer.auth.SignOut();
                    FirebaseInitializer.PlayerUsername = null;

                    DoEmailLogin();
                });
        }
        else
        {
            DoEmailLogin();
        }
    }

    private void UpdateInfoText(Color color, string text)
    {
        AudioManager.Instance.PlaySoundFX("uiDeny", transform.position, 0.8f, 0.9f, 1.1f);
        infoText.color = color;
        infoText.text = text;
        shakeEffectScript.Shake();
    }

    public void Logout()
    {
        if (!FirebaseInitializer.IsReady)
            return;

        if (FirebaseInitializer.auth.CurrentUser == null)
        {
            Debug.Log("⚠️ No user logged in");
            return;
        }
        if (FirebaseInitializer.auth.CurrentUser.IsAnonymous)
        {
            mainMenu.ShowLogin();
            return;
        }

        FirebaseInitializer.auth.SignOut();
        mainMenu.ShowLogin();
        InitialState();
        Debug.Log("🚪 Logged out");
    }

    public static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    private void WriteUserToDatabase(string uid, string username)
    {
        string key = username.ToLowerInvariant();

        var updates = new Dictionary<string, object>();
        updates["users/" + uid + "/username"] = username;
        updates["usernames/" + key] = uid;

        FirebaseInitializer.database
            .RootReference
            .UpdateChildrenAsync(updates);
    }

    private void HandleFirebaseReady()
    {
        Debug.Log("Firebase is ready in AuthManager");

        if (FirebaseInitializer.auth.CurrentUser != null)
        {
            welcomeTMP.text = "Welcome Back " + FirebaseInitializer.PlayerUsername + "!";
            mainMenu.ShowMainMenu();
        }
        else
        {
            mainMenu.ShowLogin();
            loginFields.SetActive(true);
            registerFields.SetActive(false);
        }
    }

    private void CheckUsernameExists(string username, System.Action<bool> callback)
    {
        string key = username.ToLowerInvariant();

        FirebaseInitializer.database
            .GetReference("usernames")
            .Child(key)
            .GetValueAsync()
            .ContinueWithOnMainThread(task =>
            {
                callback(task.Result.Exists);
            });
    }

    private void HandleInputError(AggregateException exception)
    {
        foreach (var ex in exception.InnerExceptions)
        {
            if (ex is FirebaseException firebaseEx)
            {
                switch ((AuthError)firebaseEx.ErrorCode)
                {
                    // 🔐 Registration
                    case AuthError.EmailAlreadyInUse:
                        UpdateInfoText(Color.yellow, "Email already exists");
                        return;

                    case AuthError.WeakPassword:
                        UpdateInfoText(Color.yellow, "Password is too weak");
                        return;

                    case AuthError.MissingEmail:
                        UpdateInfoText(Color.yellow, "Email is required");
                        return;

                    case AuthError.InvalidEmail:
                        UpdateInfoText(Color.yellow, "Invalid email address");
                        return;

                    // 🔑 Login
                    case AuthError.WrongPassword:
                        UpdateInfoText(Color.red, "Incorrect password");
                        return;

                    case AuthError.UserNotFound:
                        UpdateInfoText(Color.red, "Account not found");
                        return;

                    case AuthError.InvalidCredential:
                        UpdateInfoText(Color.red, "Incorrect email or password");
                        return;

                    case AuthError.UserDisabled:
                        UpdateInfoText(Color.red, "This account has been disabled");
                        return;

                    // 🔄 Linking / session
                    case AuthError.RequiresRecentLogin:
                        UpdateInfoText(Color.red, "Session expired. Please log in again.");
                        return;

                    // 🌐 Network / abuse protection
                    case AuthError.NetworkRequestFailed:
                        UpdateInfoText(Color.red, "No internet connection");
                        return;
                }
            }

            // 🔥 Fallback (unknown error)
            UpdateInfoText(Color.red, "Authentication failed. Please try again.");
        }
    }

    public void SkipButtonInfo()
    {

        infoText.color = Color.white;
        infoText.text = "You can skip login and link account from Main Menu whenever you want.";
        shakeEffectScript.Shake();
    }

    public void LinkAccountMainMenuButton()
    {
        mainMenu.ShowLogin();
        loginFields.SetActive(false);
        registerFields.SetActive(true);
        linkAccountOptionsGO.SetActive(true);
        LoginRegisterOptionsGO.SetActive(false);
    }

    public void LinkAccount()
    {
        if (!FirebaseInitializer.IsReady)
        {
            UpdateInfoText(Color.yellow, "Firebase not ready");
            return;
        }

        var currentUser = FirebaseInitializer.auth.CurrentUser;

        if (currentUser == null)
        {
            UpdateInfoText(Color.red, "No user logged in");
            return;
        }

        if (!currentUser.IsAnonymous)
        {
            UpdateInfoText(Color.yellow, "Account already linked");
            return;
        }

        Register(true);
    }


    private void InitialState()
    {
        loginFields.SetActive(true);
        registerFields.SetActive(false);
        linkAccountOptionsGO.SetActive(false);
        LoginRegisterOptionsGO.SetActive(true);
    }


    public void FetchUsername()
    {
        FirebaseInitializer.database
            .GetReference("users")
            .Child(FirebaseInitializer.auth.CurrentUser.UserId)
            .Child("username")
            .GetValueAsync()
            .ContinueWithOnMainThread(task =>
            {
                FirebaseInitializer.PlayerUsername = task.Result.Exists
                    ? task.Result.Value.ToString()
                    : "Player";
                welcomeTMP.text = "Welcome Back " + FirebaseInitializer.PlayerUsername + "!";
            });
    }

    private void DoEmailLogin()
    {
        FirebaseInitializer.auth
            .SignInWithEmailAndPasswordAsync(email.text, password.text)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    HandleInputError(task.Exception);
                    Debug.Log("❌ Login failed: " + task.Exception);
                    return;
                }

                AudioManager.Instance.PlaySoundFX("levelup2", transform.position, 1f, 1f, 1f);

                Debug.Log("🔐 Logged in: " + FirebaseInitializer.auth.CurrentUser.Email);

                FetchUsername();
                mainMenu.ShowMainMenu();
            });
    }
}

