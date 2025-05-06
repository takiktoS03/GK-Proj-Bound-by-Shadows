using UnityEngine;
using System.Collections;

/// <summary>
///  Zapisuje / wczytuje stan gracza (pozycja, HP, prędkość, kierunek).
///  Dodaj na ten sam GameObject co <see cref="UniqueID"/>.
/// </summary>
[RequireComponent(typeof(UniqueID))]
public class PlayerSave : MonoBehaviour, ISaveable
{
    [SerializeField] private Health health;      // odniesienie do skryptu Health
    [SerializeField] private Rigidbody2D rb;     // opcjonalnie – do zapisu prędkości

    private void Awake()
    {
        if (!health) health = GetComponent<Health>();
        if (!rb) rb = GetComponent<Rigidbody2D>();
    }

    // -------------------- STRUKTURA DANYCH --------------------
    [System.Serializable]
    private struct Data
    {
        public Vector3 position;
        public float health;
        public Vector2 velocity;
        public bool facingRight;
    }

    // -------------------- ISaveable ---------------------------
    public string CaptureState()
    {
        var d = new Data
        {
            position = transform.position,
            health = health.CurrentHealth,
            velocity = rb ? rb.linearVelocity : Vector2.zero,
            facingRight = transform.localScale.x >= 0
        };
        return JsonUtility.ToJson(d);
    }

    public void RestoreState(string state)
    {
        var d = JsonUtility.FromJson<Data>(state);

        // pozycja
        transform.position = d.position;

        // prędkość (z krótkim wstrzymaniem kolizji)
        if (rb)
        {
            rb.isKinematic = true;
            rb.linearVelocity = d.velocity;
            StartCoroutine(EnablePhysicsNextFrame());
        }

        // zdrowie
        health.SetHealth(d.health);

        // kierunek patrzenia
        var s = transform.localScale;
        s.x = Mathf.Abs(s.x) * (d.facingRight ? 1 : -1);
        transform.localScale = s;
    }

    private IEnumerator EnablePhysicsNextFrame()
    {
        yield return null;        // jedna klatka
        rb.isKinematic = false;
    }
}
