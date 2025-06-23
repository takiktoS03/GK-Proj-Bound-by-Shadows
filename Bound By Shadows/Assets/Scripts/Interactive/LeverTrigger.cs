using UnityEngine;

public class LeverTrigger : MonoBehaviour
{
    public Animator leverAnimator;

    private bool playerInRange;
    [HideInInspector] public bool leverIsOn;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && playerInRange)
        {
            if (!leverIsOn)
            {
                leverAnimator.SetTrigger("ActiveOn");
                leverIsOn = true;
                SoundManager.Instance?.PlayLever();
            }
            else
            {
                leverAnimator.SetTrigger("ActiveOff");
                leverIsOn = false;
                SoundManager.Instance?.PlayLever();
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

    private void CheckRiddle()
    {
        GetComponentInParent<LeverRiddle>().CheckCorrectness();
    }
}