using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightColorChange : MonoBehaviour
{
    public Light2D light2D;

    public Color normalColor = Color.white;
    public Color dangerColor = Color.red;
    public float dangerDuration = 3f;

    private Coroutine changeRoutine;
    private Coroutine dangerRoutine;

    private void Awake()
    {
        if (light2D == null)
            light2D = GetComponent<Light2D>();
    }

    /// <summary>
    /// Wywoływane przy otrzymaniu obrażeń.
    /// Ustawia czerwony kolor + włącza flickering + resetuje timer.
    /// </summary>
    public void SetDanger()
    {
        // natychmiast przerwij zmianę koloru
        if (changeRoutine != null)
            StopCoroutine(changeRoutine);

        // natychmiast ustaw pulsujący czerwony
        changeRoutine = StartCoroutine(ChangeColor(dangerColor));

        // reset timera
        if (dangerRoutine != null)
            StopCoroutine(dangerRoutine);

        dangerRoutine = StartCoroutine(DangerStateTimer());
    }

    /// <summary>
    /// Timer odpowiadający za powrót do normalnego koloru po określonym czasie bez obrażeń.
    /// </summary>
    private IEnumerator DangerStateTimer()
    {
        yield return new WaitForSeconds(dangerDuration);

        // po czasie wraca do normy
        SetNormal();
    }

    /// <summary>
    /// Powrót do normalnego koloru.
    /// </summary>
    public void SetNormal()
    {
        if (dangerRoutine != null)
            StopCoroutine(dangerRoutine);

        if (changeRoutine != null)
            StopCoroutine(changeRoutine);

        changeRoutine = StartCoroutine(ChangeColor(normalColor));
    }

    /// <summary>
    /// Płynna interpolacja koloru.
    /// </summary>
    private IEnumerator ChangeColor(Color target)
    {
        while (light2D.color != target)
        {
            light2D.color = Color.Lerp(light2D.color, target, Time.deltaTime * 4f);
            yield return null;
        }
    }
}
