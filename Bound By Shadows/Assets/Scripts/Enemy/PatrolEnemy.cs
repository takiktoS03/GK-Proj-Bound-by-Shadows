using UnityEngine;


/**
 *  Skrypt obsługujący patrolowanie obszaru przez przeciwnika od punktu do punktu
 *  
 *  Autor: Filip Kudła
 */
public class PatrolEnemy : MonoBehaviour
{
    [Header ("Patrol Points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("Patrolling Enemy")]
    [SerializeField] private Transform enemy;
    [SerializeField] private Animator anim;

    [Header("Movement parameters")]
    [SerializeField] private float speed;
    [SerializeField] private float idleDuration;

    private float idleTimer;
    private Vector3 initScale;
    private bool movingLeft;
    //private Rigidbody2D rb;


    private void Awake()
    {
        initScale = enemy.localScale;
        //anim = GetComponentInChildren<Animator>();
        //rb = GetComponent<Rigidbody2D>();
    }

    private void OnDisable()
    {
        anim.SetBool("Moving", false);
    }

    private void Update()
    {
        if (enemy == null) // poprawka, gdy przeciwnik zginie
        {
            Destroy(gameObject);
        }
        if (movingLeft)
        {
            if(enemy.position.x > leftEdge.position.x)
            {
                MoveInDirection(-1);
            }
            else
            {
                ChangeDirection();
            }
        }
        else
        {
            if (enemy.position.x < rightEdge.position.x)
            {
                MoveInDirection(1);
            }
            else
            {
                ChangeDirection();
            }
        }
    }

    private void ChangeDirection()
    {
        anim.SetBool("Moving", false);
        idleTimer += Time.deltaTime;
        
        if(idleTimer > idleDuration)
        {
            movingLeft = !movingLeft;
            idleTimer = 0;
        }
    }

    private void MoveInDirection(int direction)
    {
        anim.SetBool("Moving", true);
        //idleTimer = 0;

        //Facing in direction
        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * direction, initScale.y, initScale.z);

        //Moving in direction
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * direction * speed, enemy.position.y, enemy.position.z);
        //rb.MovePosition(new Vector2(transform.position.x + Time.deltaTime * direction * speed, transform.position.y));
    }
}
