using System;
using System.Collections.Generic;
using UnityEngine;

/**
 * @class SerializationWrapper
 * @brief Pomocnicza klasa do serializacji słownika `Dictionary<string, object>` w Unity.
 *
 * @details
 * Unity nie obsługuje bezpośrednio serializacji słowników i typów ogólnych (generics),
 * dlatego `SerializationWrapper` konwertuje dane do dwóch list: `keys` i `jsonValues`.
 *
 * Umożliwia bezpieczne zapisanie i odczytanie złożonych danych w systemie zapisu gry.
 * 
 * Zawiera implementację interfejsu `ISerializationCallbackReceiver` do obsługi procesów
 * serializacji i deserializacji w edytorze i czasie działania.
 *
 * @see SaveSystem
 *
 * @author Filip Kudła
 */
[Serializable]
public class SerializationWrapper : ISerializationCallbackReceiver
{
    /**
     * @brief Klucze (typów komponentów ISaveable) jako listy stringów.
     */
    public List<string> keys = new List<string>();

    /**
     * @brief Dane w postaci stringów (JSON) powiązane z każdym kluczem.
     */
    public List<string> jsonValues = new List<string>();

    /**
     * @brief Faktyczne dane zmapowane przez typy komponentów jako string (key) i obiekt (value).
     * @details To pole jest oznaczone jako [NonSerialized] i uzupełniane podczas `OnAfterDeserialize`.
     */
    [NonSerialized]
    public Dictionary<string, object> data = new Dictionary<string, object>();

    /**
     * @brief Metoda wywoływana automatycznie przed serializacją.
     *
     * @details Przekształca dane z `Dictionary<string, object>` do dwóch list: `keys` i `jsonValues`,
     * gdzie każdy obiekt serializowany jest jako JSON.
     */
    public void OnBeforeSerialize()
    {
        keys.Clear();
        jsonValues.Clear();

        foreach (var kvp in data)
        {
            keys.Add(kvp.Key);
            jsonValues.Add(JsonUtility.ToJson(kvp.Value));
        }
    }

    /**
     * @brief Metoda wywoływana automatycznie po deserializacji.
     *
     * @details Odtwarza strukturę słownika z danych zawartych w `keys` i `jsonValues`.
     * Wartości pozostają jako string (JSON) — konwersja do konkretnego typu musi być wykonana później ręcznie.
     */
    public void OnAfterDeserialize()
    {
        data = new Dictionary<string, object>();

        for (int i = 0; i < keys.Count; i++)
        {
            //var json = (string)wrapper.data[typeName];
            //var restoredObject = JsonUtility.FromJson<T>(json);

            data[keys[i]] = jsonValues[i];  // Dane są nadal jako string (JSON), wymagają deserializacji przez użytkownika
        }
    }
}

