using UnityEngine;


/**
 *  Skrypt obslugujacy przeciwnikow atakujacych wrecz
 *  Korzysta z RaycastHit2D aby wykryc czy gracz jest w poblizu za pomoca boxCollider
 *
 *  Autor: Filip Kudla  
 */
public class MeleeEnemy : MonoBehaviour
{
    [Header ("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    //[SerializeField] private float damage;
    [SerializeField] private float range;

    [Header("Collider Parameters")]
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;

    private float cooldownTimer;
    private Animator anim;
    private Health playerHealth;
    private Health enemyHealth;

    private PatrolEnemy patrolEnemy;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        patrolEnemy = GetComponentInParent<PatrolEnemy>();
        enemyHealth = GetComponent<Health>();
    }

    void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (PlayerInSight()) 
        {
            if(cooldownTimer >= attackCooldown)
            {
                //Attack
                cooldownTimer = 0;
                //DamagePlayer();
                anim.SetTrigger("Attack");

            }
        }

        if (patrolEnemy != null)
        {
            patrolEnemy.enabled = !PlayerInSight();
        }
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        if(hit.collider != null)
        {
            playerHealth = hit.transform.GetComponent<Health>();
        }

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x, new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    private void DamagePlayer()
    {
        if (PlayerInSight()) 
        {
            anim.SetTrigger("Attack");
        }    
    }
}
