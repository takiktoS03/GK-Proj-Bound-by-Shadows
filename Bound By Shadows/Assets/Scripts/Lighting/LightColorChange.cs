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
    /// Inicjuje stan zagrożenia (zmiana koloru na czerwony).
    /// Przerywa poprzednie animacje, aby uniknąć konfliktu interpolacji.
    /// </summary>
    public void SetDanger()
    {
        if (changeRoutine != null)
            StopCoroutine(changeRoutine);

        changeRoutine = StartCoroutine(ChangeColor(dangerColor));

        if (dangerRoutine != null)
            StopCoroutine(dangerRoutine);

        dangerRoutine = StartCoroutine(DangerStateTimer());
    }

    /// <summary>
    /// Odmierza czas trwania efektu wizualnego, a następnie przywraca stan domyślny.
    /// </summary>
    private IEnumerator DangerStateTimer()
    {
        yield return new WaitForSeconds(dangerDuration);
        SetNormal();
    }

    /// <summary>
    /// Przywraca stan normalny (kolor bazowy).
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
    /// Realizuje płynną interpolację (Lerp) koloru światła w pętli klatek.
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
