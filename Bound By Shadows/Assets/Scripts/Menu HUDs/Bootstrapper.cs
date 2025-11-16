using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/**
 * @class Bootstrapper
 * @brief Ładuje sceny startowe gry przy uruchomieniu aplikacji.
 *
 * Skrypt sprawdza, czy scena inicjalizacyjna (`InitScene`) jest załadowana.
 * Jeśli nie — ładuje ją jako scenę dodatkową (Additive), a następnie ładuje
 * scenę główną (`MainMenu`) jako podstawową (Single).
 *
 * Używany jako punkt wejściowy aplikacji.
 *
 * @author Julia Bigaj
 */
public class Bootstrapper : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(InitializeGame());
    }

    private IEnumerator InitializeGame()
    {
        // Jeśli InitScene nie jest jeszcze załadowana, załaduj ją ADDITIVE
        if (!SceneManager.GetSceneByName("InitScene").isLoaded)
        {
            AsyncOperation asyncInit = SceneManager.LoadSceneAsync("InitScene", LoadSceneMode.Additive);
            yield return new WaitUntil(() => asyncInit.isDone);
        }

        // Teraz InitScene jest na pewno w pamięci
        Debug.Log(" InitScene loaded, now loading Cave...");

        // Wczytaj scenę docelową jako główną
        SceneManager.LoadScene("Level 1 - Cave", LoadSceneMode.Single);
    }
}

