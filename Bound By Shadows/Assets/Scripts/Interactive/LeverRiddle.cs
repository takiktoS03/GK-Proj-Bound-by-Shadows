using UnityEngine;

/**
 * @class LeverRiddle
 * @brief Sprawdza poprawno�� u�o�enia d�wigni i aktywuje ukryt� platform�.
 *
 * Gdy d�wignie zostan� ustawione w odpowiedniej kombinacji, aktywuje ukryte kafelki (np. kamie�, most).
 *
 * @author Filip Kud�a
 */
public class LeverRiddle : MonoBehaviour
{
    /// @brief Obiekt z ukrytymi kafelkami do aktywacji.
    [SerializeField] private GameObject hiddenTiles;
    /// @brief Referencje do 3 d�wigni w zagadce.
    [SerializeField] private GameObject lever1;
    [SerializeField] private GameObject lever2;
    [SerializeField] private GameObject lever3;

    private bool lever1On;
    private bool lever2On;
    private bool lever3On;

    /**
     * @brief Sprawdza aktualny stan d�wigni i aktywuje ukryte kafelki, je�li warunek jest spe�niony.
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
