using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("DŸwiêki")]
    public AudioClip stepSound;
    public AudioClip doorOpenSound;
    public AudioClip chestOpenSound;
    public AudioClip jumpSound;
    public AudioClip dashSound;

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

        // G³ówne Ÿród³o do pojedynczych dŸwiêków
        audioSource = gameObject.AddComponent<AudioSource>();

        // Osobne Ÿród³o do dŸwiêku kroków (loopowane)
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


    // Skróty:
    public void PlayStep() => PlaySound(stepSound);
    public void PlayDoor() => PlaySound(doorOpenSound);
    public void PlayChest() => PlaySound(chestOpenSound);
    public void PlayJump() => PlaySound(jumpSound);
    public void PlayDash() => PlaySound(dashSound);
}
