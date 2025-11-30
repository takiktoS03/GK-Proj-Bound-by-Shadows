//using System.Diagnostics.Tracing;
//using UnityEngine;
//using UnityEngine.UIElements;

//public class Particle
//{
//    public Vector2 position;
//    public Vector2 velocity;
//    public float lifetime;
//    public float age;
//    public Color color;
//    public GameObject spriteObject;
//    private SpriteRenderer spriteRenderer;
//    private ParticlePool pool;

//    public Particle(Vector2 pos, Vector2 vel, float life, Color col, ParticlePool poolRef)
//    {
//        position = pos;
//        velocity = vel;
//        lifetime = life;
//        age = 0f;
//        color = col;
//        pool = poolRef;

//        spriteObject = pool.Get();
//        spriteRenderer = spriteObject.GetComponent<SpriteRenderer>();
//        spriteRenderer.color = col;
        
//        spriteObject.transform.position = pos;
//    }

//    public bool Update(float deltaTime, Vector2 gravity, float airResistance, bool enableGroundCollision, float groundY, float bounceFactor, Vector2 wind)
//    {
//        age += deltaTime;
//        if (age > lifetime)
//        {
//            pool.Release(spriteObject);
//            return false;
//        }

//        velocity += (gravity + wind) * deltaTime;

//        velocity *= Mathf.Pow(airResistance, deltaTime * 60f);

//        position += velocity * deltaTime;

//        if (enableGroundCollision && position.y <= groundY) 
//        { 
//            position.y = groundY;
//            velocity.y *= -bounceFactor;
//            if (Mathf.Abs(velocity.y) < 0.1f)
//            {
//                pool.Release(spriteObject);
//                return false;
//            }

//        }
//        spriteObject.transform.position = position;

//        //float alpha = Mathf.Lerp(1f, 0f, (age / lifetime) * 0.5f);
//        //var sr = spriteObject.GetComponent<SpriteRenderer>();
//        //sr.color = new Color(color.r, color.g, color.b, alpha);

//        return true;
//    }

//    public void ApplyEffects(Gradient colorOverLifetime, AnimationCurve scaleOverLifetime, AnimationCurve alphaOverLifetime)
//    {
//        float t = age / lifetime;

//        Color newColor = colorOverLifetime.Evaluate(t);

//        float alpha = alphaOverLifetime.Evaluate(t);
//        newColor.a *= alpha;
//        //var sr = sprite.GetComponent <SpriteRenderer>();
//        //sr.color = newColor;

//        float scale = scaleOverLifetime.Evaluate(t);
//        spriteRenderer.color = newColor;
//        spriteObject.transform.localScale = Vector3.one * scale;
//        //sprite.transform.localScale = Vector3.one * scale;
//    }
//}
