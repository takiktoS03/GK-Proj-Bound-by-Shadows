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
        DestroyedRegistry.Save();

        var save = new SceneSave();

        foreach (var so in GameObject.FindObjectsOfType<SaveableObject>())
        {
            // Jeśli obiekt jest oznaczony jako zniszczony, nie zapisujemy jego pozycji (bo i tak ma zniknąć)
            if (DestroyedRegistry.IsDestroyed(so.UniqueId))
                continue;

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

            var wrapper = new SerializationWrapper();
            wrapper.data = stateDict;

            string jsonState = JsonUtility.ToJson(wrapper);

            save.objects.Add(new ObjectSaveEntry
            {
                id = so.UniqueId,
                transform = transformData,
                customJson = jsonState
            });
        }

        File.WriteAllText(filePath, JsonUtility.ToJson(save, true));
    }

    public static void LoadCurrentScene()
    {
        DestroyedRegistry.Load();

        if (!File.Exists(filePath)) return;

        var json = File.ReadAllText(filePath);
        var save = JsonUtility.FromJson<SceneSave>(json);

        // Znajdź wszystkich SaveableObject na scenie
        foreach (var so in GameObject.FindObjectsOfType<SaveableObject>())
        {
            // 2. SPRAWDŹ CZY OBIEKT POWINIEN BYĆ MARTWY
            // Jeśli ID jest w rejestrze zniszczonych, natychmiast go wyłączamy i pomijamy resztę
            if (DestroyedRegistry.IsDestroyed(so.UniqueId))
            {
                so.gameObject.SetActive(false);
                continue;
            }

            var entry = save.objects.FirstOrDefault(e => e.id == so.UniqueId);
            if (entry == null) continue;

            // --- POPRAWKA FIZYKI ---
            var data = entry.transform;
            Vector3 targetPosition = new Vector3(data.posX, data.posY, data.posZ);
            Quaternion targetRotation = Quaternion.Euler(data.rotX, data.rotY, data.rotZ);

            var t = so.transform;
            t.position = targetPosition;
            t.rotation = targetRotation;
            so.gameObject.SetActive(data.isActive);

            Rigidbody2D rb = so.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.position = targetPosition;
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
                rb.Sleep();
            }
            Physics2D.SyncTransforms();

            var saveables = so.GetComponents<ISaveable>();
            if (!string.IsNullOrEmpty(entry.customJson))
            {
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
        }
    }

    public static void DeleteSave()
    {
        if (File.Exists(filePath))
            File.Delete(filePath);
    }
}

