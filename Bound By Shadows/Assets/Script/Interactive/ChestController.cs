using UnityEngine;
using TMPro;

public class ChestController : MonoBehaviour
{
    private Animator animator;
    private bool isOpen = false;
    private bool isPlayerNear = false;

    public TextMeshPro promptUI;

    void Start()
    {
        animator = GetComponent<Animator>();
        if(promptUI != null)
            promptUI.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isPlayerNear && !isOpen && Input.GetKeyDown(KeyCode.F))
        {
            animator.SetTrigger("Open");
            isOpen = true;
            if (promptUI != null)
                promptUI.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isOpen){
            isPlayerNear = true;
            if(promptUI != null)
                promptUI.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            if (promptUI != null)
                promptUI.gameObject.SetActive(false);
        }
    }
}
