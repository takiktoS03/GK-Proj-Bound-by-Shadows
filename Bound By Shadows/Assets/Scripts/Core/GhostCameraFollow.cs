using UnityEngine;

/**
 * @class GhostCameraFollow
 * @brief Prosty skrypt do pod��ania obiektu (np. kamery) za wskazanym celem.
 * 
 * Mo�e by� u�yty przez ducha lub inny obiekt �ledz�cy pozycj� gracza.
 * 
 * @author Filip Kud�a 
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
