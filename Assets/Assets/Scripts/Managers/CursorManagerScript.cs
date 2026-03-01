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
}