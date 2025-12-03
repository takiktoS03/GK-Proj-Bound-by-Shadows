using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FresnelTrigger : MonoBehaviour
{
    public float fadeDuration = 1f;
    public float holdTime = 0.2f;
    SpriteRenderer sr;
    MaterialPropertyBlock mpb;
    static readonly int FresnelID = Shader.PropertyToID("_Fresnel");

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        mpb = new MaterialPropertyBlock();
    }
    void Start()
    {
        sr.GetPropertyBlock(mpb);
        mpb.SetFloat(FresnelID, 0);
        sr.SetPropertyBlock(mpb);
    }

    public void PulseFresnel()
    {
        StopAllCoroutines();
        StartCoroutine(PulseRoutine());
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
