using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverSound : MonoBehaviour, IPointerEnterHandler
{
    public AudioClip hoverSound;
    private AudioSource audioSource;

    void Start()
    {
        // Szuka AudioSource w obiekcie nadrzêdnym (np. Menu lub MainMenu)
        audioSource = FindObjectOfType<AudioSource>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hoverSound);
        }
    }
}
