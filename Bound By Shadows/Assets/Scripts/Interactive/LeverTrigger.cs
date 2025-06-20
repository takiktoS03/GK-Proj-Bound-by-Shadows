using UnityEngine;

public class LeverTrigger : MonoBehaviour
{
    public Animator leverAnimator;

    private bool playerInRange = false;
    private bool leverIsOn = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            if (!leverIsOn)
            {
                leverAnimator.SetTrigger("ActiveOn");
                leverIsOn = true;
            }
            else
            {
                leverAnimator.SetTrigger("ActiveOff");
                leverIsOn = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}
