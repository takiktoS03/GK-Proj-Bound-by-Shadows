using UnityEngine;
using TMPro;

/**
 * @class HintArea
 * @brief Wyświetla wiadomość podpowiedzi, gdy gracz znajdzie się w określonym obszarze.
 *
 * Wykrywa obecność gracza w triggerze i przekazuje tekst do `HintController`,
 * który zarządza wyświetlaniem wiadomości na ekranie.
 *
 * Wspiera mechaniki eksploracji i nawigacji poprzez wskazówki środowiskowe.
 *
 * @author Julia Bigaj
 */
public class HintArea : MonoBehaviour
{
    /// @brief Tekst wiadomości, która ma być wyświetlona graczowi.
    public string message;

    /// @brief Czy gracz aktualnie znajduje się w triggerze.
    private bool playerInside = false;

    /// @brief Referencja do kontrolera podpowiedzi (`HintController`).
    private HintController hintController;

    /**
     * @brief Inicjalizuje referencję do HintController.
     */
    private void Start()
    {
        hintController = FindObjectOfType<HintController>();
    }

    /**
     * @brief Pokazuje wiadomość, gdy gracz wejdzie w obszar podpowiedzi.
     * @param other Obiekt kolidujący (np. gracz).
     */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            hintController.ShowHint(message);
        }
    }

    /**
     * @brief Ukrywa wiadomość, gdy gracz opuści obszar podpowiedzi.
     * @param other Obiekt kolidujący (np. gracz).
     */
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            hintController.HideHint();
        }
    }
}

