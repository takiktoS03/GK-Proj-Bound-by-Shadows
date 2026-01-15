using UnityEngine;

/**
 * @class Barrel
 * @brief Obsługuje niszczenie beczki po uderzeniu atakiem gracza.
 *
 * Po kolizji z atakiem, uruchamia animację zniszczenia, odtwarza dźwięk i zapisuje stan w systemie zapisu.
 * Beczka jest niszczona po 0.9s, co pozwala dokończyć animację.
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

    /** do przerobienia
     * @brief Reaguje na kolizję z atakiem gracza.
     *
     * Jeśli obiekt nie został jeszcze zniszczony i wykryto uderzenie przez tag `PlayerAttack`:
     * - odtwarza dźwięk zniszczenia,
     * - zapisuje unikalny identyfikator beczki jako zniszczoną (dla systemu zapisu),
     * - uruchamia animację,
     * - niszczy obiekt po 0.9 sekundy.
     *
     * @param other Obiekt kolidujący z beczką.
     */


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


