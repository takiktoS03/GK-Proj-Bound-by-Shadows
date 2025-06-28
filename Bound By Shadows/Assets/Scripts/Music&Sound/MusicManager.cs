using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * @class MusicManager
 * @brief Globalny mened�er muzyki i narracji dzia�aj�cy mi�dzy scenami.
 *
 * Zarz�dza odtwarzaniem odpowiednich �cie�ek d�wi�kowych i narracji w zale�no�ci od aktualnie za�adowanej sceny.
 * Implementuje wzorzec singletonu, aby zapewni� dost�pno�� mi�dzy scenami (`DontDestroyOnLoad`).
 *
 * Obs�uguje oddzielne �r�d�a d�wi�ku: jedno dla muzyki (`audioSource`), drugie dla narracji (`narrationSource`).
 *
 * @author Julia Bigaj
 */
public class MusicManager : MonoBehaviour
{
    /// @brief Instancja singletonu.
    public static MusicManager Instance { get; private set; }

    /// @brief �r�d�o audio odpowiedzialne za t�o muzyczne.
    public AudioSource audioSource;

    /// @brief �r�d�o audio odpowiedzialne za narracj� (np. intro).
    public AudioSource narrationSource;

    /// @brief Muzyka odtwarzana w menu g��wnym.
    public AudioClip menuMusic;

    /// @brief Muzyka odtwarzana podczas gry.
    public AudioClip gameplayMusic;

    /// @brief Narracja g�osowa odtwarzana w scenie Intro.
    public AudioClip introNarration;

    /// @brief Muzyka odtwarzana w scenie Intro.
    public AudioClip introMusic;

    /**
     * @brief Inicjalizacja singletonu i rejestracja zdarzenia �adowania sceny.
     */
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    /**
     * @brief Reaguje na za�adowanie nowej sceny i odtwarza odpowiedni� muzyk�.
     * @param scene Za�adowana scena.
     * @param mode Tryb �adowania sceny (Single/Additive).
     */
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StopMusic();

        if (scene.name == "Intro")
        {
            return;
        }
        else if (scene.name == "MainMenu")
        {
            PlayMenuMusic();
        }
        else if (scene.name == "Level 1 - Cave")
        {
            PlayGameplayMusic(0.3f);
        }
    }

    /**
     * @brief Odtwarza muzyk� menu, je�li jeszcze nie jest aktywna.
     */
    public void PlayMenuMusic()
    {
        if (audioSource.clip != menuMusic)
        {
            audioSource.clip = menuMusic;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    /**
     * @brief Odtwarza muzyk� t�a do rozgrywki z domy�ln� lub okre�lon� g�o�no�ci�.
     * @param volume Poziom g�o�no�ci muzyki (domy�lnie 1f).
     */
    public void PlayGameplayMusic(float volume = 1f)
    {
        if (audioSource.clip != gameplayMusic)
        {
            audioSource.clip = gameplayMusic;
            audioSource.loop = true;
            audioSource.volume = volume;
            audioSource.Play();
        }
    }

    /**
     * @brief Odtwarza narracj� i muzyk� w scenie Intro.
     */
    public void PlayIntroAudio()
    {
        // MUZYKA
        audioSource.clip = introMusic;
        audioSource.loop = true;
        audioSource.volume = 0.6f;
        audioSource.Play();
        Debug.Log("introMusic PLAY");

        // NARRACJA
        narrationSource.clip = introNarration;
        narrationSource.loop = false;
        narrationSource.volume = 1f;
        narrationSource.Play();
        Debug.Log("introNarration PLAY");
    }

    /**
     * @brief Zatrzymuje odtwarzanie aktualnej muzyki i narracji.
     */
    public void StopMusic()
    {
        if (audioSource.isPlaying) audioSource.Stop();
        if (narrationSource.isPlaying) narrationSource.Stop();
    }
}
