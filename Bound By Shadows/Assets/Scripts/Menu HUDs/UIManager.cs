using UnityEngine;

/**
 * @class UIManager
 * @brief Singleton odpowiedzialny za zarz�dzanie elementami interfejsu u�ytkownika w ca�ej grze.
 *
 * Obiekt jest przenoszony mi�dzy scenami za pomoc� `DontDestroyOnLoad`, zapewniaj�c trwa�o�� UI
 * i centralny dost�p do zarz�dzania elementami interfejsu w wielu scenach.
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
