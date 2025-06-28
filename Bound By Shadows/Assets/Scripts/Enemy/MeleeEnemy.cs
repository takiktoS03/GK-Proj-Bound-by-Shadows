using System.Collections;
using UnityEngine;

/**
 * @class MeleeEnemy
 * @brief Skrypt przeciwnika atakującego wręcz, wykrywającego gracza za pomocą BoxCast.
 *
 * Przeciwnik patroluje do momentu wykrycia gracza, a następnie wykonuje animację ataku z określonym cooldownem.
 * 
 * @author Filip Kudła
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

    /** @brief Inicjalizacja referencji do komponentów */
    private void Awake()
    {
        anim = GetComponent<Animator>();
        patrolEnemy = GetComponentInParent<PatrolEnemy>();
    }

    /** @brief Wykrywa gracza i inicjuje atak jeśli jest w zasięgu */
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

    /**
     * @brief Sprawdza obecność gracza przy pomocy BoxCast
     * @return true jeśli gracz w zasięgu
     */
    private bool PlayerInSight()
    {
        // Raycast z boxCollidera wykrywający gracza
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        return hit.collider != null;
    }

    /** @brief Rysuje gizmo zasięgu wykrywania gracza w edytorze */
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x, new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    /**
     * @brief Wykonuje atak z animacją i opóźnieniem (cooldown)
     * @return IEnumerator do użycia w coroutine
     */
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
