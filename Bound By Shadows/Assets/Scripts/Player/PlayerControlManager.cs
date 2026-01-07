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
        movement = GetComponent<PlayerMovement>();
        anim = GetComponent<PlayerAnimation>();
        attackMethod = GetComponent<PlayerAttackMethod>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Blokuje sterowanie gracza, resetuje fizykę i animacje do Idle.
    /// </summary>
    public void LockControls(bool blockMove, bool blockWallSlide, bool blockAttack, bool blockAnim)
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
            PlayIdle();
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
            movement.wallSlidingEnabled = true;
        }

        if (attackMethod != null) attackMethod.enabled = true;
        if (anim != null) anim.enabled = true;
    }

    private void PlayIdle()
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