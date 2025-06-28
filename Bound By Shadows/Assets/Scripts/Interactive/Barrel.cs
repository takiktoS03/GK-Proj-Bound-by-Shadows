using UnityEngine;

/**
 * @class Barrel
 * @brief Obs�uguje niszczenie beczki po uderzeniu atakiem gracza.
 *
 * Po kolizji z atakiem, uruchamia animacj� zniszczenia, odtwarza d�wi�k i zapisuje stan w systemie zapisu.
 * Beczka jest niszczona po 0.9s, co pozwala doko�czy� animacj�.
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
     * Wywo�ywane automatycznie przy aktywacji obiektu.
     */
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    /**
     * @brief Reaguje na kolizj� z atakiem gracza.
     *
     * Je�li obiekt nie zosta� jeszcze zniszczony i wykryto uderzenie przez tag `PlayerAttack`:
     * - odtwarza d�wi�k zniszczenia,
     * - zapisuje unikalny identyfikator beczki jako zniszczon� (dla systemu zapisu),
     * - uruchamia animacj�,
     * - niszczy obiekt po 0.9 sekundy.
     *
     * @param other Obiekt koliduj�cy z beczk�.
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

