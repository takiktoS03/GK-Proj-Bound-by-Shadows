using System;
using UnityEngine;

public class BatMovement : MonoBehaviour
{
    [Header("Patrol (relative to start position)")]
    public float patrolWidth = 6f;
    public float patrolSpeed = 2f;
    public float waitTimeAtEdge = 1f;

    [Header("Chase Settings")]
    public Transform player;
    public float chaseSpeed = 3.5f;
    public float detectionRange = 5f;    // Odleg?o?? wykrycia gracza (start po?cigu)
    public float loseAggroRange = 8f;    // Odleg?o?? zgubienia gracza (powrót do patrolu)
    public float stopDistance = 1.2f;    // Dystans zatrzymania si? przed graczem (do ataku)

    [Header("Flying")]
    public float heightOffset = 0f;
    public float heightSmooth = 5f;

    [Header("Sinusoidal Movement")]
    public float waveAmplitude = 0.5f;
    public float waveFrequency = 2f;

    private Vector2 startPosition;
    private Vector2 leftPoint;
    private Vector2 rightPoint;

    private bool movingRight = true;
    private float waitTimer;
    private bool isChasing = false;
    private SpriteRenderer sprite;
    private float sinTime;

    void Start()
    {
        startPosition = transform.position;
        sprite = GetComponentInChildren<SpriteRenderer>();
        sinTime = UnityEngine.Random.Range(0f, 2f * Mathf.PI);

        leftPoint = startPosition + Vector2.left * patrolWidth;
        rightPoint = startPosition + Vector2.right * patrolWidth;

        waitTimer = waitTimeAtEdge;

        // Automatyczne znalezienie gracza, je?li nie przypisano w Inspectorze
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        // Sprawdzanie dystansu do gracza
        float distToPlayer = Vector2.Distance(transform.position, player.position);

        // Logika prze??czania stanów (Patrol <-> Po?cig)
        if (!isChasing)
        {
            // Je?li jest w zasi?gu wykrycia -> zacznij goni?
            if (distToPlayer < detectionRange)
            {
                isChasing = true;
            }
            else
            {
                MaintainHeight(); // Falowanie tylko w patrolu
                Patrol();
            }
        }
        else
        {
            // Je?li gracz uciek? za daleko -> wróc do patrolu
            if (distToPlayer > loseAggroRange)
            {
                isChasing = false;
                // Opcjonalnie: powrót na startPosition, tutaj po prostu wróci do logiki patrolu
            }
            else
            {
                ChasePlayer(distToPlayer);
            }
        }
    }

    void HandleFlip(float targetX)
    {
        if (sprite == null) return;

        float diff = targetX - transform.position.x;
        // Ma?a strefa martwa, ?eby nie migota? przy obracaniu
        if (Mathf.Abs(diff) < 0.1f) return;

        sprite.flipX = diff > 0;
    }

    void Patrol()
    {
        Vector2 target = movingRight ? rightPoint : leftPoint;
        HandleFlip(target.x);

        transform.position = Vector2.MoveTowards(
            transform.position,
            target,
            patrolSpeed * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, target) < 0.05f)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f)
            {
                movingRight = !movingRight;
                waitTimer = waitTimeAtEdge;
            }
        }
    }

    void ChasePlayer(float distance)
    {
        HandleFlip(player.position.x);

        // Je?li jeste?my dalej ni? stopDistance, lecimy w stron? gracza
        if (distance > stopDistance)
        {
            // Celujemy dok?adnie w pozycj? gracza (X i Y), ?eby móc go dosi?gn??
            // Opcjonalnie mo?na doda? offset w Y (np. player.position.y + 0.5f), ?eby atakowa? "g?ow?"
            Vector2 target = player.position;

            transform.position = Vector2.MoveTowards(
                transform.position,
                target,
                chaseSpeed * Time.deltaTime
            );
        }
        // Je?li distance <= stopDistance, nietoperz stoi w miejscu
        // Tutaj skrypt BatAttack.cs przejmie rol? i wykona atak, bo gracz jest w zasi?gu
    }

    void MaintainHeight()
    {
        sinTime += Time.deltaTime * waveFrequency;

        float baseY = startPosition.y + heightOffset;
        float wave = Mathf.Sin(sinTime) * waveAmplitude;

        Vector2 pos = transform.position;
        pos.y = Mathf.Lerp(pos.y, baseY + wave, Time.deltaTime * heightSmooth);
        transform.position = pos;
    }

    // Metody pomocnicze dla innych skryptów (opcjonalne)
    public void StartChase(Transform target)
    {
        player = target;
        isChasing = true;
    }

    public void StopChase()
    {
        isChasing = false;
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        // Rysowanie ?cie?ki patrolu
        Vector2 pos = Application.isPlaying ? startPosition : (Vector2)transform.position;
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(pos + Vector2.left * patrolWidth, pos + Vector2.right * patrolWidth);

        // Rysowanie zasi?gów
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange); // Zasi?g wykrycia
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stopDistance);   // Gdzie si? zatrzyma
    }
#endif
}