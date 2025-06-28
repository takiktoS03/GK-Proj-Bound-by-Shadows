using UnityEngine;

/**
 * @class LeverTrigger
 * @brief Obs�uguje interakcj� gracza z d�wigni� i zmienia jej stan.
 *
 * Gracz mo�e aktywowa� lub dezaktywowa� d�wigni� po naci�ni�ciu klawisza F, je�li znajduje si� w zasi�gu.
 * D�wignia uruchamia animacj� i d�wi�k oraz przekazuje informacj� do skryptu zagadki.
 *
 * @author Julia Bigaj
 */
public class LeverTrigger : MonoBehaviour
{
    /// @brief Animator przypisany do d�wigni (obs�uguje stan On/Off).
    public Animator leverAnimator;

    private bool playerInRange;

    /// @brief Aktualny stan d�wigni (czy w��czona).
    [HideInInspector] public bool leverIsOn;

    /**
     * @brief Wykrywa naci�ni�cie klawisza F, gdy gracz znajduje si� w zasi�gu.
     *
     * W zale�no�ci od obecnego stanu d�wigni aktywuje odpowiedni� animacj�, d�wi�k oraz aktualizuje flag� `leverIsOn`.
     * Nie wywo�uje `CheckRiddle()` � powinno by� dodane, je�li d�wignia nale�y do zagadki.
     */
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && playerInRange)
        {
            if (!leverIsOn)
            {
                leverAnimator.SetTrigger("ActiveOn");
                leverIsOn = true;
                SoundManager.Instance?.PlayLever();
            }
            else
            {
                leverAnimator.SetTrigger("ActiveOff");
                leverIsOn = false;
                SoundManager.Instance?.PlayLever();
            }
        }
    }

    /**
     * @brief Wykrywa wej�cie gracza w zasi�g kolizji.
     * @param other Obiekt koliduj�cy z d�wigni�.
     */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    /**
     * @brief Wykrywa wyj�cie gracza z zasi�gu kolizji.
     * @param other Obiekt opuszczaj�cy obszar kolizji.
     */
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }

    /**
     * @brief Wywo�uje sprawdzenie poprawno�ci zagadki przez nadrz�dny obiekt LeverRiddle.
     */
    private void CheckRiddle()
    {
        GetComponentInParent<LeverRiddle>().CheckCorrectness();
    }
}