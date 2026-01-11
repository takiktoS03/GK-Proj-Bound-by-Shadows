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
public class PatrolEnemy : MonoBehaviour, IEnemyMovement
{
    [Header("Patrol Points")]
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
    private bool isDead = false;

    //private Rigidbody2D rb;

    /** @brief Zapisuje początkową skalę przeciwnika */
    private void Awake()
    {
        initScale = enemy.localScale;
    }

    private void Start()
    {
        var health = GetComponentInChildren<Health>();

        // 2. Jeśli znaleźliśmy, automatycznie dopisujemy naszą metodę do listy zdarzeń
        if (health != null)
        {
            // To jest to samo co "Plusik" w Inspectorze, ale robione przez kod
            health.onDeath.AddListener(OnEnemyDeath);
        }
        else
        {
            Debug.LogWarning($"Szkielet {name} nie ma komponentu Health w dzieciach!");
        }
    }

    /** @brief Obsługuje logikę patrolowania i zmianę kierunku */
    private void Update()
    {
        if (isDead) return;

        if (movingLeft)
        {
            if (enemy.position.x > leftEdge.position.x)
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

    public void SetMovementEnabled(bool isEnabled)
    {
        if (isDead) // Jeśli martwy, ignorujemy prośby o włączenie ruchu
        {
            this.enabled = false;
            return;
        }

        this.enabled = isEnabled;
        if (!isEnabled && anim != null)
            anim.SetBool("Moving", false);
    }

    /** @brief Zatrzymuje ruch i odlicza czas przed zmianą kierunku */
    private void ChangeDirection()
    {
        anim.SetBool("Moving", false);

        idleTimer += Time.deltaTime;

        if (idleTimer > idleDuration)
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

        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * direction, initScale.y, initScale.z);

        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * direction * speed, enemy.position.y, enemy.position.z);
    }
    public void OnGameLoaded()
    {
        idleTimer = 0f;
        movingLeft = false;

        if (anim != null)
            anim.SetBool("Moving", false);
    }
    public void OnEnemyDeath()
    {
        isDead = true;
        this.enabled = false;

        if (anim != null)
        {
            anim.SetBool("Moving", false);
            anim.SetTrigger("Death");
        }

        var rb = GetComponentInParent<Rigidbody2D>();
        if (rb == null) rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        var saveable = GetComponent<SaveableObject>();
        if (saveable == null) saveable = GetComponentInParent<SaveableObject>();
        if (saveable != null) DestroyedRegistry.MarkDestroyed(saveable.UniqueId);

        var dissolve = GetComponentInChildren<DissolveEffect>();
        if (dissolve == null) dissolve = GetComponent<DissolveEffect>();

        if (dissolve != null)
        {
            dissolve.PlayDissolve(2f, true, () =>
            {
                if (transform.parent != null) Destroy(transform.parent.gameObject);
                else Destroy(gameObject);
            },
            1f);
        }
        else
        {
            Destroy(transform.parent != null ? transform.parent.gameObject : gameObject, 3f);
        }
    }
}