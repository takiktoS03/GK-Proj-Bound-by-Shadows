using UnityEngine;


/**
 * @class PatrolEnemy
 * @brief Skrypt odpowiedzialny za patrolowanie przeciwnika między dwoma punktami.
 *
 * Przeciwnik przemieszcza się w lewo i prawo między dwoma granicami.
 * Zatrzymuje się na chwilę na krańcach i zmienia kierunek.
 *
 * @author Filip Kudła
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

    /** @brief Zapisuje początkową skalę przeciwnika */
    private void Awake()
    {
        initScale = enemy.localScale;
        //anim = GetComponentInChildren<Animator>();
        //rb = GetComponent<Rigidbody2D>();
    }

    /** @brief Zatrzymuje animację obiektu */
    private void OnDisable()
    {
        if (anim != null)
        {
            anim.SetBool("Moving", false);
        }
    }

    /** @brief Obsługuje logikę patrolowania i zmianę kierunku */
    private void Update()
    {
        if (enemy == null) // poprawka, gdy przeciwnik zginie
        {
            Destroy(gameObject);
            return;
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

    /** @brief Zatrzymuje ruch i odlicza czas przed zmianą kierunku */
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

    /**
     * @brief Porusza przeciwnika w zadanym kierunku
     * @param direction -1 dla lewo, 1 dla prawo
     */
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
