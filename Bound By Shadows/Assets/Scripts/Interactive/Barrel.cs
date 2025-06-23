using UnityEngine;

/* Ten skrypt obs�uguje niszczenie beczki po kolizji z atakiem gracza.
   - Po wykryciu uderzenia beczka odtwarza animacj� zniszczenia i d�wi�k.
   - Informacja o zniszczonej beczce jest zapisywana w systemie zapisu (SaveableObject).
   - Beczka zostaje zniszczona po kr�tkim op�nieniu (0.9s), by umo�liwi� doko�czenie animacji.

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

