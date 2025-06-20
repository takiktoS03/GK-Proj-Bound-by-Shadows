using UnityEngine;


[System.Serializable]
public class PlayerData
{
    public float hp;
    //public int level;
}

public class PlayerSaveData : MonoBehaviour, ISaveable
{
    public object CaptureState()
    {
        var health = GetComponent<Health>();
        return new PlayerData
        {
            //hp = health.currentHealth,
            //level = this.level  id sceny np.
        };
    }

    public void RestoreState(object state)
    {
        Debug.Log("[RESTORE] RestoreState zostalo wywolane");
        string json = state as string;
        var data = JsonUtility.FromJson<PlayerData>(json);
        var health = GetComponent<Health>();
        health.SetBarsValue(data.hp);
        //this.level = data.level;
    }
}