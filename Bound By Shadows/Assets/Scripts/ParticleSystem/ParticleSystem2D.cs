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
public enum ParticleEffectType { Fire, Smoke, Fog, RainDrops, Default }

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
    [Header("Typ dzwieku")]
    public ParticleEffectType effectType = ParticleEffectType.RainDrops;

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

    [Header("Performance")]
    public int maxParticles = 2000;

    [Header("Rendering")]
    [HideInInspector] public string sortingLayer = "Default";
    [HideInInspector] public int orderInLayer = 0;

    [Header("Preset")]
    public ParticleEffectPreset preset;
    private bool isRebuilding = false;

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
        //particleArray = new NativeArray<ParticleData>(maxParticles, Allocator.Persistent);
        //transformArray = new TransformAccessArray(maxParticles);
        //spriteRenderers = new SpriteRenderer[maxParticles];
        //freeIndices = new Stack<int>(maxParticles);

        //for (int i = 0; i < maxParticles; i++)
        //{
        //    GameObject go = new GameObject("Particle_" + i);
        //    var sr = go.AddComponent<SpriteRenderer>();
        //    sr.material = particleMaterial;
        //    sr.sprite = particleSprite;
        //    sr.enabled = false;
        //    sr.sortingLayerName = sortingLayer;
        //    sr.sortingOrder = orderInLayer;

        //    go.transform.position = Vector3.zero;
        //    go.transform.localScale = Vector3.zero;

        //    spriteRenderers[i] = sr;
        //    transformArray.Add(go.transform);

        //    freeIndices.Push(i);

        //    ParticleData p = new ParticleData
        //    {
        //        position = Vector3.zero,
        //        velocity = Vector3.zero,
        //        lifetime = 0f,
        //        age = 0f,
        //        alive = 0
        //    };
        //    particleArray[i] = p;
        //}

        if (preset != null)
        {
            ApplyPreset();
        }
        else
        {
            ApplyPresent(effectType);
        }

        InitializeParticleData();

        //ApplyPresent(effectType);
    }

    private void OnDestroy()
    {
        if (particleArray.IsCreated) particleArray.Dispose();
        if(transformArray.isCreated) transformArray.Dispose();
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


    void InitializeParticleData()
    {
        particleArray = new NativeArray<ParticleData>(maxParticles, Allocator.Persistent);
        transformArray = new TransformAccessArray(maxParticles);
        spriteRenderers = new SpriteRenderer[maxParticles];
        freeIndices = new Stack<int>(maxParticles);

        for (int i = 0; i < maxParticles; i++)
        {
            GameObject go = new GameObject("Particle_" + i);
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
        Vector2 basePos = transform.position;

        switch (emissionShape)
        {
            case EmissionShape.Line:
                return (Vector2)basePos + new Vector2(Random.Range(-emissionRadius, emissionRadius), 0f);
            case EmissionShape.Circle:
                Vector2 dir = Random.insideUnitCircle.normalized * emissionRadius;
                return (Vector2)basePos + dir;
            case EmissionShape.Area:
                return (Vector2)basePos + new Vector2(Random.Range(-emissionArea.x / 2f, emissionArea.x / 2f),
                    Random.Range(-emissionArea.y / 2f, emissionArea.y / 2f));

            default:
                return basePos;
        }
    }

    public void ApplyPresent(ParticleEffectType type)
    {
        switch (type)
        {
            case ParticleEffectType.Fire:
                emissionShape = EmissionShape.Circle;
                emissionRadius = 0.3f;
                emissionRate = 50;
                minSpeed = 1.5f;
                maxSpeed = 3f;
                directionAngle = 90f;
                spread = 25f;
                particleLifetime = 2f;

                colorOverLifetime = new Gradient
                {
                    colorKeys = new GradientColorKey[]
                    {
                        new GradientColorKey(Color.yellow, 0f),
                        new GradientColorKey(new Color(1f, 0.5f, 0f), 0.5f),
                        new GradientColorKey(Color.red, 1f),
                    },
                    alphaKeys = new GradientAlphaKey[]
                    {
                        new GradientAlphaKey(1f, 0f),
                        new GradientAlphaKey(0f, 1f),
                    }
                };
                scaleOverLifetime = AnimationCurve.EaseInOut(0, 0.3f, 1, 1.2f);
                alphaOverLifetime = AnimationCurve.EaseInOut(0, 0.6f, 1, 0f);
                break;

            case ParticleEffectType.Smoke:
                emissionShape = EmissionShape.Line;
                emissionRadius = 0.8f;
                emissionRate = 50;
                minSpeed = 0.5f;
                maxSpeed = 1.5f;
                directionAngle = 90f;
                spread = 100f;
                particleLifetime = 5f;

                colorOverLifetime = new Gradient
                {
                    colorKeys = new GradientColorKey[]
                    {
                        new GradientColorKey(new Color(0.2f, 0.2f, 0.2f), 0f),
                        new GradientColorKey(new Color(0.5f, 0.5f, 0.5f), 1f)
                    },
                    alphaKeys = new GradientAlphaKey[]
                    {
                        new GradientAlphaKey(0.4f, 0f),
                        new GradientAlphaKey(0.2f, 1f)
                    }
                };
                scaleOverLifetime = AnimationCurve.EaseInOut(0, 0.5f, 1, 2f);
                alphaOverLifetime = AnimationCurve.EaseInOut(0, 0.5f, 1, 0f);
                break;

            case ParticleEffectType.Fog:
                emissionShape = EmissionShape.Area;
                emissionArea = new Vector2(15f, 2f);    
                emissionRate = 25;                        
                minSpeed = 0.1f;
                maxSpeed = 0.3f;                      
                directionAngle = 0f;                   
                spread = 50f;                         
                particleLifetime = 5f;                  
                particleSize = 2.2f;                     

                enableGravity = false;
                gravity = Vector2.zero;
                airResistance = 0.99f;

                enableGroundCollision = false;
                groundY = 0f;
                bounceFactor = 0f;

                wind = new Vector2(0.3f, 0f);

                colorOverLifetime = new Gradient
                {
                    colorKeys = new GradientColorKey[]
                    {
            new GradientColorKey(new Color(0.80f, 0.80f, 0.80f), 0f),
            new GradientColorKey(new Color(0.95f, 0.95f, 0.95f), 1f)
                    },
                    alphaKeys = new GradientAlphaKey[]
                    {
            new GradientAlphaKey(0.1f, 0f),
            new GradientAlphaKey(0.001f, 1f)
                    }
                };

                scaleOverLifetime = AnimationCurve.EaseInOut(0, 1.0f, 1, 3.0f);
                alphaOverLifetime = AnimationCurve.EaseInOut(0, 0.15f, 1, 0f);
                break;

            case ParticleEffectType.RainDrops:
                emissionShape = EmissionShape.Area;
                emissionArea = new Vector2(10f, 5f);
                emissionRate = 300;
                minSpeed = 1f;
                maxSpeed = 2f;
                directionAngle = -90f;
                spread = 100f;
                particleLifetime = 10f;
                particleSize = 3f;

                enableGravity = true;
                gravity = new Vector2(0f, -9.8f);
                airResistance = 0.99f;
                enableGroundCollision = true;
                groundY = -0f;
                bounceFactor = 0.2f;
                wind = new Vector2(1f, 0f);

                colorOverLifetime = new Gradient
                {
                    colorKeys = new GradientColorKey[]
                    {
            new GradientColorKey(new Color(0.5f, 0.6f, 1f), 0f),
            new GradientColorKey(new Color(0.3f, 0.4f, 0.9f), 1f)
                    },
                    alphaKeys = new GradientAlphaKey[]
                    {
            new GradientAlphaKey(0.7f, 0f),
            new GradientAlphaKey(0.1f, 1f)
                    }
                };

                scaleOverLifetime = AnimationCurve.EaseInOut(0, 1f, 1, 0.3f);
                alphaOverLifetime = AnimationCurve.EaseInOut(0, 0.8f, 1, 0f);
                break;

            case ParticleEffectType.Default:

                emissionShape = EmissionShape.Point;
                emissionArea = new Vector2(1f, 1f);
                emissionRadius = 0.2f;

                emissionRate = 10;
                particleLifetime = 2f;
                particleSize = 1f;

                minSpeed = 0.2f;
                maxSpeed = 1f;
                directionAngle = 90f;
                spread = 20f;

                enableGravity = false;
                gravity = Vector2.zero;
                airResistance = 0.98f;
                enableGroundCollision = false;
                groundY = 0f;
                bounceFactor = 0f;

                wind = Vector2.zero;

                colorOverLifetime = new Gradient
                {
                    colorKeys = new[]
                    {
            new GradientColorKey(Color.white, 0f),
            new GradientColorKey(Color.white, 1f),
        },
                    alphaKeys = new[]
                    {
            new GradientAlphaKey(1f, 0f),
            new GradientAlphaKey(0f, 1f)
        }
                };

                scaleOverLifetime = AnimationCurve.Linear(0, 1f, 1, 1f);
                alphaOverLifetime = AnimationCurve.Linear(0, 1f, 1, 0f);
                break;

        }
    }

    public void ApplyPreset()
    {
        if (preset == null) return;

        particleMaterial = preset.particleMaterial;
        particleSprite = preset.particleSprite;

        particleSize = preset.particleSize;
        emissionRate = preset.emissionRate;
        particleLifetime = preset.particleLifetime;

        minSpeed = preset.minSpeed;
        maxSpeed = preset.maxSpeed;
        directionAngle = preset.directionAngle;
        spread = preset.spread;

        emissionShape = preset.emissionShape;
        emissionRadius = preset.emissionRadius;
        emissionArea = preset.emissionArea;

        colorOverLifetime = preset.colorOverLifetime;
        scaleOverLifetime = preset.scaleOverLifetime;
        alphaOverLifetime = preset.alphaOverLifetime;

        enableGravity = preset.enableGravity;
        gravity = preset.gravity;
        airResistance = preset.airResistance;
        enableGroundCollision = preset.enableGroundCollision;
        groundY = preset.groundY;
        bounceFactor = preset.bounceFactor;
        wind = preset.wind;
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
