using UnityEngine;

/**
 * @class GhostFollow
 * @brief Skrypt do płynnego śledzenia gracza przez duszka.
 *
 * Obiekt przypisany do tego skryptu podąża za graczem, unosząc się nad nim i zmieniając kierunek w zależności od skali gracza.
 * Dodaje także efekt sinusoidalnego 'pływania' w osi Y.
 *
 * @author Filip Kudła
 */
public class GhostFollow : MonoBehaviour
{
    /// @brief Transform gracza do śledzenia.
    public Transform player;

    /// @brief Względna pozycja duszka nad graczem.
    public Vector3 offset;

    /// @brief Prędkość płynnego podążania.
    public float smoothSpeed = 2f;

    /// @brief Amplituda 'pływania' duszka.
    public float floatAmplitude = 0.5f;

    /// @brief Częstotliwość pływania.
    public float floatFrequency = 1f;

    private Vector3 initialOffset;
    private Vector3 initialScale;
    Vector3 desiredPosition;

    /**
     * @brief Inicjalizuje pozycję początkową i skalę.
     */
    void Start()
    {
        initialOffset = offset;
        initialScale = transform.localScale;
    }

    /**
     * @brief Aktualizuje pozycję duszka względem gracza z uwzględnieniem efektu falowania.
     */
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
