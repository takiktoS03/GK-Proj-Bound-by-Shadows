using DG.Tweening;
using EthanTheHero;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogTrigger : MonoBehaviour
{
    [System.Serializable]
    public class DialogLine
    {
        [Header("Speaker Info")]
        public string speakerName;
        public Sprite portrait;

        [Header("Content")]
        [TextArea] public string text;
        public float duration = 3f;
        public AudioClip voice;
    }

    [Header("Dialog content")]
    public List<DialogLine> lines = new List<DialogLine>();

    [Header("Options")]
    public DialogType dialogType = DialogType.MainDialog;
    public bool skippable = true;
    public bool destroyAfter = true;
    public KeyCode skipKey = KeyCode.Space;

    [Header("Blocking Settings")]
    public bool blockMovement = true;
    public bool blockAnimation = true;
    public bool blockAttacks = true;
    public bool blockWallSliding = true;
    [Tooltip("Czy odblokować sterowanie natychmiast po zakończeniu dialogu?")]
    public bool restoreControlsAfter = true;

    [Header("Events")]
    public UnityEvent onDialogEnd;

    private PlayerControlManager controlManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        SoundLibrary.Instance.StopSteps();
        controlManager = other.GetComponent<PlayerControlManager>();
        controlManager.LockControls(blockMovement, blockWallSliding, blockAttacks, blockAnimation);
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
            DialogManager.Instance.Show(line, dialogType);
            AudioManager.Instance.PlaySFX(line.voice);
            yield return StartCoroutine(DialogManager.Instance.WaitOrSkip(line.duration, skippable, skipKey));
        }

        DialogManager.Instance.Clear(dialogType);
        if (onDialogEnd != null)
        {
            onDialogEnd.Invoke();
        }
        if (restoreControlsAfter) controlManager.UnlockControls();
        if (destroyAfter) Destroy(gameObject);
    }
}
