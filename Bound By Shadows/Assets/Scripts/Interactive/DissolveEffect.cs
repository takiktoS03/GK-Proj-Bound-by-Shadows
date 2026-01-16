using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/**
 * Skrypt realizujący efekt dissolve (rozpuszczania / pojawiania się obiektu)
 * przy użyciu parametru shadera i płynnej animacji w czasie.
 *
 * @author Filip Kudła
 */
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

        // Oczekiwanie przez opcjonalny czas opóźnienia przed rozpoczęciem efektu
        yield return new WaitForSeconds(prepTime);

        while (t < duration)
        {
            t += Time.deltaTime;
            // Interpolacja liniowa na podstawie upływu czasu
            float v = Mathf.Lerp(from, to, t / duration);
            UpdateMaterial(v);
            yield return null;
        }

        // Ustawienie wartości końcowej, aby wyeliminować błędy zaokrągleń czasu
        UpdateMaterial(to);

        // Wywołanie opcjonalnej akcji zwrotnej (np. usunięcia obiektu) po zakończeniu animacji
        onComplete?.Invoke();
    }

    public void UpdateMaterial(float value)
    {
        // Pobranie aktualnego bloku właściwości z renderera (optymalizacja pamięci)
        sr.GetPropertyBlock(mpb);
        // Ustawienie wartości float dla parametru shadera o zhashowanym ID
        mpb.SetFloat(DissolveID, value);
        // Przypisanie zaktualizowanego bloku właściwości z powrotem do komponentu SpriteRenderer
        sr.SetPropertyBlock(mpb);
    }
}