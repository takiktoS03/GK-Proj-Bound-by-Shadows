using UnityEngine;
using UnityEngine.EventSystems;

/**
 * @class ButtonHoverSound
 * @brief Odtwarza d�wi�k najechania na przycisk w interfejsie u�ytkownika.
 *
 * Implementuje interfejs `IPointerEnterHandler`, aby reagowa� na zdarzenie najechania kursorem na element UI.
 * Przy wej�ciu kursora na przycisk odtwarzany jest przypisany d�wi�k typu "hover".
 * �r�d�o d�wi�ku (`AudioSource`) wyszukiwane jest dynamicznie w scenie przy starcie.
 *
 * @author Julia Bigaj
 */
public class ButtonHoverSound : MonoBehaviour, IPointerEnterHandler
{
    /// @brief D�wi�k odtwarzany przy najechaniu na przycisk.
    public AudioClip hoverSound;

    /// @brief Referencja do komponentu AudioSource, przez kt�ry d�wi�k zostanie odtworzony.
    private AudioSource audioSource;

    /**
     * @brief Inicjalizuje komponent � wyszukuje pierwszy aktywny AudioSource w scenie.
     */
    void Start()
    {
        // Szuka AudioSource w obiekcie nadrz�dnym (np. Menu lub MainMenu)
        audioSource = FindFirstObjectByType<AudioSource>();
    }

    /**
     * @brief Reaguje na najechanie kursorem na komponent UI � odtwarza d�wi�k.
     * @param eventData Dane dotycz�ce zdarzenia wska�nika (myszy).
     */
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hoverSound);
        }
    }
}
