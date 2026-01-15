using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private static readonly string filePath = Path.Combine(Application.persistentDataPath, "save.json");
    public static bool loadOnSceneStart = false;
    public static bool restorePlayerPositionOnLoad = false;

    // ================== STRUKTURY DANYCH ==================

    [System.Serializable]
    public class GameSaveData
    {
        public string lastSceneName;
        public ObjectSaveData globalPlayerData;
        public List<SceneSaveData> scenes = new List<SceneSaveData>();
    }

    [System.Serializable]
    public class SceneSaveData
    {
        public string sceneName;
        public List<string> destroyedObjectIds = new List<string>();
        public List<ObjectSaveData> activeObjects = new List<ObjectSaveData>();
    }

    [System.Serializable]
    public class ObjectSaveData
    {
        public string id;
        public Vector3 position;
        public Vector3 rotation; // Euler angles
        public string customJsonData; // Dane z ISaveable
    }

    [System.Serializable]
    private class SaveableDataWrapper
    {
        public List<string> keys = new List<string>();
        public List<string> values = new List<string>();
    }

    // ================== GŁÓWNE METODY ==================

    public static void SaveCurrentScene()
    {
        GameSaveData gameData = LoadFile();
        string currentSceneName = SceneManager.GetActiveScene().name;
        gameData.lastSceneName = currentSceneName;

        // Tworzenie danych dla aktualnej sceny
        SceneSaveData sceneData = gameData.scenes.FirstOrDefault(s => s.sceneName == currentSceneName);
        if (sceneData != null)
        {
            gameData.scenes.Remove(sceneData); // Usuwamy stare dane tej sceny
        }
        sceneData = new SceneSaveData { sceneName = currentSceneName };

        // Zbieranie wszystkich SaveableObject
        var allSaveables = Object.FindObjectsByType<SaveableObject>(FindObjectsSortMode.None);

        foreach (var so in allSaveables)
        {
            var objData = CreateObjectSaveData(so);
            if (so.CompareTag("Player"))
            {
                gameData.globalPlayerData = objData;
            }
            else
            {
                sceneData.activeObjects.Add(objData);
            }
        }

        // Zapis ID obiektów, które powinny być zniszczone
        sceneData.destroyedObjectIds = new List<string>(SessionDestroyedRegistry.GetDestroyedIds(currentSceneName));

        gameData.scenes.Add(sceneData);
        string json = JsonUtility.ToJson(gameData, true);
        File.WriteAllText(filePath, json);
    }

    public static void LoadCurrentScene()
    {
        if (!File.Exists(filePath)) return;

        GameSaveData gameData = LoadFile();
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (gameData.globalPlayerData != null)
        {
            // Szukamy gracza na scenie
            var playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null && playerObj.TryGetComponent<SaveableObject>(out var playerSo))
            {
                RestoreObjectState(playerSo, gameData.globalPlayerData, restorePlayerPositionOnLoad);
            }
        }

        SceneSaveData sceneData = gameData.scenes.FirstOrDefault(s => s.sceneName == currentSceneName);
        if (sceneData == null) return;

        // Rejestr sesji (żeby kolejne zapisy pamiętały co było zniszczone)
        SessionDestroyedRegistry.SetDestroyedIds(currentSceneName, sceneData.destroyedObjectIds);

        var allSaveables = Object.FindObjectsByType<SaveableObject>(FindObjectsSortMode.None);
        foreach (var so in allSaveables)
        {
            if (so.CompareTag("Player")) continue;

            if (sceneData.destroyedObjectIds.Contains(so.UniqueId))
            {
                Object.Destroy(so.gameObject);
                continue;
            }

            var savedObj = sceneData.activeObjects.FirstOrDefault(x => x.id == so.UniqueId);
            if (savedObj != null)
            {
                RestoreObjectState(so, savedObj, true);
            }
        }

        restorePlayerPositionOnLoad = false;
    }

    // ================== METODY POMOCNICZE ==================

    private static ObjectSaveData CreateObjectSaveData(SaveableObject so)
    {
        var objData = new ObjectSaveData
        {
            id = so.UniqueId,
            position = so.transform.position,
            rotation = so.transform.eulerAngles
        };

        var components = so.GetComponents<ISaveable>();
        var wrapper = new SaveableDataWrapper();
        foreach (var saveable in components)
        {
            var data = saveable.CaptureState();
            if (data != null)
            {
                wrapper.keys.Add(saveable.GetType().ToString());
                wrapper.values.Add(JsonUtility.ToJson(data));
            }
        }
        objData.customJsonData = JsonUtility.ToJson(wrapper);
        return objData;
    }

    /// <summary>
    /// Przywraca stan obiektu.
    /// </summary>
    /// <param name="so">Obiekt docelowy</param>
    /// <param name="data">Dane z pliku</param>
    /// <param name="restoreTransform">Czy przywracać pozycję i rotację?</param>
    private static void RestoreObjectState(SaveableObject so, ObjectSaveData data, bool restoreTransform)
    {
        if (restoreTransform)
        {
            so.transform.position = data.position;
            so.transform.eulerAngles = data.rotation;

            //if (so.TryGetComponent<Rigidbody2D>(out var rb))
            //{
            //    rb.linearVelocity = Vector2.zero;
            //    rb.angularVelocity = 0f;
            //    rb.position = data.position;
            //}
        }

        // Przywracanie ISaveable
        if (!string.IsNullOrEmpty(data.customJsonData))
        {
            var wrapper = JsonUtility.FromJson<SaveableDataWrapper>(data.customJsonData);
            var components = so.GetComponents<ISaveable>();

            for (int i = 0; i < wrapper.keys.Count; i++)
            {
                string typeName = wrapper.keys[i];
                string jsonState = wrapper.values[i];

                // Znajdź odpowiedni komponent i wczytaj
                var comp = components.FirstOrDefault(c => c.GetType().ToString() == typeName);
                if (comp != null) comp.RestoreState(jsonState);
            }
        }
    }

    private static GameSaveData LoadFile()
    {
        if (!File.Exists(filePath)) return new GameSaveData();
        return JsonUtility.FromJson<GameSaveData>(File.ReadAllText(filePath));
    }

    public static string GetLastSavedScene()
    {
        if (!File.Exists(filePath)) return "";

        GameSaveData data = LoadFile();
        return data.lastSceneName;
    }
}

