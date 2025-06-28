using TMPro;
using System.Collections;
using UnityEngine;

/**
 * @class TextUI
 * @brief Wy�wietla tymczasowe wiadomo�ci dialogowe na ekranie w komponentach TextMeshProUGUI.
 *
 * Umo�liwia prezentacj� tekstu w jednym z wielu okien dialogowych (np. powiadomie�, opis�w, wskaz�wek).
 * Tekst znika automatycznie po okre�lonym czasie.
 *
 * @author Filip Kud�a
 */
public class TextUI : MonoBehaviour
{
    /// @brief Tablica okien dialogowych (TextMeshProUGUI), w kt�rych mo�e pojawi� si� tekst.
    public TextMeshProUGUI[] dialogBoxes;

    /// @brief Referencja do aktualnie dzia�aj�cej coroutine dialogu.
    private Coroutine currentRoutine;

    /**
     * @brief Wy�wietla wiadomo�� tekstow� w okre�lonym oknie dialogowym na okre�lony czas.
     * @param text Tekst do wy�wietlenia.
     * @param duration Czas trwania wy�wietlania tekstu (domy�lnie 3 sekundy).
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
     * @brief Coroutine wy�wietlaj�ca tekst i ukrywaj�ca go po up�ywie czasu.
     * @param text Tekst do wy�wietlenia.
     * @param duration Czas trwania wy�wietlania.
     * @param box Komponent TMP, w kt�rym tekst ma zosta� pokazany.
     * @return Enumerator u�ywany przez `StartCoroutine`.
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
