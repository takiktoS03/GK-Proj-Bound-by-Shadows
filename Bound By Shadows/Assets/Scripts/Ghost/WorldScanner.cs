using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class WorldScanner : MonoBehaviour
{
    [Header("Scan Settings")]
    public float maxRadius = 10f;
    public float scanSpeed = 5f;
    public int rayCount = 72; // co 5 stopni

    [Header("Layers")]
    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [Header("Cooldown")]
    public float abilityCooldown = 5f;

    private SpriteRenderer scanRenderer;
    private Material scanMat;
    private float scanRadius = 0f;
    private bool isScanning = false;
    private float cooldownTimer = 0f;

    void Start()
    {
        scanRenderer = GetComponent<SpriteRenderer>();
        scanMat = scanRenderer.material;
        scanMat.SetFloat("_MaxRadius", maxRadius);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TryActivateScan();
        }

        if (cooldownTimer > 0)
            cooldownTimer -= Time.deltaTime;

        if (isScanning)
        {
            ScanStep();
        }
    }

    void TryActivateScan()
    {
        if (cooldownTimer > 0) return;      // jeszcze trwa cooldown
        if (isScanning) return;             // już skanujemy

        scanRenderer.enabled = true;
        isScanning = true;
        scanRadius = 0f;
        cooldownTimer = abilityCooldown;    // ustaw cooldown
    }

    void ScanStep()
    {
        scanRadius += scanSpeed * Time.deltaTime;
        scanMat.SetFloat("_ScanRadius", scanRadius);

        CastRays();

        // koniec skanu
        if (scanRadius >= maxRadius)
        {
            isScanning = false;
            scanRadius = 0f;
            scanMat.SetFloat("_ScanRadius", scanRadius);
            scanRenderer.enabled = false;
        }
    }

    void CastRays()
    {
        Vector2 origin = transform.position;
        for (int i = 0; i < rayCount; i++)
        {
            float angle = i * Mathf.PI * 2f / rayCount;
            Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            // jeżeli coś blokuje promień — kończymy ray
            var hitObstacle = Physics2D.Raycast(origin, dir, scanRadius, obstacleMask);

            if (hitObstacle.collider != null)
            {
                Debug.DrawLine(origin,
                               origin + dir * hitObstacle.distance,
                               Color.red);

                continue;
            }

            // Sprawdź cele (ScanTarget)
            var hitTarget = Physics2D.Raycast(origin, dir, scanRadius, targetMask);

            if (hitTarget.collider != null)
            {
                Debug.DrawLine(origin,
                               hitTarget.point,
                               Color.yellow);

                var fresnel = hitTarget.collider.GetComponent<FresnelTrigger>();
                if (fresnel != null)
                    fresnel.PulseFresnel();
            }
            else
            {
                Debug.DrawLine(origin,
                               origin + dir * scanRadius,
                               Color.green);
            }
            float hitDistance = hitTarget.collider ? hitTarget.distance : scanRadius;//hitobstacle

            // sprawdzamy czy bloczek jest W TRAKCIE promienia
            var hit = Physics2D.Raycast(origin, dir, hitDistance, targetMask);
            if (hit)
            {
                FresnelTrigger fresnel = hit.collider.GetComponent<FresnelTrigger>();
                if (fresnel != null)
                {
                    fresnel.PulseFresnel();
                }
            }
        }
    }
}
