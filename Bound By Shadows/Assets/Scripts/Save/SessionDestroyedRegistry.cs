using System.Collections.Generic;


// Ta klasa trzyma informacje o zniszczonych obiektach TYLKO w trakcie dzia?ania gry.
// Przy zapisie (Save) dane st?d w?druj? do pliku.
// Przy wczytaniu (Load) dane z pliku w?druj? tutaj.
public static class SessionDestroyedRegistry
{
    private static Dictionary<string, HashSet<string>> destroyedMap = new Dictionary<string, HashSet<string>>();

    public static void MarkAsDestroyed(string sceneName, string id)
    {
        if (!destroyedMap.ContainsKey(sceneName))
            destroyedMap[sceneName] = new HashSet<string>();

        destroyedMap[sceneName].Add(id);
    }

    public static List<string> GetDestroyedIds(string sceneName)
    {
        if (destroyedMap.ContainsKey(sceneName))
            return new List<string>(destroyedMap[sceneName]);
        return new List<string>();
    }

    public static void SetDestroyedIds(string sceneName, List<string> ids)
    {
        destroyedMap[sceneName] = new HashSet<string>(ids);
    }

    public static void Clear()
    {
        destroyedMap.Clear();
    }
}