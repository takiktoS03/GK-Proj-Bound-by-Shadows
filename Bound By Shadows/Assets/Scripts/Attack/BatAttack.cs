using UnityEngine;

public class BatAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private AttackData attackData;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackCooldown = 1.5f;

    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private AttackController attackController;
    [SerializeField] private Animator animator;

    private bool canAttack = true;
    public float AttackRange => attackRange;


    void Awake()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
                player = p.transform;
        }
    }

    void Update()
    {
        if (!canAttack || player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            TriggerAttack();
        }
    }

    void TriggerAttack()
    {
        canAttack = false;
        animator.SetTrigger("attack");
        Invoke(nameof(ResetCooldown), attackCooldown);
    }

    // Animation Event
    public void PerformAttack()
    {
        attackController.PerformAttack(attackData);
    }

    void ResetCooldown()
    {
        canAttack = true;
    }

    public void OnDeath()
    {
        enabled = false;
    }
}
