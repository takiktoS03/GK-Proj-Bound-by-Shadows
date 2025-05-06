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
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!destroyed && other.CompareTag("PlayerAttack"))
        {
            Debug.Log("Niszczenie beczki: " + gameObject.name);
            destroyed = true;
            anim.SetTrigger("Destroy");

            Destroy(gameObject, 1.5f); // dopasuj do d³ugoœci animacji
        }
    }
    // ZAPIS stanu beczki
    public object CaptureState()
    {
        return destroyed;
    }

    // ODTWORZENIE stanu beczki
    public void RestoreState(object state)
    {
        destroyed = (bool)state;

        if (destroyed)
        {
            gameObject.SetActive(false); // beczka zosta³a ju¿ zniszczona
        }
    }
}

