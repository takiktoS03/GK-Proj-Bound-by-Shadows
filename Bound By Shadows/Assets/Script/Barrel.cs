using System.Diagnostics;
using UnityEngine;

/**
 Julia Bigaj
Logika niszczenia beczki 
 **/
public class Barrel : MonoBehaviour
{
    private Animator anim;
    private bool destroyed = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        UnityEngine.Debug.Log("Start: Destroy = " + anim.GetBool("Destroy")); // sprawdü stan na poczπtku
    }

    public void OnHit()
    {
        if (!destroyed)
        {
            destroyed = true;
            anim.SetBool("Destroy", true);
            UnityEngine.Debug.Log("Beczka trafiona i niszczona!");
            Destroy(gameObject, 1.5f); // dopasuj czas
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
