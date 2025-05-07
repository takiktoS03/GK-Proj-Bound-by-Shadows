using UnityEngine;

public class PlayerSaveData : MonoBehaviour, ISaveable
{

    public object CaptureState()
    {
        return new ObjectSaveData
        {
            id = GetComponent<SaveableObject>().UniqueId,
            posX = transform.position.x,
            posY = transform.position.y,
            posZ = transform.position.z,
            rotX = transform.eulerAngles.x,
            rotY = transform.eulerAngles.y,
            rotZ = transform.eulerAngles.z
        };
    }

    public void RestoreState(object state)
    {

        Debug.Log("[RESTORE] RestoreState zosta³o wywo³ane");

        var data = state as ObjectSaveData;
        if (data == null) return;

        Vector3 restoredPosition = new Vector3(data.posX, data.posY, data.posZ);
        transform.position = restoredPosition;
        transform.rotation = Quaternion.Euler(data.rotX, data.rotY, data.rotZ);

        var rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.position = restoredPosition;
            rb.linearVelocity = Vector2.zero;
        }

        Debug.Log($"[RESTORE] {data.id} -> {restoredPosition}");
    }
}
