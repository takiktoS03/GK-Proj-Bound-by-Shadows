using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

[DisallowMultipleComponent]
public class SaveableObject : MonoBehaviour
{
    [SerializeField] private string uniqueId = Guid.NewGuid().ToString();
    public string UniqueId => uniqueId;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(uniqueId) || !IsUnique(uniqueId))
        {
            uniqueId = Guid.NewGuid().ToString();
            UnityEditor.EditorUtility.SetDirty(this);
        }
    }

    private bool IsUnique(string candidate)
    {
        var all = FindObjectsByType<SaveableObject>(FindObjectsSortMode.None);
        foreach (var so in all)
        {
            if (so == this) continue;
            if (so.uniqueId == candidate)
                return false;
        }
        return true;
    }
#endif

    private void Awake()
    {
        if (string.IsNullOrEmpty(uniqueId))
            uniqueId = Guid.NewGuid().ToString();
    }
}

