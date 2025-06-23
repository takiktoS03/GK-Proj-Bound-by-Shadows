using UnityEngine;

public class GhostFollow : MonoBehaviour
{
    public Transform player;          // Obiekt gracza
    public Vector3 offset; // Względna pozycja nad graczem
    public float smoothSpeed = 2f;
    public float floatAmplitude = 0.5f;  // Amplituda 'pływania' góra-dół
    public float floatFrequency = 1f;    // Częstotliwość 'pływania'

    private Vector3 initialOffset;
    private Vector3 initialScale;
    Vector3 desiredPosition;

    void Start()
    {
        initialOffset = offset;
        initialScale = transform.localScale;
    }

    void Update()
    {
        if (player == null) return;

        // Dostosowanie pozycji i skali duszka do skali gracza
        float playerDir = Mathf.Sign(player.localScale.x);
        Vector3 currentOffset = initialOffset;
        currentOffset.x *= playerDir;

        Vector3 currentScale = initialScale;
        currentScale.x *= playerDir;
        transform.localScale = currentScale;

        // Dodanie efektu falowania w osi Y
        float floatY = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        desiredPosition = player.position + currentOffset + new Vector3(0, floatY, 0);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smoothSpeed);
    }
}
