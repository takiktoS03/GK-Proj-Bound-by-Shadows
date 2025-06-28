using UnityEngine;
using UnityEngine.EventSystems;

/**
 * @class ButtonHoverSound
 * @brief Odtwarza dźwięk najechania na przycisk w interfejsie użytkownika.
 *
 * Implementuje interfejs `IPointerEnterHandler`, aby reagować na zdarzenie najechania kursorem na element UI.
 * Przy wejściu kursora na przycisk odtwarzany jest przypisany dźwięk typu "hover".
 * Źródło dźwięku (`AudioSource`) wyszukiwane jest dynamicznie w scenie przy starcie.
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
        // Szuka AudioSource w obiekcie nadrzędnym (np. Menu lub MainMenu)
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

