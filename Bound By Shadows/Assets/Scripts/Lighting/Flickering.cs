using UnityEngine;
using UnityEngine.Rendering.Universal;

/**
 * Skrypt odpowiadający za migotanie światła poprzez losowe zmiany jego intensywności w czasie.
 * Wykorzystywany do nadania klimatu pochodniom i innym źródłom światła.
 *
 * @author Filip Kudła
 */

public class Flickering : MonoBehaviour
{
    public Light2D light2D;

    public float intensityVariation = 0.3f;
    public float speed = 20f;

    private float baseIntensity;

    private void Awake()
    {
        if (light2D == null)
            light2D = GetComponent<Light2D>();

        baseIntensity = light2D.intensity;
    }

    private void Update()
    {
        float noise = Mathf.PerlinNoise1D(Time.time * speed);
        light2D.intensity = baseIntensity + (noise - 0.5f) * intensityVariation;
    }
}
