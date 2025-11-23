using UnityEngine;

public class SpiderWeb : MonoBehaviour
{
    [Header("Uderzenia potrzebne do zniszczenia")]
    public int hitsToBreak = 3;

    private int currentHits = 0;
    private SpriteRenderer sr;
    private bool destroyed = false;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!destroyed && other.CompareTag("PlayerAttack"))
        {
            Hit();
        }
    }

    void Hit()
    {
        currentHits++;

        float newAlpha = Mathf.Lerp(0f, 1f, (hitsToBreak - currentHits) / (float)hitsToBreak);
        var c = sr.color;
        c.a = newAlpha;
        sr.color = c;

        if (currentHits >= hitsToBreak)
        {
            destroyed = true;

            var anim = GetComponent<Animator>();
            if (anim != null)
                anim.SetTrigger("Destroy");

            Destroy(gameObject, anim ? 0.9f : 0.1f);
        }
    }
}
