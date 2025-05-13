using UnityEngine;


/* 
 * Skrypt kamery podlaczony do gracza
 * Autor: Filip Kudła
*/

public class CameraController : MonoBehaviour
{
    [SerializeField] private float cameraSpeed = 4f;
    [SerializeField] private Transform player;
    [SerializeField] private float aheadDistance = 0.5f;
    [SerializeField] private float jumpHeight = 2f;
    private float lookAhead;
    private Vector3 targetPosition;

    private void Awake()
    {
        transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
    }

    void Update()
    {
        targetPosition = new Vector3(player.position.x + lookAhead, Mathf.Abs(player.position.y - transform.position.y) > jumpHeight ? player.position.y : transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * cameraSpeed);
        lookAhead = Mathf.Lerp(lookAhead, aheadDistance * player.localScale.x, Time.deltaTime * cameraSpeed);
    }

}
