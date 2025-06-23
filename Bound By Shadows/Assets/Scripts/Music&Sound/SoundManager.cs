using UnityEngine;

/* Centralny mened�er efekt�w d�wi�kowych w grze.
   - Zawiera skr�cone metody do odtwarzania konkretnych d�wi�k�w (atak, beczka, skrzynia, itp.).
   - Rozdziela d�wi�ki krok�w i inne efekty na osobne �r�d�a AudioSource.
   - Singleton dzia�aj�cy globalnie.

   Autor: Julia Bigaj
*/

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Dzwieki otoczenia")]
    public AudioClip doorOpenSound;
    public AudioClip chestOpenSound;
    public AudioClip destroyBarrelSound;

    [Header("Bohater")]
    public AudioClip lightAttackSound;
    public AudioClip heavyAttackSound;
    public AudioClip jumpSound;
    public AudioClip dashSound;
    public AudioClip hurtSound;
    public AudioClip stepSound;

    [Header("Lamiglowki")]
    public AudioClip leverPullSound;
    public AudioClip moveStoneSound;

    private AudioSource audioSource;
    private AudioSource stepSource;

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

        // Gl�wne do pojedynczych dzwiek�w
        audioSource = gameObject.AddComponent<AudioSource>();

        // Osobne do dzwieku krok�w (loopowane)
        stepSource = gameObject.AddComponent<AudioSource>();
        stepSource.loop = true;
        stepSource.clip = stepSound;
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
            audioSource.PlayOneShot(clip);
    }

    public void StartSteps()
    {
        if (stepSound != null && !stepSource.isPlaying)
            stepSource.Play();
    }

    public void StopSteps()
    {
        if (stepSource.isPlaying)
            stepSource.Stop();
    }

    // Skr�ty:
    public void PlayStep() => PlaySound(stepSound);
    public void PlayChest() => PlaySound(chestOpenSound);
    public void PlayJump() => PlaySound(jumpSound);
    public void PlayDash() => PlaySound(dashSound);
    public void PlayHurt() => PlaySound(hurtSound);
    public void PlayDoor() => PlaySound(doorOpenSound);
    public void PlayBarrel() => PlaySound(destroyBarrelSound);
    public void PlayLightAttack() => PlaySound(lightAttackSound);
    public void PlayHeavyAttack() => PlaySound(heavyAttackSound);
    public void PlayLever() => PlaySound(leverPullSound);
    public void PlayStone() => PlaySound(moveStoneSound);
}
