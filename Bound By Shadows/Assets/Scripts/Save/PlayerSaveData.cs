using UnityEngine;

/**
 * @struct PlayerData
 * @brief Struktura danych zawieraj�ca informacje do zapisu o graczu.
 *
 * Aktualnie przechowuje jedynie ilo�� punkt�w �ycia gracza (`hp`), ale mo�na j� �atwo rozszerzy�
 * o inne dane (np. poziom, pozycj�, ekwipunek).
 * 
 * @author Filip Kud�a
 */
[System.Serializable]
public class PlayerData
{
    /// @brief Aktualny poziom zdrowia gracza.
    public float hp;

    // Mo�na rozszerzy�:
    // public int level;
}


/**
 * @class PlayerSaveData
 * @brief Klasa odpowiedzialna za zapis i odczyt stanu gracza.
 *
 * Implementuje interfejs `ISaveable`, integruj�c si� z globalnym systemem zapisu gry.
 * Pobiera i przywraca stan komponentu `Health` przypisanego do gracza.
 *
 * @note Wymaga obecno�ci komponentu `Health` w tym samym obiekcie.
 *
 * @author Filip Kud�a
 */
public class PlayerSaveData : MonoBehaviour, ISaveable
{
    /**
     * @brief Tworzy obiekt stanu gracza do zapisania.
     * @return Obiekt `PlayerData` reprezentuj�cy aktualne dane gracza.
     */
    public object CaptureState()
    {
        var health = GetComponent<Health>();
        return JsonUtility.ToJson(new PlayerData
        {
            //hp = health.currentHealth
            // level = ... // mo�na doda� inne dane
        });
    }


    /**
     * @brief Przywraca stan gracza na podstawie zapisanych danych.
     * @param state Obiekt w formacie JSON zawieraj�cy dane gracza.
     */
    public void RestoreState(object state)
    {
        Debug.Log("[RESTORE] RestoreState zosta�o wywo�ane");

        string json = state as string;
        var data = JsonUtility.FromJson<PlayerData>(json);

        var health = GetComponent<Health>();
        health.SetBarsValue(data.hp);

        // Mo�na odtworzy� wi�cej: poziom, statystyki, itp.
    }
}
