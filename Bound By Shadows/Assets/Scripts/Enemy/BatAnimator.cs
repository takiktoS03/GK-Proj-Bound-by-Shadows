using UnityEngine;

public class BatAnimator : MonoBehaviour
{
    private Animator animator;
    private BatMovement movement;
    private Health health;

    void Awake()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<BatMovement>();
        health = GetComponent<Health>();
    }

    void Update()
    {
        animator.SetBool("isMoving", movement != null);
    }

    public void SetMoving(bool value)
    {
        animator.SetBool("isMoving", value);
    }

    public void PlayAttack()
    {
        animator.SetBool("isAttacking", true);
    }

    public void StopAttack()
    {
        animator.SetBool("isAttacking", false);
    }

    public void PlayHurt()
    {
        animator.SetTrigger("hurt");
    }

    public void PlayDie()
    {
        animator.SetBool("isDead", true);
    }
}
