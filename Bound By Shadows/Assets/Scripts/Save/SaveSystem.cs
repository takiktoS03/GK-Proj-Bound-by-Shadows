using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Główny system odpowiedzialny za zapisywanie i wczytywanie stanu gry
 * oraz zarządzanie danymi obiektów i scen.
 *
 * @author Julia Bigaj
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
        public Vector3 rotation;
        public string customJsonData;
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

        SceneSaveData sceneData = gameData.scenes.FirstOrDefault(s => s.sceneName == currentSceneName);
        if (sceneData != null)
        {
            gameData.scenes.Remove(sceneData);
        }
        sceneData = new SceneSaveData { sceneName = currentSceneName };

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
            var playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null && playerObj.TryGetComponent<SaveableObject>(out var playerSo))
            {
                RestoreObjectState(playerSo, gameData.globalPlayerData, restorePlayerPositionOnLoad);
            }
        }

        SceneSaveData sceneData = gameData.scenes.FirstOrDefault(s => s.sceneName == currentSceneName);
        if (sceneData == null) return;

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

    private static void RestoreObjectState(SaveableObject so, ObjectSaveData data, bool restoreTransform)
    {
        if (restoreTransform)
        {
            so.transform.position = data.position;
            so.transform.eulerAngles = data.rotation;
        }

        if (!string.IsNullOrEmpty(data.customJsonData))
        {
            var wrapper = JsonUtility.FromJson<SaveableDataWrapper>(data.customJsonData);
            var components = so.GetComponents<ISaveable>();

            for (int i = 0; i < wrapper.keys.Count; i++)
            {
                string typeName = wrapper.keys[i];
                string jsonState = wrapper.values[i];

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

