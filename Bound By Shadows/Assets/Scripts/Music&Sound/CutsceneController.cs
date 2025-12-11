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
        [Header("Visuals")]
        public Sprite image;

        [Header("Audio & Text")]
        [TextArea] public string subtitle;
        public AudioClip voice;

        [Header("Timing")]
        public float duration = 5f;
        public bool autoDurationByAudio = true;

        [Header("Zoom Effect")]
        public bool enableZoom = false;
        public float zoomScale = 1.2f;
    }

    [Header("Configuration")]
    public string nextSceneName;   // Scena do załadowania po cutscence
    public Image displayImage;     // UI Image na Canvasie
    public float fadeDuration = 1f; // Czas przenikania między obrazkami

    private AudioSource voiceSource;

    [Header("Content")]
    public List<CutsceneSlide> slides = new List<CutsceneSlide>();

    private void Awake()
    {
        voiceSource = GetComponent<AudioSource>();
        voiceSource.playOnAwake = false;
        voiceSource.loop = false;
    }

    private void Start()
    {
        if (displayImage != null)
        {
            displayImage.color = new Color(1, 1, 1, 0);
            displayImage.enabled = true;
        }

        StartCoroutine(PlaySequence());
    }

    private IEnumerator PlaySequence()
    {
        foreach (var slide in slides)
        {
            // Konfiguracja
            displayImage.rectTransform.localScale = Vector3.one;
            if (slide.image != null)
                displayImage.sprite = slide.image;            

            if (!string.IsNullOrEmpty(slide.subtitle))
            {
                DialogManager.Instance.Show(slide.subtitle, slide.duration);
            }

            // Wyświetlenie obrazu i dźwięku
            yield return displayImage.DOFade(1f, fadeDuration).SetEase(Ease.Linear).WaitForCompletion();
            float waitTime = CalculateSlideDuration(slide);
            if (slide.enableZoom)
            {
                float zoomTime = waitTime + fadeDuration;
                displayImage.rectTransform.DOScale(slide.zoomScale, zoomTime).SetEase(Ease.InOutSine);
            }
            if (slide.voice != null)
            {
                AudioManager.Instance.PlaySFX(slide.voice);
            }
            yield return new WaitForSeconds(waitTime);


            yield return displayImage.DOFade(0f, fadeDuration).SetEase(Ease.Linear).WaitForCompletion();
        }

        GameManager.Instance.LoadLevel(nextSceneName);
    }

    // Pomocnicza funkcja do liczenia czasu
    private float CalculateSlideDuration(CutsceneSlide slide)
    {
        if (slide.autoDurationByAudio && slide.voice != null)
        {
            return slide.voice.length;
        }
        return slide.duration;
    }
}