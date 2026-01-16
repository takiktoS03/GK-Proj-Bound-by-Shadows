using UnityEngine;

/**
 * Skrypt umożliwiający zapis i przywracanie poziomu zdrowia obiektów
 * korzystających z komponentu Health.
 *
 * @author Julia Bigaj
 */

public class HealthSaveData : MonoBehaviour, ISaveable
{
    [System.Serializable]
    private struct HealthData
    {
        public float hp;
    }

    private Health health;

    private void Awake()
    {
        health = GetComponentInChildren<Health>();
    }

    public object CaptureState()
    {
        if (health == null) return null;

        return new HealthData
        {
            hp = health.currentHealth
        };
    }

    public void RestoreState(object state)
    {
        if (health == null || state == null) return;

        string json = state as string;
        if (string.IsNullOrEmpty(json)) return;
        var data = JsonUtility.FromJson<HealthData>(json);

        health.SetHealth(data.hp);
    }
}
