using UnityEngine;
using DG.Tweening;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    public AudioSource musicSource; // zapętlona muzyka
    public AudioSource sfxSource;   // krótkie efekty dźwiękowe
    public AudioSource sfxLoopSource;   // zapętlone efekty

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void PlayMusic(AudioClip clip, float volume = 1f)
    {
        // Nie resetujemy muzyki która już gra
        if (musicSource.clip == clip && musicSource.isPlaying)
            return;

        // Płynne przejście (Crossfade)
        Sequence s = DOTween.Sequence();
        s.Append(musicSource.DOFade(0f, 0.5f));
        s.AppendCallback(() =>
        {
            musicSource.clip = clip;
            musicSource.loop = true;
            musicSource.volume = 0f;
            if (clip != null) musicSource.Play();
            else musicSource.Stop();
        });

        if (clip != null)
        {
            s.Append(musicSource.DOFade(volume, 0.5f));
        }
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip, volume);
    }

    public void StartLoopingSFX(AudioClip clip, float volume = 1f)
    {
        // Nie resetujemy dźwięku która już gra
        if (sfxLoopSource.isPlaying && sfxLoopSource.clip == clip) 
            return;

        sfxLoopSource.clip = clip;
        sfxLoopSource.loop = true;
        sfxLoopSource.volume = volume;
        sfxLoopSource.Play();
    }

    public void StopLoopingSFX()
    {
        if (sfxLoopSource.isPlaying)
        {
            sfxLoopSource.Stop();
            sfxLoopSource.clip = null;
        }
    }

}