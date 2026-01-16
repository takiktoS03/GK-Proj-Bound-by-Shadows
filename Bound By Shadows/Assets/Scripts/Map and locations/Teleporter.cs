using DG.Tweening;
using System.Collections;
using UnityEngine;

/**
 * Skrypt obsługujący teleportację gracza do wyznaczonego punktu,
 * wraz z płynnym przejściem wizualnym kamery.
 *
 * @author Filip Kudła
 */

public class Teleporter : MonoBehaviour
{
    [Header("Teleport parameters")]
    public Transform targetPoint;           // miejsce docelowe teleportacji
    public KeyCode key = KeyCode.F;         // klawisz aktywacji
    public bool automaticTeleport = false;  // brak potrzeby wciskania klawisza
    public bool fadeIn = true;
    public bool fadeOut = true;

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

        if (fadeIn) yield return GameManager.Instance.FadeIn(1.0f).WaitForCompletion();

        var cam = Camera.main.GetComponent<CameraController>();
        cam.enabled = false;

        player.position = targetPoint.position;

        Camera.main.transform.position = new Vector3(
            player.position.x,
            player.position.y,
            Camera.main.transform.position.z
        );

        cam.enabled = true;

        if (fadeOut) yield return GameManager.Instance.FadeOut(1.0f).WaitForCompletion();

        isTeleporting = false;
    }
}
