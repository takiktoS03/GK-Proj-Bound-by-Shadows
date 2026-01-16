using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

/**
 * Skrypt nadający obiektom unikalny identyfikator, umożliwiający ich zapis
 * i poprawne odtworzenie w systemie zapisu gry.
 *
 * @author Julia Bigaj
 */

[DisallowMultipleComponent]
public class SaveableObject : MonoBehaviour
{
    /// @brief Unikalny identyfikator przypisany do obiektu.
    [SerializeField] private string uniqueId = Guid.NewGuid().ToString();

    /**
     * @brief Publiczny dostęp do unikalnego ID.
     * @return Niezmienny identyfikator GUID.
     */
    public string UniqueId => uniqueId;

    /**
     * @brief Gwarantuje, że obiekt zawsze ma przypisany identyfikator po załadowaniu.
     */
    private void Awake()
    {
        if (gameObject.CompareTag("Player"))
        {
            uniqueId = "Ethan-The-Hero-Unique-ID";
        }

        if (string.IsNullOrEmpty(uniqueId))
        {
            uniqueId = System.Guid.NewGuid().ToString();
        }
    }

#if UNITY_EDITOR
    /**
     * @brief Weryfikuje poprawność ID w edytorze Unity.
     *
     * Jeśli ID jest puste lub nieunikalne, przypisuje nowy `GUID` i oznacza obiekt jako zmodyfikowany.
     */
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(uniqueId) || !IsUnique(uniqueId))
        {
            uniqueId = Guid.NewGuid().ToString();
            UnityEditor.EditorUtility.SetDirty(this);
        }
    }

    /**
     * @brief Sprawdza, czy dany identyfikator jest unikalny w scenie.
     * @param candidate Kandydat na identyfikator.
     * @return `true` jeśli unikalny, `false` jeśli duplikat.
     */
    private bool IsUnique(string candidate)
    {
        var all = FindObjectsByType<SaveableObject>(FindObjectsSortMode.None);
        foreach (var so in all)
        {
            if (so == this) continue;
            if (so.uniqueId == candidate)
                return false;
        }
        return true;
    }
#endif
}

