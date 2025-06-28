using UnityEngine;
using TMPro;

/**
 * @class HintArea
 * @brief Wy�wietla wiadomo�� podpowiedzi, gdy gracz znajdzie si� w okre�lonym obszarze.
 *
 * Wykrywa obecno�� gracza w triggerze i przekazuje tekst do `HintController`,
 * kt�ry zarz�dza wy�wietlaniem wiadomo�ci na ekranie.
 *
 * Wspiera mechaniki eksploracji i nawigacji poprzez wskaz�wki �rodowiskowe.
 *
 * @author Julia Bigaj
 */
public class HintArea : MonoBehaviour
{
    /// @brief Tekst wiadomo�ci, kt�ra ma by� wy�wietlona graczowi.
    public string message;

    /// @brief Czy gracz aktualnie znajduje si� w triggerze.
    private bool playerInside = false;

    /// @brief Referencja do kontrolera podpowiedzi (`HintController`).
    private HintController hintController;

    /**
     * @brief Inicjalizuje referencj� do HintController.
     */
    private void Start()
    {
        hintController = FindObjectOfType<HintController>();
    }

    /**
     * @brief Pokazuje wiadomo��, gdy gracz wejdzie w obszar podpowiedzi.
     * @param other Obiekt koliduj�cy (np. gracz).
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
     * @brief Ukrywa wiadomo��, gdy gracz opu�ci obszar podpowiedzi.
     * @param other Obiekt koliduj�cy (np. gracz).
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
