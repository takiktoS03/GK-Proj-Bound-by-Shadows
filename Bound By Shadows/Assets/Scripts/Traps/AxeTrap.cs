using UnityEngine;

/**
 * Skrypt obslugujacy zmiane kierunku i polozenia pulapki (toporu) o zadany kat
 * Autor skryptu: Filip Kudla
*/
public class AxeTrap : MonoBehaviour
{
    [SerializeField] private float deflectionAngle = 45f; // maksymalne wychylenie w stopniach
    [SerializeField] private float speed = 5f; // predkosc obrotu
    [SerializeField] private float damage = 30f;
    [SerializeField] private float ropeLength = 4.7f; // dlugosc lancucha

    private float currentAngle;
    private float angularVelocity = 0f;
    private Vector3 pivotPosition;

    private void Start()
    {
        pivotPosition = transform.position;
        currentAngle = deflectionAngle * Mathf.Deg2Rad; // start z maksymalnego wychyłu
    }

    private void Update()
    {
        // prosty model ruchu wahadla: przyspieszenie zależy od sin(kata)
        float angularAcceleration = -speed * Mathf.Sin(currentAngle);
        angularVelocity += angularAcceleration * Time.deltaTime;
        currentAngle += angularVelocity * Time.deltaTime;

        // ogranicz wychylenie
        float maxRadians = deflectionAngle * Mathf.Deg2Rad;
        currentAngle = Mathf.Clamp(currentAngle, -maxRadians, maxRadians);

        // pozycja na koncu 'lancucha'
        float offsetX = Mathf.Sin(currentAngle) * ropeLength;
        float offsetY = -Mathf.Cos(currentAngle) * ropeLength;
        transform.position = pivotPosition + new Vector3(offsetX / 2.2f, offsetY / 2.2f + 2.2f, 0f);
        transform.eulerAngles = new Vector3(0f, 0f, currentAngle * Mathf.Rad2Deg);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Health>().TakeDamage(damage);
        }
    }
}
