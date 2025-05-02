using UnityEngine;

/**
 Julia Bigaj
Logika niszczenia beczki 
 **/
public class Barrel : MonoBehaviour, IDamageable
{
    private Animator anim;
    private bool destroyed = false;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void OnHit()
    {
        if (!destroyed)
        {
            destroyed = true;
            anim.SetBool("Destroy", true);
            Destroy(gameObject, 1.2f);
        }
    }
}
