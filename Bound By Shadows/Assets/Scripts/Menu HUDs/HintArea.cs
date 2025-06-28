using UnityEngine;
using TMPro;

/**
 * @class HintArea
 * @brief Wyœwietla wiadomoœæ podpowiedzi, gdy gracz znajdzie siê w okreœlonym obszarze.
 *
 * Wykrywa obecnoœæ gracza w triggerze i przekazuje tekst do `HintController`,
 * który zarz¹dza wyœwietlaniem wiadomoœci na ekranie.
 *
 * Wspiera mechaniki eksploracji i nawigacji poprzez wskazówki œrodowiskowe.
 *
 * @author Julia Bigaj
 */
public class HintArea : MonoBehaviour
{
    /// @brief Tekst wiadomoœci, która ma byæ wyœwietlona graczowi.
    public string message;

    /// @brief Czy gracz aktualnie znajduje siê w triggerze.
    private bool playerInside = false;

    /// @brief Referencja do kontrolera podpowiedzi (`HintController`).
    private HintController hintController;

    /**
     * @brief Inicjalizuje referencjê do HintController.
     */
    private void Start()
    {
        hintController = FindObjectOfType<HintController>();
    }

    /**
     * @brief Pokazuje wiadomoœæ, gdy gracz wejdzie w obszar podpowiedzi.
     * @param other Obiekt koliduj¹cy (np. gracz).
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
     * @brief Ukrywa wiadomoœæ, gdy gracz opuœci obszar podpowiedzi.
     * @param other Obiekt koliduj¹cy (np. gracz).
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
