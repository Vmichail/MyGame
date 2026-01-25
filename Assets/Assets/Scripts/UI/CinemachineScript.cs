using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CinemachineScript : MonoBehaviour
{
    public static CinemachineScript Instance { get; private set; }

    private CinemachineCamera vcam;
    private CinemachineBasicMultiChannelPerlin noise;

    private void Awake()
    {
        Instance = this;
        vcam = GetComponent<CinemachineCamera>();
        noise = vcam.GetComponent<CinemachineBasicMultiChannelPerlin>();
    }
    private Coroutine shakeRoutine;

    public void Shake(float intensity, float time)
    {
        if (noise == null)
            return;

        if (shakeRoutine != null)
            StopCoroutine(shakeRoutine);

        shakeRoutine = StartCoroutine(ShakeRoutine(intensity, time));
    }

    private IEnumerator ShakeRoutine(float intensity, float time)
    {
        noise.AmplitudeGain = intensity;

        float elapsed = 0f;
        while (elapsed < time)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / time;

            // Smoothly interpolate down to 0
            noise.AmplitudeGain = Mathf.Lerp(intensity, 0f, t);

            yield return null;
        }

        noise.AmplitudeGain = 0f;
        shakeRoutine = null;
    }

    public void ShakeUnscaled(float intensity, float time)
    {
        if (noise == null)
            return;

        if (shakeRoutine != null)
            StopCoroutine(shakeRoutine);

        shakeRoutine = StartCoroutine(ShakeRoutineUnscaled(intensity, time));
    }

    private IEnumerator ShakeRoutineUnscaled(float intensity, float time)
    {
        noise.AmplitudeGain = intensity;

        float elapsed = 0f;
        while (elapsed < time)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / time;

            noise.AmplitudeGain = Mathf.Lerp(intensity, 0f, t);

            yield return null;
        }

        noise.AmplitudeGain = 0f;
        shakeRoutine = null;
    }
}