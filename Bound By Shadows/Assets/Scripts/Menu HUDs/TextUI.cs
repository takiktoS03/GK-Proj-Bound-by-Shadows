using TMPro;
using System.Collections;
using UnityEngine;

/**
 * @class TextUI
 * @brief Wyświetla tymczasowe wiadomości dialogowe na ekranie w komponentach TextMeshProUGUI.
 *
 * Umożliwia prezentację tekstu w jednym z wielu okien dialogowych (np. powiadomień, opisów, wskazówek).
 * Tekst znika automatycznie po określonym czasie.
 *
 * @author Filip Kudła
 */
public class TextUI : MonoBehaviour
{
    /// @brief Tablica okien dialogowych (TextMeshProUGUI), w których może pojawić się tekst.
    public TextMeshProUGUI[] dialogBoxes;

    /// @brief Referencja do aktualnie działającej coroutine dialogu.
    private Coroutine currentRoutine;

    /**
     * @brief Wyświetla wiadomość tekstową w określonym oknie dialogowym na określony czas.
     * @param text Tekst do wyświetlenia.
     * @param duration Czas trwania wyświetlania tekstu (domyślnie 3 sekundy).
     * @param boxIndex Indeks okna dialogowego z tablicy `dialogBoxes`.
     */
    public void ShowTextDialog(string text, float duration = 3f, int boxIndex = 0)
    {
        if (boxIndex < 0 || boxIndex >= dialogBoxes.Length) return;

        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(ShowRoutine(text, duration, dialogBoxes[boxIndex]));
    }

    /**
     * @brief Coroutine wyświetlająca tekst i ukrywająca go po upływie czasu.
     * @param text Tekst do wyświetlenia.
     * @param duration Czas trwania wyświetlania.
     * @param box Komponent TMP, w którym tekst ma zostać pokazany.
     * @return Enumerator używany przez `StartCoroutine`.
     */
    private IEnumerator ShowRoutine(string text, float duration, TextMeshProUGUI box)
    {
        box.text = text;
        box.alpha = 1;

        yield return new WaitForSeconds(duration);

        box.alpha = 0;
        box.text = "";
    }
}

