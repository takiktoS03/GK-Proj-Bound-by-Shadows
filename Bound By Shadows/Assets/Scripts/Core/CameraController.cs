using System.Collections;
using UnityEngine;


/**
 * @class CameraController
 * @brief Klasa odpowiadająca za płynne podążanie kamery za graczem.
 * 
 * Kamera przewiduje ruch gracza w poziomie (look ahead) oraz dynamicznie reaguje na pionowe przemieszczenia (skoki).
 * 
 * @author Filip Kudła
 */
public class CameraController : MonoBehaviour
{
    [SerializeField] private float cameraSpeed = 4f;
    [SerializeField] private Transform player;
    [SerializeField] private float aheadDistance = 0.5f;
    [SerializeField] private float jumpHeight = 2f;
    private float lookAhead;
    private Vector3 targetPosition;

    /**
    * @brief Ustawia początkową pozycję kamery względem gracza.
    */
    private void Awake()
    {
        transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
    }

    /**
    * @brief Aktualizuje pozycję kamery w zależności od pozycji gracza.
    */
    void Update()
    {
        targetPosition = new Vector3(player.position.x + lookAhead, Mathf.Abs(player.position.y - transform.position.y) > jumpHeight ? player.position.y : transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * cameraSpeed);
        lookAhead = Mathf.Lerp(lookAhead, aheadDistance * player.localScale.x, Time.deltaTime * cameraSpeed);
    }


    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;
        this.enabled = false;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        this.enabled = true;
        transform.localPosition = originalPos;
    }

}

