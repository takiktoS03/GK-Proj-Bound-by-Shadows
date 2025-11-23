//using UnityEngine;

//public class ParticleSpawner : MonoBehaviour
//{
//    public Material particleMaterial;
//    public Sprite mySprite;

//    void Start()
//    {
//        var ps = gameObject.AddComponent<ParticleSystem2D>();
//        ps.particleMaterial = particleMaterial;
//        ps.particleSprite = mySprite;
//        ps.emissionRate = 300;
//        ps.particleLifetime = 8f;
//        ps.minSpeed = 1f;
//        ps.maxSpeed = 4f;
//        ps.directionAngle = 90f;
//        ps.spread = 50f;
//        ps.emissionShape = EmissionShape.Line;
//        ps.emissionRadius = 6f;
        
//        ps.enableGravity = true;
//        ps.gravity = new Vector2(0f, -9.8f);
//        ps.airResistance = 0.99f;
//        ps.enableGroundCollision = true;
//        ps.groundY = -60f;
//        ps.bounceFactor = 0.2f;
//        ps.wind = new Vector2(1f, 0f);

//        ps.colorOverLifetime = new Gradient
//        {
//            colorKeys = new GradientColorKey[]
//            {
//            new GradientColorKey(Color.blue, 0f),
//            new GradientColorKey(Color.blue, 1f)
//            },
//            alphaKeys = new GradientAlphaKey[]
//            {
//            new GradientAlphaKey(1f, 0f),
//            new GradientAlphaKey(0f, 1f)
//            }
//        };

//        ps.scaleOverLifetime = AnimationCurve.EaseInOut(0, 0.2f, 1, 1.5f);
//    }
//}
