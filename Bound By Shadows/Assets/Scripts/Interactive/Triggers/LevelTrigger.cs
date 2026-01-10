using UnityEngine;

public class LevelTrigger : MonoBehaviour
{
    [Header("Target Scene")]
    public string nextSceneName;

    [Header("Transition Settings")]
    public float fadeDuration = 1.0f;
    public KeyCode key = KeyCode.F;         // klawisz aktywacji
    public bool automaticTransition = false;  // brak potrzeby wciskania klawisza

    [Tooltip("Natychmiastowe przejście (bez fade)?")]
    public bool instantCut = false;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (automaticTransition || Input.GetKey(key))
            {
                float duration = instantCut ? 0f : fadeDuration;
                GameManager.Instance.LoadLevel(nextSceneName, duration);
            }
        }
    }
}