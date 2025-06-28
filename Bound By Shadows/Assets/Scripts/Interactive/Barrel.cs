using UnityEngine;

/**
 * @class Barrel
 * @brief Obs³uguje niszczenie beczki po uderzeniu atakiem gracza.
 *
 * Po kolizji z atakiem, uruchamia animacjê zniszczenia, odtwarza dŸwiêk i zapisuje stan w systemie zapisu.
 * Beczka jest niszczona po 0.9s, co pozwala dokoñczyæ animacjê.
 *
 * @author Julia Bigaj
 */
public class Barrel : MonoBehaviour
{
    private Animator anim;
    private bool destroyed = false;

    /**
     * @brief Inicjalizuje komponent Animator przypisany do beczki.
     *
     * Wywo³ywane automatycznie przy aktywacji obiektu.
     */
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    /**
     * @brief Reaguje na kolizjê z atakiem gracza.
     *
     * Jeœli obiekt nie zosta³ jeszcze zniszczony i wykryto uderzenie przez tag `PlayerAttack`:
     * - odtwarza dŸwiêk zniszczenia,
     * - zapisuje unikalny identyfikator beczki jako zniszczon¹ (dla systemu zapisu),
     * - uruchamia animacjê,
     * - niszczy obiekt po 0.9 sekundy.
     *
     * @param other Obiekt koliduj¹cy z beczk¹.
     */
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!destroyed && other.CompareTag("PlayerAttack"))
        {
            destroyed = true;

            SoundManager.Instance?.PlayBarrel();

            var saveable = GetComponent<SaveableObject>();
            BarrelSaveData.RegisterDestroyedBarrel(saveable.UniqueId);
            anim.SetTrigger("Destroy");
            Destroy(gameObject, 0.9f);
        }
    }
}

