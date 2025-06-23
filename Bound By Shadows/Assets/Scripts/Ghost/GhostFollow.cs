using UnityEngine;

public class GhostFollow : MonoBehaviour
{
    public Transform player;          // Obiekt gracza
    public Vector3 offset = new Vector3(0.5f, 2.2f, 0f); // Wzgl�dna pozycja nad graczem
    public float smoothSpeed = 2f;    // Jak szybko duszek si� porusza
    public float floatAmplitude = 0.5f;  // Amplituda p�ywania g�ra-d�
    public float floatFrequency = 1f;    // Cz�stotliwo��

    private Vector3 initialOffset;

    void Start()
    {
        initialOffset = offset;
    }

    void Update()
    {
        if (player == null) return;

        // Dodanie efektu falowania w osi Y
        float floatY = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        Vector3 desiredPosition = player.position + initialOffset + new Vector3(0, floatY, 0);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smoothSpeed);
    }
}
