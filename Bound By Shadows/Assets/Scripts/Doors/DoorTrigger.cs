using UnityEngine;
using TMPro;

/* Ten skrypt obs�uguje interakcj� gracza z drzwiami w grze 2D w Unity.
   Gdy gracz wejdzie w stref� kolizji drzwi (trigger), pojawia si� komunikat zach�caj�cy do interakcji.
   Naci�ni�cie klawisza "F" powoduje:
   - zmian� sprite'a drzwi na otwarte (je�li przypisano grafik�),
   - odtworzenie d�wi�ku drzwi (je�li SoundManager istnieje),
   - teleportacj� gracza w okre�lone miejsce (teleportTarget).

   Skrypt oparty jest na systemie kolizji 2D (OnTriggerEnter2D / OnTriggerExit2D), a tekst promptu jest ukrywany/ujawniany dynamicznie.

   Autor: Julia Bigaj
*/

public class DoorTrigger : MonoBehaviour
{
    public GameObject promptText;           // Obiekt z tekstem informuj�cym gracza o mo�liwo�ci interakcji
    public Sprite openDoorSprite;           // Sprite drzwi po otwarciu
    public Transform teleportTarget;        // Pozycja, do kt�rej teleportowany jest gracz
    private SpriteRenderer spriteRenderer;  // Komponent renderuj�cy sprite drzwi
    private bool playerInTrigger = false;   // Czy gracz znajduje si� w triggerze
    private GameObject player;              // Referencja do obiektu gracza

    private void Start()
    {
        promptText.SetActive(false);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            promptText.SetActive(true);
            playerInTrigger = true;
            player = other.gameObject;
        }
    }

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
