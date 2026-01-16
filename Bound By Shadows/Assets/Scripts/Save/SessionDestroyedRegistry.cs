using System.Collections.Generic;

/**
 * Klasa przechowująca informacje o zniszczonych obiektach w trakcie jednej sesji gry,
 * wykorzystywana przez system zapisu.
 *
 * @author Julia Bigaj
 */

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