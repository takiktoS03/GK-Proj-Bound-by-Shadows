using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

/**
 * @class SaveableObject
 * @brief Przypisuje unikalny identyfikator (`UniqueId`) do obiektu gry, aby umo�liwi� jego zapis i odtworzenie.
 *
 * Klasa ta zapewnia, �e ka�dy obiekt w scenie posiada unikalny identyfikator, kt�ry mo�e by� wykorzystany
 * w systemie zapisu stanu gry. Je�li identyfikator nie jest unikalny, zostaje wygenerowany nowy.
 * Dzia�a tylko w edytorze Unity � podczas dzia�ania gry identyfikator nie jest zmieniany.
 *
 * @note Wsp�pracuje z interfejsem `ISaveable`.
 * @note `DisallowMultipleComponent` gwarantuje, �e komponent nie zostanie dodany wielokrotnie do jednego obiektu.
 *
 * @author Filip Kud�a
 */
[DisallowMultipleComponent]
public class SaveableObject : MonoBehaviour
{
    /// @brief Unikalny identyfikator przypisany do obiektu.
    [SerializeField] private string uniqueId = Guid.NewGuid().ToString();

    /**
     * @brief Publiczny dost�p do unikalnego ID.
     * @return Niezmienny identyfikator GUID.
     */
    public string UniqueId => uniqueId;

#if UNITY_EDITOR
    /**
     * @brief Weryfikuje poprawno�� ID w edytorze Unity.
     *
     * Je�li ID jest puste lub nieunikalne, przypisuje nowy `GUID` i oznacza obiekt jako zmodyfikowany.
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
     * @return `true` je�li unikalny, `false` je�li duplikat.
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
     * @brief Gwarantuje, �e obiekt zawsze ma przypisany identyfikator po za�adowaniu.
     */
    private void Awake()
    {
        if (string.IsNullOrEmpty(uniqueId))
            uniqueId = Guid.NewGuid().ToString();
    }
}
