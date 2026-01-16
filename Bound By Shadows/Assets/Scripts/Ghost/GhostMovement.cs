using UnityEngine;

/**
 * Skrypt odpowiadający za płynny ruch duszka, łączący efekt unoszenia się
 * oraz opcjonalne podążanie za graczem.
 *
 * @author Filip Kudła
 */

public class GhostMovement : MonoBehaviour
{
    [Header("Follow Player")]
    [Tooltip("Jeśli ustawione, duszek będzie podążać za graczem. Jeśli nie, będzie tylko falować.")]
    [SerializeField] private Transform player;

    public Vector3 offset = new Vector3(1f, 1f, 0f);
    private Vector3 initialOffset;
    private Vector3 initialScale;

    [Header("Floating Effect")]
    public float floatAmplitude = 0.5f;
    public float floatFrequency = 1f;

    [Header("Movement")]
    public float smoothSpeed = 2f;
    public bool isFollowingPlayer = false;

    void Start()
    {
        initialOffset = offset;
        initialScale = transform.localScale;
    }

    void Update()
    {
        float floatY = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;

        // Tryb 1: Brak gracza -> tylko unoszenie się
        if (player == null || !isFollowingPlayer)
        {
            // Dodanie efektu falowania w osi Y
            Vector3 floatingPos = transform.position;
            floatingPos.y += floatY;
            transform.position = Vector3.Lerp(transform.position, floatingPos, Time.deltaTime * smoothSpeed);
            return;
        }

        // Tryb 2: Jest gracz -> śledzenie + unoszenie

        // Dostosowanie pozycji i skali duszka do skali gracza
        float playerDir = Mathf.Sign(player.localScale.x);
        transform.localScale = new Vector3(Mathf.Abs(initialScale.x) * playerDir, initialScale.y, initialScale.z);

        Vector3 currentOffset = initialOffset;
        currentOffset.x *= playerDir;

        Vector3 desiredPos = player.position + currentOffset + new Vector3(0, floatY, 0);

        transform.position = Vector3.Lerp(transform.position, desiredPos, Time.deltaTime * smoothSpeed);
    }

    public void FollowUnlocked()
    {
        isFollowingPlayer = true;
    }

    public void FollowLocked()
    {
        isFollowingPlayer = false;
    }
}
