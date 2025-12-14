using System.Collections;
using UnityEngine;

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
        sr.GetPropertyBlock(mpb);
        mpb.SetFloat(FresnelID, 0);
        sr.SetPropertyBlock(mpb);
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
        // set to 1
        sr.GetPropertyBlock(mpb);
        mpb.SetFloat(FresnelID, 1f);
        sr.SetPropertyBlock(mpb);

        yield return new WaitForSeconds(holdTime);

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float v = Mathf.Lerp(1f, 0f, t / fadeDuration);
            sr.GetPropertyBlock(mpb);
            mpb.SetFloat(FresnelID, v);
            sr.SetPropertyBlock(mpb);
            yield return null;
        }

        mpb.SetFloat(FresnelID, 0f);
        sr.SetPropertyBlock(mpb);
    }
}
