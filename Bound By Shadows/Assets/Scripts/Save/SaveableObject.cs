using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

/**
 * @class SaveableObject
 * @brief Przypisuje unikalny identyfikator (`UniqueId`) do obiektu gry, aby umo¿liwiæ jego zapis i odtworzenie.
 *
 * Klasa ta zapewnia, ¿e ka¿dy obiekt w scenie posiada unikalny identyfikator, który mo¿e byæ wykorzystany
 * w systemie zapisu stanu gry. Jeœli identyfikator nie jest unikalny, zostaje wygenerowany nowy.
 * Dzia³a tylko w edytorze Unity — podczas dzia³ania gry identyfikator nie jest zmieniany.
 *
 * @note Wspó³pracuje z interfejsem `ISaveable`.
 * @note `DisallowMultipleComponent` gwarantuje, ¿e komponent nie zostanie dodany wielokrotnie do jednego obiektu.
 *
 * @author Filip Kud³a
 */
[DisallowMultipleComponent]
public class SaveableObject : MonoBehaviour
{
    /// @brief Unikalny identyfikator przypisany do obiektu.
    [SerializeField] private string uniqueId = Guid.NewGuid().ToString();

    /**
     * @brief Publiczny dostêp do unikalnego ID.
     * @return Niezmienny identyfikator GUID.
     */
    public string UniqueId => uniqueId;

#if UNITY_EDITOR
    /**
     * @brief Weryfikuje poprawnoœæ ID w edytorze Unity.
     *
     * Jeœli ID jest puste lub nieunikalne, przypisuje nowy `GUID` i oznacza obiekt jako zmodyfikowany.
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
     * @return `true` jeœli unikalny, `false` jeœli duplikat.
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
     * @brief Gwarantuje, ¿e obiekt zawsze ma przypisany identyfikator po za³adowaniu.
     */
    private void Awake()
    {
        if (string.IsNullOrEmpty(uniqueId))
            uniqueId = Guid.NewGuid().ToString();
    }
}
