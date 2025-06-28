using UnityEngine;

/**
 * @class SoundManager
 * @brief Centralny mened�er efekt�w d�wi�kowych w grze.
 *
 * Implementuje wzorzec singletonu. Udost�pnia metody do odtwarzania konkretnych efekt�w d�wi�kowych,
 * takich jak ataki, otwieranie skrzyni, d�wi�ki �amig��wek czy kroki bohatera.
 * Kroki obs�ugiwane s� osobnym �r�d�em d�wi�ku (`stepSource`) jako d�wi�k zap�tlony.
 *
 * @author Julia Bigaj
 */
public class SoundManager : MonoBehaviour
{
    /// @brief Instancja singletonu SoundManager.
    public static SoundManager Instance;

    [Header("D�wi�ki otoczenia")]
    /// @brief D�wi�k otwierania drzwi.
    public AudioClip doorOpenSound;

    /// @brief D�wi�k otwierania skrzyni.
    public AudioClip chestOpenSound;

    /// @brief D�wi�k zniszczenia beczki.
    public AudioClip destroyBarrelSound;

    [Header("Bohater")]
    /// @brief D�wi�k lekkiego ataku.
    public AudioClip lightAttackSound;

    /// @brief D�wi�k ci�kiego ataku.
    public AudioClip heavyAttackSound;

    /// @brief D�wi�k skoku.
    public AudioClip jumpSound;

    /// @brief D�wi�k dashowania.
    public AudioClip dashSound;

    /// @brief D�wi�k otrzymania obra�e�.
    public AudioClip hurtSound;

    /// @brief D�wi�k krok�w bohatera.
    public AudioClip stepSound;

    [Header("�amig��wki")]
    /// @brief D�wi�k poci�gni�cia za d�wigni�.
    public AudioClip leverPullSound;

    /// @brief D�wi�k przesuwania kamienia.
    public AudioClip moveStoneSound;

    /// @brief G��wne �r�d�o d�wi�k�w do jednorazowych efekt�w.
    private AudioSource audioSource;

    /// @brief Oddzielne �r�d�o d�wi�ku do krok�w (loopowane).
    private AudioSource stepSource;

    /**
     * @brief Inicjalizacja singletonu i �r�de� d�wi�ku.
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

        // G��wne �r�d�o d�wi�k�w
        audioSource = gameObject.AddComponent<AudioSource>();

        // �r�d�o krok�w
        stepSource = gameObject.AddComponent<AudioSource>();
        stepSource.loop = true;
        stepSource.clip = stepSound;
    }

    /**
     * @brief Odtwarza pojedynczy d�wi�k z podanego AudioClip.
     * @param clip D�wi�k do odtworzenia.
     */
    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
            audioSource.PlayOneShot(clip);
    }

    /**
     * @brief Rozpoczyna zap�tlone odtwarzanie d�wi�ku krok�w.
     */
    public void StartSteps()
    {
        if (stepSound != null && !stepSource.isPlaying)
            stepSource.Play();
    }

    /**
     * @brief Zatrzymuje zap�tlony d�wi�k krok�w.
     */
    public void StopSteps()
    {
        if (stepSource.isPlaying)
            stepSource.Stop();
    }

    // --- Skr�cone metody do efekt�w d�wi�kowych ---

    /// @brief Odtwarza d�wi�k kroku.
    public void PlayStep() => PlaySound(stepSound);

    /// @brief Odtwarza d�wi�k otwierania skrzyni.
    public void PlayChest() => PlaySound(chestOpenSound);

    /// @brief Odtwarza d�wi�k skoku.
    public void PlayJump() => PlaySound(jumpSound);

    /// @brief Odtwarza d�wi�k dashowania.
    public void PlayDash() => PlaySound(dashSound);

    /// @brief Odtwarza d�wi�k obra�e�.
    public void PlayHurt() => PlaySound(hurtSound);

    /// @brief Odtwarza d�wi�k otwierania drzwi.
    public void PlayDoor() => PlaySound(doorOpenSound);

    /// @brief Odtwarza d�wi�k zniszczenia beczki.
    public void PlayBarrel() => PlaySound(destroyBarrelSound);

    /// @brief Odtwarza d�wi�k lekkiego ataku.
    public void PlayLightAttack() => PlaySound(lightAttackSound);

    /// @brief Odtwarza d�wi�k ci�kiego ataku.
    public void PlayHeavyAttack() => PlaySound(heavyAttackSound);

    /// @brief Odtwarza d�wi�k u�ycia d�wigni.
    public void PlayLever() => PlaySound(leverPullSound);

    /// @brief Odtwarza d�wi�k przesuwania kamienia.
    public void PlayStone() => PlaySound(moveStoneSound);
}
