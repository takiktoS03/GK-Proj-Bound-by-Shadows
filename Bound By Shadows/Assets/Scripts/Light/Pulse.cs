using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Pulse : MonoBehaviour
{
    public Light2D light2D;

    [Header("Intensity Settings")]
    public float minIntensity = 0.8f;
    public float maxIntensity = 1.2f;
    public float speed = 2f;

    [Header("Radius Settings")]
    public float minRadius = 3f;
    public float maxRadius = 4.5f;

    private float t;

    private void Awake()
    {
        if (light2D == null)
            light2D = GetComponent<Light2D>();
    }

    private void Update()
    {
        t += Time.deltaTime * speed;
        float value = (Mathf.Sin(t) + 1f) / 2f;

        light2D.intensity = Mathf.Lerp(minIntensity, maxIntensity, value);
        light2D.pointLightOuterRadius = Mathf.Lerp(minRadius, maxRadius, value);
    }
}
