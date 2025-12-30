using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
public class DissolveEffect : MonoBehaviour
{
    SpriteRenderer sr;
    MaterialPropertyBlock mpb;
    static readonly int DissolveID = Shader.PropertyToID("_Dissolve");

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        mpb = new MaterialPropertyBlock();
    }

    /// <summary>
    /// Uruchamia efekt dissolve (rozpuszczanie lub pojawianie się obiektu) z shadera.
    /// </summary>
    /// <param name="duration">Czas trwania</param>
    /// <param name="invert">True: 1->0 (znikanie), False: 0->1 (pojawianie)</param>
    /// <param name="onComplete">Opcjonalna akcja do wykonania po zakończeniu (np. Destroy)</param>
    /// <param name="prepTime">Opcjonalny czas trwania zanim efekt się zacznie wykonywać</param>
    public void PlayDissolve(float duration, bool invert, System.Action onComplete = null, float prepTime = 0f)
    {
        StopAllCoroutines();
        StartCoroutine(DissolveRoutine(duration, invert, onComplete, prepTime));
    }

    private IEnumerator DissolveRoutine(float duration, bool invert, System.Action onComplete, float prepTime)
    {
        float t = 0f;
        float from = invert ? 1f : 0f;
        float to = invert ? 0f : 1f;

        yield return new WaitForSeconds(prepTime);

        while (t < duration)
        {
            t += Time.deltaTime;
            float v = Mathf.Lerp(from, to, t / duration);
            UpdateMaterial(v);
            yield return null;
        }

        // Ustawienie wartości końcowej dla pewności
        UpdateMaterial(to);

        // Wywołanie akcji po zakończeniu (jeśli istnieje)
        onComplete?.Invoke();
    }

    public void UpdateMaterial(float value)
    {
        sr.GetPropertyBlock(mpb);
        mpb.SetFloat(DissolveID, value);
        sr.SetPropertyBlock(mpb);
    }
}