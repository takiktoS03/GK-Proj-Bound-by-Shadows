using UnityEngine;

/**
 * @class GhostFloating
 * @brief Odpowiada za animację 'unoszenia się' duszka w górę i w dół.
 *
 * Skrypt dodaje efekt pływania dla obiektu (najczęściej duszka), poruszając go sinusoidalnie w osi Y.
 * Może być używany np. do ozdobnych duszków czy elementów interaktywnych.
 *
 * @author Filip Kudła
 */
public class GhostFloating : MonoBehaviour
{
    /// @brief Prędkość płynnego przejścia do nowej pozycji.
    public float smoothSpeed = 2f;

    /// @brief Amplituda unoszenia się w osi Y.
    public float floatAmplitude = 0.5f;

    /// @brief Częstotliwość unoszenia się.
    public float floatFrequency = 1f;

    Vector3 desiredPosition;

    /**
     * @brief Wykonuje płynne unoszenie w górę i w dół z użyciem funkcji sinus.
     */
    void Update()
    {
        // Dodanie efektu falowania w osi Y
        float floatY = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        desiredPosition = transform.position + new Vector3(0, floatY, 0);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smoothSpeed);
    }
}

