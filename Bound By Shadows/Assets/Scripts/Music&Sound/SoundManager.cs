using UnityEngine;

/**
 * @class SoundManager
 * @brief Centralny menedżer efektów dźwiękowych w grze.
 *
 * Implementuje wzorzec singletonu. Udostępnia metody do odtwarzania konkretnych efektów dźwiękowych,
 * takich jak ataki, otwieranie skrzyni, dźwięki łamigłówek czy kroki bohatera.
 * Kroki obsługiwane są osobnym źródłem dźwięku (`stepSource`) jako dźwięk zapętlony.
 *
 * @author Julia Bigaj
 */
public class SoundManager : MonoBehaviour
{
    /// @brief Instancja singletonu SoundManager.
    public static SoundManager Instance;

    [Header("Dźwięki otoczenia")]
    /// @brief Dźwięk otwierania drzwi.
    public AudioClip doorOpenSound;

    /// @brief Dźwięk otwierania skrzyni.
    public AudioClip chestOpenSound;

    /// @brief Dźwięk zniszczenia beczki.
    public AudioClip destroyBarrelSound;

    [Header("Bohater")]
    /// @brief Dźwięk lekkiego ataku.
    public AudioClip lightAttackSound;

    /// @brief Dźwięk ciężkiego ataku.
    public AudioClip heavyAttackSound;

    /// @brief Dźwięk skoku.
    public AudioClip jumpSound;

    /// @brief Dźwięk dashowania.
    public AudioClip dashSound;

    /// @brief Dźwięk otrzymania obrażeń.
    public AudioClip hurtSound;

    /// @brief Dźwięk kroków bohatera.
    public AudioClip stepSound;

    [Header("Łamigłówki")]
    /// @brief Dźwięk pociągnięcia za dźwignię.
    public AudioClip leverPullSound;

    /// @brief Dźwięk przesuwania kamienia.
    public AudioClip moveStoneSound;

    /// @brief Główne źródło dźwięków do jednorazowych efektów.
    private AudioSource audioSource;

    /// @brief Oddzielne źródło dźwięku do kroków (loopowane).
    private AudioSource stepSource;

    /**
     * @brief Inicjalizacja singletonu i źródeł dźwięku.
     */
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Główne źródło dźwięków
        audioSource = gameObject.AddComponent<AudioSource>();

        // Źródło kroków
        stepSource = gameObject.AddComponent<AudioSource>();
        stepSource.loop = true;
        stepSource.clip = stepSound;
    }

    /**
     * @brief Odtwarza pojedynczy dźwięk z podanego AudioClip.
     * @param clip Dźwięk do odtworzenia.
     */
    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
            audioSource.PlayOneShot(clip);
    }

    /**
     * @brief Rozpoczyna zapętlone odtwarzanie dźwięku kroków.
     */
    public void StartSteps()
    {
        if (stepSound != null && !stepSource.isPlaying)
            stepSource.Play();
    }

    /**
     * @brief Zatrzymuje zapętlony dźwięk kroków.
     */
    public void StopSteps()
    {
        if (stepSource.isPlaying)
            stepSource.Stop();
    }

    // --- Skrócone metody do efektów dźwiękowych ---

    /// @brief Odtwarza dźwięk kroku.
    public void PlayStep() => PlaySound(stepSound);

    /// @brief Odtwarza dźwięk otwierania skrzyni.
    public void PlayChest() => PlaySound(chestOpenSound);

    /// @brief Odtwarza dźwięk skoku.
    public void PlayJump() => PlaySound(jumpSound);

    /// @brief Odtwarza dźwięk dashowania.
    public void PlayDash() => PlaySound(dashSound);

    /// @brief Odtwarza dźwięk obrażeń.
    public void PlayHurt() => PlaySound(hurtSound);

    /// @brief Odtwarza dźwięk otwierania drzwi.
    public void PlayDoor() => PlaySound(doorOpenSound);

    /// @brief Odtwarza dźwięk zniszczenia beczki.
    public void PlayBarrel() => PlaySound(destroyBarrelSound);

    /// @brief Odtwarza dźwięk lekkiego ataku.
    public void PlayLightAttack() => PlaySound(lightAttackSound);

    /// @brief Odtwarza dźwięk ciężkiego ataku.
    public void PlayHeavyAttack() => PlaySound(heavyAttackSound);

    /// @brief Odtwarza dźwięk użycia dźwigni.
    public void PlayLever() => PlaySound(leverPullSound);

    /// @brief Odtwarza dźwięk przesuwania kamienia.
    public void PlayStone() => PlaySound(moveStoneSound);
}

