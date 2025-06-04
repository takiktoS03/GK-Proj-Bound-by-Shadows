using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class SaveSystem
{
    private static readonly string filePath =
        Path.Combine(Application.persistentDataPath, "save.json");

    [System.Serializable]
    public class SaveEntry
    {
        public string id;
        public string jsonData;
        public string type;
    }

    [System.Serializable]
    public class SaveData
    {
        public List<SaveEntry> entries = new List<SaveEntry>();
    }

    [System.Serializable]
    private class SceneSave
    {
        public List<ObjectSaveEntry> objects = new List<ObjectSaveEntry>();
    }

    [System.Serializable]
    private class ObjectSaveData
    {
        public float posX, posY, posZ;
        public float rotX, rotY, rotZ;
        public bool isActive;
    }


    [System.Serializable]
    private class ObjectSaveEntry
    {
        public string id;
        public ObjectSaveData transform;
        public string customJson;
    }


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

            // Pobieranie niestandardowych danych
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


    public static void LoadCurrentScene()
    {
        if (!File.Exists(filePath)) return;

        var json = File.ReadAllText(filePath);
        var save = JsonUtility.FromJson<SceneSave>(json);

        foreach (var so in GameObject.FindObjectsByType<SaveableObject>(FindObjectsSortMode.None))
        {
            var entry = save.objects.FirstOrDefault(e => e.id == so.UniqueId);
            if (entry == null) continue;

            // Przywracanie transformacji
            var t = so.transform;
            var data = entry.transform;
            t.position = new Vector3(data.posX, data.posY, data.posZ);
            t.eulerAngles = new Vector3(data.rotX, data.rotY, data.rotZ);
            so.gameObject.SetActive(data.isActive);

            // Przywracanie komponentów
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
