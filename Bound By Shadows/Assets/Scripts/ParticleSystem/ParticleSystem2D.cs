using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Scripting;

public enum EmissionShape { Point, Line, Circle, Area }

public enum ParticleEffectType { Fire, Smoke, Fog, RainDrops }// Explosion

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

    private float emissionTimer = 0f;
    private List<Particle> particles = new List<Particle>();

    void Start()
    {
        ApplyPresent(effectType);
    }
    void Update()
    {
        float dt = Time.deltaTime;

        emissionTimer += dt;
        while (emissionTimer > 1f / emissionRate)
        {
            EmitParticle();
            emissionTimer -= 1f / emissionRate;
        }

        for (int i = particles.Count - 1; i >= 0; i--)
        {
            if (!particles[i].Update(dt, gravity, airResistance, enableGroundCollision, groundY, bounceFactor, wind))
                particles.RemoveAt(i);
            else
                particles[i].ApplyEffects(colorOverLifetime, scaleOverLifetime, alphaOverLifetime);
        }

    }

    void EmitParticle()
    {
        Vector2 pos = GetEmissionPosition();
        float speed = Random.Range(minSpeed, maxSpeed);
        float angle = directionAngle + Random.Range(-spread / 2f, spread / 2f);
        Vector2 vel = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * speed;

        Color starColor = colorOverLifetime.Evaluate(0f);
        Particle p = new Particle(pos, vel, particleLifetime, starColor, particleMaterial, particleSprite);
        p.spriteObject.transform.localScale = Vector3.one * particleSize;
        particles.Add(p);
    }

    Vector2 GetEmissionPosition()
    {
        switch (emissionShape)
        {
            case EmissionShape.Line:
                return (Vector2)transform.position + new Vector2(Random.Range(-emissionRadius, emissionRadius), 0f);
            case EmissionShape.Circle:
                Vector2 dir = Random.insideUnitCircle.normalized * emissionRadius;
                return (Vector2)transform.position + dir;
            case EmissionShape.Area:
                return (Vector2)transform.position + new Vector2(Random.Range(-emissionArea.x / 2f, emissionArea.x / 2f),
                    Random.Range(-emissionArea.y / 2f, emissionArea.y / 2f));

            default:
                return transform.position;
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
                //emissionShape = EmissionShape.Line;
                //emissionRadius = 0.8f;
                //emissionRate = 50;
                //minSpeed = 0.5f;
                //maxSpeed = 1.5f;
                //directionAngle = 90f;
                //spread = 100f;
                //particleLifetime = 5f;

                //colorOverLifetime = new Gradient
                //{
                //    colorKeys = new GradientColorKey[]
                //    {
                //        new GradientColorKey(new Color(0.4f, 0.4f, 0.4f), 0f),
                //        new GradientColorKey(new Color(0.7f, 0.7f, 0.7f), 1f)
                //    },
                //    alphaKeys = new GradientAlphaKey[]
                //    {
                //        new GradientAlphaKey(0.4f, 0f),
                //        new GradientAlphaKey(0.2f, 1f)
                //    }
                //};
                //scaleOverLifetime = AnimationCurve.EaseInOut(0, 0.5f, 1, 2f);
                //alphaOverLifetime = AnimationCurve.EaseInOut(0, 0.5f, 1, 0f);
                emissionShape = EmissionShape.Area;
                emissionArea = new Vector2(3f, 1.5f);
                emissionRate = 10;
                minSpeed = 0.2f;
                maxSpeed = 0.5f;
                directionAngle = 80f;
                spread = 40f;
                particleLifetime = 6f;

                colorOverLifetime = new Gradient
                {
                    colorKeys = new GradientColorKey[]
                    {
                        new GradientColorKey(new Color(0.8f, 0.8f, 0.8f), 0f),
                        new GradientColorKey(new Color(0.9f, 0.9f, 0.9f), 1f)
                    },
                    alphaKeys = new GradientAlphaKey[]
                    {
                        new GradientAlphaKey(0.2f, 0f),
                        new GradientAlphaKey(0.1f, 1f)
                    }
                };

                scaleOverLifetime = AnimationCurve.EaseInOut(0, 1f, 1, 2.5f);
                alphaOverLifetime = AnimationCurve.EaseInOut(0, 0.2f, 1, 0f);
                break;

            case ParticleEffectType.RainDrops:
                emissionShape = EmissionShape.Area;
                emissionArea = new Vector2(10f, 5f);
                emissionRate = 120;
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
                groundY = -62.55f;
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
        }
    }
}
