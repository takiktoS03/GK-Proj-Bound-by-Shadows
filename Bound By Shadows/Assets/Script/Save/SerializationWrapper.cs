using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializationWrapper : ISerializationCallbackReceiver
{
    public List<string> keys = new List<string>();
    public List<string> jsonValues = new List<string>();

    [NonSerialized]
    public Dictionary<string, object> data = new Dictionary<string, object>();

    public void OnBeforeSerialize()
    {
        keys.Clear();
        jsonValues.Clear();

        foreach (var kvp in data)
        {
            keys.Add(kvp.Key);
            jsonValues.Add(JsonUtility.ToJson(kvp.Value));
        }
    }

    public void OnAfterDeserialize()
    {
        data = new Dictionary<string, object>();

        for (int i = 0; i < keys.Count; i++)
        {
            data[keys[i]] = jsonValues[i];
        }
    }
}
