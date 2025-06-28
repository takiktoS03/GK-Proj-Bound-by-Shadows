using UnityEngine;

/**
 * @class UIManager
 * @brief Singleton odpowiedzialny za zarz¹dzanie elementami interfejsu u¿ytkownika w ca³ej grze.
 *
 * Obiekt jest przenoszony miêdzy scenami za pomoc¹ `DontDestroyOnLoad`, zapewniaj¹c trwa³oœæ UI
 * i centralny dostêp do zarz¹dzania elementami interfejsu w wielu scenach.
 *
 * @author Julia Bigaj
 */
public class UIManager : MonoBehaviour
{
    /// @brief Statyczna instancja singletonu UIManager.
    public static UIManager Instance { get; private set; }

    /**
     * @brief Inicjalizuje singleton i zapobiega duplikatom przy zmianie sceny.
     */
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
