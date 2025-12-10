using UnityEngine;

public class LevelTrigger : MonoBehaviour
{
    [Header("Target Scene")]
    public string nextSceneName;

    [Header("Transition Settings")]
    public float fadeDuration = 1.0f;
    [Tooltip("Natychmiastowe przejście (bez fade)?")]
    public bool instantCut = false;

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;
            
            float duration = instantCut ? 0f : fadeDuration;
            GameManager.Instance.LoadLevel(nextSceneName, duration);
        }
    }
}