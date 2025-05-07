using UnityEngine;

public class UIManager : MonoBehaviour
{
    void Awake()
    {
        if (FindObjectsOfType<UIManager>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

}
