using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float cameraSpeed;
    [SerializeField] private Transform player;
    [SerializeField] private float aheadDistance;
    private float lookAhead;

    void Update()
    {
        transform.position = new Vector3(player.position.x + lookAhead, player.position.y > Mathf.Abs(transform.position.y - 2) ? player.position.y : transform.position.y, transform.position.z);
        lookAhead = Mathf.Lerp(lookAhead, aheadDistance * player.localScale.x, Time.deltaTime * cameraSpeed);
    }

}
