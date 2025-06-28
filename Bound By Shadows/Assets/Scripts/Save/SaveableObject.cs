using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

/**
 * @class SaveableObject
 * @brief Przypisuje unikalny identyfikator (`UniqueId`) do obiektu gry, aby umożliwić jego zapis i odtworzenie.
 *
 * Klasa ta zapewnia, że każdy obiekt w scenie posiada unikalny identyfikator, który może być wykorzystany
 * w systemie zapisu stanu gry. Jeśli identyfikator nie jest unikalny, zostaje wygenerowany nowy.
 * Działa tylko w edytorze Unity — podczas działania gry identyfikator nie jest zmieniany.
 *
 * @note Współpracuje z interfejsem `ISaveable`.
 * @note `DisallowMultipleComponent` gwarantuje, że komponent nie zostanie dodany wielokrotnie do jednego obiektu.
 *
 * @author Filip Kudła
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

    /**
     * @brief Gwarantuje, że obiekt zawsze ma przypisany identyfikator po załadowaniu.
     */
    private void Awake()
    {
        if (string.IsNullOrEmpty(uniqueId))
            uniqueId = Guid.NewGuid().ToString();
    }
}

