using UnityEngine;
using TMPro;

/**
 * @class DoorTrigger
 * @brief Skrypt obs�uguj�cy teleportacj� gracza po naci�ni�ciu klawisza, gdy znajduje si� przy drzwiach.
 *
 * Po wej�ciu gracza w obszar drzwi, pojawia si� tekst podpowiedzi.
 * Je�li gracz naci�nie F, gracz zostaje teleportowany, a drzwi zmieniaj� wygl�d.
 * 
 * @author Julia Bigaj
 */
public class DoorTrigger : MonoBehaviour
{
    [Tooltip("Obiekt z tekstem podpowiedzi")]
    public GameObject promptText;

    [Tooltip("Sprite przedstawiaj�cy otwarte drzwi")]
    public Sprite openDoorSprite;

    [Tooltip("Pozycja docelowa teleportacji gracza")]
    public Transform teleportTarget;

    private SpriteRenderer spriteRenderer;  // Komponent renderuj�cy sprite drzwi
    private bool playerInTrigger = false;   // Czy gracz znajduje si� w triggerze
    private GameObject player;              // Referencja do obiektu gracza

    /** @brief Inicjalizacja zmiennych i ukrycie podpowiedzi */
    private void Start()
    {
        promptText.SetActive(false);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /** 
     * @brief Sprawdza naci�ni�cie klawisza F i teleportuje gracza 
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
     * @brief Pokazuje podpowied� po wej�ciu gracza w trigger
     * @param other Obiekt koliduj�cy (oczekiwany tag: "Player")
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
     * @brief Ukrywa podpowied� po wyj�ciu gracza z triggera
     * @param other Obiekt koliduj�cy (oczekiwany tag: "Player")
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
