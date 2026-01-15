using UnityEngine;

/**
 * @class PlayerSaveData
 * @brief Klasa odpowiedzialna za zapis i odczyt stanu gracza.
 *
 * Implementuje interfejs `ISaveable`, integrując się z globalnym systemem zapisu gry.
 * Pobiera i przywraca stan komponentu `PlayerHealth` przypisanego do gracza.
 *
 * @author Filip Kudła
 */
public class PlayerSaveData : MonoBehaviour, ISaveable
{
    [System.Serializable]
    private struct PlayerData
    {
        public float hp;
        public float stamina;
        // public float[] position; 
        // public int xp;
    }

    private PlayerHealth playerHealth;

    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();

    }

    public object CaptureState()
    {
        if (playerHealth == null) return null;

        return new PlayerData
        {
            hp = playerHealth.currentHealth,
            stamina = playerHealth.currentStamina
        };
    }

    public void RestoreState(object state)
    {
        if (playerHealth == null || state == null) return;

        var json = state as string;
        if (string.IsNullOrEmpty(json)) return;
        var data = JsonUtility.FromJson<PlayerData>(json);

        playerHealth.SetHealth(data.hp);
        playerHealth.SetStamina(data.stamina);
    }
}