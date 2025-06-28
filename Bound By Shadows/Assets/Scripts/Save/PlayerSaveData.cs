using UnityEngine;

/**
 * @struct PlayerData
 * @brief Struktura danych zawieraj¹ca informacje do zapisu o graczu.
 *
 * Aktualnie przechowuje jedynie iloœæ punktów ¿ycia gracza (`hp`), ale mo¿na j¹ ³atwo rozszerzyæ
 * o inne dane (np. poziom, pozycjê, ekwipunek).
 * 
 * @author Filip Kud³a
 */
[System.Serializable]
public class PlayerData
{
    /// @brief Aktualny poziom zdrowia gracza.
    public float hp;

    // Mo¿na rozszerzyæ:
    // public int level;
}


/**
 * @class PlayerSaveData
 * @brief Klasa odpowiedzialna za zapis i odczyt stanu gracza.
 *
 * Implementuje interfejs `ISaveable`, integruj¹c siê z globalnym systemem zapisu gry.
 * Pobiera i przywraca stan komponentu `Health` przypisanego do gracza.
 *
 * @note Wymaga obecnoœci komponentu `Health` w tym samym obiekcie.
 *
 * @author Filip Kud³a
 */
public class PlayerSaveData : MonoBehaviour, ISaveable
{
    /**
     * @brief Tworzy obiekt stanu gracza do zapisania.
     * @return Obiekt `PlayerData` reprezentuj¹cy aktualne dane gracza.
     */
    public object CaptureState()
    {
        var health = GetComponent<Health>();
        return JsonUtility.ToJson(new PlayerData
        {
            //hp = health.currentHealth
            // level = ... // mo¿na dodaæ inne dane
        });
    }


    /**
     * @brief Przywraca stan gracza na podstawie zapisanych danych.
     * @param state Obiekt w formacie JSON zawieraj¹cy dane gracza.
     */
    public void RestoreState(object state)
    {
        Debug.Log("[RESTORE] RestoreState zosta³o wywo³ane");

        string json = state as string;
        var data = JsonUtility.FromJson<PlayerData>(json);

        var health = GetComponent<Health>();
        health.SetBarsValue(data.hp);

        // Mo¿na odtworzyæ wiêcej: poziom, statystyki, itp.
    }
}
