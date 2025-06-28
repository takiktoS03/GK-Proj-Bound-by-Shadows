using UnityEngine;

/**
 * @class AxeTrap
 * @brief Obsługuje ruchome pułapki typu wahadłowy topór.
 *
 * Topór porusza się jak fizyczne wahadło (z tłumieniem) pomiędzy dwoma wychyleniami.
 * Po kolizji z graczem zadaje obrażenia.
 *
 * @author Filip Kudla
 */
public class AxeTrap : MonoBehaviour
{
    /** @brief Maksymalny kąt wychylenia (w stopniach) od pionu. */
    [SerializeField] private float deflectionAngle = 45f;

    /** @brief Prędkość obrotu wahadła (wpływa na przyspieszenie kątowe). */
    [SerializeField] private float speed = 5f;

    /** @brief Ilość obrażeń zadawanych graczowi przy kolizji. */
    [SerializeField] private float damage = 30f;

    /** @brief Długość łańcucha, czyli promień wahadła. */
    [SerializeField] private float ropeLength = 4.7f;

    /** @brief Współczynnik tłumienia wahadła (1 = brak tłumienia). */
    [SerializeField] private float damping = 0.95f;

    private float currentAngle;              ///< Aktualny kąt wychylenia (w radianach).
    private float angularVelocity = 0f;      ///< Prędkość kątowa wahadła.
    private Vector3 pivotPosition;           ///< Stała pozycja punktu zawieszenia (pivot).

    /**
     * @brief Ustawia początkowy kąt i pozycję pivotu.
     */
    private void Start()
    {
        pivotPosition = transform.position;
        currentAngle = deflectionAngle * Mathf.Deg2Rad;
    }

    /**
     * @brief Oblicza ruch wahadła, tłumienie oraz obrót obiektu.
     */
    private void Update()
    {
        float angularAcceleration = -speed * Mathf.Sin(currentAngle);
        angularVelocity += angularAcceleration * Time.deltaTime;
        angularVelocity *= Mathf.Pow(damping, Time.deltaTime);
        currentAngle += angularVelocity * Time.deltaTime;

        float maxRadians = deflectionAngle * Mathf.Deg2Rad;
        currentAngle = Mathf.Clamp(currentAngle, -maxRadians, maxRadians);

        float offsetX = Mathf.Sin(currentAngle) * ropeLength;
        float offsetY = -Mathf.Cos(currentAngle) * ropeLength;

        transform.position = pivotPosition + new Vector3(offsetX / 2.2f, offsetY / 2.2f + 2.2f, 0f);
        transform.eulerAngles = new Vector3(0f, 0f, currentAngle * Mathf.Rad2Deg);
    }

    /**
     * @brief Zadaje obrażenia graczowi po wejściu w collider pułapki.
     * 
     * @param collision Obiekt, który wszedł w obszar pułapki.
     */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Health>()?.TakeDamage(damage);
        }
    }
}
