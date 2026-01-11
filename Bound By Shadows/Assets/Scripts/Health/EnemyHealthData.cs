using UnityEngine;

public class EnemyHealthSaveData : MonoBehaviour, ISaveable
{
    [System.Serializable]
    private class EnemyHealthData
    {
        public float hp;
    }

    private Health health;
    private SaveableObject saveable;

    private void Awake()
    {
        health = GetComponentInChildren<Health>(true);
        saveable = GetComponent<SaveableObject>();
    }

    public object CaptureState()
    {
        if (health == null) return null;

        if (saveable != null && DestroyedRegistry.IsDestroyed(saveable.UniqueId))
            return null;

        return new EnemyHealthData
        {
            hp = health.CurrentHealth
        };
    }

    public void RestoreState(object state)
    {
        if (health == null || state == null) return;

        if (saveable != null && DestroyedRegistry.IsDestroyed(saveable.UniqueId))
            return;

        string json = state as string;
        var data = JsonUtility.FromJson<EnemyHealthData>(json);

        if (data.hp <= 0)
            return;

        health.SetBarsValue(Mathf.Max(1, data.hp));
        health.Revive();

    }
}
