using UnityEngine;

/**
 * Skrypt obsługujący niszczenie beczki po uderzeniu atakiem gracza
 * oraz zapis jej stanu w systemie zapisu gry.
 *
 * @author Julia Bigaj
 */

public class Barrel : MonoBehaviour
{
    private Animator anim;
    private SaveableObject saveable;

    /**
     * @brief Inicjalizuje komponent Animator przypisany do beczki.
     *
     * Wywoływane automatycznie przy aktywacji obiektu.
     */
    void Awake()
    {
        anim = GetComponent<Animator>();
        saveable = GetComponent<SaveableObject>();
    }

    public void OnBarrelDestroyed()
    {
        SoundLibrary.Instance.PlayBarrel();
        anim.SetTrigger("Destroy");

        // Zapis stanu
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        SessionDestroyedRegistry.MarkAsDestroyed(sceneName, saveable.UniqueId);

        Destroy(gameObject, 0.9f);
    }
}


