using UnityEngine;

/**
 * @class LeverTrigger
 * @brief Obs³uguje interakcjê gracza z dŸwigni¹ i zmienia jej stan.
 *
 * Gracz mo¿e aktywowaæ lub dezaktywowaæ dŸwigniê po naciœniêciu klawisza F, jeœli znajduje siê w zasiêgu.
 * DŸwignia uruchamia animacjê i dŸwiêk oraz przekazuje informacjê do skryptu zagadki.
 *
 * @author Julia Bigaj
 */
public class LeverTrigger : MonoBehaviour
{
    /// @brief Animator przypisany do dŸwigni (obs³uguje stan On/Off).
    public Animator leverAnimator;

    private bool playerInRange;

    /// @brief Aktualny stan dŸwigni (czy w³¹czona).
    [HideInInspector] public bool leverIsOn;

    /**
     * @brief Wykrywa naciœniêcie klawisza F, gdy gracz znajduje siê w zasiêgu.
     *
     * W zale¿noœci od obecnego stanu dŸwigni aktywuje odpowiedni¹ animacjê, dŸwiêk oraz aktualizuje flagê `leverIsOn`.
     * Nie wywo³uje `CheckRiddle()` – powinno byæ dodane, jeœli dŸwignia nale¿y do zagadki.
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
     * @brief Wykrywa wejœcie gracza w zasiêg kolizji.
     * @param other Obiekt koliduj¹cy z dŸwigni¹.
     */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    /**
     * @brief Wykrywa wyjœcie gracza z zasiêgu kolizji.
     * @param other Obiekt opuszczaj¹cy obszar kolizji.
     */
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }

    /**
     * @brief Wywo³uje sprawdzenie poprawnoœci zagadki przez nadrzêdny obiekt LeverRiddle.
     */
    private void CheckRiddle()
    {
        GetComponentInParent<LeverRiddle>().CheckCorrectness();
    }
}