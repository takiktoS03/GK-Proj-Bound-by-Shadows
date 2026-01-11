using System.Collections;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour, IEnemyMovement
{
    [Header("References")]
    [SerializeField] private Transform enemy;
    [SerializeField] private Transform player;

    [Header("Patrol Settings")]
    [SerializeField] private bool useTransforms = false;
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float patrolWidth = 6f;
    [SerializeField] private float patrolSpeed = 2f;

    [Header("Sinusoidal Movement (Idle)")]
    [SerializeField] private float waveAmplitude = 0.5f;
    [SerializeField] private float waveFrequency = 2f;

    [Header("Chase Settings")]
    [SerializeField] private float chaseSpeed = 3.5f;
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float stopDistance = 1.0f;

    private Vector3 startPosition;
    private Vector3 targetPatrolPoint;
    private bool movingToB = true;
    private float sinTime;
    private bool canMove = true;
    private Vector3 initialScale;
    private Rigidbody2D rb;

    private void Start()
    {
        // USUNĄŁEM kod od Health. Nietoperz nie powinien sam nasłuchiwać śmierci.
        // Od tego jest MeleeEnemy.

        startPosition = transform.position;

        if (enemy != null)
            initialScale = enemy.localScale;

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
        RecalculatePatrolTarget();
    }

    private void Update()
    {
        if (!canMove || enemy == null) return;

        if (player == null) return;

        float distToPlayer = Vector2.Distance(transform.position, player.position);

        if (distToPlayer < detectionRange)
        {
            ChasePlayer(distToPlayer);
        }
        else
        {
            Patrol();
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Patrol()
    {
        // Wyznaczanie celu (A/B lub lewo/prawo od startu)
        Vector3 target = targetPatrolPoint;

        // Utrzymuj stałą wysokość Y (sinusoida startowa + wave)
        sinTime += Time.deltaTime * waveFrequency;
        float waveY = Mathf.Sin(sinTime) * waveAmplitude;

        // Celujemy w X celu, ale Y modyfikujemy falą
        Vector3 moveTarget = new Vector3(target.x, startPosition.y + waveY, transform.position.z);

        transform.position = Vector2.MoveTowards(transform.position, moveTarget, patrolSpeed * Time.deltaTime);

        HandleRotation(moveTarget.x);

        if (Mathf.Abs(transform.position.x - target.x) < 0.2f)
        {
            movingToB = !movingToB;
            RecalculatePatrolTarget();
        }
    }

    private void RecalculatePatrolTarget()
    {
        if (useTransforms && pointA != null && pointB != null)
        {
            targetPatrolPoint = movingToB ? pointB.position : pointA.position;
        }
        else
        {
            // Patrol względem punktu startowego
            float offset = movingToB ? patrolWidth : -patrolWidth;
            targetPatrolPoint = startPosition + Vector3.right * offset;
        }
    }

    private void ChasePlayer(float distance)
    {
        HandleRotation(player.position.x);

        // Jeśli jesteśmy dalej niż dystans ataku -> lecimy do gracza
        if (distance > stopDistance)
        {
            // Podążamy bezpośrednio do pozycji gracza (także w osi Y)
            transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
        }
        // Jeśli distance <= stopDistance, skrypt po prostu przestaje przesuwać transform.
        // Wtedy MeleeEnemy (który jest na dziecku) wykryje gracza swoim BoxCastem i odpali atak.
    }

    private void HandleRotation(float targetX)
    {
        if (enemy == null) return;

        float direction = targetX - transform.position.x;
        if (Mathf.Abs(direction) < 0.1f) return; // Martwa strefa
        float targetScaleX = (direction > 0) ? -1f : 1f; // sprite domyślnie obrócony w lewo
        enemy.localScale = new Vector3(Mathf.Abs(initialScale.x) * targetScaleX, initialScale.y, initialScale.z);
    }

    // Implementacja interfejsu IEnemyMovement
    public void SetMovementEnabled(bool isEnabled)
    {
        canMove = isEnabled;
    }

    public void ResetAfterLoad()
    {
        canMove = true;
        sinTime = 0f;

        startPosition = transform.position;
        RecalculatePatrolTarget();

        StartCoroutine(EnableMovementNextFrame());
    }

    private IEnumerator EnableMovementNextFrame()
    {
        yield return null;
        canMove = true;
    }
    public void OnGameLoaded()
    {
        startPosition = transform.position;

        sinTime = 0f;
        canMove = true;

        if (rb != null)
            rb.linearVelocity = Vector2.zero;
    }

    public void OnDeath()
    {
        if (!canMove) return; // Zapobiega wielokrotnemu wywołaniu

        canMove = false; // Zatrzymuje pętlę Update (ruch)

        // 1. Wyłącz fizykę, żeby nie popychał gracza
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false; // Wyłącza kolizje i fizykę Rigidbody
        }

        // 2. Wyłącz collidery (żeby gracz nie otrzymywał obrażeń wchodząc w "trupa")
        foreach (var col in GetComponentsInChildren<Collider2D>())
        {
            col.enabled = false;
        }

        // 3. Wyłącz skrypty ataku na dzieciach (np. MeleeEnemy)
        foreach (Transform child in transform)
        {
            // Wyłącza wszystkie skrypty MonoBehaviour na dzieciach
            foreach (var component in child.GetComponents<MonoBehaviour>())
            {
                component.enabled = false;
            }
        }

        // Obsługa zapisu (Twoja oryginalna logika)
        var saveable = GetComponent<SaveableObject>();
        if (saveable != null)
        {
            DestroyedRegistry.MarkDestroyed(saveable.UniqueId);
        }

        // Zniszcz obiekt (ewentualnie z opóźnieniem dla animacji: Destroy(gameObject, 1f));
        Destroy(gameObject);
    }
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stopDistance);

        if (!useTransforms)
        {
            Vector3 center = Application.isPlaying ? startPosition : transform.position;
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(center + Vector3.left * patrolWidth, center + Vector3.right * patrolWidth);
        }
    }
#endif
}