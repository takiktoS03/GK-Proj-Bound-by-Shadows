using UnityEngine;
using System.Collections.Generic;

public class ParticleSystem2D : MonoBehaviour
{
    public GameObject particlePrefab;
    public int emissionRate = 20;
    public float particleLifetime = 2f;
    public float particleSpeed = 2f;

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
        }

    }

    void EmitParticle()
    {
        Vector2 pos = transform.position + new Vector3(Random.Range(-1f, 1f), 0f, 0f);
        Vector2 vel = new Vector2(0, -particleSpeed * Random.Range(0.8f, 1.2f));
        Color col = new Color(1f, Random.Range(0.5f, 1f), 0f, 1f);

        Particle p = new Particle(pos, vel, particleLifetime, col, particlePrefab);
        particles.Add(p);
    }
}
