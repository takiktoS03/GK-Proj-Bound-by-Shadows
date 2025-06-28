using EthanTheHero;
using UnityEngine;

/**
 * @class TriggerThoughtZone
 * @brief Wyświetla myśl bohatera po wejściu do specjalnej strefy w grze.
 *
 * Skrypt przypisany do triggera środowiskowego – po wejściu gracza:
 * - wyłącza ślizganie po ścianach (jeśli dotyczy),
 * - uruchamia wiadomość dialogową przez `TextUI`, wyświetlaną nad głową bohatera.
 *
 * Myśl znika automatycznie po kilku sekundach.
 *
 * @author Julia Bigaj
 */
public class TriggerThoughtZone : MonoBehaviour
{
    /// @brief Treść myśli wyświetlanej po wejściu do triggera.
    [TextArea]
    public string thought = "Nie ma innego wyjścia... \r\nChyba muszę po prostu tam skoczyć i zobaczyć co się stanie";

    /**
     * @brief Reaguje na wejście gracza w trigger – wyświetla myśl i wyłącza ślizganie po ścianie.
     * @param other Obiekt kolidujący (spodziewany: gracz).
     */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement movement = other.GetComponent<PlayerMovement>();
            if (movement != null)
                movement.wallSlidingEnabled = false;

            TextUI thoughtUI = FindObjectOfType<TextUI>();
            if (thoughtUI != null)
            {
                thoughtUI.ShowTextDialog(thought, 3f, 1); ///< Wyświetlenie myśli przez 3 sekundy w polu nr 1.
            }
        }
    }
}

