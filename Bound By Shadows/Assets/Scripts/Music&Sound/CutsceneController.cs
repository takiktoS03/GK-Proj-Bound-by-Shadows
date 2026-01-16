using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

/**
 * Skrypt odpowiedzialny za odtwarzanie sekwencji przerywników filmowych (cutscenek).
 * Zarządza wyświetlaniem slajdów, animacjami przejść (fade, zoom),
 * odtwarzaniem narracji dźwiękowej oraz napisów dialogowych.
 *
 * Umożliwia pomijanie cutscenek, automatyczne dopasowanie czasu slajdu
 * do długości nagrania audio oraz przejście do kolejnej sceny po zakończeniu sekwencji.
 * Integruje się z systemami dialogów, zapisu gry oraz zarządzania scenami.
 *
 * @author Filip Kudła
 */
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
    public string nextSceneName; 
    public Image displayImage;
    public float fadeDuration = 1f;
    public KeyCode skipKey = KeyCode.Space;
    public bool skippable = true;

    [Header("Content")]
    public List<CutsceneSlide> slides = new List<CutsceneSlide>();

    private void Start()
    {
        if (displayImage != null)
        {
            displayImage.color = new Color(1, 1, 1, 0);
            displayImage.enabled = true;
        }
        SoundLibrary.Instance.StopSteps();
        StartCoroutine(PlaySequence());
    }

    private IEnumerator PlaySequence()
    {
        foreach (var slide in slides)
        {
            // Konfiguracja obrazka
            displayImage.rectTransform.localScale = Vector3.one;
            if (slide.image != null)
                displayImage.sprite = slide.image;

            // Fade In
            yield return displayImage.DOFade(1f, fadeDuration).SetEase(Ease.Linear).WaitForCompletion();

            // Wyświetlenie obrazu z zoomem
            float waitTime = CalculateSlideDuration(slide);
            if (slide.enableZoom)
            {
                float zoomTime = waitTime + fadeDuration;
                displayImage.rectTransform.DOScale(slide.zoomScale, zoomTime).SetEase(Ease.InOutSine);
            }
            // Odegranie dźwięku ze slajdu
            if (slide.voice != null)
            {
                AudioManager.Instance.PlaySFX(slide.voice);
            }
            // Wyświetlenie tekstu z DialogManagera
            if (!string.IsNullOrEmpty(slide.subtitle))
            {
                DialogManager.Instance.Show(slide.subtitle, waitTime, DialogType.Cutscene);
            }
            yield return StartCoroutine(DialogManager.Instance.WaitOrSkip(waitTime, skippable, skipKey));
            DialogManager.Instance.Clear(DialogType.Cutscene);
            // Fade Out
            yield return displayImage.DOFade(0f, fadeDuration).SetEase(Ease.Linear).WaitForCompletion();
        }
        DialogManager.Instance.Clear(DialogType.Cutscene);
        SaveSystem.restorePlayerPositionOnLoad = true;
        SaveSystem.loadOnSceneStart = true;
        GameManager.Instance.LoadLevel(nextSceneName);
    }

    private float CalculateSlideDuration(CutsceneSlide slide)
    {
        if (slide.autoDurationByAudio && slide.voice != null)
        {
            return slide.voice.length;
        }
        return slide.duration;
    }
}