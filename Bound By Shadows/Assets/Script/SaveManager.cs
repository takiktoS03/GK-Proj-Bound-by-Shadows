using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    public GameObject player;
    private string path;

    private List<string> destroyedBarrels = new List<string>();

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

        path = Application.persistentDataPath + "/save.dat";
    }

    public void AddDestroyedBarrel(string id)
    {
        if (!destroyedBarrels.Contains(id))
            destroyedBarrels.Add(id);
    }

    public bool IsBarrelDestroyed(string id)
    {
        return destroyedBarrels.Contains(id);
    }

    public void SaveGame()
    {
        SaveData data = new SaveData();
        data.playerX = player.transform.position.x;
        data.playerY = player.transform.position.y;
        data.playerZ = player.transform.position.z;
        data.sceneName = SceneManager.GetActiveScene().name;

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(path);
        formatter.Serialize(file, data);
        file.Close();

        Debug.Log("Gra zapisana");
    }

    public void LoadGame()
    {
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);
            SaveData data = (SaveData)formatter.Deserialize(file);
            file.Close();

            SceneManager.LoadScene(data.sceneName);
            PlayerPrefs.SetFloat("loadX", data.playerX);
            PlayerPrefs.SetFloat("loadY", data.playerY);
            PlayerPrefs.SetFloat("loadZ", data.playerZ);
            PlayerPrefs.SetInt("shouldLoad", 1);
        }
        else
        {
            Debug.LogWarning("Nie znaleziono zapisu gry");
        }
        Time.timeScale = 1f;
        PauseMenu.isPaused = false;
    }
}