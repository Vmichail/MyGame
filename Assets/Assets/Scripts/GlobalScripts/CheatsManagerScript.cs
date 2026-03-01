using UnityEngine;

public class CheatsManagerScript : MonoBehaviour
{
    public static CheatsManagerScript Instance { get; private set; }

    [Header("Cheats State")]
    public bool cheatsEnabled = false;

    [Header("Settings")]
    [SerializeField] private float cheatsDuration = 60f;

    private float cheatsTimer = 0f;

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        // Enable / refresh cheats: Left Ctrl + Left Shift + C
        if (Input.GetKey(KeyCode.Z) && Input.GetKeyDown(KeyCode.E))
        {
            EnableCheats();
        }

        if (cheatsEnabled)
        {
            cheatsTimer -= Time.unscaledDeltaTime;

            if (cheatsTimer <= 0f)
            {
                DisableCheats();
            }
        }

        if (cheatsEnabled && Input.GetKeyDown(KeyCode.H))
        {
            PlayerStatsManager.Instance.CurrentExp += 100;
        }

        if (cheatsEnabled && Input.GetKeyDown(KeyCode.G))
        {
            CurrencyManager.instance.Add(1000);
        }
    }

    private void EnableCheats()
    {
        cheatsEnabled = true;
        cheatsTimer = cheatsDuration;
        Debug.Log($"Cheats ENABLED for {cheatsDuration} seconds");
    }

    private void DisableCheats()
    {
        cheatsEnabled = false;
        cheatsTimer = 0f;
        Debug.Log("Cheats DISABLED");

        // TODO: reset cheat effects here
        // e.g. godMode = false; reset stats; restore damage; etc.
    }
}