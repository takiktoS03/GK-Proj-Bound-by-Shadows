using EthanTheHero;
using System.Collections;
using UnityEngine;

/**
 * @class EncounteredGhostDialog
 * @brief Obsługuje dialog pomiędzy graczem a napotkanym duszkiem.
 *
 * Wyświetla sekwencję tekstów z opcjonalnym dźwiękiem, zatrzymując ruch gracza.
 * Po zakończeniu dialogu jeden duszek znika, a inny (z kamerą) się pojawia.
 *
 * @author Filip Kudła
 */
public class EncounteredGhostDialog : MonoBehaviour
{
    [TextArea] public string dialog1 = "Kim jesteś?";
    [TextArea] public string dialog2 = "Kimś kto pomoże ci wydostać się z tego zamku. Tak się składa, że jestem jednym z mieszkańców";
    [TextArea] public string dialog3 = "Tak po prostu mi pomożesz? Nie mam nic co mógłbym ci dać w zamian";
    [TextArea] public string dialog4 = "Masz ale jeszcze o tym nie wiesz. Chodź, musimy sie spieszyć";

    [Header("Nagrania dialogów")]
    public AudioClip dialog1Audio;
    public AudioClip dialog2Audio;
    public AudioClip dialog3Audio;
    public AudioClip dialog4Audio;

    private AudioSource audioSource;

    /// @brief Duszek do ukrycia po zakończeniu dialogu.
    public GameObject encounteredGhost;

    /// @brief Duszek z kamerą do pokazania po dialogu.
    public GameObject ghostWithCam;

    private PlayerMovement movement;
    private PlayerAnimation animation;
    private PlayerAttackMethod attackMethod;
    private bool hasTriggered = false;

    /**
     * @brief Wywołuje dialog, gdy gracz wchodzi w strefę kolizji.
     * @param other Obiekt kolidujący (oczekiwany: Player).
     */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return; // zapobiega powtórnemu wywołaniu
        if (!other.CompareTag("Player")) return;

        audioSource = gameObject.AddComponent<AudioSource>();


        hasTriggered = true;
        SoundManager.Instance?.StopSteps();

        // Wyłącz ruch gracza
        movement = other.GetComponent<PlayerMovement>();
        if (movement != null)
            movement.enabled = false;

        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        animation = other.GetComponent<PlayerAnimation>();
        if (animation != null)
            animation.enabled = false;

        attackMethod = other.GetComponent<PlayerAttackMethod>();
        if(attackMethod != null)
            attackMethod.enabled = false;

        Animator playerAnim = other.GetComponent<Animator>();
        if (playerAnim != null)
        {
            playerAnim.SetFloat("Speed", 0f);
            playerAnim.SetBool("RunIdlePlayying", false);
            playerAnim.SetBool("Grounded", true);
            playerAnim.SetBool("Dashing", false);
            playerAnim.SetTrigger("NotAttacking");
        }

        // Rozpocznij dialog
        TextUI textUI = FindObjectOfType<TextUI>();
        if (textUI != null)
        {
            StartCoroutine(DialogSequence(textUI));
        }
    }

    /**
     * @brief Sekwencja dialogu — pokazuje kolejne linie tekstu i zarządza stanami.
     * @param textUI Obiekt UI odpowiedzialny za wyświetlanie tekstu.
     */
    private IEnumerator DialogSequence(TextUI textUI)
    {
        textUI.ShowTextDialog(dialog1, 2f);
        PlayVoice(dialog1Audio);
        yield return new WaitForSeconds(2f);

        textUI.ShowTextDialog(dialog2, 10f);
        PlayVoice(dialog2Audio);
        yield return new WaitForSeconds(10f);

        textUI.ShowTextDialog(dialog3, 6f);
        PlayVoice(dialog3Audio);
        yield return new WaitForSeconds(6f);

        textUI.ShowTextDialog(dialog4, 10f);
        PlayVoice(dialog4Audio);
        yield return new WaitForSeconds(10f);

        if (encounteredGhost != null)
            encounteredGhost.SetActive(false);

        if (ghostWithCam != null)
            ghostWithCam.SetActive(true);

        if (movement != null)
            movement.enabled = true;

        if (animation != null)
            animation.enabled = true;

        if (attackMethod != null)
            attackMethod.enabled = true;

        Destroy(gameObject);
    }

    /**
     * @brief Odtwarza dźwięk mowy (głosu) z podanego klipu audio.
     * 
     * Jeśli audioSource jest dostępny i klip nie jest pusty, zatrzymuje obecny dźwięk i odtwarza nowy.
     * 
     * @param clip Klip audio do odtworzenia.
     */
    private void PlayVoice(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(clip);
        }
    }

}

