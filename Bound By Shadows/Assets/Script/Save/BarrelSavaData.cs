using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;

public class BarrelSaveData : MonoBehaviour, ISaveable
{
    private static HashSet<string> destroyedBarrels = new HashSet<string>();

    public static void RegisterDestroyedBarrel(string uniqueId)
    {
        destroyedBarrels.Add(uniqueId);
    }

    private void Awake()
    {
        var allBarrels = FindObjectsByType<Barrel>(FindObjectsSortMode.None);

        foreach (var barrel in allBarrels)
        {
            var saveable = barrel.GetComponent<SaveableObject>();
            if (saveable != null && destroyedBarrels.Contains(saveable.UniqueId))
            {
                Destroy(barrel.gameObject);
            }
        }
    }

    public object CaptureState()
    {
        return destroyedBarrels.ToList();
    }

    public void RestoreState(object state)
    {
        var list = state as List<string>;
        destroyedBarrels = new HashSet<string>(list);
    }
}
