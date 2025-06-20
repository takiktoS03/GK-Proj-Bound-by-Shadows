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
    [SerializeField] private float range;

    [Header("Collider Parameters")]
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;

    private Animator anim;

    private PatrolEnemy patrolEnemy;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        patrolEnemy = GetComponentInParent<PatrolEnemy>();
    }

    void Update()
    {
        if (PlayerInSight())
        {
            anim.SetTrigger("Attack");
            Debug.Log("Attack szkieletora");
        }
        if (patrolEnemy != null)
        {
            patrolEnemy.enabled = !PlayerInSight();
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
}
