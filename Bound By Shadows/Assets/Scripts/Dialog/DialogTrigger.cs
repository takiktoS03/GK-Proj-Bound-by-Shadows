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
    public bool skippable = true;
    public bool destroyAfter = true;

    [Header("Blocking Settings")]
    public bool blockMovement = true;
    public bool blockAnimation = true;
    public bool blockAttacks = true;
    public bool blockWallSliding = true;

    [Tooltip("Czy odblokować sterowanie natychmiast po zakończeniu dialogu?")]
    public bool restoreControlsAfter = true;

    [Header("References")]
    public Transform ghost;   // optional
    public Transform player;  // optional

    private PlayerControlManager controlManager;
    private KeyCode skipKey = KeyCode.Space;
    private bool skipPressed = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        SoundLibrary.Instance.StopSteps();

        controlManager = other.GetComponent<PlayerControlManager>();

        controlManager.LockControls(blockMovement, blockAnimation, blockAttacks, blockWallSliding);

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

        if (restoreControlsAfter)
        {
            controlManager.UnlockControls();
        }


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
                AudioManager.Instance.StopSFX();
                break;
            }

            timer += Time.deltaTime;
            yield return null;
        }
        yield return null;
    }
}
