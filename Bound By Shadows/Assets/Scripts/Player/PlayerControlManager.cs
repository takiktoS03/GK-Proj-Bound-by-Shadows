using UnityEngine;
using EthanTheHero;

/**
 * Skrypt zarządzający blokowaniem i odblokowywaniem sterowania graczem,
 * wykorzystywany podczas animacji, umiejętności i zdarzeń specjalnych.
 *
 * @author Filip Kudła
 */
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

    public void LockControls(bool blockMove, bool blockWallSlide, bool blockAttack, bool blockAnim)
    {
        if (blockMove && movement != null)
        {
            movement.enabled = false;
            if (rb != null) rb.linearVelocity = Vector2.zero;
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