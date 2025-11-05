using UnityEngine;
using System.Collections.Generic;

public enum EmissionShape { Point, Line, Circle, Area}

public class ParticleSystem2D : MonoBehaviour
{

    [Header("Basic parameters")]
    public GameObject particlePrefab;
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
    public AnimationCurve scaleOverLifetime = AnimationCurve.Linear(0, 1f, 1f, 1f);

    private float emissionTimer = 0f;
    private List<Particle> particles = new List<Particle>();

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
            if (!particles[i].Update(dt))
                particles.RemoveAt(i);
            else
                particles[i].ApplyEffects(colorOverLifetime, scaleOverLifetime);
        }

    }

    void EmitParticle()
    {
        Vector2 pos = GetEmissionPosition();
        float speed = Random.Range(minSpeed, maxSpeed);
        float angle = directionAngle + Random.Range(-spread / 2f, spread / 2f);
        Vector2 vel = new Vector2(Mathf.Cos(angle*Mathf.Deg2Rad), Mathf.Sin(angle*Mathf.Deg2Rad)) * speed;

        Color starColor = colorOverLifetime.Evaluate(0f);
        Particle p = new Particle(pos, vel, particleLifetime, starColor, particlePrefab);
        particles.Add(p);
    }

    Vector2 GetEmissionPosition()
    {
        switch (emissionShape) 
        {
            case EmissionShape.Line:
                return(Vector2)transform.position + new Vector2(Random.Range(-emissionRadius, emissionRadius), 0f);
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
}
