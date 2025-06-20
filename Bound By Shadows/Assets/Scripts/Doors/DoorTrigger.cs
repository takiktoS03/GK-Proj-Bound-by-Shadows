using UnityEngine;
using TMPro;

public class DoorTrigger : MonoBehaviour
{
    public GameObject promptText;
    public Sprite openDoorSprite;
    public Transform teleportTarget;
    private SpriteRenderer spriteRenderer;
    private bool playerInTrigger = false;
    private GameObject player;

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
