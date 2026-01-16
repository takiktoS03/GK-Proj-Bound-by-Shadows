using UnityEngine;

/**
 * Skrypt inicjalizujący globalne systemy gry,
 * zapewniający ich istnienie pomiędzy zmianami scen.
 *
 * @author Filip Kudła
 */
public class SystemsBoot : MonoBehaviour
{
    private static bool exists = false;

    private void Awake()
    {
        // Zabezpieczenie: jeśli z jakiegoś powodu wrócimy do sceny BootScene,
        // nie chcemy stworzyć drugiego obiektu SYSTEMS
        if (exists)
        {
            Destroy(gameObject);
        }
        else
        {
            exists = true;
            DontDestroyOnLoad(gameObject);
        }
    }
}