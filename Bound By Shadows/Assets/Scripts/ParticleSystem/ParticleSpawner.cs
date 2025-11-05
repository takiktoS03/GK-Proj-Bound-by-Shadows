using UnityEngine;

public class ParticleSpawner : MonoBehaviour
{
    public GameObject particlePrefab;

    void Start()
    {
        var ps = gameObject.AddComponent<ParticleSystem2D>();
        ps.particlePrefab = particlePrefab;
        ps.emissionRate = 50;
        ps.particleLifetime = 5f;
        ps.minSpeed = 1f;
        ps.maxSpeed = 4f;
        ps.directionAngle = 90f;
        ps.spread = 80f;
        ps.emissionShape = EmissionShape.Line;
        ps.emissionRadius = 6f;

        ps.colorOverLifetime = new Gradient
        {
            colorKeys = new GradientColorKey[]
            {
            new GradientColorKey(Color.red, 0f),
            new GradientColorKey(Color.yellow, 1f)
            },
            alphaKeys = new GradientAlphaKey[]
            {
            new GradientAlphaKey(1f, 0f),
            new GradientAlphaKey(0f, 1f)
            }
        };

        ps.scaleOverLifetime = AnimationCurve.EaseInOut(0, 0.2f, 1, 1.5f);
    }
}
