using UnityEngine;

/**
 * @class LeverRiddle
 * @brief Sprawdza poprawność ułożenia dźwigni i aktywuje ukrytą platformę.
 *
 * Gdy dźwignie zostaną ustawione w odpowiedniej kombinacji, aktywuje ukryte kafelki (np. kamień, most).
 *
 * @author Filip Kudła
 */
public class LeverRiddle : MonoBehaviour
{
    /// @brief Obiekt z ukrytymi kafelkami do aktywacji.
    [SerializeField] private GameObject hiddenTiles;
    /// @brief Referencje do 3 dźwigni w zagadce.
    [SerializeField] private GameObject lever1;
    [SerializeField] private GameObject lever2;
    [SerializeField] private GameObject lever3;

    private bool lever1On;
    private bool lever2On;
    private bool lever3On;

    /**
     * @brief Sprawdza aktualny stan dźwigni i aktywuje ukryte kafelki, jeśli warunek jest spełniony.
     */
    public void CheckCorrectness()
    {
        hiddenTiles.SetActive(false);
        lever1On = lever1.GetComponent<LeverTrigger>().leverIsOn;
        lever2On = lever2.GetComponent<LeverTrigger>().leverIsOn;
        lever3On = lever3.GetComponent<LeverTrigger>().leverIsOn;

        if (lever1On && !lever2On && lever3On)
        {
            hiddenTiles.SetActive(true);
            // dzwiek przesuwania kamienia
            SoundManager.Instance?.PlayStone();
        }
    }
}

