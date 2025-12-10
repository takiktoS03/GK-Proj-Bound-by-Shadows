using DG.Tweening;
using System.Collections;
using UnityEngine;


/**
 * @class Teleporter
 * @brief Obsługuje teleportacje gracza do wskazanego miejsca.
 * 
 * Przemieszcza transform gracza do zadanego miejsca docelowego.
 * Używane do przechodzenia do innych pomieszczeń i lokalizacji w obrębie jednej sceny.
 * Podczas przejścia zatrzymuje kamerę, robi fade out fade in, po czym ją włącza.
 * 
 * @author Filip Kudła
 */
public class Teleporter : MonoBehaviour
{
    [Header("Teleport parameters")]
    public Transform targetPoint;           // miejsce docelowe teleportacji
    public bool automaticTeleport = false;  // brak potrzeby wciskania klawisza
    public KeyCode key = KeyCode.F;         // klawisz aktywacji

    private bool isTeleporting = false;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isTeleporting)
        {
            if (automaticTeleport || Input.GetKey(key))
            {
                StartCoroutine(TeleportSequence(collision.transform));
            }
        }
    }

    private IEnumerator TeleportSequence(Transform player)
    {
        isTeleporting = true;

        // Blokada sterowanie gracza
        // var playerScript = player.GetComponent<PlayerController>();
        // if(playerScript) playerScript.enabled = false;

        yield return GameManager.Instance.FadeIn(1.0f).WaitForCompletion();

        var cam = Camera.main.GetComponent<CameraController>();
        cam.enabled = false;

        player.position = targetPoint.position;

        Camera.main.transform.position = new Vector3(
            player.position.x,
            player.position.y,
            Camera.main.transform.position.z
        );

        cam.enabled = true;

        yield return GameManager.Instance.FadeOut(1.0f).WaitForCompletion();

        // Odblokowanie sterowania
        // if(playerScript) playerScript.enabled = true;

        isTeleporting = false;
    }
}
