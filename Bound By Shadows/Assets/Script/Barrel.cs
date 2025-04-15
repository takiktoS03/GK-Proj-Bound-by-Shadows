using UnityEngine;

/**
 Julia Bigaj
Logika niszczenia beczki 
 **/
public class Barrel : MonoBehaviour
{
    private Animator anim;
    private bool destroyed = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();   
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!destroyed && other.CompareTag("PlayerAttack"))
        {
            destroyed = true;
            anim.SetTrigger("Destroy");
            Destroy(gameObject, 0.5f); // zniknie po animacji
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
