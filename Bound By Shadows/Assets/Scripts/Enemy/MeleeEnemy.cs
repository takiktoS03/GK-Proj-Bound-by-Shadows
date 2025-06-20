using System.Collections;
using UnityEngine;

/**
 *  Skrypt obsługujący przeciwników atakujących wręcz
 *  Korzysta z RaycastHit2D aby wykryć czy gracz jest w pobliżu za pomocą boxCollider
 *
 *  Autor: Filip Kudła  
 */
public class MeleeEnemy : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private AttackData data;
    [SerializeField] private float range;

    [Header("Collider Parameters")]
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;

    private Animator anim;
    private PatrolEnemy patrolEnemy;
    private bool isAttacking;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        patrolEnemy = GetComponentInParent<PatrolEnemy>();
    }

    private void Update()
    {
        if (isAttacking) return;

        bool playerDetected = PlayerInSight();

        if (playerDetected)
        {
            StartCoroutine(DamagePlayer());
        }

        if (patrolEnemy != null)
        {
            patrolEnemy.enabled = !playerDetected;
        }
    }

    private bool PlayerInSight()
    {
        // Raycast z boxCollidera wykrywający gracza
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x, new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    private IEnumerator DamagePlayer()
    {
        isAttacking = true;
        anim.SetTrigger("Attack");

        if (patrolEnemy != null)
            patrolEnemy.enabled = false;

        yield return new WaitForSeconds(data.cooldown);

        isAttacking = false;
    }

}
