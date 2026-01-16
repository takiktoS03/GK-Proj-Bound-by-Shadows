using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;
/**
 * Skrypt zarządzający wyświetlaniem dialogów w grze, obsługujący różne typy wiadomości
 * oraz efekt pisania tekstu (typewriter).
 *
 * @author Filip Kudła
 */

/**
 * 0 – dialogi fabularne NPC (dół ekranu)
 * 1 – myśli gracza (nad głową)
 * 2 - komunikaty UI do skrzynek/drzwi itp.
 * 3 - Podpowiedzi
 * 4 - dialogu przerywników fabularnych
 */
public enum DialogType
{
    MainDialog = 0,
    Thoughts = 1,
    SystemInfo = 2,
    Hint = 3,
    Cutscene = 4
}

/**
 * @class DialogManager
 * @brief Wyświetla tymczasowe wiadomości dialogowe na ekranie w komponentach TextMeshProUGUI.
 *
 * Umożliwia prezentację tekstu w jednym z wielu okien dialogowych (np. powiadomień, opisów, wskazówek).
 * Tekst znika automatycznie po określonym czasie.
 *
 * @author Filip Kudła
 */
public class DialogManager : MonoBehaviour
{
    /// @brief Singleton systemu dialogów
    public static DialogManager Instance;

    [System.Serializable]
    public struct DialogChannel
    {
        public DialogType type;
        public TextMeshProUGUI text;
    }

    [Header("UI References")]
    public GameObject dialogPanel;
    public GameObject portraitFrame;
    public Image portraitImage;
    public TextMeshProUGUI nameText;
    public List<DialogChannel> channels = new List<DialogChannel>();

    [Header("Settings")]
    public float defaultTypingSpeed = 0.1f;

    public bool IsTyping { get; private set; }
    private Coroutine activeTypingRoutine;
    private string currentFullText;
    private TextMeshProUGUI activeTextBox;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (dialogPanel != null) dialogPanel.SetActive(false);
        ClearAll();
    }

    /// <summary>
    /// Wyświetla prosty tekst (bez portretu i imienia). Jeśli duration > 0, dostosowywuje prędkość pisania
    /// </summary>
    /// <param name="text">Treść wiadomości</param>
    /// <param name="duration">Czas trwania</param>
    /// <param name="type">Na którym kanale wyświetlić</param>
    public void Show(string text, float duration, DialogType type)
    {
        InternalShow(text, duration, null, null, type);
    }

    /// <summary>
    /// Wyświetla pełny dialog z danymi (portret, imię).
    /// </summary>
    /// <param name="lineData">Dane dialogu z imieniem, portretem, treścią i czasem trwania</param>
    /// <param name="type">Na którym kanale wyświetlić</param>
    public void Show(DialogTrigger.DialogLine lineData, DialogType type)
    {
        InternalShow(lineData.text, lineData.duration, lineData.portrait, lineData.speakerName, type);
    }

    private void InternalShow(string text, float duration, Sprite portrait, string speakerName, DialogType type)
    {
        TextMeshProUGUI targetBox = GetBoxByType(type);
        if (targetBox == null) return;

        StopTyping();

        float typingSpeed = defaultTypingSpeed;

        // Obliczenie prędkości pisania aby zakończyła się w 70% czasu dialogu
        if (duration > 0 && text.Length > 0)
        {
            float typingDuration = duration * 0.7f;
            typingSpeed = typingDuration / text.Length;
            typingSpeed = Mathf.Clamp(typingSpeed, 0.01f, defaultTypingSpeed);
        }

        if (type == DialogType.MainDialog)
        {
            if (dialogPanel) dialogPanel.SetActive(true);

            if (portraitFrame != null)
            {
                portraitFrame.SetActive(portrait != null);
                portraitImage.sprite = portrait;
            }
            if (nameText != null)
            {
                nameText.text = string.IsNullOrEmpty(speakerName) ? "" : speakerName;
            }
            activeTypingRoutine = StartCoroutine(TypewriterRoutine(targetBox, text, typingSpeed));
        }
        else if (type == DialogType.Cutscene)
        {
            activeTypingRoutine = StartCoroutine(TypewriterRoutine(targetBox, text, typingSpeed));
        }
        else
        {
            targetBox.text = text;
        }
    }

    /// <summary>
    /// Korutyna "Maszyny do pisania"
    /// </summary>
    private IEnumerator TypewriterRoutine(TextMeshProUGUI tmp, string textToType, float typingSpeed)
    {
        IsTyping = true;
        currentFullText = textToType;
        activeTextBox = tmp;

        tmp.text = "";

        foreach (char letter in textToType.ToCharArray())
        {
            tmp.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        IsTyping = false;
        activeTypingRoutine = null;
    }


    /// <summary>
    /// Pozwala na pominięcie dialogu za pomocą klawisza, lub normalne odtworzenie
    /// </summary>
    public IEnumerator WaitOrSkip(float t, bool skippable, KeyCode skipKey)
    {
        float timer = 0f;

        while (timer < t)
        {
            if (Input.GetKeyDown(skipKey))
            {
                if (IsTyping)
                {
                    CompleteTypingInstant();
                }
                else if (skippable)
                {
                    AudioManager.Instance.StopSFX();
                    break;
                }
            }

            timer += Time.deltaTime;
            yield return null;
        }
        yield return null;
    }

    /// <summary>
    /// Wywoływane, gdy gracz wciśnie spację w trakcie pisania.
    /// Natychmiast kończy pisanie i pokazuje cały tekst.
    /// </summary>
    public void CompleteTypingInstant()
    {
        if (IsTyping && activeTextBox != null)
        {
            StopCoroutine(activeTypingRoutine);
            activeTextBox.text = currentFullText;
            IsTyping = false;
            activeTypingRoutine = null;
        }
    }

    // Zatrzymuje korutynę bez kończenia tekstu (używane przy czyszczeniu)
    private void StopTyping()
    {
        if (activeTypingRoutine != null) StopCoroutine(activeTypingRoutine);
        IsTyping = false;
        activeTypingRoutine = null;
    }

    public void Clear(DialogType type)
    {
        if (type == DialogType.MainDialog) dialogPanel.SetActive(false);
        GetBoxByType(type).text = "";
    }

    public void ClearAll()
    {
        StopTyping();
        if (dialogPanel) dialogPanel.SetActive(false);

        foreach (var channel in channels)
        {
            if (channel.text != null) channel.text.text = "";
        }
    }

    // Pomocnicza funkcja do szukania enuma w liście
    private TextMeshProUGUI GetBoxByType(DialogType type)
    {
        foreach (var channel in channels)
        {
            if (channel.type == type) return channel.text;
        }
        return null;
    }
}

