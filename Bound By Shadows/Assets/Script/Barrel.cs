using UnityEngine;

/**
 Julia Bigaj
Logika niszczenia beczki 
 **/
public class Barrel : MonoBehaviour
{
    public string barrelID;

    private Animator anim;
    private bool destroyed = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        if (SaveManager.Instance != null && SaveManager.Instance.IsBarrelDestroyed(gameObject.name))
        {
            Debug.Log("Beczka ju¿ wczeœniej zniszczona: " + gameObject.name);
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Kolizja z: " + other.name + " | tag: " + other.tag + " | scena: " + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);

        if (!destroyed && other.CompareTag("PlayerAttack"))
        {
            Debug.Log("Niszczenie beczki: " + gameObject.name);
            destroyed = true;
            anim.SetTrigger("Destroy");

            SaveManager.Instance.AddDestroyedBarrel(gameObject.name);

            Destroy(gameObject, 1.5f); // dopasuj do d³ugoœci animacji
        }
    }
}

