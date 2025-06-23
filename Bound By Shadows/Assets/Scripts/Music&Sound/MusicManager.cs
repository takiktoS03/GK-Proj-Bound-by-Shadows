using UnityEngine;
using UnityEngine.SceneManagement;

/* Globalny mened¿er muzyki i narracji.
   - Odtwarza odpowiedni¹ muzykê w zale¿noœci od sceny (menu, intro, gameplay).
   - Obs³uguje oddzielne Ÿród³a dŸwiêku dla narracji i muzyki.
   - Singleton dzia³aj¹cy miêdzy scenami.

   Autor: Julia Bigaj
*/

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    public AudioSource audioSource;
    public AudioSource narrationSource;

    public AudioClip menuMusic;
    public AudioClip gameplayMusic;
    public AudioClip introNarration;
    public AudioClip introMusic;

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

    public void PlayMenuMusic()
    {
        if (audioSource.clip != menuMusic)
        {
            audioSource.clip = menuMusic;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

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


    public void StopMusic()
    {
        if (audioSource.isPlaying) audioSource.Stop();
        if (narrationSource.isPlaying) narrationSource.Stop();
    }
}