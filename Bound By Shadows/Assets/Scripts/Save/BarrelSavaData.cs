using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/**
 * @class BarrelSaveData
 * @brief Odpowiada za trwałe przechowywanie informacji o zniszczonych beczkach między sesjami gry.
 *
 * Implementuje interfejs `ISaveable` do integracji z globalnym systemem zapisu stanu gry.
 * Przechowuje unikalne identyfikatory (`UniqueId`) zniszczonych beczek i usuwa je z sceny podczas wczytywania.
 *
 * Beczki powinny mieć komponent `SaveableObject` z przypisanym `UniqueId`.
 *
 * @author Filip Kudła
 */
public class BarrelSaveData : MonoBehaviour, ISaveable
{
    /// @brief Zestaw identyfikatorów beczek, które zostały zniszczone.
    private static HashSet<string> destroyedBarrels = new HashSet<string>();

    /**
     * @brief Rejestruje beczkę jako zniszczoną w pamięci podręcznej.
     * @param uniqueId Unikalny identyfikator obiektu beczki.
     */
    public static void RegisterDestroyedBarrel(string uniqueId)
    {
        destroyedBarrels.Add(uniqueId);
    }

    /**
     * @brief Przy uruchomieniu sceny usuwa z niej zniszczone wcześniej beczki.
     */
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

    /**
     * @brief Zapisuje listę zniszczonych beczek.
     * @return Obiekt zawierający listę identyfikatorów zniszczonych beczek.
     */
    public object CaptureState()
    {
        return destroyedBarrels.ToList();
    }

    /**
     * @brief Przywraca stan listy zniszczonych beczek.
     * @param state Obiekt zawierający listę identyfikatorów z poprzedniego zapisu.
     */
    public void RestoreState(object state)
    {
        var list = state as List<string>;
        destroyedBarrels = new HashSet<string>(list);
    }
}

