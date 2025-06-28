using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * @class MusicManager
 * @brief Globalny menedżer muzyki i narracji działający między scenami.
 *
 * Zarządza odtwarzaniem odpowiednich ścieżek dźwiękowych i narracji w zależności od aktualnie załadowanej sceny.
 * Implementuje wzorzec singletonu, aby zapewnić dostępność między scenami (`DontDestroyOnLoad`).
 *
 * Obsługuje oddzielne źródła dźwięku: jedno dla muzyki (`audioSource`), drugie dla narracji (`narrationSource`).
 *
 * @author Julia Bigaj
 */
public class MusicManager : MonoBehaviour
{
    /// @brief Instancja singletonu.
    public static MusicManager Instance { get; private set; }

    /// @brief Źródło audio odpowiedzialne za tło muzyczne.
    public AudioSource audioSource;

    /// @brief Źródło audio odpowiedzialne za narrację (np. intro).
    public AudioSource narrationSource;

    /// @brief Muzyka odtwarzana w menu głównym.
    public AudioClip menuMusic;

    /// @brief Muzyka odtwarzana podczas gry.
    public AudioClip gameplayMusic;

    /// @brief Narracja głosowa odtwarzana w scenie Intro.
    public AudioClip introNarration;

    /// @brief Muzyka odtwarzana w scenie Intro.
    public AudioClip introMusic;

    /**
     * @brief Inicjalizacja singletonu i rejestracja zdarzenia ładowania sceny.
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
     * @brief Reaguje na załadowanie nowej sceny i odtwarza odpowiednią muzykę.
     * @param scene Załadowana scena.
     * @param mode Tryb ładowania sceny (Single/Additive).
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
     * @brief Odtwarza muzykę menu, jeśli jeszcze nie jest aktywna.
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
     * @brief Odtwarza muzykę tła do rozgrywki z domyślną lub określoną głośnością.
     * @param volume Poziom głośności muzyki (domyślnie 1f).
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
     * @brief Odtwarza narrację i muzykę w scenie Intro.
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

