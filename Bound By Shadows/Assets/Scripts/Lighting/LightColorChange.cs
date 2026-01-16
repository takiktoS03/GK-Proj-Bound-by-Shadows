using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

/**
 * Skrypt sterujący zmianą koloru światła w zależności od stanu (normalny / zagrożenie).
 * Wykorzystuje płynną interpolację koloru oraz czasowe efekty wizualne.
 *
 * @author Filip Kudła
 */
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

    public void SetDanger()
    {
        if (changeRoutine != null)
            StopCoroutine(changeRoutine);

        changeRoutine = StartCoroutine(ChangeColor(dangerColor));

        if (dangerRoutine != null)
            StopCoroutine(dangerRoutine);

        dangerRoutine = StartCoroutine(DangerStateTimer());
    }

    private IEnumerator DangerStateTimer()
    {
        yield return new WaitForSeconds(dangerDuration);
        SetNormal();
    }

    public void SetNormal()
    {
        if (dangerRoutine != null)
            StopCoroutine(dangerRoutine);

        if (changeRoutine != null)
            StopCoroutine(changeRoutine);

        changeRoutine = StartCoroutine(ChangeColor(normalColor));
    }
    private IEnumerator ChangeColor(Color target)
    {
        while (light2D.color != target)
        {
            light2D.color = Color.Lerp(light2D.color, target, Time.deltaTime * 4f);
            yield return null;
        }
    }
}
