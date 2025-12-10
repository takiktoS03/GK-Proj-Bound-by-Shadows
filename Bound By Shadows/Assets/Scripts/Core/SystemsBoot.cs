using UnityEngine;

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