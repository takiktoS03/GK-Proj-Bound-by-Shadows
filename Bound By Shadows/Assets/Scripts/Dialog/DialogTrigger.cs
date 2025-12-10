using DG.Tweening;
using EthanTheHero;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    [System.Serializable]
    public class DialogLine
    {
        [TextArea] public string text;
        public float duration = 3f;
        public AudioClip voice;
    }

    [Header("Dialog content")]
    public List<DialogLine> lines = new List<DialogLine>();

    [Header("Options")]
    public int textUIType;
    public bool blockPlayerMovement = false;
    public bool blockPlayerAnimation = false;
    public bool blockWallSliding = false;
    public bool skippable = true;
    public bool destroyAfter = true;

    [Header("References")]
    public Transform ghost;   // optional
    public Transform player;  // optional

    private PlayerMovement movement;
    private PlayerAnimation anim;
    private PlayerAttackMethod attackMethod;

    private KeyCode skipKey = KeyCode.Space;
    private bool skipPressed = false;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        SoundManager.Instance?.StopSteps();

        // block movement?
        movement = other.GetComponent<PlayerMovement>();
        if (blockPlayerMovement)
        {
            movement.enabled = false;
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null) rb.linearVelocity = Vector2.zero;
            attackMethod = other.GetComponent<PlayerAttackMethod>();
            attackMethod.enabled = false;
        }

        if (blockWallSliding)
        {
            movement.wallSlidingEnabled = false;
        }

        if (blockPlayerAnimation)
        {
            anim = other.GetComponent<PlayerAnimation>();
            anim.enabled = false;
        }
        Animator playerAnim = other.GetComponent<Animator>();
        if (playerAnim != null)
        {
            playerAnim.Play("Idle");
            playerAnim.SetFloat("Speed", 0f);
            playerAnim.SetBool("RunIdlePlayying", false);
            playerAnim.SetBool("Grounded", true);
            playerAnim.SetBool("Dashing", false);
            playerAnim.SetTrigger("NotAttacking");
        }
        StartCoroutine(DialogSequence());
    }

    /**
     * @brief Sekwencja dialogu — pokazuje kolejne linie tekstu i zarządza stanami.
     * Przywraca ruch gracza i niszczy obiekt zależnie od ustawień
     */
    private IEnumerator DialogSequence()
    {
        foreach (var line in lines)
        {
            DialogManager.Instance.Show(line.text, line.duration, textUIType);
            AudioManager.Instance.PlaySFX(line.voice);

            yield return StartCoroutine(WaitOrSkip(line.duration));
        }

        // restore movement
        if (blockPlayerMovement && movement != null) movement.enabled = true;
        if (blockPlayerAnimation && anim != null) anim.enabled = true;
        if (blockPlayerMovement && attackMethod != null) attackMethod.enabled = true;
        if (ghost != null)
        {
            ghost.GetComponent<GhostMovement>().player = player;
        }

        if (destroyAfter)
            Destroy(gameObject);
    }

    /**
     * @brief Pozwala na pominięcie dialogu za pomocą klawisza, lub normalne odtworzenie
     * 
     * @param time Czas pojedynczego dialogu
     */
    private IEnumerator WaitOrSkip(float t)
    {
        skipPressed = false;
        float timer = 0f;

        while (timer < t)
        {
            if (skippable && !skipPressed && Input.GetKeyDown(skipKey))
            {
                skipPressed = true;
                DialogManager.Instance.Clear();
                break;
            }

            timer += Time.deltaTime;
            yield return null;
        }
        yield return null;
    }
}
