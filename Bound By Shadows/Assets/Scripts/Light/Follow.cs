using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Follow : MonoBehaviour
{
    public Transform objectToFollow;
    public Vector3 offset;
    public float followSpeed = 10f;

    private void Update()
    {
        if (objectToFollow == null) return;

        transform.position = Vector3.Lerp(
            transform.position,
            objectToFollow.position + offset,
            Time.deltaTime * followSpeed
        );
    }
}
