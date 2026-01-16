using UnityEngine;
using DG.Tweening;

/**
 * Skrypt zarządzający globalnym systemem dźwięku w grze.
 * Odpowiada za odtwarzanie muzyki, jednorazowych efektów dźwiękowych
 * oraz zapętlonych efektów (np. kroki postaci).
 *
 * Implementuje wzorzec singletonu oraz obsługuje płynne przejścia
 * pomiędzy utworami muzycznymi (crossfade) z wykorzystaniem biblioteki DOTween.
 * Stanowi centralny punkt dostępu do dźwięku dla pozostałych systemów gry.
 *
 * @author Filip Kudła
 */

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioSource sfxLoopSource;

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
        if (musicSource.clip == clip && musicSource.isPlaying)
            return;

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

    public void StopSFX()
    {
        sfxSource.Stop();
    }

    public void StartLoopingSFX(AudioClip clip, float volume = 1f)
    {
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