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
    [SerializeField] private float range = 1f;

    [Header("Collider Parameters")]
    [SerializeField] private DefaultFacingDirection defaultSpriteFacing = DefaultFacingDirection.Right;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;

    [Space(10)]
    [SerializeField] private float prepDissolveTime;

    private Animator anim;
    private IEnemyMovement enemyMovement;
    private DissolveEffect dissolveEffect;
    private enum DefaultFacingDirection { Right, Left }
    private bool isAttacking;

    // Dodaj to do MeleeEnemy.cs
    private void Start()
    {
        // Szukamy zdrowia na tym samym obiekcie (bo MeleeEnemy i Health są sąsiadami)
        var health = GetComponent<Health>();

        if (health != null)
        {
            health.onDeath.AddListener(OnEnemyDeath);
        }
    }
    /** @brief Inicjalizacja referencji do komponentów */
    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyMovement = GetComponentInParent<IEnemyMovement>();
        dissolveEffect = GetComponent<DissolveEffect>();
    }

    /** @brief Wykrywa gracza i inicjuje atak jeśli jest w zasięgu */
    private void Update()
    {
        if (isAttacking) return;

        bool playerDetected = PlayerInSight();

        if (enemyMovement != null)
        {
            enemyMovement.SetMovementEnabled(!playerDetected);
        }

        if (playerDetected)
        {
            StartCoroutine(DamagePlayer());
        }
    }

    /**
     * @brief Oblicza aktualny wektor "przodu" postaci.
     * Bierze pod uwagę to, czy sprite domyślnie patrzy w lewo/prawo
     * ORAZ to, czy jest aktualnie obrócony (localScale.x).
     */
    private Vector2 GetFacingDirection()
    {
        float currentScaleDir = Mathf.Sign(transform.localScale.x);
        float defaultDir = (defaultSpriteFacing == DefaultFacingDirection.Right) ? 1f : -1f;
        return (currentScaleDir * defaultDir > 0) ? Vector2.right : Vector2.left;
    }

    /**
     * @brief Sprawdza obecność gracza przy pomocy BoxCast
     * @return true jeśli gracz w zasięgu
     */
    private bool PlayerInSight()
    {
        Vector2 direction = GetFacingDirection();
        Vector3 origin = boxCollider.bounds.center + (Vector3)(direction * range);

        RaycastHit2D hit = Physics2D.BoxCast(
            origin,
            boxCollider.bounds.size,
            0,
            direction,
            0,
            playerLayer);

        return hit.collider != null;
    }

    /** @brief Rysuje gizmo zasięgu wykrywania gracza w edytorze */
    private void OnDrawGizmos()
    {
        if (boxCollider == null) return;
        Vector2 direction = GetFacingDirection();
        Vector3 origin = boxCollider.bounds.center + (Vector3)(direction * range);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(origin, boxCollider.bounds.size);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(boxCollider.bounds.center, origin);
    }

    /**
     * @brief Wykonuje atak z animacją i opóźnieniem (cooldown)
     * @return IEnumerator do użycia w coroutine
     */
    private IEnumerator DamagePlayer()
    {
        isAttacking = true;

        if (enemyMovement != null) enemyMovement.SetMovementEnabled(false);

        //anim.ResetTrigger("Hurt");
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(data.cooldown);
        isAttacking = false;
    }

    public void OnEnemyDeath()
    {
        // 1. ZATRZYMANIE RUCHU (Rodzic)
        if (enemyMovement != null)
        {
            enemyMovement.SetMovementEnabled(false);
        }

        // Wyłączamy logikę ataku
        this.enabled = false;

        // 2. ZATRZYMANIE ANIMACJI
        if (anim != null)
        {
            // Resetujemy ewentualne flagi ruchu/ataku
            anim.SetBool("Moving", false);
            anim.ResetTrigger("Attack"); // Przerywamy atak jeśli trwał
            anim.SetTrigger("Death");
        }

        // 3. ZATRZYMANIE FIZYKI
        var rb = GetComponentInParent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.constraints = RigidbodyConstraints2D.FreezeAll; // Zatrzymuje w miejscu
        }

        // 4. ZAPISYWANIE
        var saveable = GetComponentInParent<SaveableObject>();
        if (saveable != null)
        {
            DestroyedRegistry.MarkDestroyed(saveable.UniqueId);
        }

        // 5. EFEKT DISSOLVE
        var dissolve = GetComponentInChildren<DissolveEffect>();
        if (dissolve == null) dissolve = GetComponent<DissolveEffect>();

        if (dissolve != null)
        {
            // Opóźnienie 1.0f pozwala animacji "Death" odegrać się w pełni
            dissolve.PlayDissolve(2f, true, () =>
            {
                if (transform.parent != null) Destroy(transform.parent.gameObject);
                else Destroy(gameObject);
            },
            1.0f);
        }
        else
        {
            Destroy(transform.parent != null ? transform.parent.gameObject : gameObject, 3f);
        }
    }
}