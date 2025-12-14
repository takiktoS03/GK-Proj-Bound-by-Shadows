using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DissolveTrigger : MonoBehaviour
{
    public float duration = 1f;
    public bool invert = false; // w celu odwrócenia efektu (1->0)
    SpriteRenderer sr;
    MaterialPropertyBlock mpb;
    static readonly int DissolveID = Shader.PropertyToID("_Dissolve");
    [HideInInspector] public bool triggered = false;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        mpb = new MaterialPropertyBlock();
    }

    void Start()
    {
        float startValue = invert ? 1f : 0f;

        sr.GetPropertyBlock(mpb);
        mpb.SetFloat(DissolveID, startValue);
        sr.SetPropertyBlock(mpb);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player") || triggered) return;
        triggered = true;
        StartDissolve();
    }

    public void StartDissolve()
    {
        StopAllCoroutines();
        StartCoroutine(DissolveRoutine());
    }

    IEnumerator DissolveRoutine()
    {
        float t = 0f;
        float from = invert ? 1f : 0f;
        float to = invert ? 0f : 1f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float v = Mathf.Lerp(from, to, t / duration);
            sr.GetPropertyBlock(mpb);
            mpb.SetFloat(DissolveID, v);
            sr.SetPropertyBlock(mpb);
            yield return null;
        }
        // ensure final
        sr.GetPropertyBlock(mpb);
        mpb.SetFloat(DissolveID, to);
        sr.SetPropertyBlock(mpb);
    }
}
