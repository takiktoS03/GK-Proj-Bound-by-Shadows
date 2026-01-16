using System.Collections;
using UnityEngine;
/**
 * Skrypt obsługujący niszczenie pajęczyny po kilku uderzeniach gracza,
 * wraz z wizualnym zmniejszaniem jej widoczności.
 *
 * @author Julia Bigaj
 */

public class SpiderWeb : MonoBehaviour
{
    [Header("Ustawienia")]
    public int hitsToBreak = 3;
    public float hitCooldown = 0.1f;

    private int currentHits = 0;
    private float lastHitTime = 0f;
    private SpriteRenderer sr;
    private SaveableObject saveable;

    void Awake()
    {
        saveable = GetComponent<SaveableObject>();
        sr = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerAttack") && Time.time > lastHitTime + hitCooldown)
        {
            Hit();
        }
    }

    void Hit()
    {
        lastHitTime = Time.time;

        currentHits++;
        float healthPercentage = (float)(hitsToBreak - currentHits) / hitsToBreak;

        var c = sr.color;
        c.a = Mathf.Clamp01(healthPercentage);
        sr.color = c;

        if (currentHits >= hitsToBreak)
        {
            string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            SessionDestroyedRegistry.MarkAsDestroyed(sceneName, saveable.UniqueId);
            Destroy(gameObject);
        }
    }
}
