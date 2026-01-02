using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
// JOB SYSTEM
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.Scripting;

/**
 * @enum EmissionShape
 * @brief Okre?la kszta?t obszaru emisji cz?steczek.
 */
public enum EmissionShape { Point, Line, Circle, Area }

/**
 * @struct ParticleData
 * @brief Struktura przechowuj?ca dane pojedynczej cz?steczki.
 */
public struct ParticleData
{
    public Vector2 position;
    public Vector2 prevPosition;
    public Vector2 velocity;
    public float lifetime;
    public float age;
    public int alive; 
}

/**
 * @enum ParticleCollisionMode
 * @brief Typ reakcji cz?steczki na kolizj?.
 */
public enum ParticleCollisionMode
{
    None,       // brak kolizji
    Stop,       // zatrzymanie na powierzchni
    Bounce,     // odbicie
    Stick,      // przyklejenie si?
    Slide       // ?lizganie po powierzchni
}

/**
 * @struct ParticleUpdateJob
 * @brief Job odpowiedzialny za aktualizacj? fizyki cz?steczek.
 */
[BurstCompile]
public struct ParticleUpdateJob : IJobParallelForTransform
{
    public NativeArray<ParticleData> particles;

    public float deltaTime;
    public Vector2 gravity;
    public float airResistance;
    public float bounceFactor;
    public Vector2 wind;

    /**
    * @brief Aktualizuje pozycj? i pr?dko?? cz?steczki.
    *
    * @param index Indeks cz?steczki
    * @param transform Transform przypisany do cz?steczki
    */
    public void Execute(int index, TransformAccess transform)
    {
        ParticleData p = particles[index];

        if (p.alive == 0)
            return;

        p.prevPosition = p.position;

        p.age += deltaTime;
        if (p.age > p.lifetime)
        {
            p.alive = 0;
            particles[index] = p;
            return;
        }

        // fizyka
        p.velocity += (gravity + wind) * deltaTime;
        p.position += p.velocity * deltaTime;

        p.velocity *= Mathf.Pow(airResistance, deltaTime * 60f);

        transform.position = p.position;
        particles[index] = p;

    }
}

/**
 * @class ParticleSystem2D
 * @brief W?asny system cz?steczek 2D oparty o Job System.
 *
 * Skrypt obs?uguje emisj?, fizyk?, kolizje, renderowanie
 * oraz prac? z presetami efektów cz?steczek.
 *
 * @author Julia Bigaj
 */
public class ParticleSystem2D : MonoBehaviour
{
    [Header("Basic parameters")]
    public Material particleMaterial;
    public Sprite particleSprite;
    public float particleSize = 0.2f;
    public int emissionRate = 20;
    public float particleLifetime = 2f;

    [Header("Speed")]
    public float minSpeed = 1f;
    public float maxSpeed = 3f;
    public float directionAngle = 90f;
    public float spread = 30f;

    [Header("Emission shape")]
    public EmissionShape emissionShape = EmissionShape.Point;
    public float emissionRadius = 1f;
    public Vector2 emissionArea = new Vector2(2f, 1f);

    [Header("Color and scaling")]
    public Gradient colorOverLifetime;
    public AnimationCurve scaleOverLifetime = AnimationCurve.Linear(0, 1f, 1, 1f);
    public AnimationCurve alphaOverLifetime = AnimationCurve.Linear(0, 1f, 1, 0f);

    [Header("Physics")]
    public bool enableGravity = true;
    public Vector2 gravity = new Vector2(0f, -9.81f);
    public float airResistance = 0.98f;
    public float bounceFactor = 0.5f;
    public Vector2 wind = Vector2.zero;

    [Header("Collision 2D")]
    public bool useCollision = false;
    public ParticleCollisionMode collisionMode = ParticleCollisionMode.Stop;
    public LayerMask collisionMask = ~0;

    [Header("Rendering")]
    [HideInInspector] public string sortingLayer = "Default";
    [HideInInspector] public int orderInLayer = 0;

    [Header("Performance")]
    [Min(1)]
    public int maxParticles = 2000;

    [Header("Preset")]
    public ParticleEffectPreset preset;
    private bool isRebuilding = false;

    [SerializeField]
    public ParticleEffectPreset overridePresetData;


    private Transform particleRoot;

    // JOB SYSTEM – dane
    private NativeArray<ParticleData> particleArray;
    private TransformAccessArray transformArray;
    private SpriteRenderer[] spriteRenderers;
    private Stack<int> freeIndices;

    private float emissionTimer = 0f;

    // testowanie
    private Stopwatch updateTimer = new Stopwatch();
    private float fpsTimer = 0f;
    private int frameCount = 0;
    private int logIndex = 0;

    // =======================
    // Unity lifecycle
    // =======================

    /**
     * @brief Inicjalizuje system cz?steczek oraz preset.
     */
    void Start()
    {
        
        if (overridePresetData == null && preset != null)
        {
            overridePresetData = Instantiate(preset);
        }

        ApplyPresetFromOverride();

        InitializeParticleData();

    }

    /**
    * @brief Zwalnia pami?? NativeArray oraz Job Systemu.
    */
    private void OnDestroy()
    {
        if (particleArray.IsCreated) particleArray.Dispose();
        if (transformArray.isCreated) transformArray.Dispose();
    }

    /**
     * @brief G?ówna p?tla aktualizacji systemu cz?steczek.
     *
     * Odpowiada za emisj?, fizyk?, kolizje oraz rendering.
     */
    void Update()
    {
        updateTimer.Restart();

        if (isRebuilding) return;

        float dt = Time.deltaTime;

        emissionTimer += dt;
        float interval = 1f / Mathf.Max(1, emissionRate);
        while (emissionTimer > interval)
        {
            EmitParticle();
            emissionTimer -= interval;
        }

        // job: fizyka cz.
        var job = new ParticleUpdateJob
        {
            particles = particleArray,
            deltaTime = dt,
            gravity = enableGravity ? gravity : Vector2.zero,
            airResistance = airResistance,
            bounceFactor = bounceFactor,
            wind = wind,
        };
        JobHandle handle = job.Schedule(transformArray);
        handle.Complete();

        int aliveCount = 0;

        for (int i = 0; i < maxParticles; i++)
        {
            ParticleData p = particleArray[i];

            if (p.alive == 0)
            {
                if (spriteRenderers[i].enabled)
                {
                    spriteRenderers[i].enabled = false;
                    spriteRenderers[i].transform.localScale = Vector3.zero;

                    if (!freeIndices.Contains(i))
                        freeIndices.Push(i);
                }
                continue;
            }

            if (useCollision)
            {
                Vector2 oldPos = p.prevPosition;
                Vector2 newPos = p.position;
                Vector2 dir = newPos - oldPos;
                float dist = dir.magnitude;

                if (dist > 0.0001f)
                {
                    RaycastHit2D hit = Physics2D.Raycast(
                        oldPos,
                        dir.normalized,
                        dist,
                        collisionMask
                    );

                    if (hit.collider != null)
                    {
                        switch (collisionMode)
                        {
                            case ParticleCollisionMode.Stop:
                                p.position = hit.point;
                                p.velocity = Vector2.zero;
                                break;

                            case ParticleCollisionMode.Bounce:
                                p.position = hit.point;
                                p.velocity = Vector2.Reflect(
                                    p.velocity,
                                    hit.normal.normalized
                                ) * bounceFactor;
                                break;

                            case ParticleCollisionMode.Stick:
                                p.position = hit.point;
                                p.velocity = Vector2.zero;
                                p.age = Mathf.Max(p.age, p.lifetime * 0.7f);
                                break;

                            case ParticleCollisionMode.Slide:
                                p.position = hit.point;
                                {
                                    Vector2 n = hit.normal.normalized;
                                    Vector2 v = p.velocity;
                                    Vector2 vn = Vector2.Dot(v, n) * n; // prostopadla
                                    Vector2 vt = v - vn;                // styczna
                                    p.velocity = vt;
                                }
                                break;

                            case ParticleCollisionMode.None:
                            default:
                                break;
                        }
                    }
                }
            }

            particleArray[i] = p;

            aliveCount++;

            float t = p.age / Mathf.Max(0.0001f, p.lifetime);

            // kolor
            Color newColor = colorOverLifetime.Evaluate(t);
            float alpha = alphaOverLifetime.Evaluate(t);
            newColor.a *= alpha;

            float scale = scaleOverLifetime.Evaluate(t) * particleSize;

            var sr = spriteRenderers[i];
            sr.enabled = true;
            sr.color = newColor;
            sr.transform.position = p.position;                 
            sr.transform.localScale = Vector3.one * scale;
        }

        updateTimer.Stop();
        double ms = updateTimer.Elapsed.TotalMilliseconds;

        UnityEngine.Debug.Log($"Frame: {Time.frameCount} | Update: {ms:F4} ms | Particles: {aliveCount}");

        logIndex++;
        
        FPSCounter();
    }

    /**
     * @brief Rysuje Gizmosy obszaru emisji w edytorze.
     */
    void OnDrawGizmos()
    {
        EmissionShape shape;
        float radius;
        Vector2 area;

        if (overridePresetData != null)
        {
            shape = overridePresetData.emissionShape;
            radius = overridePresetData.emissionRadius;
            area = overridePresetData.emissionArea;
        }
        else
        {
            shape = emissionShape;
            radius = emissionRadius;
            area = emissionArea;
        }

        Gizmos.color = Color.cyan;
        Vector3 pos = transform.position;

        switch (shape)
        {
            case EmissionShape.Point:
                Gizmos.DrawWireSphere(pos, 0.1f);
                break;

            case EmissionShape.Circle:
                Gizmos.DrawWireSphere(pos, radius);
                break;

            case EmissionShape.Line:
                float half = area.x * 0.5f;
                Gizmos.DrawLine(
                    pos + new Vector3(-half, 0, 0),
                    pos + new Vector3(half, 0, 0)
                );
                break;

            case EmissionShape.Area:
                Gizmos.DrawWireCube(pos, new Vector3(area.x, area.y, 0));
                break;
        }
    }

    /**
     * @brief Inicjalizuje dane cz?steczek oraz obiekty renderuj?ce.
     */
    void InitializeParticleData()
    {
        particleArray = new NativeArray<ParticleData>(maxParticles, Allocator.Persistent);
        transformArray = new TransformAccessArray(maxParticles);
        spriteRenderers = new SpriteRenderer[maxParticles];
        freeIndices = new Stack<int>(maxParticles);

        // --- CREATE OR FIND PARTICLE ROOT ---
        if (particleRoot == null)
        {
            var existing = transform.Find("Particles (Children)");
            if (existing != null)
            {
                particleRoot = existing;
            }
            else
            {
                GameObject root = new GameObject("Particles (Children)");
                particleRoot = root.transform;
                particleRoot.SetParent(transform);
                particleRoot.localPosition = Vector3.zero;
            }
        }

        // --- CREATE PARTICLES INSIDE THE ROOT ---
        for (int i = 0; i < maxParticles; i++)
        {
            GameObject go = new GameObject("Particle_" + i);

            go.transform.SetParent(particleRoot);

            var sr = go.AddComponent<SpriteRenderer>();

            sr.material = particleMaterial;
            sr.sprite = particleSprite;
            sr.enabled = false;
            sr.sortingLayerName = sortingLayer;
            sr.sortingOrder = orderInLayer;

            go.transform.position = transform.position;
            go.transform.localScale = Vector3.zero;

            spriteRenderers[i] = sr;

            transformArray.Add(go.transform);
            freeIndices.Push(i);
        }
    }

    /**
     * @brief Emisja pojedynczej cz?steczki.
     */
    void EmitParticle()
    {
        if (freeIndices.Count == 0) return;

        int index = freeIndices.Pop();

        Vector2 pos = GetEmissionPosition();
        float speed = Random.Range(minSpeed, maxSpeed);
        float angle = directionAngle + Random.Range(-spread / 2, spread / 2);
        Vector2 vel = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad),
                                  Mathf.Sin(angle * Mathf.Deg2Rad)) * speed;

        var p = particleArray[index];
        p.position = pos;
        p.prevPosition = pos;
        p.velocity = vel;
        p.lifetime = particleLifetime;
        p.age = 0;
        p.alive = 1;

        particleArray[index] = p;

        var sr = spriteRenderers[index];
        sr.enabled = true;
        sr.transform.position = pos;
        sr.transform.localScale = Vector3.one * particleSize;
    }

    /**
     * @brief Oblicza pozycj? startow? cz?steczki.
     *
     * @return Pozycja emisji cz?steczki
     */
    Vector2 GetEmissionPosition()
    {
        Vector2 pos = transform.position;

        switch (emissionShape)
        {
            case EmissionShape.Point:
                return pos;

            case EmissionShape.Circle:
                return pos + Random.insideUnitCircle.normalized * emissionRadius;

            case EmissionShape.Line:
                float half = emissionArea.x / 2f;
                return pos + new Vector2(Random.Range(-half, half), 0f);

            case EmissionShape.Area:
                float halfX = emissionArea.x / 2f;
                float halfY = emissionArea.y / 2f;
                return pos + new Vector2(
                    Random.Range(-halfX, halfX),
                    Random.Range(-halfY, halfY)
                );

            default:
                return pos;
        }
    }

    /**
     * @brief Aplikuje ustawienia z presetu roboczego.
     */
    public void ApplyPresetFromOverride()
    {
        if (overridePresetData != null)
            overridePresetData.CopyTo(this);
    }

    /**
     * @brief Prosty licznik FPS (debug).
     */
    void FPSCounter()
    {
        fpsTimer += Time.deltaTime;
        frameCount++;

        if (fpsTimer >= 1f)
        {
            UnityEngine.Debug.Log($"FPS: {frameCount}");
            fpsTimer = 0f;
            frameCount = 0;
        }

    }
}
