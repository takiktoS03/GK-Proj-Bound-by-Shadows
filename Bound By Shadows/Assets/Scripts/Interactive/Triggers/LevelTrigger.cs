using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Skrypt odpowiedzialny za przejście gracza do innej sceny
 * z obsługą zapisu stanu gry i efektu przejścia.
 *
 * @author Filip Kudła
 */
public class LevelTrigger : MonoBehaviour
{
    [Header("Target Scene Options")]
    public string nextSceneName;
    public string targetSpawnID;

    [Header("Transition Settings")]
    public float fadeDuration = 1.0f;
    public KeyCode key = KeyCode.F;           // klawisz aktywacji
    public bool automaticTransition = false;  // brak potrzeby wciskania klawisza
    [Tooltip("Natychmiastowe przejście (bez fade)?")]
    public bool instantCut = false;
    public bool destroyAfter = false;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !GameManager.Instance.IsLoading)
        {
            if (automaticTransition || Input.GetKey(key))
            {
                if (destroyAfter)
                {
                    var saveable = GetComponent<SaveableObject>();
                    if (saveable != null)
                    {
                        string currentSceneName = SceneManager.GetActiveScene().name;
                        SessionDestroyedRegistry.MarkAsDestroyed(currentSceneName, saveable.UniqueId);
                    }
                }
                SaveSystem.SaveCurrentScene();
                var controlManager = collision.GetComponent<PlayerControlManager>();
                controlManager.LockControls(true, true, true, true);

                float duration = instantCut ? 0f : fadeDuration;
                SaveSystem.restorePlayerPositionOnLoad = false;
                SaveSystem.loadOnSceneStart = true;
                GameManager.Instance.targetSpawnId = targetSpawnID;
                GameManager.Instance.LoadLevel(nextSceneName, duration);
            }
        }
    }
}