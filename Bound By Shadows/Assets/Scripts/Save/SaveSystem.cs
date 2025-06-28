using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/**
 * @class SaveSystem
 * @brief Statyczna klasa odpowiedzialna za zapis i odczyt stanu sceny w grze.
 *
 * System opiera się na serializacji danych transformacji (pozycja, rotacja, aktywność)
 * oraz na interfejsie `ISaveable`, dzięki któremu można przechować i przywrócić dowolne dane komponentów.
 *
 * @details Zapis przechowywany jest w formacie JSON w lokalizacji `Application.persistentDataPath/save.json`.
 *
 * @author Filip Kudła
 */

public static class SaveSystem
{
    /// @brief Pełna ścieżka do pliku zapisu.
    private static readonly string filePath = Path.Combine(Application.persistentDataPath, "save.json");

    // ====================== KLASY DANYCH ======================

    /**
     * @class SaveEntry
     * @brief (Nieużywana) Struktura zapisu jednego obiektu z danymi jako JSON.
     */
    [System.Serializable]
    public class SaveEntry
    {
        public string id;
        public string jsonData;
        public string type;
    }

    /**
     * @class SaveData
     * @brief (Nieużywana) Lista wpisów `SaveEntry` — używana w alternatywnym podejściu.
     */
    [System.Serializable]
    public class SaveData
    {
        public List<SaveEntry> entries = new List<SaveEntry>();
    }

    /**
     * @class SceneSave
     * @brief Główna struktura przechowująca dane wszystkich zapisywanych obiektów sceny.
     */
    [System.Serializable]
    private class SceneSave
    {
        public List<ObjectSaveEntry> objects = new List<ObjectSaveEntry>();
    }

    /**
     * @class ObjectSaveData
     * @brief Zapis pozycji, rotacji i stanu aktywności obiektu.
     */
    [System.Serializable]
    private class ObjectSaveData
    {
        public float posX, posY, posZ;
        public float rotX, rotY, rotZ;
        public bool isActive;
    }

    /**
     * @class ObjectSaveEntry
     * @brief Zapis jednego obiektu sceny.
     * @details Zawiera ID, dane transformacji i serializowane dane komponentów `ISaveable`.
     */
    [System.Serializable]
    private class ObjectSaveEntry
    {
        public string id;
        public ObjectSaveData transform;
        public string customJson;
    }

    // ====================== METODY GŁÓWNE ======================

    /**
     * @brief Zapisuje stan aktualnie załadowanej sceny do pliku.
     *
     * @details
     * - Zbiera wszystkie `SaveableObject` w scenie.
     * - Dla każdego zapamiętuje: pozycję, rotację, aktywność oraz dane komponentów `ISaveable`.
     * - Zapisuje wynik do pliku w formacie JSON.
     */
    public static void SaveCurrentScene()
    {
        var save = new SceneSave();

        foreach (var so in GameObject.FindObjectsByType<SaveableObject>(FindObjectsSortMode.None))
        {
            var t = so.transform;

            var transformData = new ObjectSaveData
            {
                posX = t.position.x,
                posY = t.position.y,
                posZ = t.position.z,
                rotX = t.eulerAngles.x,
                rotY = t.eulerAngles.y,
                rotZ = t.eulerAngles.z,
                isActive = so.gameObject.activeSelf
            };

            // Serializacja komponentów ISaveable
            var saveables = so.GetComponents<ISaveable>();
            Dictionary<string, object> stateDict = new();
            foreach (var s in saveables)
            {
                var state = s.CaptureState();
                if (state != null)
                    stateDict[s.GetType().ToString()] = state;
            }

            string jsonState = JsonUtility.ToJson(new SerializationWrapper());

            save.objects.Add(new ObjectSaveEntry
            {
                id = so.UniqueId,
                transform = transformData,
                customJson = jsonState
            });
        }

        File.WriteAllText(filePath, JsonUtility.ToJson(save, true));
        Debug.Log("[SAVE] Zapisano grę.");
    }

    /**
     * @brief Wczytuje stan sceny z pliku zapisu.
     *
     * @details
     * - Wczytuje plik JSON.
     * - Dla każdego `SaveableObject` na scenie odtwarza:
     *   - pozycję, rotację, aktywność,
     *   - stan komponentów `ISaveable` z użyciem `RestoreState`.
     */
    public static void LoadCurrentScene()
    {
        if (!File.Exists(filePath)) return;

        var json = File.ReadAllText(filePath);
        var save = JsonUtility.FromJson<SceneSave>(json);

        foreach (var so in GameObject.FindObjectsByType<SaveableObject>(FindObjectsSortMode.None))
        {
            var entry = save.objects.FirstOrDefault(e => e.id == so.UniqueId);
            if (entry == null) continue;

            // Odtwarzanie transformacji
            var t = so.transform;
            var data = entry.transform;
            t.position = new Vector3(data.posX, data.posY, data.posZ);
            t.eulerAngles = new Vector3(data.rotX, data.rotY, data.rotZ);
            so.gameObject.SetActive(data.isActive);

            // Odtwarzanie komponentów ISaveable
            var saveables = so.GetComponents<ISaveable>();
            var wrapper = JsonUtility.FromJson<SerializationWrapper>(entry.customJson);

            foreach (var s in saveables)
            {
                var key = s.GetType().ToString();
                if (wrapper.data.ContainsKey(key))
                {
                    s.RestoreState(wrapper.data[key]);
                }
            }
        }

        Debug.Log("[LOAD] Wczytano grę.");
    }
}
