using TMPro;
using System.Collections;
using UnityEngine;

/**
 * @class TextUI
 * @brief Wyœwietla tymczasowe wiadomoœci dialogowe na ekranie w komponentach TextMeshProUGUI.
 *
 * Umo¿liwia prezentacjê tekstu w jednym z wielu okien dialogowych (np. powiadomieñ, opisów, wskazówek).
 * Tekst znika automatycznie po okreœlonym czasie.
 *
 * @author Filip Kud³a
 */
public class TextUI : MonoBehaviour
{
    /// @brief Tablica okien dialogowych (TextMeshProUGUI), w których mo¿e pojawiæ siê tekst.
    public TextMeshProUGUI[] dialogBoxes;

    /// @brief Referencja do aktualnie dzia³aj¹cej coroutine dialogu.
    private Coroutine currentRoutine;

    /**
     * @brief Wyœwietla wiadomoœæ tekstow¹ w okreœlonym oknie dialogowym na okreœlony czas.
     * @param text Tekst do wyœwietlenia.
     * @param duration Czas trwania wyœwietlania tekstu (domyœlnie 3 sekundy).
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
     * @brief Coroutine wyœwietlaj¹ca tekst i ukrywaj¹ca go po up³ywie czasu.
     * @param text Tekst do wyœwietlenia.
     * @param duration Czas trwania wyœwietlania.
     * @param box Komponent TMP, w którym tekst ma zostaæ pokazany.
     * @return Enumerator u¿ywany przez `StartCoroutine`.
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
