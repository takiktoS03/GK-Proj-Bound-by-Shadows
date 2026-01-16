using System.Collections;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

/**
 * Skrypt sterujący krótkim efektem wizualnym Fresnela na obiekcie,
 * wykorzystywanym jako podświetlenie lub sygnał interakcji.
 *
 * @author Filip Kudła
 */

[RequireComponent(typeof(SpriteRenderer))]
public class FresnelTrigger : MonoBehaviour
{
    public float fadeDuration = 1f;
    public float holdTime = 0.2f;

    static readonly int FresnelID = Shader.PropertyToID("_Fresnel");
    SpriteRenderer sr;
    MaterialPropertyBlock mpb;
    DissolveTrigger dissolveTrigger;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        mpb = new MaterialPropertyBlock();
        dissolveTrigger = GetComponent<DissolveTrigger>();
    }
    void Start()
    {
        UpdateMaterial(0f);
    }

    public void PulseFresnel()
    {
        if (!dissolveTrigger.triggered)
        {
            StopAllCoroutines();
            StartCoroutine(PulseRoutine());
        }
    }

    IEnumerator PulseRoutine()
    {
        UpdateMaterial(1f);

        yield return new WaitForSeconds(holdTime);

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float v = Mathf.Lerp(1f, 0f, t / fadeDuration);
            UpdateMaterial(v);
            yield return null;
        }

        UpdateMaterial(0f);
    }

    private void UpdateMaterial(float value)
    {
        sr.GetPropertyBlock(mpb);
        mpb.SetFloat(FresnelID, value);
        sr.SetPropertyBlock(mpb);
    }
}
