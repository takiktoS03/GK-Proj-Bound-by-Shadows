using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DissolveTrigger : MonoBehaviour
{
    public float duration = 1f;
    public bool invert = false; // w celu odwrócenia efektu (1->0)
    
    [HideInInspector] public bool triggered = false;

    private DissolveEffect effect;

    void Awake()
    {
        effect = GetComponent<DissolveEffect>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player") || triggered) return;
        triggered = true;

        effect.PlayDissolve(duration, invert);
    }
}
