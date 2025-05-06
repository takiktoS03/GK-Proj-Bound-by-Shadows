using UnityEngine;

[DisallowMultipleComponent]
public class UniqueID : MonoBehaviour
{
    [SerializeField] private string uniqueID = string.Empty; // Do not touch in Inspector
    public string ID => uniqueID;

    private void Awake()
    {
        // Generate once in Editor/at runtime if missing
        if (string.IsNullOrEmpty(uniqueID))
        {
            uniqueID = System.Guid.NewGuid().ToString();
#if UNITY_EDITOR
            if (!Application.isPlaying)
                UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
    }
}
