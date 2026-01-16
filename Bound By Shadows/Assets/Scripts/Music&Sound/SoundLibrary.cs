using UnityEngine;

/**
 * Centralna biblioteka efektów dźwiękowych używanych w grze.
 * Udostępnia uproszczone metody do odtwarzania konkretnych dźwięków
 * związanych z postacią, otoczeniem oraz elementami rozgrywki.
 *
 * Klasa pełni rolę warstwy pośredniej pomiędzy logiką gry
 * a systemem AudioManager, porządkując dostęp do efektów dźwiękowych
 * i ułatwiając ich późniejszą rozbudowę lub modyfikację.
 *
 * @author Julia Bigaj
 */

public class SoundLibrary : MonoBehaviour
{
    /// @brief Instancja singletonu SoundLibrary.
    public static SoundLibrary Instance { get; private set; }

    [Header("Dźwięki otoczenia")]
    /// @brief Dźwięk otwierania drzwi.
    public AudioClip doorOpenSound;

    /// @brief Dźwięk otwierania skrzyni.
    public AudioClip chestOpenSound;

    /// @brief Dźwięk zniszczenia beczki.
    public AudioClip destroyBarrelSound;

    /// @brief Dźwięk przegranej gry.
    public AudioClip gameOverSound;

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

    [Header("Torch Puzzle")]
    public AudioClip torchLightSound;

    public AudioClip puzzleSolvedSound;


    /**
     * @brief Inicjalizacja singletonu.
     */
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // --- Skrócone metody do efektów dźwiękowych ---

    /// @brief Odtwarza dźwięk otwierania skrzyni.
    public void PlayChest() => AudioManager.Instance.PlaySFX(chestOpenSound);

    /// @brief Odtwarza dźwięk skoku.
    public void PlayJump() => AudioManager.Instance.PlaySFX(jumpSound);

    /// @brief Odtwarza dźwięk dashowania.
    public void PlayDash() => AudioManager.Instance.PlaySFX(dashSound);

    /// @brief Odtwarza dźwięk obrażeń.
    public void PlayHurt() => AudioManager.Instance.PlaySFX(hurtSound);

    /// @brief Odtwarza dźwięk otwierania drzwi.
    public void PlayDoor() => AudioManager.Instance.PlaySFX(doorOpenSound);

    /// @brief Odtwarza dźwięk zniszczenia beczki.
    public void PlayBarrel() => AudioManager.Instance.PlaySFX(destroyBarrelSound, 0.6f);

    /// @brief Odtwarza dźwięk lekkiego ataku.
    public void PlayLightAttack() => AudioManager.Instance.PlaySFX(lightAttackSound, 0.8f);

    /// @brief Odtwarza dźwięk ciężkiego ataku.
    public void PlayHeavyAttack() => AudioManager.Instance.PlaySFX(heavyAttackSound);

    /// @brief Odtwarza dźwięk użycia dźwigni.
    public void PlayLever() => AudioManager.Instance.PlaySFX(leverPullSound);

    /// @brief Odtwarza dźwięk przesuwania kamienia.
    public void PlayStone() => AudioManager.Instance.PlaySFX(moveStoneSound);

    /// @brief Odtwarza dźwięk zapalania pochodni.
    public void PlayTorch() => AudioManager.Instance.PlaySFX(torchLightSound, 0.7f);

    /// @brief Odtwarza dźwięk rozwiązania puzzli.
    public void PlayPuzzleSolved() => AudioManager.Instance.PlaySFX(puzzleSolvedSound);

    /// @brief Odtwarza dźwięk rozwiązania puzzli.
    public void PlayGameOver() => AudioManager.Instance.PlaySFX(gameOverSound);

    // Zapętlone (Kroki)
    public void StartSteps() => AudioManager.Instance.StartLoopingSFX(stepSound, 0.5f);
    public void StopSteps() => AudioManager.Instance.StopLoopingSFX();
}

