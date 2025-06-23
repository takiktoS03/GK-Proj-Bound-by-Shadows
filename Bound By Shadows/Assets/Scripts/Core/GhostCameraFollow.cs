using UnityEngine;

public class GhostCameraFollow : MonoBehaviour
{
    public Transform mainCamera;

    void LateUpdate()
    {
        transform.position = mainCamera.position;
        transform.rotation = mainCamera.rotation;
    }
}
