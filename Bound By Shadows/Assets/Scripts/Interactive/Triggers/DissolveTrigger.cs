using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

/**
 * Skrypt uruchamiający efekt dissolve obiektu po wejściu gracza w trigger.
 * Może opcjonalnie aktywować cienie i zapobiega wielokrotnemu uruchomieniu efektu.
 *
 * @author Filip Kudła
 */

[RequireComponent(typeof(SpriteRenderer))]
public class DissolveTrigger : MonoBehaviour
{
    public float duration = 1f;
    public bool invert = false; // w celu odwrócenia efektu (1->0)
    
    [HideInInspector] public bool triggered = false;

    private DissolveEffect effect;
    private ShadowCaster2D shadowCaster;

    void Awake()
    {
        effect = GetComponent<DissolveEffect>();
        shadowCaster = GetComponent<ShadowCaster2D>();
        if (shadowCaster != null)
        {
            shadowCaster.enabled = false;
        }
    }

    private void Start()
    {
        effect.UpdateMaterial(invert ? 1f : 0f);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player") || triggered) return;
        triggered = true;

        if (shadowCaster != null)
        {
            shadowCaster.enabled = true;
        }

        effect.PlayDissolve(duration, invert);
    }
}
