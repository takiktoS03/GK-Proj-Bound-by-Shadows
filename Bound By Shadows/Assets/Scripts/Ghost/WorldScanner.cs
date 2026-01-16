using UnityEngine;
using UnityEngine.Rendering.Universal;

/**
 * Skrypt realizujący umiejętność skanowania otoczenia, wykrywając ukryte obiekty
 * i aktywując efekty wizualne w zasięgu skanu.
 *
 * @author Filip Kudła
 */
public class WorldScanner : MonoBehaviour
{
    [Header("Scan Settings")]
    public float maxRadius = 20f;
    public float scanSpeed = 5f;
    public int rayCount = 72;

    [Header("Visuals")]
    public Light2D scanLight;
    public float visualMultiplier = 1f;
    public float lightIntensity = 2f;

    [Header("Layers")]
    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [Header("Ability Settings")]
    [SerializeField] private bool isAbilityUnlocked = false;
    public float abilityCooldown = 5f;

    private float scanRadius = 0f;
    private bool isScanning = false;
    private float cooldownTimer = 0f;

    void Start()
    {
        if (scanLight != null)
        {
            scanLight.enabled = false;
            scanLight.transform.localPosition = Vector3.zero;
        }
    }

    void Update()
    {
        if (isAbilityUnlocked && Input.GetKeyDown(KeyCode.Q))
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

    public void UnlockAbility()
    {
        isAbilityUnlocked = true;
    }

    public void LockAbility()
    {
        isAbilityUnlocked = false;
    }

    void TryActivateScan()
    {
        if (cooldownTimer > 0 || isScanning) return;

        isScanning = true;
        scanRadius = 0f;
        cooldownTimer = abilityCooldown;

        if (scanLight != null)
        {
            scanLight.enabled = true;
            scanLight.intensity = lightIntensity;
            scanLight.transform.localScale = Vector3.zero;
        }
    }

    void ScanStep()
    {
        scanRadius += scanSpeed * Time.deltaTime;

        // Synchronizacja wizualna
        if (scanLight != null)
        {
            float diameter = scanRadius * 2f * visualMultiplier;
            scanLight.transform.localScale = new Vector3(diameter, diameter, 1f);
        }

        CastRays();

        if (scanRadius >= maxRadius)
        {
            StopScan();
        }
    }

    void StopScan()
    {
        isScanning = false;
        scanRadius = 0f;

        if (scanLight != null)
        {
            scanLight.enabled = false;
            scanLight.transform.localScale = Vector3.zero;
        }
    }

    void CastRays()
    {
        Vector2 origin = transform.position;

        for (int i = 0; i < rayCount; i++)
        {
            float angle = i * Mathf.PI * 2f / rayCount;
            Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            // Promień do przeszkód
            var hitObstacle = Physics2D.Raycast(origin, dir, scanRadius, obstacleMask);

            // Obliczenie długości promienia
            float hitDistance = hitObstacle.collider ? hitObstacle.distance : scanRadius;
            Vector2 hitPoint = origin + (dir * hitDistance);

            // Debug - rysowanie promieni w gizmos
            if (hitObstacle.collider != null)
            {
                // Przeszkoda - czerwony
                Debug.DrawLine(origin, hitPoint, Color.red);
            }
            else
            {
                // Brak - zielony
                Debug.DrawLine(origin, hitPoint, Color.green);
            }

            // Promień tylko do momentu uderzenia w ścianę (hitDistance)
            var hitTarget = Physics2D.Raycast(origin, dir, hitDistance, targetMask);

            if (hitTarget.collider != null)
            {
                // promień trafienia
                Debug.DrawLine(origin, hitTarget.point, Color.yellow);

                FresnelTrigger fresnel = hitTarget.collider.GetComponent<FresnelTrigger>();
                if (fresnel != null)
                {
                    fresnel.PulseFresnel();
                }
            }
        }
    }
}