using UnityEngine;

public class CursorManagerScript : MonoBehaviour
{
    [Header("Cursor Textures")]
    public Texture2D defaultCursor;
    public Texture2D pointerCursor;
    public Texture2D buyCursor;

    [Header("Hotspots")]
    public Vector2 defaultHotspot = Vector2.zero;
    public Vector2 pointerHotspot = Vector2.zero;
    public Vector2 buyHotspot = Vector2.zero;

    [SerializeField] private float hideDelay = 3f;

    private float timer;
    private Vector3 lastMousePosition;
    private bool isHidden;

    private static CursorManagerScript instance;

    void Awake()
    {
        // Singleton (simple & safe)
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        SetDefault(); // set default cursor on startup
    }

    private void Update()
    {
        if (Input.mousePosition != lastMousePosition)
        {
            // Mouse moved — show it and reset timer
            lastMousePosition = Input.mousePosition;
            timer = 0f;

            if (isHidden)
            {
                Cursor.visible = true;
                isHidden = false;
            }
        }
        else
        {
            // Mouse idle — count down
            timer += Time.deltaTime;
            if (timer >= hideDelay && !isHidden && !GlobalVariables.Instance.mainMenuScene)
            {
                Cursor.visible = false;
                isHidden = true;
            }
        }
    }

    // ===== Public API =====

    public static void SetDefault()
    {
        if (instance == null) return;
        Cursor.SetCursor(
            instance.defaultCursor,
            instance.defaultHotspot,
            CursorMode.Auto
        );
    }

    public static void SetPointer()
    {
        if (instance == null) return;
        Cursor.SetCursor(
            instance.pointerCursor,
            instance.pointerHotspot,
            CursorMode.Auto
        );
    }

    public static void SetBuyPointer()
    {
        if (instance == null) return;
        Cursor.SetCursor(
            instance.buyCursor,
            instance.buyHotspot,
            CursorMode.Auto
        );
    }

    private void OnDisable()
    {
        // Safety — always restore cursor when script is disabled
        Cursor.visible = true;
    }
}