using UnityEngine;

public class SceneSettings : MonoBehaviour
{
    [Header("Music Settings")]
    [Tooltip("Pozostaw puste, aby wyłączyć muzykę")]
    public AudioClip backgroundMusic;
    [Range(0f, 1f)] public float musicVolume = 0.5f;

    [Header("Camera / Listeners")]
    public bool removeExtraAudioListeners = true;

    private void Start()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic(backgroundMusic, musicVolume);
        }

        // Usuwanie zbędnych AudioListenery (żeby nie było błędu "2 listeners")
        //if (removeExtraAudioListeners)
        //{
        //    var listeners = FindObjectsOfType<AudioListener>();
        //    foreach (var listener in listeners)
        //    {
        //        // Usuwamy listenera jeśli to nie ten główny z systemów
        //        // (Zakładamy, że główny jest w DontDestroyOnLoad)
        //        if (listener.gameObject.scene.name != "DontDestroyOnLoad" && listener.gameObject.scene.name != "BootScene")
        //        {
        //            Destroy(listener);
        //        }
        //    }
        //}
    }
}