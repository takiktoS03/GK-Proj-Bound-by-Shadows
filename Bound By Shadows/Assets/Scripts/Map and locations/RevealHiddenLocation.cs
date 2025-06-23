using System.Diagnostics;
using UnityEngine;


/* Ten skrypt ujawnia ukryt� lokacj�, gdy okre�lony obiekt (oznaczony tagiem "LeafArea") wejdzie w trigger.
   - Po wej�ciu wybranego obiektu, aktywowany jest `hiddenLocation`.
   - Wykorzystywany do element�w �rodowiska, kt�re maj� si� ujawnia� w reakcji na gracza lub inne obiekty.

   Autor: Julia Bigaj
*/

public class RevealHiddenLocation : MonoBehaviour
{

    public GameObject hiddenLocation;

    private void onTriggerEnter2D(Collider2D other)
    {
        UnityEngine.Debug.Log("Trigger wszed?: " + other.name);

        if (other.CompareTag("LeafArea"))
        {
            UnityEngine.Debug.Log("To by? li??!");
            hiddenLocation.SetActive(true);
        }
    }
}
