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

public enum EmissionShape { Point, Line, Circle, Area }

public struct ParticleData
{
    public Vector2 position;
    public Vector2 velocity;
    public float lifetime;
    public float age;
    public int alive; 
}

[BurstCompile]
public struct ParticleUpdateJob : IJobParallelForTransform
{
    public NativeArray<ParticleData> particles;

    public float deltaTime;
    public Vector2 gravity;
    public float airResistance;
    public bool enableGroundCollision;
    public float groundY;
    public float bounceFactor;
    public Vector2 wind;

    public void Execute(int index, TransformAccess transform)
    {
        ParticleData p = particles[index];

        if (p.alive == 0)
            return;

        p.age += deltaTime;
        if (p.age > p.lifetime)
        {
            p.alive = 0;
            particles[index] = p;
            return;
        }

        // fizyka
        p.velocity += (gravity + wind) * deltaTime;
        p.velocity *= Mathf.Pow(airResistance, deltaTime * 60f);
        p.position += p.velocity * deltaTime;

        if (enableGroundCollision && p.position.y <= groundY)
        {
            p.position.y = groundY;
            p.velocity.y *= -bounceFactor;

            if (Mathf.Abs(p.velocity.y) < 0.1f)
            {
                p.alive = 0;
                particles[index] = p;
                return;
            }
        }

        transform.position = p.position;
        particles[index] = p;

    }
}

public class ParticleSystem2D : MonoBehaviour
{
    //[Header("Typ dzwieku")]
    //public ParticleEffectType effectType = ParticleEffectType.RainDrops;

    [Header("Basic parameters")]
    public Material particleMaterial;
    public Sprite particleSprite;
    public float particleSize = 0.2f;
    public int emissionRate = 20;
    public float particleLifetime = 2f;

    [Header("Speed")]
    public float minSpeed = 1f;
    public float maxSpeed = 3f;
    public float directionAngle = 90f; // 90 degress top
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
    public bool enableGroundCollision = true;
    public float groundY = 0f;
    public float bounceFactor = 0.5f;
    public Vector2 wind = Vector2.zero; 

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

    void Start()
    {
        
        if (overridePresetData == null && preset != null)
        {
            overridePresetData = Instantiate(preset);
        }

        // 2. Skopiuj override warto?ci do systemu - MUSI by? przed inicjalizacj? renderów
        ApplyPresetFromOverride();

        // 3. Teraz renderer zna poprawne warto?ci (sprite, materiale, wielko?ci)
        InitializeParticleData();

        //ApplyPresent(effectType);
    }

    private void OnDestroy()
    {
        if (particleArray.IsCreated) particleArray.Dispose();
        if (transformArray.isCreated) transformArray.Dispose();
    }
    void Update()
    {

        if (isRebuilding) return;

        //updateTimer.Restart();

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
            enableGroundCollision = enableGroundCollision,
            groundY = groundY,
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

            aliveCount++;

            float t = p.age / Mathf.Max(0.0001f, p.lifetime);

            // kolor
            Color newColor = colorOverLifetime.Evaluate(t);
            float alpha = alphaOverLifetime.Evaluate(t);
            newColor.a *= alpha;

            // skala
            float scale = scaleOverLifetime.Evaluate(t) * particleSize;

            var sr = spriteRenderers[i];
            sr.enabled = true;
            sr.color = newColor;
            sr.transform.localScale = Vector3.one * scale;
        }

        updateTimer.Stop();
        double ms = updateTimer.Elapsed.TotalMilliseconds;

        logIndex++;
        //UnityEngine.Debug.Log($"[{logIndex}] Jobified Update time: {ms:F4} ms | Particles: {aliveCount}");

        FPSCounter();
    }
    void OnDrawGizmos()
    {
        // ?ród?o danych do Gizmo
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


    void InitializeParticleData()
    {
        particleArray = new NativeArray<ParticleData>(maxParticles, Allocator.Persistent);
        transformArray = new TransformAccessArray(maxParticles);
        spriteRenderers = new SpriteRenderer[maxParticles];
        freeIndices = new Stack<int>(maxParticles);

        // --- CREATE OR FIND PARTICLE ROOT ---
        if (particleRoot == null)
        {
            // Try to find existing child (important when entering Play mode)
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

            go.transform.position = transform.position;  // IMPORTANT FIX
            go.transform.localScale = Vector3.zero;

            spriteRenderers[i] = sr;

            transformArray.Add(go.transform);
            freeIndices.Push(i);
        }
    }

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

    public void ApplyPreset()
    {
        if (preset != null)
            preset.CopyTo(this);
    }

    public void ApplyPresetFromOverride()
    {
        if (overridePresetData != null)
            overridePresetData.CopyTo(this);
    }
    
    void FPSCounter()
    {
        fpsTimer += Time.deltaTime;
        frameCount++;

        if (fpsTimer >= 1f)
        {
            //UnityEngine.Debug.Log($"FPS: {frameCount}");
            fpsTimer = 0f;
            frameCount = 0;
        }

    }
}
