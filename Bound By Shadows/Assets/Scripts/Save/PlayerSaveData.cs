using UnityEngine;

/**
 * @struct PlayerData
 * @brief Struktura danych zawierająca informacje do zapisu o graczu.
 *
 * Aktualnie przechowuje jedynie ilość punktów życia gracza (`hp`), ale można ją łatwo rozszerzyć
 * o inne dane (np. poziom, pozycję, ekwipunek).
 * 
 * @author Filip Kudła
 */
[System.Serializable]
public class PlayerData
{
    /// @brief Aktualny poziom zdrowia gracza.
    public float hp;

    // Można rozszerzyć:
    // public int level;
}


/**
 * @class PlayerSaveData
 * @brief Klasa odpowiedzialna za zapis i odczyt stanu gracza.
 *
 * Implementuje interfejs `ISaveable`, integrując się z globalnym systemem zapisu gry.
 * Pobiera i przywraca stan komponentu `Health` przypisanego do gracza.
 *
 * @note Wymaga obecności komponentu `Health` w tym samym obiekcie.
 *
 * @author Filip Kudła
 */
public class PlayerSaveData : MonoBehaviour, ISaveable
{
    /**
     * @brief Tworzy obiekt stanu gracza do zapisania.
     * @return Obiekt `PlayerData` reprezentujący aktualne dane gracza.
     */
    public object CaptureState()
    {
        var health = GetComponent<Health>();

        Debug.Log($"[SAVE] HP = {health.CurrentHealth}");

        return new PlayerData
        {
            hp = health.CurrentHealth
        };
    }


    /**
     * @brief Przywraca stan gracza na podstawie zapisanych danych.
     * @param state Obiekt w formacie JSON zawierający dane gracza.
     */
    public void RestoreState(object state)
    {
        Debug.Log("[RESTORE] RestoreState zostało wywołane");

        string json = state as string;
        var data = JsonUtility.FromJson<PlayerData>(json);

        Debug.Log($"[LOAD] HP = {data.hp}");

        var health = GetComponent<Health>();
        health.SetBarsValue(data.hp);

        var movement = GetComponent<EthanTheHero.PlayerMovement>();
        if (movement != null)
        {
            movement.ResetAfterLoad();
        }

        var playerHealth = GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.Revive();
        }

        // Można odtworzyć więcej: poziom, statystyki, itp.
    }
}

