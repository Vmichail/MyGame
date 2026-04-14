using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BaseLightScript : MonoBehaviour
{
    Light2D gameLight;
    [SerializeField] private bool isMainLight;
    private void OnEnable()
    {
        gameLight = GetComponent<Light2D>();
        GlobalVariables.Instance.OnShowAllLightsChanged += OnLightsVisibilityChanged;
        OnLightsVisibilityChanged(GlobalVariables.Instance.showAllLights);
        gameLight.shadowsEnabled = false;
    }

    private void OnDisable()
    {
        GlobalVariables.Instance.OnShowAllLightsChanged -= OnLightsVisibilityChanged;
    }

    private void OnLightsVisibilityChanged(bool show)
    {
        if (isMainLight)
        {
            gameLight.enabled = true;
            return;
        }
        gameLight.enabled = show;
    }
}
