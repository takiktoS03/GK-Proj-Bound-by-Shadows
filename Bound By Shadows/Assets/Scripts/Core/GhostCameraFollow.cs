using UnityEngine;

/**
 * @class GhostCameraFollow
 * @brief Prosty skrypt do pod¹¿ania obiektu (np. kamery) za wskazanym celem.
 * 
 * Mo¿e byæ u¿yty przez ducha lub inny obiekt œledz¹cy pozycjê gracza.
 * 
 * @author Filip Kud³a 
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
