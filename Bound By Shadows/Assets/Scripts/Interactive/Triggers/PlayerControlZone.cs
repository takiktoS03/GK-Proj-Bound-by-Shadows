using UnityEngine;

public class PlayerControlZone : MonoBehaviour
{
    [System.Serializable]
    public enum ZoneAction { UnlockControls, LockControls }

    [Header("Settings")]
    public ZoneAction actionType = ZoneAction.UnlockControls;
    public bool destroyAfterTrigger = true;

    [Header("Lock Settings (Only if Locking)")]
    public bool blockMovement = true;
    public bool blockAnimation = true;
    public bool blockAttacks = true;
    public bool blockWallSliding = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var controlManager = other.GetComponent<PlayerControlManager>();

        if (controlManager != null)
        {
            if (actionType == ZoneAction.UnlockControls)
            {
                controlManager.UnlockControls();
            }
            else
            {
                controlManager.LockControls(blockMovement, blockWallSliding, blockAttacks, blockAnimation);
            }
        }

        if (destroyAfterTrigger)
            Destroy(gameObject);
    }
}