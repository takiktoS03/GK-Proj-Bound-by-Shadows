using UnityEngine;
using UnityEngine.EventSystems;

/* Odtwarza d�wi�k najechania na przycisk UI.
   - Wykrywa zdarzenie `OnPointerEnter` i odtwarza przypisany d�wi�k hover.
   - Wyszukuje AudioSource w scenie dynamicznie.

   Autor: Julia Bigaj
*/

public class ButtonHoverSound : MonoBehaviour, IPointerEnterHandler
{
    public AudioClip hoverSound;
    private AudioSource audioSource;

    void Start()
    {
        // Szuka AudioSource w obiekcie nadrz�dnym (np. Menu lub MainMenu)
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
