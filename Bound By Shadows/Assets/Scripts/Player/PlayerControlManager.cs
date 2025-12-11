using UnityEngine;
using EthanTheHero;

public class PlayerControlManager : MonoBehaviour
{
    private PlayerMovement movement;
    private PlayerAnimation anim;
    private PlayerAttackMethod attackMethod;
    private Rigidbody2D rb;
    private Animator animator;

    private void Awake()
    {
        // Pobieramy referencje automatycznie na starcie
        movement = GetComponent<PlayerMovement>();
        anim = GetComponent<PlayerAnimation>();
        attackMethod = GetComponent<PlayerAttackMethod>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Blokuje sterowanie gracza, resetuje fizykę i animacje do Idle.
    /// </summary>
    public void LockControls(bool blockMove, bool blockAnim, bool blockAttack, bool blockWallSlide)
    {
        if (blockMove && movement != null)
        {
            movement.enabled = false;
            if (rb != null) rb.linearVelocity = Vector2.zero; // Zatrzymanie w miejscu
        }

        if (blockWallSlide && movement != null)
        {
            movement.wallSlidingEnabled = false;
        }

        if (blockAttack && attackMethod != null)
        {
            attackMethod.enabled = false;
        }

        if (blockAnim)
        {
            if (anim != null) anim.enabled = false;
            ResetAnimator();
        }
    }

    /// <summary>
    /// Przywraca sterowanie.
    /// </summary>
    public void UnlockControls()
    {
        if (movement != null)
        {
            movement.enabled = true;
            movement.wallSlidingEnabled = true; // Zakładamy, że domyślnie chcemy to włączyć
        }

        if (attackMethod != null) attackMethod.enabled = true;
        if (anim != null) anim.enabled = true;
    }

    private void ResetAnimator()
    {
        if (animator != null)
        {
            animator.Play("Idle");
            animator.SetFloat("Speed", 0f);
            animator.SetBool("RunIdlePlayying", false);
            animator.SetBool("Grounded", true);
            animator.SetBool("Dashing", false);
            animator.SetTrigger("NotAttacking");
        }
    }
}