using System.Collections;
using UnityEngine;

[System.Serializable]
public class SpiderWebData
{
    public bool destroyed;
}

public class SpiderWeb : MonoBehaviour, ISaveable
{
    [Header("Uderzenia potrzebne do zniszczenia")]
    public int hitsToBreak = 3;

    private int currentHits = 0;
    private SpriteRenderer sr;
    private bool destroyed = false;
    private Collider2D col;
    private Animator anim;
    private SaveableObject saveId;

    void Awake()
    {
        saveId = GetComponent<SaveableObject>();

        if (saveId != null && DestroyedRegistry.IsDestroyed(saveId.UniqueId))
        {
            gameObject.SetActive(false);
            return;
        }

        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
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

        float newAlpha = Mathf.Lerp(
            0f,
            1f,
            (hitsToBreak - currentHits) / (float)hitsToBreak
        );

        var c = sr.color;
        c.a = newAlpha;
        sr.color = c;

        if (currentHits >= hitsToBreak)
        {
            destroyed = true;

            if (saveId != null)
            {
                DestroyedRegistry.MarkDestroyed(saveId.UniqueId);
                DestroyedRegistry.Save();
            }

            var anim = GetComponent<Animator>();
            if (anim != null)
                anim.SetTrigger("Destroy");

            gameObject.SetActive(false);
        }

    }

    // ================= SAVE SYSTEM =================

    public object CaptureState()
    {
        return new SpiderWebData
        {
            destroyed = destroyed
        };
    }

    public void RestoreState(object state)
    {
        string json = state as string;
        var data = JsonUtility.FromJson<SpiderWebData>(json);

        destroyed = data.destroyed;

        if (destroyed)
        {
            gameObject.SetActive(false);
            return;
        }
    }
}
