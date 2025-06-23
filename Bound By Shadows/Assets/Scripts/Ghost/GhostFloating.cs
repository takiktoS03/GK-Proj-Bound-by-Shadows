using UnityEngine;

public class GhostFloating : MonoBehaviour
{
    public float smoothSpeed = 2f;
    public float floatAmplitude = 0.5f;  // Amplituda 'pływania' góra-dół
    public float floatFrequency = 1f;    // Częstotliwość 'pływania'

    Vector3 desiredPosition;

    void Update()
    {
        // Dodanie efektu falowania w osi Y
        float floatY = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        desiredPosition = transform.position + new Vector3(0, floatY, 0);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smoothSpeed);
    }
}
