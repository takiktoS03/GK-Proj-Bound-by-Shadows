using UnityEngine;

/**
 * @class LeverTrigger
 * @brief Obsługuje interakcję gracza z dźwignią i zmienia jej stan.
 *
 * Gracz może aktywować lub dezaktywować dźwignię po naciśnięciu klawisza F, jeśli znajduje się w zasięgu.
 * Dźwignia uruchamia animację i dźwięk oraz przekazuje informację do skryptu zagadki.
 *
 * @author Julia Bigaj
 */
public class LeverTrigger : MonoBehaviour
{
    /// @brief Animator przypisany do dźwigni (obsługuje stan On/Off).
    public Animator leverAnimator;

    private bool playerInRange;

    /// @brief Aktualny stan dźwigni (czy włączona).
    [HideInInspector] public bool leverIsOn;

    /**
     * @brief Wykrywa naciśnięcie klawisza F, gdy gracz znajduje się w zasięgu.
     *
     * W zależności od obecnego stanu dźwigni aktywuje odpowiednią animację, dźwięk oraz aktualizuje flagę `leverIsOn`.
     * Nie wywołuje `CheckRiddle()` – powinno być dodane, jeśli dźwignia należy do zagadki.
     */
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && playerInRange)
        {
            if (!leverIsOn)
            {
                leverAnimator.SetTrigger("ActiveOn");
                leverIsOn = true;
                SoundManager.Instance?.PlayLever();
            }
            else
            {
                leverAnimator.SetTrigger("ActiveOff");
                leverIsOn = false;
                SoundManager.Instance?.PlayLever();
            }
        }
    }

    /**
     * @brief Wykrywa wejście gracza w zasięg kolizji.
     * @param other Obiekt kolidujący z dźwignią.
     */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    /**
     * @brief Wykrywa wyjście gracza z zasięgu kolizji.
     * @param other Obiekt opuszczający obszar kolizji.
     */
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }

    /**
     * @brief Wywołuje sprawdzenie poprawności zagadki przez nadrzędny obiekt LeverRiddle.
     */
    private void CheckRiddle()
    {
        GetComponentInParent<LeverRiddle>().CheckCorrectness();
    }
}
