using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.Collections.Generic;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    public AudioSource audioSource;
    public AudioSource narrationSource;

    [System.Serializable]
    public class SceneMusicData
    {
        public string sceneGUID;
        public AudioClip music;
        [Range(0f, 1f)] public float volume = 1f;
    }

    public List<SceneMusicData> sceneMusicList = new List<SceneMusicData>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StopMusic();

        foreach (var entry in sceneMusicList)
        {
            string sceneName = GetSceneNameFromGUID(entry.sceneGUID);
            if (sceneName == scene.name)
            {
                audioSource.clip = entry.music;
                audioSource.volume = entry.volume;
                audioSource.loop = true;
                audioSource.Play();
                return;
            }
        }
    }

    public void StopMusic()
    {
        if (audioSource != null && audioSource.isPlaying)
            audioSource.Stop();

        if (narrationSource != null && narrationSource.isPlaying)
            narrationSource.Stop();
    }

    private string GetSceneNameFromGUID(string guid)
    {
#if UNITY_EDITOR
        string path = AssetDatabase.GUIDToAssetPath(guid);
        var scene = AssetDatabase.LoadAssetAtPath<SceneAsset>(path);
        return scene != null ? scene.name : null;
#else
        return null;
#endif
    }
}
