using UnityEngine;
using TMPro;

/**
 * @class DoorTrigger
 * @brief Skrypt obs³uguj¹cy teleportacjê gracza po naciœniêciu klawisza, gdy znajduje siê przy drzwiach.
 *
 * Po wejœciu gracza w obszar drzwi, pojawia siê tekst podpowiedzi.
 * Jeœli gracz naciœnie F, gracz zostaje teleportowany, a drzwi zmieniaj¹ wygl¹d.
 * 
 * @author Julia Bigaj
 */
public class DoorTrigger : MonoBehaviour
{
    [Tooltip("Obiekt z tekstem podpowiedzi")]
    public GameObject promptText;

    [Tooltip("Sprite przedstawiaj¹cy otwarte drzwi")]
    public Sprite openDoorSprite;

    [Tooltip("Pozycja docelowa teleportacji gracza")]
    public Transform teleportTarget;

    private SpriteRenderer spriteRenderer;  // Komponent renderuj¹cy sprite drzwi
    private bool playerInTrigger = false;   // Czy gracz znajduje siê w triggerze
    private GameObject player;              // Referencja do obiektu gracza

    /** @brief Inicjalizacja zmiennych i ukrycie podpowiedzi */
    private void Start()
    {
        promptText.SetActive(false);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /** 
     * @brief Sprawdza naciœniêcie klawisza F i teleportuje gracza 
     */
    private void Update()
    {
        if (playerInTrigger && Input.GetKeyDown(KeyCode.F))
        {
            if (spriteRenderer != null && openDoorSprite != null)
            {
                spriteRenderer.sprite = openDoorSprite;
            }

            SoundManager.Instance?.PlayDoor();

            if (player != null && teleportTarget != null)
            {
                player.transform.position = teleportTarget.position;
                promptText.SetActive(false);
            }
        }
    }

    /**
     * @brief Pokazuje podpowiedŸ po wejœciu gracza w trigger
     * @param other Obiekt koliduj¹cy (oczekiwany tag: "Player")
     */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            promptText.SetActive(true);
            playerInTrigger = true;
            player = other.gameObject;
        }
    }

    /**
     * @brief Ukrywa podpowiedŸ po wyjœciu gracza z triggera
     * @param other Obiekt koliduj¹cy (oczekiwany tag: "Player")
     */
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            promptText.SetActive(false);
            playerInTrigger = false;
            player = null;
        }
    }
}
