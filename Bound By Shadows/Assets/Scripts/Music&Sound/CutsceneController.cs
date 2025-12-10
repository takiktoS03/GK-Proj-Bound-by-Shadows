using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

public class CutsceneController : MonoBehaviour
{
    [System.Serializable]
    public class CutsceneSlide
    {
        public Sprite image;
        [TextArea] public string subtitle; // Opcjonalnie napisy
        public AudioClip voiceOver;        // Dźwięk/Dialog
        public float duration = 5f;        // Czas trwania (jeśli brak audio)
        public bool autoDurationByAudio = true; // Czy czas ma zależeć od długości audio?
    }

    [Header("Configuration")]
    public string nextSceneName;   // Scena do załadowania po cutscence
    public Image backgroundImage;     // UI Image na Canvasie w tej scenie
    public Text subtitleText;      // Opcjonalne UI Text
    public AudioSource audioSource;
    public float crossfadeDuration = 1f; // Czas przenikania między obrazkami

    [Header("Content")]
    public List<CutsceneSlide> slides = new List<CutsceneSlide>();

    private void Start()
    {
        // Upewnij się, że zaczynamy z czystym stanem
        if (backgroundImage != null)
        {
            backgroundImage.color = Color.black;
            backgroundImage.enabled = true;
        }

        StartCoroutine(PlaySequence());
    }

    private IEnumerator PlaySequence()
    {
        // Ukryj muzykę z tła (jeśli chcesz, by cutscenka miała swoje audio)
        // Możesz tu wyciszyć AudioManager.Instance

        foreach (var slide in slides)
        {
            // 1. Ustawienie zasobów
            if (slide.image != null)
            {
                // Jeśli obrazek ma się zmienić, zróbmy to płynnie
                // (Tutaj uproszczona wersja: Fade Out -> Zmiana -> Fade In)
                // Dla pełnego Crossfade (A znika, B się pojawia) potrzebne byłyby 2 obrazy UI

                // Fade out starego
                if (backgroundImage.sprite != null)
                    yield return backgroundImage.DOFade(0f, 0.5f).WaitForCompletion();

                backgroundImage.sprite = slide.image;

                // Fade in nowego
                yield return backgroundImage.DOFade(1f, 1f).WaitForCompletion();
            }

            if (subtitleText != null) subtitleText.text = slide.subtitle;

            // 2. Odtwarzanie dźwięku
            if (slide.voiceOver != null)
            {
                audioSource.clip = slide.voiceOver;
                audioSource.Play();
            }

            // 3. Czekanie
            float waitTime = slide.duration;
            if (slide.autoDurationByAudio && slide.voiceOver != null)
            {
                waitTime = slide.voiceOver.length;
            }

            yield return new WaitForSeconds(waitTime);
        }

        // Koniec cutscenki - Fade Out całości
        if (backgroundImage != null)
            yield return backgroundImage.DOFade(0f, 1f).WaitForCompletion();

        GameManager.Instance.LoadLevel(nextSceneName);
    }
}