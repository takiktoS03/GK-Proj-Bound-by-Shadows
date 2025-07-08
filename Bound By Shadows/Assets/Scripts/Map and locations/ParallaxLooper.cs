using UnityEngine;

public class ParallaxLooper : MonoBehaviour
{
    public Transform cameraTransform;
    public float parallaxMultiplier = 0.5f;
    public float bufferPercent = 0.01f;

    private Vector3 lastCameraPosition;
    private Transform[] segments;
    private float spriteWidth;

    void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        lastCameraPosition = cameraTransform.position;

        Transform original = transform.GetChild(0);
        spriteWidth = original.GetComponent<SpriteRenderer>().bounds.size.x;

        segments = new Transform[2];
        segments[0] = original;
        segments[1] = Instantiate(original, transform);
        segments[1].position = original.position + Vector3.right * spriteWidth;
    }

    void LateUpdate()
    {
        Vector3 delta = cameraTransform.position - lastCameraPosition;
        transform.position += delta * parallaxMultiplier;
        lastCameraPosition = cameraTransform.position;

        float buffer = spriteWidth * bufferPercent;

        for (int i = 0; i < segments.Length; i++)
        {
            Transform segment = segments[i];
            float distance = cameraTransform.position.x - segment.position.x;

            if (distance > spriteWidth + buffer)
            {
                segment.position += Vector3.right * spriteWidth * segments.Length;
            }
            else if (distance < -spriteWidth - buffer)
            {
                segment.position -= Vector3.right * spriteWidth * segments.Length;
            }
        }
    }
}
