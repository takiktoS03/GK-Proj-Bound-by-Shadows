using UnityEngine;

/* Ten skrypt obs³uguje niszczenie beczki po kolizji z atakiem gracza.
   - Po wykryciu uderzenia beczka odtwarza animacjê zniszczenia i dŸwiêk.
   - Informacja o zniszczonej beczce jest zapisywana w systemie zapisu (SaveableObject).
   - Beczka zostaje zniszczona po krótkim opóŸnieniu (0.9s), by umo¿liwiæ dokoñczenie animacji.

   Autor: Julia Bigaj
*/
public class Barrel : MonoBehaviour
{
    private Animator anim;
    private bool destroyed = false;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!destroyed && other.CompareTag("PlayerAttack"))
        {
            destroyed = true;

            SoundManager.Instance?.PlayBarrel();

            var saveable = GetComponent<SaveableObject>();
            BarrelSaveData.RegisterDestroyedBarrel(saveable.UniqueId);
            anim.SetTrigger("Destroy");
            Destroy(gameObject, 0.9f);
        }
    }
}

