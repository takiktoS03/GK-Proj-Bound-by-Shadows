using System.Collections.Generic;
using UnityEngine;

public static class DestroyedRegistry
{
    private const string Key = "DestroyedIds";

    private static HashSet<string> destroyed = new();

    public static void MarkDestroyed(string id)
    {
        if (string.IsNullOrEmpty(id)) return;
        destroyed.Add(id);
    }

    public static bool IsDestroyed(string id)
    {
        return !string.IsNullOrEmpty(id) && destroyed.Contains(id);
    }

    public static void Clear()
    {
        destroyed.Clear();
        PlayerPrefs.DeleteKey(Key);
    }

    public static void Save()
    {
        PlayerPrefs.SetString(Key, string.Join("|", destroyed));
        PlayerPrefs.Save();
    }

    public static void Load()
    {
        destroyed.Clear();
        var s = PlayerPrefs.GetString(Key, "");
        if (string.IsNullOrEmpty(s)) return;

        foreach (var id in s.Split('|'))
            if (!string.IsNullOrEmpty(id))
                destroyed.Add(id);
    }
}
