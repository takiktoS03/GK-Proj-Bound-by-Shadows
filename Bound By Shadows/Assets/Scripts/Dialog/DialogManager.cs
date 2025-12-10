using TMPro;
using System.Collections;
using UnityEngine;

/**
 * @class DialogManager
 * @brief Wyświetla tymczasowe wiadomości dialogowe na ekranie w komponentach TextMeshProUGUI.
 *
 * Umożliwia prezentację tekstu w jednym z wielu okien dialogowych (np. powiadomień, opisów, wskazówek).
 * Tekst znika automatycznie po określonym czasie.
 *
 * @author Filip Kudła
 */
public class DialogManager : MonoBehaviour
{
    /// @brief Singleton systemu dialogów
    public static DialogManager Instance;

    /// @brief Tablica okien dialogowych (TextMeshProUGUI), w których może pojawić się tekst.
    /// 1 – dialogi fabularne NPC (dół ekranu)
    /// 2 – myśli gracza (nad głową)
    /// 3 - komunikaty UI do skrzynek/drzwi itp.
    /// 4 - Podpowiedzi

    public TextMeshProUGUI[] dialogBoxes;

    private Coroutine[] routines;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        routines = new Coroutine[dialogBoxes.Length];
    }

    /**
     * @brief Wyświetla wiadomość tekstową w określonym oknie dialogowym na określony czas.
     * @param text Tekst do wyświetlenia.
     * @param duration Czas trwania wyświetlania tekstu (domyślnie 3 sekundy).
     * @param boxIndex Indeks okna dialogowego z tablicy `dialogBoxes`.
     */
    public void Show(string text, float duration = 3f, int boxIndex = 0)
    {
        if (boxIndex < 0 || boxIndex >= dialogBoxes.Length) return;

        if (routines[boxIndex] != null)
            StopCoroutine(routines[boxIndex]);

        routines[boxIndex] = StartCoroutine(ShowRoutine(text, duration, dialogBoxes[boxIndex], boxIndex));
    }

    /**
     * @brief Coroutine wyświetlająca tekst i ukrywająca go po upływie czasu.
     * @param text Tekst do wyświetlenia.
     * @param duration Czas trwania wyświetlania.
     * @param box Komponent TMP, w którym tekst ma zostać pokazany.
     * @return Enumerator używany przez `StartCoroutine`.
     */
    private IEnumerator ShowRoutine(string text, float duration, TextMeshProUGUI box, int i)
    {
        box.text = text;
        box.alpha = 1f;

        yield return new WaitForSeconds(duration);

        box.text = "";
        box.alpha = 0f;

        routines[i] = null;
    }

    /**
     * @brief Funkcja czyszcząca aktualnie wyświetlany dialog (box)
     */
    public void Clear(int boxIndex = 0)
    {
        if (boxIndex < 0 || boxIndex >= dialogBoxes.Length) return;

        if (routines[boxIndex] != null)
            StopCoroutine(routines[boxIndex]);

        dialogBoxes[boxIndex].text = "";
        dialogBoxes[boxIndex].alpha = 0f;

        routines[boxIndex] = null;
    }

    /**
     * @brief Funkcja czyszcząca wszystkie dialogi
     */
    public void ClearAll()
    {
        for (int i = 0; i < dialogBoxes.Length; i++)
        {
            if (routines[i] != null)
                StopCoroutine(routines[i]);

            dialogBoxes[i].text = "";
            dialogBoxes[i].alpha = 0f;
            routines[i] = null;
        }
    }
}

