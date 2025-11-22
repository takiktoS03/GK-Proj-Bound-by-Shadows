using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Transform targetPoint;   // miejsce docelowe teleportacji
    public KeyCode key = KeyCode.F; // klawisz aktywacji
    private bool playerInRange = false;
    private Transform player;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(key))
        {
            FadeController.instance.FadeOutIn(() =>
            { 
                var cam = Camera.main.GetComponent<CameraController>();
                cam.enabled = false;

                player.position = targetPoint.position;

                Camera.main.transform.position = new Vector3(
                    player.position.x,
                    player.position.y,
                    Camera.main.transform.position.z
                );

                cam.enabled = true;
            });
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            player = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            player = null;
        }
    }
}
