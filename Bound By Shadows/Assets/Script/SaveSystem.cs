using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class SaveSystem
{
    private static readonly string filePath =
        Path.Combine(Application.persistentDataPath, "save.json");

    [System.Serializable]
    private class SceneSave
    {
        public List<ObjectSaveEntry> objects = new List<ObjectSaveEntry>();
    }

    [System.Serializable]
    private class ObjectSaveData
    {
        public string id;
        public float posX, posY, posZ;
        public float rotX, rotY, rotZ;
    }

    [System.Serializable]
    private class ObjectSaveEntry
    {
        public string id;
        public ObjectSaveData transformData;
    }


    public static void SaveCurrentScene()
    {
        var save = new SceneSave();

        foreach (var so in GameObject.FindObjectsOfType<SaveableObject>())
        {
            var saveables = so.GetComponents<ISaveable>();
            foreach (var saveable in saveables)
            {
                var state = saveable.CaptureState();
                if (state != null)
                {
                    save.objects.Add(new ObjectSaveEntry
                    {
                        id = so.UniqueId,
                        transformData = state as ObjectSaveData
                    });
                }
            }
        }

        var json = JsonUtility.ToJson(save, true);
        File.WriteAllText(filePath, json);
        Debug.Log($"[SAVE] Zapisano grê: {filePath}");
    }

    public static void LoadCurrentScene()
    {

        Debug.Log("[LOAD] LoadCurrentScene uruchomione");

        if (!File.Exists(filePath)) return;

        var json = File.ReadAllText(filePath);
        var save = JsonUtility.FromJson<SceneSave>(json);

        foreach (var saveable in GameObject.FindObjectsOfType<MonoBehaviour>().OfType<ISaveable>())
        {
            var component = (MonoBehaviour)saveable;
            var guid = component.GetComponent<SaveableObject>()?.UniqueId;
            if (string.IsNullOrEmpty(guid)) continue;

            var entry = save.objects.Find(e => e.id == guid);
            if (entry != null)
            {
                Debug.Log($"[LOAD] Znaleziono dane dla {guid}");
                saveable.RestoreState(entry.transformData);
            }
            else
            {
                Debug.LogWarning($"[LOAD] NIE ZNALEZIONO danych dla {guid}");
            }
        }
    }
}
