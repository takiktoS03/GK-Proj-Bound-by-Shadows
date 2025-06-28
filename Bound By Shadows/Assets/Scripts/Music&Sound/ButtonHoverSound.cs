using UnityEngine;
using UnityEngine.EventSystems;

/**
 * @class ButtonHoverSound
 * @brief Odtwarza düwiÍk najechania na przycisk w interfejsie uøytkownika.
 *
 * Implementuje interfejs `IPointerEnterHandler`, aby reagowaÊ na zdarzenie najechania kursorem na element UI.
 * Przy wejúciu kursora na przycisk odtwarzany jest przypisany düwiÍk typu "hover".
 * èrÛd≥o düwiÍku (`AudioSource`) wyszukiwane jest dynamicznie w scenie przy starcie.
 *
 * @author Julia Bigaj
 */
public class ButtonHoverSound : MonoBehaviour, IPointerEnterHandler
{
    /// @brief DüwiÍk odtwarzany przy najechaniu na przycisk.
    public AudioClip hoverSound;

    /// @brief Referencja do komponentu AudioSource, przez ktÛry düwiÍk zostanie odtworzony.
    private AudioSource audioSource;

    /**
     * @brief Inicjalizuje komponent ó wyszukuje pierwszy aktywny AudioSource w scenie.
     */
    void Start()
    {
        // Szuka AudioSource w obiekcie nadrzÍdnym (np. Menu lub MainMenu)
        audioSource = FindFirstObjectByType<AudioSource>();
    }

    /**
     * @brief Reaguje na najechanie kursorem na komponent UI ó odtwarza düwiÍk.
     * @param eventData Dane dotyczπce zdarzenia wskaünika (myszy).
     */
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hoverSound);
        }
    }
}
