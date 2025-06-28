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
    private bool destroyed = false;

    /**
     * @brief Inicjalizuje komponent Animator przypisany do beczki.
     *
     * Wywoływane automatycznie przy aktywacji obiektu.
     */
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    /**
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


