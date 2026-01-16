using UnityEngine;

/**
 * Skrypt sceny zarządzający ustawieniami dźwięku tła,
 * w szczególności odtwarzaniem muzyki dla danej lokacji.
 *
 * @author Filip Kudła
 */
public class SceneSettings : MonoBehaviour
{
    [Header("Music Settings")]
    [Tooltip("Pozostaw puste, aby wyłączyć muzykę")]
    public AudioClip backgroundMusic;
    [Range(0f, 1f)] public float musicVolume = 0.5f;

    private void Start()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic(backgroundMusic, musicVolume);
        }
    }
}