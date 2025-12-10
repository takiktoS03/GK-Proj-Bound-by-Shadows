using UnityEngine;
using TMPro;

/**
 * @class DoorTrigger
 * @brief Skrypt obsługujący teleportację gracza po naciśnięciu klawisza, gdy znajduje się przy drzwiach.
 *
 * Po wejściu gracza w obszar drzwi, pojawia się tekst podpowiedzi.
 * Jeśli gracz naciśnie F, gracz zostaje teleportowany, a drzwi zmieniają wygląd.
 * 
 * @author Julia Bigaj
 */
public class DoorTrigger : MonoBehaviour
{
    [Tooltip("Obiekt z tekstem podpowiedzi")]
    public GameObject promptText;

    [Tooltip("Sprite przedstawiający otwarte drzwi")]
    public Sprite openDoorSprite;

    [Tooltip("Pozycja docelowa teleportacji gracza")]
    public Transform teleportTarget;

    private SpriteRenderer spriteRenderer;  // Komponent renderujący sprite drzwi
    private bool playerInTrigger = false;   // Czy gracz znajduje się w triggerze
    private GameObject player;              // Referencja do obiektu gracza

    /** @brief Inicjalizacja zmiennych i ukrycie podpowiedzi */
    private void Start()
    {
        promptText.SetActive(false);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /** 
     * @brief Sprawdza naciśnięcie klawisza F i teleportuje gracza 
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
     * @brief Pokazuje podpowiedź po wejściu gracza w trigger
     * @param other Obiekt kolidujący (oczekiwany tag: "Player")
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
     * @brief Ukrywa podpowiedź po wyjściu gracza z triggera
     * @param other Obiekt kolidujący (oczekiwany tag: "Player")
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

