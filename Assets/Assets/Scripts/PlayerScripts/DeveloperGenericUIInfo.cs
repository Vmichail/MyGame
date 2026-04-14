using DG.Tweening;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;

public class DeveloperGenericUIInfo : MonoBehaviour
{
    [Header("Text Fields")]
    [SerializeField] private TextMeshProUGUI fpsText;
    [SerializeField] private TextMeshProUGUI frameTimeText;
    [SerializeField] private TextMeshProUGUI cpuFrameText;
    [SerializeField] private TextMeshProUGUI targetFpsText;
    [SerializeField] private TextMeshProUGUI memoryText;
    [SerializeField] private TextMeshProUGUI tweensText;
    [SerializeField] private TextMeshProUGUI resolutionText;
    [SerializeField] private TextMeshProUGUI gcAllocText;

    [Header("Settings")]
    [SerializeField] private float updateInterval = 0.5f;

    private float deltaTime;
    private float timer;
    private long lastGC;

    void Start()
    {
        lastGC = System.GC.GetTotalMemory(false);
    }

    void Update()
    {
        if (GlobalVariables.Instance.developerMode)
        {
            deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
            timer += Time.unscaledDeltaTime;

            if (timer >= updateInterval)
            {
                UpdateStats();
                timer = 0f;
            }
        }
    }

    void UpdateStats()
    {
        int fps = Mathf.CeilToInt(1f / deltaTime);
        float frameMs = deltaTime * 1000f;

        // CPU frame time
        float cpuFrameMs = Time.deltaTime * 1000f;

        // Memory
        float memoryMB = Profiler.GetTotalAllocatedMemoryLong() / (1024f * 1024f);

        // GC allocations
        long currentGC = System.GC.GetTotalMemory(false);
        long gcAlloc = currentGC - lastGC;
        lastGC = currentGC;

        // DOTween stats
        int activeTweens = DOTween.TotalActiveTweens();
        int playingTweens = DOTween.TotalPlayingTweens();

        // Target FPS
        int targetFPS = Application.targetFrameRate;
        int vSync = QualitySettings.vSyncCount;

        fpsText.text = $"FPS: {fps}";
        frameTimeText.text = $"Frame: {frameMs:0.0} ms";
        targetFpsText.text = $"Target FPS: {targetFPS} | VSync: {vSync}";
        cpuFrameText.text = $"CPU Frame: {cpuFrameMs:0.0} ms";
        memoryText.text = $"Memory: {memoryMB:0.0} MB";
        tweensText.text = $"Tweens: {activeTweens} (Playing {playingTweens})";
        resolutionText.text = $"Resolution: {Screen.width}x{Screen.height}";
        gcAllocText.text = $"GC Alloc: {gcAlloc / 1024f:0.0} KB";
    }
}
