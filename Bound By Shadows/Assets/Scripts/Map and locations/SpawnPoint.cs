using UnityEngine;

/**
 * Skrypt definiujący punkt pojawienia się gracza na scenie,
 * wykorzystywany przy starcie lub po teleportacji.
 *
 * @author Filip Kudła
 */

public class SpawnPoint : MonoBehaviour
{
    [Header("Unikalne ID tego punktu na tej scenie")]
    public string spawnID;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
        Gizmos.DrawLine(transform.position, transform.position + transform.right * 0.5f);
    }
}