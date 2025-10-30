using UnityEngine;

public class ParticleSpawner : MonoBehaviour
{
    public GameObject particlePrefab;

    void Start()
    {
        var ps = gameObject.AddComponent<ParticleSystem2D>();
        ps.particlePrefab = particlePrefab;
        ps.emissionRate = 1;
        ps.particleLifetime = 10f;
        ps.particleSpeed = 2f;
    }
}
