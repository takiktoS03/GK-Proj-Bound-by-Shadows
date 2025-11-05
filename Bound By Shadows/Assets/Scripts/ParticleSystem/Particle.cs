using UnityEngine;
using UnityEngine.UIElements;

public class Particle
{
    public Vector2 position;
    public Vector2 velocity;
    public float lifetime;
    public float age;
    public Color color;
    public GameObject sprite;

    public Particle (Vector2 pos, Vector2 vel, float life, Color col, GameObject prefab)
    {
        position = pos;
        velocity = vel;
        lifetime = life;
        age = 0f;
        color = col;

        sprite = GameObject.Instantiate(prefab, pos, Quaternion.identity);
        sprite.GetComponent<SpriteRenderer>().color = col;
    }

    public bool Update(float deltaTime) 
    {
        age += deltaTime;
        if (age > lifetime)
        {
            GameObject.Destroy(sprite);
            return false;
        }

        position += velocity * deltaTime;
        sprite.transform.position = position;

        float alpha = Mathf.Lerp(1f, 0f, (age / lifetime) * 0.5f);
        var sr = sprite.GetComponent<SpriteRenderer>();
        sr.color = new Color(color.r, color.g, color.b, alpha);

        return true;
    }

    public void ApplyEffects(Gradient colorOverLifetime, AnimationCurve scaleOverLifetime)
    {
        float t = age / lifetime;

        Color newColor = colorOverLifetime.Evaluate(t);
        var sr = sprite.GetComponent <SpriteRenderer>();
        sr.color = newColor;

        float scale = scaleOverLifetime.Evaluate(t);
        sprite.transform.localScale = Vector3.one * scale;
    }
}
