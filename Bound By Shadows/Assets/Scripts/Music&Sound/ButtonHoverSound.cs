using UnityEngine;
using UnityEngine.EventSystems;

/* Odtwarza dŸwiêk najechania na przycisk UI.
   - Wykrywa zdarzenie `OnPointerEnter` i odtwarza przypisany dŸwiêk hover.
   - Wyszukuje AudioSource w scenie dynamicznie.

   Autor: Julia Bigaj
*/

public class ButtonHoverSound : MonoBehaviour, IPointerEnterHandler
{
    public AudioClip hoverSound;
    private AudioSource audioSource;

    void Start()
    {
        // Szuka AudioSource w obiekcie nadrzêdnym (np. Menu lub MainMenu)
        audioSource = FindFirstObjectByType<AudioSource>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hoverSound);
        }
    }
}
