using UnityEngine;

[CreateAssetMenu(fileName = "ParticleEffectPreset", menuName = "Particles/Effect Preset")]
public class ParticleEffectPreset : ScriptableObject
{
    public Material particleMaterial;
    public Sprite particleSprite;

    [Header("Basic parameters")]
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
    public AnimationCurve scaleOverLifetime;
    public AnimationCurve alphaOverLifetime;

    [Header("Physics")]
    public bool enableGravity = true;
    public Vector2 gravity = new Vector2(0f, -9.81f);
    public float airResistance = 0.98f;
    public bool enableGroundCollision = false;
    public float groundY = 0f;
    public float bounceFactor = 0.5f;
    public Vector2 wind = Vector2.zero;

    private void OnEnable()
    {
        if (colorOverLifetime == null || colorOverLifetime.colorKeys.Length == 0)
        {
            colorOverLifetime = new Gradient
            {
                colorKeys = new[] {
                    new GradientColorKey(Color.white, 0f),
                    new GradientColorKey(Color.white, 1f)
                },
                alphaKeys = new[] {
                    new GradientAlphaKey(1f, 0f),
                    new GradientAlphaKey(1f, 1f)
                }
            };
        }

        if (scaleOverLifetime == null || scaleOverLifetime.keys.Length == 0)
            scaleOverLifetime = AnimationCurve.Linear(0, 1, 1, 1);

        if (alphaOverLifetime == null || alphaOverLifetime.keys.Length == 0)
            alphaOverLifetime = AnimationCurve.Linear(0, 1, 1, 0);
    }
}
