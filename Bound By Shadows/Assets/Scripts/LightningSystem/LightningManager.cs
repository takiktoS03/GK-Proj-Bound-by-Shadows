using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightingManager : MonoBehaviour
{
    [SerializeField] Light2D globalLight;
    [SerializeField] Light2D ghostLight;

    public void SetGlobalLight(float intensity, Color color)
    {
        globalLight.intensity = intensity;
        globalLight.color = color;
    }

    public void EnableGhostLight(bool enable)
    {
        ghostLight.enabled = enable;
    }

    public void FadeLight(Light2D light, float targetIntensity, float duration)
    {
        StartCoroutine(FadeRoutine(light, targetIntensity, duration));
    }

    private IEnumerator FadeRoutine(Light2D light, float target, float duration)
    {
        float start = light.intensity;
        float time = 0;
        while (time < duration)
        {
            light.intensity = Mathf.Lerp(start, target, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        light.intensity = target;
    }
}
