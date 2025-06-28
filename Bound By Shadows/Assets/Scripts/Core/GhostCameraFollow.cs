using UnityEngine;

/**
 * @class GhostCameraFollow
 * @brief Prosty skrypt do podążania obiektu (np. kamery) za wskazanym celem.
 * 
 * Może być użyty przez ducha lub inny obiekt śledzący pozycję gracza.
 * 
 * @author Filip Kudła 
 */
public class GhostCameraFollow : MonoBehaviour
{
    public Transform mainCamera;

    /**
    * @brief Przesuwa obiekt do pozycji celu z zadanym offsetem.
    */
    void LateUpdate()
    {
        transform.position = mainCamera.position;
        transform.rotation = mainCamera.rotation;
    }
}

