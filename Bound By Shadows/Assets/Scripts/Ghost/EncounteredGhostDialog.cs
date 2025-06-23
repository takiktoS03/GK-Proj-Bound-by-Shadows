using EthanTheHero;
using System.Collections;
using UnityEngine;


public class EncounteredGhostDialog : MonoBehaviour
{
    [TextArea] public string dialog1 = "Kim jesteś?";
    [TextArea] public string dialog2 = "Kimś kto pomoże ci wydostać się z tego zamku. Tak się składa, że jestem jednym z mieszkańców";
    [TextArea] public string dialog3 = "Tak po prostu mi pomożesz? Nie mam nic co mógłbym ci dać w zamian";
    [TextArea] public string dialog4 = "Masz ale jeszcze o tym nie wiesz. Chodź, musimy sie spieszyć";

    public GameObject encounteredGhost; // Duszek, który znika
    public GameObject ghostWithCam;     // Duszek, który pojawi się po dialogu

    private PlayerMovement movement;
    private PlayerAnimation animation;
    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return; // zapobiega powtórnemu wywołaniu
        if (!other.CompareTag("Player")) return;

        hasTriggered = true;

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

        Animator playerAnim = other.GetComponent<Animator>();
        if (playerAnim != null)
        {
            playerAnim.SetFloat("Speed", 0f);
            playerAnim.SetBool("RunIdlePlayying", false);
            playerAnim.SetBool("Grounded", true);
            playerAnim.SetTrigger("NotAttacking");
        }

        // Rozpocznij dialog
        TextUI textUI = FindObjectOfType<TextUI>();
        if (textUI != null)
        {
            StartCoroutine(DialogSequence(textUI));
        }
    }

    private IEnumerator DialogSequence(TextUI textUI)
    {
        textUI.ShowTextDialog(dialog1, 3f);
        yield return new WaitForSeconds(3f);

        textUI.ShowTextDialog(dialog2, 3f);
        yield return new WaitForSeconds(3f);

        textUI.ShowTextDialog(dialog3, 3f);
        yield return new WaitForSeconds(3f);

        textUI.ShowTextDialog(dialog4, 3f);
        yield return new WaitForSeconds(3f);

        if (encounteredGhost != null)
            encounteredGhost.SetActive(false);

        if (ghostWithCam != null)
            ghostWithCam.SetActive(true);

        if (movement != null)
            movement.enabled = true;

        if (animation != null)
            animation.enabled = true;

        Destroy(gameObject);
    }
}
