using UnityEngine;
using UnityEngine.EventSystems;

/**
 * Skrypt obsługujący dźwięk najechania kursorem na przyciski interfejsu użytkownika.
 * Reaguje na zdarzenie wejścia wskaźnika myszy na element UI
 * i odtwarza przypisany efekt dźwiękowy typu „hover”.
 *
 * Wykorzystywany w menu oraz interfejsach gry w celu poprawy
 * informacji zwrotnej dla użytkownika.
 *
 * @author Julia Bigaj
 */

public class ButtonHoverSound : MonoBehaviour, IPointerEnterHandler
{
    /// @brief Dźwięk odtwarzany przy najechaniu na przycisk.
    public AudioClip hoverSound;

    /// @brief Referencja do komponentu AudioSource, przez który dźwięk zostanie odtworzony.
    private AudioSource audioSource;

    /**
     * @brief Inicjalizuje komponent — wyszukuje pierwszy aktywny AudioSource w scenie.
     */
    void Start()
    {
        audioSource = FindFirstObjectByType<AudioSource>();
    }

    /**
     * @brief Reaguje na najechanie kursorem na komponent UI — odtwarza dźwięk.
     * @param eventData Dane dotyczące zdarzenia wskaźnika (myszy).
     */
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hoverSound);
        }
    }
}

