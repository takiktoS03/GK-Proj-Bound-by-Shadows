using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    private string SavePath => Path.Combine(Application.persistentDataPath, "save.json");

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #region Public API
    public void SaveGame()
    {
        var saveFile = new SaveFile();
        foreach (var mono in FindObjectsOfType<MonoBehaviour>(true))
        {
            if (mono is ISaveable saveable)
            {
                var id = mono.GetComponent<UniqueID>()?.ID;
                if (string.IsNullOrEmpty(id))
                {
                    Debug.LogWarning($"[SaveManager] Object {mono.name} implements ISaveable but has no UniqueID component.");
                    continue;
                }
                saveFile.entries.Add(new SaveEntry { id = id, json = saveable.CaptureState() });
            }
        }
        File.WriteAllText(SavePath, JsonUtility.ToJson(saveFile, prettyPrint: true));
        PlayerPrefs.SetString("LastScene", UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        Debug.Log($"[SaveManager] Game saved to: {SavePath}");
    }

    public void LoadGame()
    {
        if (!File.Exists(SavePath))
        {
            Debug.LogWarning("[SaveManager] No save file found!");
            return;
        }

        var saveFile = JsonUtility.FromJson<SaveFile>(File.ReadAllText(SavePath));
        var lookup = new Dictionary<string, string>();
        foreach (var entry in saveFile.entries) lookup[entry.id] = entry.json;

        foreach (var mono in FindObjectsOfType<MonoBehaviour>(true))
        {
            if (mono is ISaveable saveable)
            {
                var id = mono.GetComponent<UniqueID>()?.ID;
                if (string.IsNullOrEmpty(id)) continue;
                if (lookup.TryGetValue(id, out var json))
                    saveable.RestoreState(json);
            }
        }
        Debug.Log("[SaveManager] Game loaded");
    }
    #endregion

    #region Internal data structures
    [System.Serializable]
    private class SaveEntry
    {
        public string id;
        public string json;
    }

    [System.Serializable]
    private class SaveFile
    {
        public List<SaveEntry> entries = new();
    }
    #endregion
}
