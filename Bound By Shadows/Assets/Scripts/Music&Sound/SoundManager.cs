using UnityEngine;

/**
 * @class SoundManager
 * @brief Centralny mened¿er efektów dŸwiêkowych w grze.
 *
 * Implementuje wzorzec singletonu. Udostêpnia metody do odtwarzania konkretnych efektów dŸwiêkowych,
 * takich jak ataki, otwieranie skrzyni, dŸwiêki ³amig³ówek czy kroki bohatera.
 * Kroki obs³ugiwane s¹ osobnym Ÿród³em dŸwiêku (`stepSource`) jako dŸwiêk zapêtlony.
 *
 * @author Julia Bigaj
 */
public class SoundManager : MonoBehaviour
{
    /// @brief Instancja singletonu SoundManager.
    public static SoundManager Instance;

    [Header("DŸwiêki otoczenia")]
    /// @brief DŸwiêk otwierania drzwi.
    public AudioClip doorOpenSound;

    /// @brief DŸwiêk otwierania skrzyni.
    public AudioClip chestOpenSound;

    /// @brief DŸwiêk zniszczenia beczki.
    public AudioClip destroyBarrelSound;

    [Header("Bohater")]
    /// @brief DŸwiêk lekkiego ataku.
    public AudioClip lightAttackSound;

    /// @brief DŸwiêk ciê¿kiego ataku.
    public AudioClip heavyAttackSound;

    /// @brief DŸwiêk skoku.
    public AudioClip jumpSound;

    /// @brief DŸwiêk dashowania.
    public AudioClip dashSound;

    /// @brief DŸwiêk otrzymania obra¿eñ.
    public AudioClip hurtSound;

    /// @brief DŸwiêk kroków bohatera.
    public AudioClip stepSound;

    [Header("£amig³ówki")]
    /// @brief DŸwiêk poci¹gniêcia za dŸwigniê.
    public AudioClip leverPullSound;

    /// @brief DŸwiêk przesuwania kamienia.
    public AudioClip moveStoneSound;

    /// @brief G³ówne Ÿród³o dŸwiêków do jednorazowych efektów.
    private AudioSource audioSource;

    /// @brief Oddzielne Ÿród³o dŸwiêku do kroków (loopowane).
    private AudioSource stepSource;

    /**
     * @brief Inicjalizacja singletonu i Ÿróde³ dŸwiêku.
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

        // G³ówne Ÿród³o dŸwiêków
        audioSource = gameObject.AddComponent<AudioSource>();

        // ród³o kroków
        stepSource = gameObject.AddComponent<AudioSource>();
        stepSource.loop = true;
        stepSource.clip = stepSound;
    }

    /**
     * @brief Odtwarza pojedynczy dŸwiêk z podanego AudioClip.
     * @param clip DŸwiêk do odtworzenia.
     */
    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
            audioSource.PlayOneShot(clip);
    }

    /**
     * @brief Rozpoczyna zapêtlone odtwarzanie dŸwiêku kroków.
     */
    public void StartSteps()
    {
        if (stepSound != null && !stepSource.isPlaying)
            stepSource.Play();
    }

    /**
     * @brief Zatrzymuje zapêtlony dŸwiêk kroków.
     */
    public void StopSteps()
    {
        if (stepSource.isPlaying)
            stepSource.Stop();
    }

    // --- Skrócone metody do efektów dŸwiêkowych ---

    /// @brief Odtwarza dŸwiêk kroku.
    public void PlayStep() => PlaySound(stepSound);

    /// @brief Odtwarza dŸwiêk otwierania skrzyni.
    public void PlayChest() => PlaySound(chestOpenSound);

    /// @brief Odtwarza dŸwiêk skoku.
    public void PlayJump() => PlaySound(jumpSound);

    /// @brief Odtwarza dŸwiêk dashowania.
    public void PlayDash() => PlaySound(dashSound);

    /// @brief Odtwarza dŸwiêk obra¿eñ.
    public void PlayHurt() => PlaySound(hurtSound);

    /// @brief Odtwarza dŸwiêk otwierania drzwi.
    public void PlayDoor() => PlaySound(doorOpenSound);

    /// @brief Odtwarza dŸwiêk zniszczenia beczki.
    public void PlayBarrel() => PlaySound(destroyBarrelSound);

    /// @brief Odtwarza dŸwiêk lekkiego ataku.
    public void PlayLightAttack() => PlaySound(lightAttackSound);

    /// @brief Odtwarza dŸwiêk ciê¿kiego ataku.
    public void PlayHeavyAttack() => PlaySound(heavyAttackSound);

    /// @brief Odtwarza dŸwiêk u¿ycia dŸwigni.
    public void PlayLever() => PlaySound(leverPullSound);

    /// @brief Odtwarza dŸwiêk przesuwania kamienia.
    public void PlayStone() => PlaySound(moveStoneSound);
}
