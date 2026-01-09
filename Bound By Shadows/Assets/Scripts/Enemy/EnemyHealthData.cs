using UnityEngine;

public class EnemyHealthSaveData : MonoBehaviour, ISaveable
{
    [System.Serializable]
    private class EnemyHealthData
    {
        public float hp;
        public bool dead;
    }

    private Health health;
    private FlyingEnemy flying;

    private void Awake()
    {
        health = GetComponentInChildren<Health>(true);
        flying = GetComponent<FlyingEnemy>();
    }

    public object CaptureState()
    {
        return new EnemyHealthData
        {
            hp = health != null ? health.CurrentHealth : 0,
            dead = health == null || health.CurrentHealth <= 0
        };
    }

    public void RestoreState(object state)
    {
        string json = state as string;
        var data = JsonUtility.FromJson<EnemyHealthData>(json);

        if (data.dead || data.hp <= 0)
        {
            gameObject.SetActive(false);
            return;
        }

        if (health != null)
        {
            health.SetBarsValue(data.hp);
            health.Revive();
        }

        if (flying != null)
        {
            flying.OnGameLoaded();
        }

        var patrol = GetComponent<PatrolEnemy>();
        if (patrol != null)
        {
            patrol.OnGameLoaded();
        }

    }
}
