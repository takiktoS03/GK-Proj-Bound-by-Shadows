using UnityEngine;

/**
 * @class UIManager
 * @brief Singleton odpowiedzialny za zarządzanie elementami interfejsu użytkownika w całej grze.
 *
 * Obiekt jest przenoszony między scenami za pomocą `DontDestroyOnLoad`, zapewniając trwałość UI
 * i centralny dostęp do zarządzania elementami interfejsu w wielu scenach.
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

